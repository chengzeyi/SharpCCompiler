using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCCompiler
{
    class Printer
    {
        public static void PrintWordList(List<Word> wordList)
        {
            Console.WriteLine("Started to print word list!");
            foreach (var word in wordList)
            {
                Console.WriteLine("Line: {0}, IsValid: {1}, Type: {2}, Value: {3}", word.Line, word.IsValid, word.Type, word.Value);
            }
            Console.WriteLine("Finished printing word list!");
        }

        public static void PrintTokenList(List<Token> tokenList)
        {
            Console.WriteLine("Started to print token list!");
            foreach (var token in tokenList)
            {
                Console.WriteLine("Line: {0}, Type: {1}, Raw: {2}", token.Line, token.Type, token.Raw);

            }
            Console.WriteLine("Finished printing token list!");
        }

        public static void PrintLexAnalyzerErrorList(List<LexAnalyzerError> lexAnalyzerError)
        {
            Console.WriteLine("Started to print lexical analyzer error list!");
            foreach (var textError in lexAnalyzerError)
            {
                Console.WriteLine("Line: {0}, Info: {1}, Value: {2}", textError.Line, textError.Info, textError.Value);
            }
            Console.WriteLine("Finished printing lexical analyzer error list!");
        }

        private static void PrintSyntaxTree(SyntaxTreeNode root, int tabCount)
        {
            for (int i = 0; i < tabCount; i++)
            {
                Console.Write('\t');
            }
            Console.WriteLine("{0}", root.Type);
            if (root.ChildList != null)
            {
                foreach (var syntaxTreeNode in root.ChildList)
                {
                    PrintSyntaxTree(syntaxTreeNode, tabCount + 1);
                }
            }
        }

        public static void PrintSyntaxTree(SyntaxTreeNode root)
        {
            Console.WriteLine("Started to print syntax tree!");
            PrintSyntaxTree(root, 0);
            Console.WriteLine("Finished printing syntax tree!");
        }

        public static void PrintParserError(ParserError parserError)
        {
            Console.WriteLine("Started to print parser error!");
            Console.WriteLine("Line: {0}, Info: {1}", parserError.Line, parserError.Info);
            Console.WriteLine("Finished printing parser error!");
        }
    }
}
