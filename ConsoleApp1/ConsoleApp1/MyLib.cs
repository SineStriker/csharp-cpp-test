using System.Runtime.InteropServices;

namespace ConsoleApp1;

public unsafe struct Foo
{
    public int a;
}

public unsafe struct Bar
{
    public int val;
    public Foo* foo;
}

public unsafe static class MyLib
{
    [DllImport("MyLib.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern Bar* newBar(int a, int b);

    [DllImport("MyLib.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int getFooProp(Bar* bar);
}