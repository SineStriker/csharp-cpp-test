using System.Runtime.InteropServices;
using System.Text;

namespace SvipResolver;

public static class QSvipLibrary
{
    /* Wrapped functions */
    public static bool Load(byte[] data)
    {
        // Init
        var gch = GCHandle.Alloc(data, GCHandleType.Pinned);
        qsvip_reader_init(Marshal.UnsafeAddrOfPinnedArrayElement(data, 0), data.Length);
        gch.Free();

        // Load
        bool success = qsvip_reader_load();
        if (success)
        {
            // Get output
            int size = qsvip_reader_alloc_output();
            IntPtr ptr = qsvip_reader_get_output();

            byte[] buf = new byte[size];
            for (int i = 0; i < size; ++i)
            {
                unsafe
                {
                    buf[i] = *((byte*)ptr + i);
                }
            }

            OutputData = buf;
        }
        else
        {
            // Get error
            int size = qsvip_reader_alloc_error();
            IntPtr ptr = qsvip_reader_get_error();
            byte[] buf = new byte[size];
            for (int i = 0; i < size; ++i)
            {
                unsafe
                {
                    buf[i] = *((byte*)ptr + i);
                }
            }

            ErrorMessage = Encoding.GetEncoding("UTF-8").GetString(buf);
        }

        // Free
        qsvip_reader_free();

        return success;
    }

    public static void Reset()
    {
        ErrorMessage = string.Empty;
        OutputData = Array.Empty<byte>();
    }

    public static string ErrorMessage { get; private set; } = string.Empty;

    public static byte[] OutputData { get; private set; } = Array.Empty<byte>();

    /* Unmanaged functions */
    [DllImport("qsvip.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void qsvip_reader_init(IntPtr buf, int size);

    [DllImport("qsvip.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern void qsvip_reader_free();

    [DllImport("qsvip.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern bool qsvip_reader_load();

    [DllImport("qsvip.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int qsvip_reader_alloc_output();

    [DllImport("qsvip.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int qsvip_reader_alloc_error();

    [DllImport("qsvip.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr qsvip_reader_get_output();

    [DllImport("qsvip.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr qsvip_reader_get_error();

    // C++ source: https://github.com/SineStriker/QNrbf
}