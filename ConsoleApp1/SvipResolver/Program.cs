using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SvipResolver;

public static class Program
{
    static int Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine(
                $"Usage: {Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().ProcessName)} <svip file> [output file]");
            return 0;
        }

        // Open file
        var filename = args[0];
        if (!File.Exists(filename))
        {
            Console.WriteLine($"File \"{filename}\" not exists.");
            return -1;
        }

        // Load by data
        if (!QSvipLibrary.Load(File.ReadAllBytes(filename)))
        {
            Console.WriteLine(QSvipLibrary.ErrorMessage);
            return -1;
        }

        // Write json file
        string outputFilename = args.Length < 2 ? (Path.GetFileNameWithoutExtension(filename) + ".json") : args[1];
        File.WriteAllBytes(outputFilename, QSvipLibrary.OutputData);
        
        Console.WriteLine("Successfully convert binary svip to json.");
        return 0;
    }
}