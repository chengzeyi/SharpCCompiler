using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCCompiler
{
    class Printer
    {
        public static void PrintWordList(ref List<Word> wordList)
        {
            Console.WriteLine("Started to print word list!");
            foreach (var word in wordList)
            {
                Console.WriteLine(string.Format("Line: {0}, IsValid: {1}, Type: {2}, Value: {3}", word.Line, word.IsValid, word.Type, word.Value));
            }
            Console.WriteLine("Finished printing word list!");
        }

        public static void PrintTokenList(ref List<Token> tokenList)
        {
            Console.WriteLine("Started to print token list!");
            foreach (var token in tokenList)
            {
                Console.WriteLine(string.Format("Line: {0}, type: {1}, raw: {2}", token.Line, token.Type, token.Raw));

            }
            Console.WriteLine("Finished printing token list!");
        }

        public static void PrintLexAnalyzerErrorList(ref List<LexAnalyzerError> lexAnalyzerError)
        {
            Console.WriteLine("Started to print lexical analyzer error list!");
            foreach (var textError in lexAnalyzerError)
            {
                Console.WriteLine(string.Format("Line: {0}, Info: {1}, Value: {2}", textError.Line, textError.Info, textError.Value));
            }
            Console.WriteLine("Finished printing lexical analyzer error list!");
        }
    }
}
