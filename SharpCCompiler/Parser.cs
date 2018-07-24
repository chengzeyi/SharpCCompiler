using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SharpCCompiler
{
    class Parser
    {
        private List<Token> tokenList;
        private SyntaxTreeNode root = null;
        private ParserError parserError = null;
        private bool hasError = false;
        public bool HasError => hasError;

        public static void Test()
        {
            Console.WriteLine("Parser test started!");
            string source =
                "// This is a comment.\n" +
                "int integer;\n" +
                "int count;\n" +
                "count = 10;\n" +
                "output(\"Hello! Please enter an integer: \");\n" +
                "input(integer);\n" +
                "if(integer > 100)\n" +
                "\tinteger = integer + 1;\n" +
                "else\n" +
                "\tinteger = integer - 1;\n" +
                "while(count > 0)\n" +
                "{\n" +
                "\tinteger = integer + 2;\n" +
                "\tcount = count - 1;\n" +
                "}\n" +
                "output(\"The result is: \");\n" +
                "output(integer);\n" +
                "output(\".\\n\");\n";
            Console.WriteLine("Source code is:");
            Console.WriteLine(source);
            LexAnalyzer lexAnalyzer = new LexAnalyzer(source);
            if (!lexAnalyzer.HasError)
            {
                Parser parser = new Parser(lexAnalyzer.RetrieveTokenList());
            }
            Console.WriteLine("Parser test finished!");
        }

        public Parser(List<Token> tokenList)
        {
            this.tokenList = tokenList;
            ParseTokenList();
            if (hasError)
            {
                Printer.PrintParserError(parserError);
            }

            if (root != null)
            {
                Printer.PrintSyntaxTree(root);
            }
        }

        public SyntaxTreeNode RetrieveSyntaxTree()
        {
            return root;
        }

        private void ParseTokenList()
        {
            List<Token>.Enumerator enumerator = tokenList.GetEnumerator();
            {
                try
                {
                    root = CreateProgram(ref enumerator);
                }
                catch (ParserError e)
                {
                    parserError = e;
                    hasError = true;
                }
            }
        }

        private static bool JudgeAndAssign(out SyntaxTreeNode destination, SyntaxTreeNode source)
        {
            return (destination = source) != null;
        }

        private static bool MatchLeftBrace(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Boundary) && enumerator.Current.Value.Equals("{"))
            {
                return true;
            }
            else
            {
                enumerator = initialEnumerator;
                return false;
            }
        }

        private static bool MatchRightBrace(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Boundary) && enumerator.Current.Value.Equals("}"))
            {
                return true;
            }
            else
            {
                enumerator = initialEnumerator;
                return false;
            }
        }

        private static bool MatchLeftParenthese(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Boundary) && enumerator.Current.Value.Equals("("))
            {
                return true;
            }
            else
            {
                enumerator = initialEnumerator;
                return false;
            }
        }

        private static bool MatchRightParenthese(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Boundary) && enumerator.Current.Value.Equals(")"))
            {
                return true;
            }
            else
            {
                enumerator = initialEnumerator;
                return false;
            }
        }

        private static bool MatchSemicolon(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Boundary) && enumerator.Current.Value.Equals(";"))
            {
                return true;
            }
            else
            {
                enumerator = initialEnumerator;
                return false;
            }
        }

        private static bool MatchIf(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Keyword) && enumerator.Current.Value.Equals("if"))
            {
                return true;
            }
            else
            {
                enumerator = initialEnumerator;
                return false;
            }
        }

        private static bool MatchElse(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Keyword) && enumerator.Current.Value.Equals("else"))
            {
                return true;
            }
            else
            {
                enumerator = initialEnumerator;
                return false;
            }
        }

        private static bool MatchWhile(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Keyword) && enumerator.Current.Value.Equals("while"))
            {
                return true;
            }
            else
            {
                enumerator = initialEnumerator;
                return false;
            }
        }

        private static bool MatchInput(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Keyword) && enumerator.Current.Value.Equals("input"))
            {
                return true;
            }
            else
            {
                enumerator = initialEnumerator;
                return false;
            }
        }

        private static bool MatchOutput(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Keyword) && enumerator.Current.Value.Equals("output"))
            {
                return true;
            }
            else
            {
                enumerator = initialEnumerator;
                return false;
            }
        }

        private static SyntaxTreeNode CreateIntSpecifier(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Keyword) && enumerator.Current.Value.Equals("int"))
            {
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.IntSpecifier, enumerator.Current.Value);
            }
            else
            {
                enumerator = initialEnumerator;
                return null;
            }
        }

        private static SyntaxTreeNode CreateTypeSpecifier(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            if (JudgeAndAssign(out node, CreateIntSpecifier(ref enumerator)))
            {
                childList.Add(node);
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.TypeSpecifier) { ChildList = childList };
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateIdentifier(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Identifier))
            {
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.Identifier, enumerator.Current.Value);
            }
            else
            {
                enumerator = initialEnumerator;
                return null;
            }
        }

        private static SyntaxTreeNode CreateIntConst(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.IntConst))
            {
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.IntConst, enumerator.Current.Value);
            }
            else
            {
                enumerator = initialEnumerator;
                return null;
            }
        }

        private static SyntaxTreeNode CreateStringConst(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.StringConst))
            {
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.StringConst, enumerator.Current.Value);
            }
            else
            {
                enumerator = initialEnumerator;
                return null;
            }
        }

        private static SyntaxTreeNode CreateAssignmentOperator(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Operator) && enumerator.Current.Value.Equals("="))
            {
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.AssignmentOperator);
            }
            else
            {
                enumerator = initialEnumerator;
                return null;
            }
        }

        private static SyntaxTreeNode CreateAddOperator(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Operator) && enumerator.Current.Value.Equals("+"))
            {
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.AddOperator);
            }
            else
            {
                enumerator = initialEnumerator;
                return null;
            }
        }

        private static SyntaxTreeNode CreateSubtractionOperator(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Operator))
            {
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.SubtractionOperator, enumerator.Current.Value);
            }
            else
            {
                enumerator = initialEnumerator;
                return null;
            }
        }

        private static SyntaxTreeNode CreateLessThanOperator(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Operator))
            {
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.LessThanOperator, enumerator.Current.Value);
            }
            else
            {
                enumerator = initialEnumerator;
                return null;
            }
        }

        private static SyntaxTreeNode CreateGreaterThanOperator(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Operator))
            {
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.GreaterThanOperator, enumerator.Current.Value);
            }
            else
            {
                enumerator = initialEnumerator;
                return null;
            }
        }

        private static SyntaxTreeNode CreateEqualOperator(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Operator))
            {
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.EqualOperator, enumerator.Current.Value);
            }
            else
            {
                enumerator = initialEnumerator;
                return null;
            }
        }

        private static SyntaxTreeNode CreateNotEqualOperator(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Operator))
            {
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.NotEqualOperator, enumerator.Current.Value);
            }
            else
            {
                enumerator = initialEnumerator;
                return null;
            }
        }

        private static SyntaxTreeNode CreateAddExpression(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            if (JudgeAndAssign(out node, CreateIntConst(ref enumerator)) || JudgeAndAssign(out node, CreateIdentifier(ref enumerator)))
            {
                childList.Add(node);
                if (JudgeAndAssign(out node, CreateAddOperator(ref enumerator)))
                {
                    childList.Add(node);
                    if (JudgeAndAssign(out node, CreateIntConst(ref enumerator)) || JudgeAndAssign(out node, CreateIdentifier(ref enumerator)))
                    {
                        childList.Add(node);
                        return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.AddExpression){ChildList = childList};
                    }
                }
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateSubtractionExpression(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            if (JudgeAndAssign(out node, CreateIntConst(ref enumerator)) || JudgeAndAssign(out node, CreateIdentifier(ref enumerator)))
            {
                childList.Add(node);
                if (JudgeAndAssign(out node, CreateSubtractionOperator(ref enumerator)))
                {
                    childList.Add(node);
                    if (JudgeAndAssign(out node, CreateIntConst(ref enumerator)) || JudgeAndAssign(out node, CreateIdentifier(ref enumerator)))
                    {
                        childList.Add(node);
                        return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.SubtractionExprssion) { ChildList = childList };
                    }
                }
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateLessThanExpression(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            if (JudgeAndAssign(out node, CreateIntConst(ref enumerator)) || JudgeAndAssign(out node, CreateIdentifier(ref enumerator)))
            {
                childList.Add(node);
                if (JudgeAndAssign(out node, CreateLessThanOperator(ref enumerator)))
                {
                    childList.Add(node);
                    if (JudgeAndAssign(out node, CreateIntConst(ref enumerator)) || JudgeAndAssign(out node, CreateIdentifier(ref enumerator)))
                    {
                        childList.Add(node);
                        return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.LessThanExpression) { ChildList = childList };
                    }
                }
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateGreaterThanExpression(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            if (JudgeAndAssign(out node, CreateIntConst(ref enumerator)) || JudgeAndAssign(out node, CreateIdentifier(ref enumerator)))
            {
                childList.Add(node);
                if (JudgeAndAssign(out node, CreateGreaterThanOperator(ref enumerator)))
                {
                    childList.Add(node);
                    if (JudgeAndAssign(out node, CreateIntConst(ref enumerator)) || JudgeAndAssign(out node, CreateIdentifier(ref enumerator)))
                    {
                        childList.Add(node);
                        return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.GreaterThanExpression) { ChildList = childList };
                    }
                }
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateEqualExpression(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            if (JudgeAndAssign(out node, CreateIntConst(ref enumerator)) || JudgeAndAssign(out node, CreateIdentifier(ref enumerator)))
            {
                childList.Add(node);
                if (JudgeAndAssign(out node, CreateEqualOperator(ref enumerator)))
                {
                    childList.Add(node);
                    if (JudgeAndAssign(out node, CreateIntConst(ref enumerator)) || JudgeAndAssign(out node, CreateIdentifier(ref enumerator)))
                    {
                        childList.Add(node);
                        return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.EqualExpression) { ChildList = childList };
                    }
                }
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateNotEqualExpression(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            if (JudgeAndAssign(out node, CreateIntConst(ref enumerator)) || JudgeAndAssign(out node, CreateIdentifier(ref enumerator)))
            {
                childList.Add(node);
                if (JudgeAndAssign(out node, CreateNotEqualOperator(ref enumerator)))
                {
                    childList.Add(node);
                    if (JudgeAndAssign(out node, CreateIntConst(ref enumerator)) || JudgeAndAssign(out node, CreateIdentifier(ref enumerator)))
                    {
                        childList.Add(node);
                        return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.NotEqualExpression) { ChildList = childList };
                    }
                }
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateAssignmentExpression(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            if (JudgeAndAssign(out node, CreateIdentifier(ref enumerator)))
            {
                childList.Add(node);
                if (JudgeAndAssign(out node, CreateAssignmentOperator(ref enumerator)))
                {
                    childList.Add(node);
                    if (JudgeAndAssign(out node, CreateExpression(ref enumerator)) || JudgeAndAssign(out node, CreateIntConst(ref enumerator)) || JudgeAndAssign(out node, CreateIdentifier(ref enumerator)))
                    {
                        childList.Add(node);
                        return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.AssignmentExpression) { ChildList = childList };
                    }
                }
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateExpression(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            if (JudgeAndAssign(out node, CreateAssignmentExpression(ref enumerator)) || JudgeAndAssign(out node, CreateAddExpression(ref enumerator)) || JudgeAndAssign(out node, CreateSubtractionExpression(ref enumerator)) || JudgeAndAssign(out node, CreateLessThanExpression(ref enumerator)) || JudgeAndAssign(out node, CreateGreaterThanExpression(ref enumerator)) || JudgeAndAssign(out node, CreateEqualExpression(ref enumerator)) || JudgeAndAssign(out node, CreateNotEqualExpression(ref enumerator)) || JudgeAndAssign(out node, CreateIntConst(ref enumerator)) || JudgeAndAssign(out node, CreateIdentifier(ref enumerator)))
            {
                childList.Add(node);
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.Expression){ChildList = childList};
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateVariableDeclaration(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            if (JudgeAndAssign(out node, CreateTypeSpecifier(ref enumerator)))
            {
                childList.Add(node);
                if (JudgeAndAssign(out node, CreateIdentifier(ref enumerator)))
                {
                    childList.Add(node);
                    return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.VariableDeclaration){ChildList = childList};
                }
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateDeclaration(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            if (JudgeAndAssign(out node, CreateVariableDeclaration(ref enumerator)))
            {
                childList.Add(node);
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.Declaration) {ChildList = childList};
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateExpressionStatement(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            if (JudgeAndAssign(out node, CreateExpression(ref enumerator)))
            {
                childList.Add(node);
                if (MatchSemicolon(ref enumerator))
                {
                    return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.ExpressionStatement){ChildList = childList};
                }
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateNullStatement(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            if (MatchSemicolon(ref enumerator))
            {
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.NullStatement);
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateIfStatement(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            bool ifPassed = false;
            if (MatchIf(ref enumerator))
            {
                if (MatchLeftParenthese(ref enumerator))
                {
                    if (JudgeAndAssign(out node, CreateExpression(ref enumerator)))
                    {
                        childList.Add(node);
                        if (MatchRightParenthese(ref enumerator))
                        {
                            if (JudgeAndAssign(out node, CreateStatement(ref enumerator)))
                            {
                                childList.Add(node);
                                ifPassed = true;
                            }
                        }
                    }
                }

                if (!ifPassed)
                {
                    throw new ParserError(enumerator.Current.Line, ParserError.InvalidIfStatement);
                }
            }

            if (ifPassed)
            {
                if (MatchElse(ref enumerator))
                {
                    if (JudgeAndAssign(out node, CreateStatement(ref enumerator)))
                    {
                        childList.Add(node);
                        return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.IfStatement){ChildList = childList};
                    }
                }
                else
                {
                    return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.IfStatement){ChildList = childList};
                }
                throw new ParserError(enumerator.Current.Line, ParserError.InvalidIfStatement);
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateWhileStatement(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            if (MatchWhile(ref enumerator))
            {
                if (MatchLeftParenthese(ref enumerator))
                {
                    if (JudgeAndAssign(out node, CreateExpression(ref enumerator)))
                    {
                        childList.Add(node);
                        if (MatchRightParenthese(ref enumerator))
                        {
                            if (JudgeAndAssign(out node, CreateStatement(ref enumerator)))
                            {
                                childList.Add(node);
                                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.WhileStatement){ChildList = childList};
                            }
                        }
                    }
                }
                throw new ParserError(enumerator.Current.Line, ParserError.InvalidWhileStatement);
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateInputStatement(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            if (MatchInput(ref enumerator))
            {
                if (MatchLeftParenthese(ref enumerator))
                {
                    if (JudgeAndAssign(out node, CreateIdentifier(ref enumerator)))
                    {
                        childList.Add(node);
                        if (MatchRightParenthese(ref enumerator))
                        {
                            if (MatchSemicolon(ref enumerator))
                            {
                                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.InputStatement){ChildList = childList};
                            }
                        }
                    }
                }
                throw new ParserError(enumerator.Current.Line, ParserError.InvalidInputStatement);
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateOutputStatement(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            if (MatchOutput(ref enumerator))
            {
                if (MatchLeftParenthese(ref enumerator))
                {
                    if (JudgeAndAssign(out node, CreateIdentifier(ref enumerator)) || JudgeAndAssign(out node, CreateStringConst(ref enumerator)) || JudgeAndAssign(out node, CreateIntConst(ref enumerator)))
                    {
                        childList.Add(node);
                        if (MatchRightParenthese(ref enumerator))
                        {
                            if (MatchSemicolon(ref enumerator))
                            {
                                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.OutputStatement) { ChildList = childList };
                            }
                        }
                    }
                }
                throw new ParserError(enumerator.Current.Line, ParserError.InvalidOutputStatement);
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateDeclarationStatement(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            if (JudgeAndAssign(out node, CreateDeclaration(ref enumerator)))
            {
                childList.Add(node);
                if (MatchSemicolon(ref enumerator))
                {
                    return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.DeclarationStatement) { ChildList = childList };
                }
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateStatement(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            if (JudgeAndAssign(out node, CreateNullStatement(ref enumerator)) || JudgeAndAssign(out node, CreateExpressionStatement(ref enumerator)) || JudgeAndAssign(out node, CreateIfStatement(ref enumerator)) || JudgeAndAssign(out node, CreateWhileStatement(ref enumerator)) || JudgeAndAssign(out node, CreateInputStatement(ref enumerator)) || JudgeAndAssign(out node, CreateOutputStatement(ref enumerator)) || JudgeAndAssign(out node, CreateDeclarationStatement(ref enumerator)) || JudgeAndAssign(out node, CreateCompoundStatement(ref enumerator)))
            {
                childList.Add(node);
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.Statement) {ChildList = childList};
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateCompoundStatement(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            if (MatchLeftBrace(ref enumerator))
            {
                while (JudgeAndAssign(out node, CreateStatement(ref enumerator)))
                {
                    childList.Add(node);
                }

                if (MatchRightBrace(ref enumerator))
                {
                    return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.CompoundStatement){ChildList = childList};
                }
                throw new ParserError(enumerator.Current.Line, ParserError.InvalidCompoundStatement);
            }

            enumerator = initialEnumerator;
            return null;
        }

        private static SyntaxTreeNode CreateProgram(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            while (JudgeAndAssign(out node, CreateStatement(ref enumerator)))
            {
                childList.Add(node);
            }

            List<Token>.Enumerator tmpEnumerator = enumerator;
            if (enumerator.MoveNext())
            {
                throw new ParserError(enumerator.Current.Line, ParserError.UnrecognizedStatement);
                enumerator = initialEnumerator;
                return null;
            }
            else
            {
                return new SyntaxTreeNode(tmpEnumerator.Current.Line, SyntaxType.Program){ChildList = childList};
            }
        }

/*        private static string ParseCharConst(string value, int line)
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
        }*/
    }
}
