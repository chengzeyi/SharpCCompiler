using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCCompiler
{
    class SyntaxType
    {
        public static readonly string Program = "program";

        public static readonly string Statement = "statement";
        public static readonly string CompoundStatement = "compound statement";
        public static readonly string NullStatement = "null statement";
        public static readonly string IfStatement = "if statement";
        public static readonly string WhileStatement = "while statement";
        public static readonly string InputStatement = "input statement";
        public static readonly string OutputStatement = "output statement";
        public static readonly string ExpressionStatement = "expression statement";
        public static readonly string DeclarationStatement = "declaration statement";

        public static readonly string Expression = "expression";
        public static readonly string AssignmentExpression = "assignment expression";
        public static readonly string AddExpression = "add expression";
        public static readonly string SubtractionExprssion = "subtraction expression";
        public static readonly string LessThanExpression = "less than expression";
        public static readonly string GreaterThanExpression = "greater than expression";
        public static readonly string EqualExpression = "equal expression";
        public static readonly string NotEqualExpression = "not equal expression";

        public static readonly string Declaration = "declaration";
        public static readonly string VariableDeclaration = "variable declaration";

        public static readonly string AssignmentOperator = "assignment operator";
        public static readonly string AddOperator = "add operator";
        public static readonly string SubtractionOperator = "subtraction operator";
        public static readonly string LessThanOperator = "less than operator";
        public static readonly string GreaterThanOperator = "greater than operator";
        public static readonly string EqualOperator = "equal operator";
        public static readonly string NotEqualOperator = "not equal operator";

        public static readonly string IntConst = "integer constant";
        public static readonly string StringConst = "string constant";
        public static readonly string Identifier = "identifier";
        public static readonly string TypeSpecifier = "type specifier";

        public static readonly string IntSpecifier = "integer specifier";
    }
}
