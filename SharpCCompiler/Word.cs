using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCCompiler
{
    class Word
    {
        private readonly string value;
        private readonly string type;
        private readonly int line;
        private readonly bool isValid;

        public string Value => value;

        public string Type => type;

        public int Line => line;

        public bool IsValid => isValid;

        public Word(int line, string type, string value, bool isValid)
        {
            this.value = value;
            this.type = type;
            this.line = line;
            this.isValid = isValid;
        }
    }
}