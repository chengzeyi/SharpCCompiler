using System;

namespace SharpCCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            LexAnalyzer.Test();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }
    }
}
