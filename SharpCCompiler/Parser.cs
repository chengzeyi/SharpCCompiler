using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SharpCCompiler
{
    class Parser
    {
        private List<Token> tokenList = new List<Token>();
        private int nodeCount = 0;
        private int textErrorCount = 0;
        private bool hasError = false;

        public bool HasError => hasError;

        private void ParseTokenList()
        {
            using (List<Token>.Enumerator enumerator = tokenList.GetEnumerator())
            {

            }
        }

        private bool JudgeAndAssign(out SyntaxTreeNode destination, SyntaxTreeNode source)
        {
            return (destination = source) != null;
        }

        private bool MatchLeftBrace(ref List<Token>.Enumerator enumerator)
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

        private bool MatchRightBrace(ref List<Token>.Enumerator enumerator)
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

        private bool MatchLeftParenthese(ref List<Token>.Enumerator enumerator)
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

        private bool MatchRightParenthese(ref List<Token>.Enumerator enumerator)
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

        private bool MatchSemicolon(ref List<Token>.Enumerator enumerator)
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

        private bool MatchIf(ref List<Token>.Enumerator enumerator)
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

        private bool MatchElse(ref List<Token>.Enumerator enumerator)
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

        private bool MatchWhile(ref List<Token>.Enumerator enumerator)
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

        private bool MatchInput(ref List<Token>.Enumerator enumerator)
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

        private bool MatchOutput(ref List<Token>.Enumerator enumerator)
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

        private SyntaxTreeNode CreateIdentifier(ref List<Token>.Enumerator enumerator)
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

        private SyntaxTreeNode CreateIntConst(ref List<Token>.Enumerator enumerator)
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

        private SyntaxTreeNode CreateStringConst(ref List<Token>.Enumerator enumerator)
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

        private SyntaxTreeNode CreateAssignmentOperator(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Operator) && enumerator.Current.Value.Equals("="))
            {
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.AssignmentOperator, enumerator.Current.Value);
            }
            else
            {
                enumerator = initialEnumerator;
                return null;
            }
        }

        private SyntaxTreeNode CreateAddOperator(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Operator) && enumerator.Current.Value.Equals("+"))
            {
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.AddOperator, enumerator.Current.Value);
            }
            else
            {
                enumerator = initialEnumerator;
                return null;
            }
        }

        private SyntaxTreeNode CreateSubtractionOperator(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Operator) && enumerator.Current.Value.Equals("-"))
            {
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.SubtractionOperator, enumerator.Current.Value);
            }
            else
            {
                enumerator = initialEnumerator;
                return null;
            }
        }

        private SyntaxTreeNode CreateLessThanOperator(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Operator) && enumerator.Current.Value.Equals("<"))
            {
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.LessThanOperator, enumerator.Current.Value);
            }
            else
            {
                enumerator = initialEnumerator;
                return null;
            }
        }

        private SyntaxTreeNode CreateGreaterThanOperator(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Operator) && enumerator.Current.Value.Equals(">"))
            {
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.GreaterThanOperator, enumerator.Current.Value);
            }
            else
            {
                enumerator = initialEnumerator;
                return null;
            }
        }

        private SyntaxTreeNode CreateEqualOperator(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Operator) && enumerator.Current.Value.Equals("=="))
            {
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.EqualOperator, enumerator.Current.Value);
            }
            else
            {
                enumerator = initialEnumerator;
                return null;
            }
        }

        private SyntaxTreeNode CreateNotEqualOperator(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            if (enumerator.MoveNext() && enumerator.Current.Type.Equals(SymbolType.Operator) && enumerator.Current.Value.Equals("!="))
            {
                return new SyntaxTreeNode(enumerator.Current.Line, SyntaxType.NotEqualOperator, enumerator.Current.Value);
            }
            else
            {
                enumerator = initialEnumerator;
                return null;
            }
        }

        private SyntaxTreeNode CreateAddExpression(ref List<Token>.Enumerator enumerator)
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

        private SyntaxTreeNode CreateSubtractionExpression(ref List<Token>.Enumerator enumerator)
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

        private SyntaxTreeNode CreateLessThanExpression(ref List<Token>.Enumerator enumerator)
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

        private SyntaxTreeNode CreateGreaterThanExpression(ref List<Token>.Enumerator enumerator)
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

        private SyntaxTreeNode CreateEqualExpression(ref List<Token>.Enumerator enumerator)
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

        private SyntaxTreeNode CreateNotEqualExpression(ref List<Token>.Enumerator enumerator)
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

        private SyntaxTreeNode CreateAssignmentExpression(ref List<Token>.Enumerator enumerator)
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

        private SyntaxTreeNode CreateExpression(ref List<Token>.Enumerator enumerator)
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

        private SyntaxTreeNode CreateExpressionStatement(ref List<Token>.Enumerator enumerator)
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

        private SyntaxTreeNode CreateIfStatement(ref List<Token>.Enumerator enumerator)
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
            }

            enumerator = initialEnumerator;
            return null;
        }

        private SyntaxTreeNode CreateWhileStatement(ref List<Token>.Enumerator enumerator)
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
            }

            enumerator = initialEnumerator;
            return null;
        }

        private SyntaxTreeNode CreateInputStatement(ref List<Token>.Enumerator enumerator)
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
            }

            enumerator = initialEnumerator;
            return null;
        }

        private SyntaxTreeNode CreateOutputStatement(ref List<Token>.Enumerator enumerator)
        {
            List<Token>.Enumerator initialEnumerator = enumerator;
            List<SyntaxTreeNode> childList = new List<SyntaxTreeNode>();
            SyntaxTreeNode node;
            if (MatchInput(ref enumerator))
            {
                if (MatchLeftParenthese(ref enumerator))
                {
                    if (JudgeAndAssign(out node, CreateIdentifier(ref enumerator)) || JudgeAndAssign(out node, CreateStringConst(ref enumerator)))
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
            }

            enumerator = initialEnumerator;
            return null;
        }
    }
}
