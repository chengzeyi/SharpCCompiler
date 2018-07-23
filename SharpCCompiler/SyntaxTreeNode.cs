using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCCompiler
{
    class SyntaxTreeNode
    {
        private readonly int line;
        private readonly string type;
        private readonly string value;
        private List<SyntaxTreeNode> childList = null;

        public SyntaxTreeNode(int line, string type, string value = null)
        {
            this.line = line;
            this.type = type;
            this.value = value;
        }

        public string Value => value;

        public int Line => line;

        public string Type => type;

        internal List<SyntaxTreeNode> ChildList { get => childList; set => childList = value; }
    }
}
