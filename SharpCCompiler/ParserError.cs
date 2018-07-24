using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SharpCCompiler
{
    class ParserError : Exception
    {
        public static readonly string UnrecognizedStatement = "unrecognized statement";

        public static readonly string InvalidIfStatement = "invalid if statement";
        public static readonly string InvalidWhileStatement = "invalid while statement";
        public static readonly string InvalidInputStatement = "invalid input statement";
        public static readonly string InvalidOutputStatement = "invalid output statement";
        public static readonly string InvalidCompoundStatement = "invalid compound statement";

        public static readonly string InvalidVariableDeclaration = "invalid variable declaration";

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
