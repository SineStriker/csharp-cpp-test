namespace XSAppModel.NrbfFormat;

public class NrbfStream
{
    public NrbfStream()
    {
        _impl = new NrbfStreamImpl();
    }

    public XStudio.AppModel? Read(byte[] data)
    {
        return _impl.Read(data);
    }

    public byte[] Write(XStudio.AppModel appModel)
    {
        return _impl.Write(appModel);
    }

    public void Reset()
    {
        _impl.ErrorMessage = "";
        _impl.Status = StatusType.Ok;
    }

    public enum StatusType
    {
        Ok,
        ReadPastEnd,
        ReadCorruptData,
        WriteFailed,
    }

    public string ErrorMessage => _impl.ErrorMessage;

    public StatusType Status => _impl.Status;

    private NrbfStreamImpl _impl;
}