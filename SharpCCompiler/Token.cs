using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCCompiler
{
    class Token
    {
        private readonly int line;
        private readonly string type;
        private readonly string value;
        private readonly string raw;

        public Token(int line, string type, string value, string raw)
        {
            this.line = line;
            this.type = type;
            this.value = value;
            this.raw = raw;
        }

        public int Line => line;

        public string Type => type;

        public string Value => value;

        public string Raw => raw;
    }
}
