namespace XSAppModel.NrbfFormat;

public class NrbfStream : IDisposable
{
    private bool _disposed = false;

    public NrbfStream()
    {
        // Load library
        NrbfLibrary.Load();

        _impl = new NrbfStreamImpl();
    }

    ~NrbfStream()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            // Unload library
            NrbfLibrary.Unload();

            _disposed = true;
        }
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