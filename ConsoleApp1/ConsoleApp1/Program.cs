// See https://aka.ms/new-console-template for more information

using System.Runtime.InteropServices;

namespace ConsoleApp1
{
    public static unsafe class Program
    {
        private static Bar* pBar;

        public static void fun1()
        {
            var bar = MyLib.newBar(1, 2);

            Console.WriteLine("[1]");
            Console.WriteLine($"Bar Addr: {(int)bar}");
            Console.WriteLine($"Foo Val2: {MyLib.getFooProp(bar)}");
            Console.WriteLine($"Bar Val: {bar->val}");
            Console.WriteLine($"Foo Addr: {(int)bar->foo}");
            Console.WriteLine($"Foo Val1: {bar->foo->a}");

            pBar = bar;
        }

        public static void fun2()
        {
            var bar = pBar;

            Console.WriteLine("[2]");
            Console.WriteLine($"Bar Addr: {(int)bar}");
            Console.WriteLine($"Foo Val2: {MyLib.getFooProp(bar)}");
            Console.WriteLine($"Bar Val: {bar->val}");
            Console.WriteLine($"Foo Addr: {(int)bar->foo}");
            Console.WriteLine($"Foo Val1: {bar->foo->a}");
        }

        static int Main(String[] args)
        {
            fun1();

            fun2();

            Console.WriteLine("Hello, World!");

            int i = Console.Read();
            Console.WriteLine(i + 1);
            
            
            Console.WriteLine("FreeLibrary");

            i = Console.Read();
            Console.WriteLine(i + 1);

            return 0;
        }
    }
}