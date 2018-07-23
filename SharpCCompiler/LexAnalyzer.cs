using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SharpCCompiler
{
    class LexAnalyzer
    {
        private delegate int Match(string lineRemain);

        // Keyword set.
        private static readonly List<string> KeywordList = new List<string>();
        // Boundary sign set.
        private static readonly List<string> BoundaryList = new List<string>();
        // Operator set.
        private static readonly List<string> OperatorList = new List<string>();
        // Break set.
        private static readonly List<char> BreakElemList = new List<char>();
        // Match set.
        private static readonly List<Tuple<Match, string>> MatchList = new List<Tuple<Match, string>>();

        private List<Word> wordList = new List<Word>();
        private List<Token> tokenList = new List<Token>();
        private List<LexAnalyzerError> lexAnalyzerErrorList = new List<LexAnalyzerError>();
        private int wordCount = 0;
        private int tokenCount = 0;
        private int lexAnalyzerErrorCount = 0;
        private bool hasError = false;

        public bool HasError => hasError;

        static LexAnalyzer()
        {
            KeywordList.Add("int");
            KeywordList.Add("char");
            KeywordList.Add("if");
            KeywordList.Add("else");
            KeywordList.Add("while");
            KeywordList.Add("input");
            KeywordList.Add("output");

            BoundaryList.Add("(");
            BoundaryList.Add(")"); 
            BoundaryList.Add("{");
            BoundaryList.Add("}");
            BoundaryList.Add(";");

            OperatorList.Add("==");
            OperatorList.Add("!=");
            OperatorList.Add("+");
            OperatorList.Add("-");
            OperatorList.Add(">");
            OperatorList.Add("<");
            OperatorList.Add("=");

            BreakElemList.Add(' ');
            BreakElemList.Add('\t');

            MatchList.Add(new Tuple<Match, string>(MatchBreak, SymbolType.Break));
            MatchList.Add(new Tuple<Match, string>(MatchSingleLineComment, SymbolType.SingleLineComment));
            MatchList.Add(new Tuple<Match, string>(MatchKeyword, SymbolType.Keyword));
            MatchList.Add(new Tuple<Match, string>(MatchIdentifier, SymbolType.Identifier));
            MatchList.Add(new Tuple<Match, string>(MatchCharConst, SymbolType.CharConst));
            MatchList.Add(new Tuple<Match, string>(MatchStringConst, SymbolType.StringConst));
            MatchList.Add(new Tuple<Match, string>(MatchIntConst, SymbolType.IntConst));
            MatchList.Add(new Tuple<Match, string>(MatchBoundary, SymbolType.Boundary));
            MatchList.Add(new Tuple<Match, string>(MatchOperator, SymbolType.Operator));
            MatchList.Add(new Tuple<Match, string>(MatchIncomCharConstant, SymbolType.IncomCharConst));
            MatchList.Add(new Tuple<Match, string>(MatchIncomStringConst, SymbolType.IncomStringConst));
        }

        public LexAnalyzer(string source)
        {
            Console.WriteLine("Lexical analyzer started!");
            int line = 0;
            string[] splitSource = source.Split('\n');
            foreach (var s in splitSource)
            {
                line++;
                AnalyzeLine(s, line);
            }

            ParseWordList();
            Console.WriteLine("Finished analyzing!");
            if (lexAnalyzerErrorCount > 0)
            {
                hasError = true;
                Printer.PrintLexAnalyzerErrorList(ref lexAnalyzerErrorList);
            }

            if (wordCount > 0)
            {
                Printer.PrintWordList(ref wordList);
            }

            if (tokenCount > 0)
            {
                Printer.PrintTokenList(ref tokenList);
            }
        }

        private void ParseWordList()
        {
            foreach (var word in wordList)
            {
                if (!word.IsValid || word.Type == SymbolType.Break)
                {
                    continue;
                }

                string value;
                try
                {
                    if (word.Type.Equals(SymbolType.IntConst))
                    {
                        value = ParseIntConst(word.Value, word.Line);
                    }
                    else if (word.Type.Equals(SymbolType.CharConst))
                    {
                        value = ParseCharConst(word.Value, word.Line);
                    }
                    else if (word.Type.Equals(SymbolType.StringConst))
                    {
                        value = ParseStringConst(word.Value, word.Line);
                    }
                    else
                    {
                        value = word.Value;
                    }
                }
                catch (LexAnalyzerError e)
                {
                    hasError = true;
                    lexAnalyzerErrorCount++;
                    lexAnalyzerErrorList.Add(e);
                    continue;
                }

                tokenCount++;
                tokenList.Add(new Token(word.Line, word.Type, value, word.Value));
            }
        }

        private static string ParseIntConst(string value, int line)
        {
            if (Int32.TryParse(value, out int _))
            {
                return value;
            }

            throw new LexAnalyzerError(line, LexAnalyzerError.IntConst, value);
        }

        private static string ParseCharConst(string value, int line)
        {
            if (value.Length == 3)
            {
                return value[1].ToString();
            }
            else if (value.Length == 4 && value[1] == '\\')
            {
                switch (value[2])
                {
                    case '\'': return "\'";
                    case 't': return "\t";
                    case 'n': return "\n";
                    case 'r': return "\r";
                    case 'b': return "\b";
                }
            }

            throw new LexAnalyzerError(line, LexAnalyzerError.CharConst, value);
        }

        private static string ParseStringConst(string value, int line)
        {
            StringBuilder stringBuilder = new StringBuilder();
            bool needEscape = false;
            foreach (var c in value.Substring(1, value.Length - 2))
            {
                if (c == '\\')
                {
                    needEscape = true;
                }
                else
                {
                    char charToAdd;
                    if (needEscape)
                    {
                        switch (c)
                        {
                            case '"': charToAdd = '"'; break;
                            case 't': charToAdd = '\t'; break;
                            case 'n': charToAdd = '\n'; break;
                            case 'r': charToAdd = '\r'; break;
                            case 'b': charToAdd = '\b'; break;
                            default: throw new LexAnalyzerError(line, LexAnalyzerError.StringConst, value);
                        }
                    }
                    else
                    {
                        charToAdd = c;
                    }

                    needEscape = false;
                    stringBuilder.Append(charToAdd);
                }
            }

            return stringBuilder.ToString();
        }

        private void AnalyzeLine(string lineStr, int line)
        {
            int begin = 0, length = lineStr.Length;
            while (begin < length)
            {
                Tuple<int, string> matchResult = MatchAll(lineStr.Substring(begin));
                string value = lineStr.Substring(begin, matchResult.Item1);
                bool isValid = true;
                try
                {
                    if (matchResult.Item2.Equals(SymbolType.End))
                    {
                        lexAnalyzerErrorCount++;
                        throw new LexAnalyzerError(line, LexAnalyzerError.End, value);
                    }
                    else if (matchResult.Item2.Equals(SymbolType.IncomCharConst))
                    {
                        lexAnalyzerErrorCount++;
                        throw new LexAnalyzerError(line, LexAnalyzerError.IncomCharConst, value);
                    }
                    else if (matchResult.Item2.Equals(SymbolType.IncomStringConst))
                    {
                        lexAnalyzerErrorCount++;
                        throw new LexAnalyzerError(line, LexAnalyzerError.IncomStringConst, value);
                    }
                    else if (matchResult.Item2.Equals(SymbolType.Unknown))
                    {
                        lexAnalyzerErrorCount++;
                        throw new LexAnalyzerError(line, SymbolType.IncomStringConst, value);
                    }
                }
                catch (LexAnalyzerError e)
                {
                    lexAnalyzerErrorList.Add(e);
                    isValid = false;
                }
                finally
                {
                    wordCount++;
                    wordList.Add(new Word(line, matchResult.Item2, value, isValid));
                    begin += matchResult.Item1;
                }
            }
        }

        public List<Token> RetrieveTokenList()
        {
            return tokenList;
        }

        private static Tuple<int, string> MatchAll(string lineRemain)
        {
            foreach (var match in MatchList)
            {
                int count = match.Item1(lineRemain);
                if (count > 0)
                {
                    return new Tuple<int, string>(count, match.Item2);
                }
            }
            return new Tuple<int, string>(lineRemain.Length, SymbolType.Unknown);
        }

        private static int MatchKeyword(string lineRemain)
        {
            if (!(char.IsLetter(lineRemain.First()) || lineRemain.First() == '_'))
            {
                return 0;
            }

            int count = 1;
            foreach (var c in lineRemain.Substring(1))
            {
                if (char.IsLetter(c) || c == '_' || char.IsDigit(c))
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            return KeywordList.Contains(lineRemain.Substring(0, count)) ? count : 0;
        }

        private static int MatchIdentifier(string lineRemain)
        {
            if (!(char.IsLetter(lineRemain.First()) || lineRemain.First() == '_'))
            {
                return 0;
            }

            int count = 1;
            foreach (var c in lineRemain.Substring(1))
            {
                if (char.IsLetter(c) || c == '_' || char.IsDigit(c))
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            return KeywordList.Contains(lineRemain.Substring(0, count)) ? 0 : count;
        }

        private static int MatchOperator(string lineRemain)
        {
            foreach (var str in OperatorList)
            {
                if (lineRemain.StartsWith(str))
                {
                    return str.Length;
                }
            }

            return 0;
        }

        private static int MatchIntConst(string lineRemain)
        {
            int count = 0;
            foreach (var c in lineRemain)
            {
                if (char.IsDigit(c))
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            return count;
        }

        private static int MatchCharConst(string lineRemain)
        {
            if (lineRemain.First() != '\'')
            {
                return 0;
            }

            int count = 1;
            bool flag = true;
            foreach (var c in lineRemain.Substring(1))
            {
                count++;
                if (c == '\\')
                {
                    flag = false;
                }
                else
                {
                    if (c == '\'' && flag)
                    {
                        return count;
                    }
                    flag = true;
                }
            }

            return 0;
        }

        private static int MatchIncomCharConstant(string lineRemain)
        {
            if (lineRemain.First() != '\'')
            {
                return 0;
            }

            int count = 1;
            bool flag = true;
            foreach (var c in lineRemain.Substring(1))
            {
                count++;
                if (c == '\\')
                {
                    flag = false;
                }
                else
                {
                    if (c == '\'' && flag)
                    {
                        return 0;
                    }
                    flag = true;
                }
            }

            return count;
        }

        private static int MatchStringConst(string lineRemain)
        {
            if (lineRemain.First() != '"')
            {
                return 0;
            }

            int count = 1;
            bool flag = true;
            foreach (var c in lineRemain.Substring(1))
            {
                count++;
                if (c == '\\')
                {
                    flag = false;
                }
                else
                {
                    if (c == '"' && flag)
                    {
                        return count;
                    }
                    flag = true;
                }
            }

            return 0;
        }

        private static int MatchIncomStringConst(string lineRemain)
        {
            if (lineRemain.First() != '"')
            {
                return 0;
            }

            int count = 1;
            bool flag = true;
            foreach (var c in lineRemain.Substring(1))
            {
                count++;
                if (c == '\\')
                {
                    flag = false;
                }
                else
                {
                    if (c == '"' && flag)
                    {
                        return 0;
                    }
                    flag = true;
                }
            }

            return count;
        }

        private static int MatchBoundary(string lineRemain)
        {
            foreach (var str in BoundaryList)
            {
                if (lineRemain.StartsWith(str))
                {
                    return str.Length;
                }
            }

            return 0;
        }

        private static int MatchSingleLineComment(string lineRemain)
        {
            return lineRemain.StartsWith("//") ? lineRemain.Length : 0;
        }

        private static int MatchBreak(string lineRemain)
        {
            int count = 0;
            foreach (var c in lineRemain)
            {
                if (BreakElemList.Contains(c))
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            return count;
        }

        public static void Test()
        {
            Console.WriteLine("Lexical analyzer test started!");
            string source =
                "// This is a comment.\n" +
                "int integer;\n" +
                "output(\"Hello! Please enter an integer: \");\n" +
                "input(integer);\n" +
                "if(integer > 100)\n" +
                "\tinteger = integer + 1;\n" +
                "else\n" +
                "\tinteger = integer - 1;\n" +
                "output(\"The result of this integer plus 1 is: \");\n" +
                "output(integer);\n" +
                "output(\".\\n\");\n";
            Console.WriteLine("Source code is:");
            Console.WriteLine(source);
            LexAnalyzer lexAnalyzer = new LexAnalyzer(source);
            Console.WriteLine("Lexical analyzer test finished!");
        }
    }
}
