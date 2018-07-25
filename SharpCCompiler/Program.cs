using System;

namespace SharpCCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Tester.TestParser();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
