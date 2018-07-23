using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCCompiler
{
    class ParserError : Exception
    {
        private readonly int line;
        private readonly string info;

        public int Line => line;

        public string Info => info;

        public ParserError(int line, string info)
        {
            this.line = line;
            this.info = info;
        }
    }
}
