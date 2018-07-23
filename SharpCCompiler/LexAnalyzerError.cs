using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCCompiler
{
    class LexAnalyzerError : Exception
    {
        public static readonly string End = "end sign";
        public static readonly string IncomCharConst = "incomplete character constant";
        public static readonly string IncomStringConst = "incomplete string constant";
        public static readonly string Unknown = "unknown type";

        public static readonly string IntConst = "invalid integer constant";
        public static readonly string CharConst = "invalid character constant";
        public static readonly string StringConst = "invalid string constant";

        private readonly int line;
        private readonly string info;
        private readonly string value;

        public LexAnalyzerError(int line, string info, string value)
        {
            this.line = line;
            this.info = info;
            this.value = value;
        }

        public int Line => line;

        public string Info => info;

        public string Value => value;
    }
}
