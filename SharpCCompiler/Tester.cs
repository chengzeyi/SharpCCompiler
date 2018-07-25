using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCCompiler
{
    class Tester
    {
        public static void TestLexAnalyzer()
        {
            Console.WriteLine("Lexical analyzer test started!");
            string source =
                "// This is a comment.\n" +
                "int integer;\n" +
                "int count;\n" +
                "count = 10;\n" +
                "output(\"Hello! Please enter an integer:\\n\");\n" +
                "input(integer);\n" +
                "if(integer > 100)\n" +
                "\tinteger = integer + 1;\n" +
                "else\n" +
                "\tinteger = integer - 1;\n" +
                "while(count > 0)\n" +
                "{\n" +
                "\tinteger = integer + 2;\n" +
                "\tcount = count - 1;\n" +
                "}\n" +
                "output(\"The result is: \");\n" +
                "output(integer);\n" +
                "output(\".\\n\");\n";
            Console.WriteLine("Source code is:");
            Console.WriteLine(source);
            LexAnalyzer lexAnalyzer = new LexAnalyzer(source);
            Console.WriteLine("Lexical analyzer test finished!");
        }

        public static void TestParser()
        {
            Console.WriteLine("Parser test started!");
            string source =
                "// This is a comment.\n" +
                "int integer;\n" +
                "int count;\n" +
                "count = 10;\n" +
                "output(\"Hello! Please enter an integer:\\n\");\n" +
                "input(integer);\n" +
                "if(integer > 100)\n" +
                "\tinteger = integer + 1;\n" +
                "else\n" +
                "\tinteger = integer - 1;\n" +
                "while(count > 0)\n" +
                "{\n" +
                "\tinteger = integer + 2;\n" +
                "\tcount = count - 1;\n" +
                "}\n" +
                "output(\"The result is: \");\n" +
                "output(integer);\n" +
                "output(\".\\n\");\n";
            Console.WriteLine("Source code is:");
            Console.WriteLine(source);
            LexAnalyzer lexAnalyzer = new LexAnalyzer(source);
            if (!lexAnalyzer.HasError)
            {
                Parser parser = new Parser(lexAnalyzer.RetrieveTokenList());
            }
            Console.WriteLine("Parser test finished!");
        }
    }
}
