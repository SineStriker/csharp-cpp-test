// See https://aka.ms/new-console-template for more information

using System.Runtime.Serialization.Formatters.Binary;
using SingingTool.Model;
using SingingTool.Model.Line;

public static class Program
{
    static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine(
                $"Usage: {Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().ProcessName)} <svip file> [svip file]");
            return 0;
        }

        var model1 = ReadModel(args[0]);
        if (model1 == null)
        {
            return -1;
        }

        Console.WriteLine("Read model 1 succeed");

        if (args.Length < 2)
        {
            return 0;
        }

        var model2 = ReadModel(args[1]);
        if (model2 == null)
        {
            return -1;
        }

        Console.WriteLine("Read model 2 succeed");

        var traverseLineParam = (LineParam lineParam) =>
        {
            var it = lineParam.Begin;
            while (it != null)
            {
                Console.WriteLine($"pos: {it.Value.Pos} val: {it.Value.Value}");
                it = it.Next;
            }

            return false;
        };

        var compareLineParam = (LineParam param1, LineParam param2, string name) =>
        {
            equal(param1 == null, param2 == null, $"{name} Nullable");

            if (param1 == null)
            {
                return true;
            }

            var it1 = param1.Begin;
            var it2 = param2.Begin;

            while (it1 != null && it2 != null)
            {
                equal(it1.Value, it2.Value, $"{name} Node");
                it1 = it1.Next;
                it2 = it2.Next;
            }

            return false;
        };

        equal(model1.ProjectFilePath, model2.ProjectFilePath, "ProjectFilePath");
        equal(model1.IsTriplet, model2.IsTriplet, "IsTriplet");
        equal(model1.QuantizeValue, model2.QuantizeValue, "QuantizeValue");
        equal(model1.IsNumerialKeyName, model2.IsNumerialKeyName, "IsNumerialKeyName");
        equal(model1.FirstNumerialKeyNameAtIndex, model2.FirstNumerialKeyNameAtIndex, "FirstNumerialKeyNameAtIndex");

        equal(model1.TempoList.Count, model2.TempoList.Count, "Tempo Count");
        equal(model1.BeatList.Count, model2.BeatList.Count, "Beat Count");
        equal(model1.TrackList.Count, model2.TrackList.Count, "Track Count");

        for (int i = 0; i < model1.TempoList.Count; ++i)
        {
            var tempo1 = model1.TempoList[i];
            var tempo2 = model2.TempoList[i];
            equal(tempo1.Pos, tempo2.Pos, "Tempo Pos");
            equal(tempo1.Tempo, tempo2.Tempo, "Tempo");
            equal(tempo1.Overlaped, tempo2.Overlaped, "Tempo Overlapped");
        }

        for (int i = 0; i < model1.BeatList.Count; ++i)
        {
            var beat1 = model1.BeatList[i];
            var beat2 = model2.BeatList[i];
            equal(beat1.BarIndex, beat2.BarIndex, "Beat BarIndex");
            equal(beat1.BeatSize.X, beat2.BeatSize.X, "Beat BeatSize X");
            equal(beat1.BeatSize.Y, beat2.BeatSize.Y, "Beat BeatSize Y");
            equal(beat1.Overlaped, beat2.Overlaped, "Beat Overlapped");
        }

        for (int i = 0; i < model1.TrackList.Count; ++i)
        {
            var track1 = model1.TrackList[i];
            var track2 = model2.TrackList[i];
            equal(track1.GetType(), track2.GetType(), "Track type");

            equal(track1.Volume, track2.Volume, "Volume");
            equal(track1.Pan, track2.Pan, "Pan");
            equal(track1.Name, track2.Name, "Name");
            equal(track1.Mute, track2.Mute, "Mute");
            equal(track1.Solo, track2.Solo, "Solo");

            if (track1 is SingingTrack)
            {
                var singingTrack1 = (SingingTrack)track1;
                var singingTrack2 = (SingingTrack)track2;

                equal(singingTrack1.AISingerId, singingTrack2.AISingerId, "AISingerId");
                equal(singingTrack1.NeedRefreshBaseMetadataFlag, singingTrack2.NeedRefreshBaseMetadataFlag,
                    "NeedRefreshBaseMetadataFlag");
                equal(singingTrack1.ReverbPreset, singingTrack2.ReverbPreset, "ReverbPreset");

                // Console.WriteLine(i);
                // traverseLineParam(singingTrack1.EditedPowerLine);
                // Console.WriteLine(i);
                // traverseLineParam(singingTrack2.EditedPowerLine);

                compareLineParam(singingTrack1.EditedPitchLine, singingTrack2.EditedPitchLine, "EditedPitchLine");
                compareLineParam(singingTrack1.EditedVolumeLine, singingTrack2.EditedVolumeLine, "EditedVolumeLine");
                compareLineParam(singingTrack1.EditedBreathLine, singingTrack2.EditedBreathLine, "EditedBreathLine");
                compareLineParam(singingTrack1.EditedGenderLine, singingTrack2.EditedGenderLine, "EditedGenderLine");
                compareLineParam(singingTrack1.EditedPowerLine, singingTrack2.EditedPowerLine, "EditedPowerLine");

                equal(singingTrack1.NoteList.Count, singingTrack2.NoteList.Count, "NoteList Count");

                for (int j = 0; j < singingTrack1.NoteList.Count; ++j)
                {
                    var note1 = singingTrack1.NoteList[i];
                    var note2 = singingTrack2.NoteList[i];

                    equal(note1.StartPos, note2.StartPos, "StartPos");
                    equal(note1.WidthPos, note2.WidthPos, "WidthPos");
                    equal(note1.KeyIndex, note2.KeyIndex, "KeyIndex");
                    equal(note1.Lyric, note2.Lyric, "Lyric");
                    equal(note1.Pronouncing, note2.Pronouncing, "Pronouncing");
                    equal(note1.HeadTag, note2.HeadTag, "HeadTag");

                    equal(note1.NotePhoneInfo == null, note2.NotePhoneInfo == null, "NotePhoneInfo");
                    equal(note1.VibratoPercentInfo == null, note2.VibratoPercentInfo == null, "VibratoPercentInfo");
                    equal(note1.Vibrato == null, note2.Vibrato == null, "Vibrato Style");

                    // Console.WriteLine($"{j} {note1.Vibrato == null} {note2.Vibrato == null}");
                    
                    if (note1.NotePhoneInfo != null)
                    {
                        var info1 = note1.NotePhoneInfo;
                        var info2 = note2.NotePhoneInfo;
                        equal(info1.HeadPhoneTimeInSec, info2.HeadPhoneTimeInSec, "HeadPhoneTimeInSec");
                        equal(info1.MidPartOverTailPartRatio, info2.MidPartOverTailPartRatio,
                            "MidPartOverTailPartRatio");
                    }

                    if (note1.VibratoPercentInfo != null)
                    {
                        var info1 = note1.VibratoPercentInfo;
                        var info2 = note2.VibratoPercentInfo;
                        equal(info1.StartPercent, info2.StartPercent, "StartPercent");
                        equal(info1.EndPercent, info2.EndPercent, "EndPercent");
                    }

                    if (note1.Vibrato != null)
                    {
                        var vibrato1 = note1.Vibrato;
                        var vibrato2 = note2.Vibrato;
                        equal(vibrato1.IsAntiPhase, vibrato2.IsAntiPhase, "IsAntiPhase");
                        compareLineParam(vibrato1.AmpLine, vibrato2.AmpLine, "AmpLine");
                        compareLineParam(vibrato1.FreqLine, vibrato2.FreqLine, "FreqLine");
                    }
                }
            }
            else
            {
                var instrumentTrack1 = (InstrumentTrack)track1;
                var instrumentTrack2 = (InstrumentTrack)track2;

                equal(instrumentTrack1.SampleRate, instrumentTrack2.SampleRate, "SampleRate");
                equal(instrumentTrack1.SampleCount, instrumentTrack2.SampleCount, "SampleCount");
                equal(instrumentTrack1.ChannelCount, instrumentTrack2.ChannelCount, "ChannelCount");
                equal(instrumentTrack1.OffsetInPos, instrumentTrack2.OffsetInPos, "OffsetInPos");
                equal(instrumentTrack1.InstrumentFilePath, instrumentTrack2.InstrumentFilePath, "InstrumentFilePath");
            }
        }

        return 0;
    }

    private static bool equal<T>(T a, T b, string name)
    {
        if (!a.Equals(b))
        {
            throw new Exception($"Not match {name} of {a.GetType()}, {a} {b}");
        }

        return true;
    }

    private static AppModel? ReadModel(string path)
    {
        var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        var reader = new BinaryReader(stream);
        var version = reader.ReadString();
        var versionNumber = reader.ReadString();
        version += versionNumber;
        AppModel? model;
        // if (version != "0.0.0" && new Version(versionNumber) < new Version("2.0.0"))
        // {
        stream.Close();
        Console.WriteLine("Use Project Model Manager");
        model = ProjectModelFileMgr.ReadModelFile(path, out _, out _);
        // }
        // else
        // {
        // model = (AppModel)new BinaryFormatter().Deserialize(stream);
        // stream.Close();
        // }

        return model;
    }
}