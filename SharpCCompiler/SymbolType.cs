using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCCompiler
{
    abstract class SymbolType
    {
        public static readonly string Keyword = "keyword";
        public static readonly string Identifier = "identifier";
        public static readonly string Operator = "operator";
        public static readonly string IntConst = "integer constant";
        public static readonly string StringConst = "string constant";
        public static readonly string IncomStringConst = "incomplete string constant";
        public static readonly string Boundary = "boundary symbol";
        public static readonly string SingleLineComment = "single line comment";
        public static readonly string Break = "break";
        public static readonly string Unknown = "unknown type";
    }
}
