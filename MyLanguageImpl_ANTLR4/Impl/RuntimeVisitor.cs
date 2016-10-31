using Antlr4.Runtime.Misc;
using MyLanguageGrammar_ANTLR4.Grammar;
using MyLanguageImpl.Runtime;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Tree;
using System;

namespace MyLanguageImpl_ANTLR4.Impl
{
    public class RuntimeVisitor : MyLangV4ParserBaseVisitor<MyAbstractNode>
    {
        public override MyAbstractNode VisitProgram([NotNull] MyLangV4Parser.ProgramContext program)
        {
            Console.WriteLine("VisitProgram: {0}", program.GetText());
            string programName = program.progname.Text;

            List<MyFunctionDecleration> funcs = new List<MyFunctionDecleration>();
            foreach (MyLangV4Parser.FuncdeclContext f in program.funcdecl())
            {
                MyFunctionDecleration myFuncDecl = VisitFuncdecl(f) as MyFunctionDecleration;
                if (myFuncDecl != null)
                {
                    funcs.Add(myFuncDecl);
                }
            }

            MyLangV4Parser.BlockstatementContext bsc = program.blockstatement();
            MyStatementListNode bs = VisitBlockstatement(bsc) as MyStatementListNode;

            return new MyProgramDeclNode(programName, funcs, bs);
        }


        public override MyAbstractNode VisitFuncdecl([NotNull] MyLangV4Parser.FuncdeclContext funcdecl)
        {
            Console.WriteLine("VisitFuncdecl: {0}", funcdecl.GetText());
            string funcName = funcdecl.funcname.Text;
            var paramz = funcdecl._params.Select(x => x.Text).ToList();
            MyFunctionDecleration myFuncDecl = new MyFunctionDecleration(funcName, paramz);
            myFuncDecl.Statements = VisitBlockstatement(funcdecl.blockstatement()) as MyStatementListNode;
            return myFuncDecl;
        }

        public override MyAbstractNode VisitStatement([NotNull] MyLangV4Parser.StatementContext stmt)
        {
            Console.WriteLine("VisitStatement: {0}", stmt.GetText());

            MyStatementListNode s = null;

            if (stmt.assignment() != null)
            {
                return VisitAssignment(stmt.assignment());
            }

            if (stmt.returnstatement() != null)
            {
                return VisitReturnstatement(stmt.returnstatement());
            }

            if (stmt.blockstatement() != null)
            {
                return VisitBlockstatement(stmt.blockstatement());
            }

            if (stmt.ifstatement() != null)
            {
                return VisitIfstatement(stmt.ifstatement());
            }

            if (stmt.whilestatement() != null)
            {
                return VisitWhilestatement(stmt.whilestatement());
            }

            if (stmt.printstatement() != null)
            {
                return VisitPrintstatement(stmt.printstatement());
            }

            return s;
        }

        protected override bool ShouldVisitNextChild(IRuleNode node, MyAbstractNode currentResult)
        {
            bool b = base.ShouldVisitNextChild(node, currentResult);
            //if (currentResult != null)
            //{
            //    Console.WriteLine("ShouldVisitNextChild: node={0}, currentResult={1}, b={2}", node.GetText(), currentResult, !b);
            //    return false;
            //}

            //Console.WriteLine("ShouldVisitNextChild: node={0}, currentResult={1}, b={2}", node.GetText(), currentResult, b);
            return b;
        }

        public override MyAbstractNode VisitBlockstatement([NotNull] MyLangV4Parser.BlockstatementContext context)
        {
            Console.WriteLine("VisitBlockstatement: {0}", context.GetText()); 

            var statementListNode = new MyStatementListNode();

            //Console.WriteLine("VisitBlockstatement 1: " + context.GetText());
            //Console.WriteLine("VisitBlockstatement 2: " + context.statement());

            int i = 0;

            foreach (MyLangV4Parser.StatementContext s in context.statement())
            {
                Console.WriteLine("$$$: {0}, {1}", i, s.GetText());

                MyStatementNode stmt = VisitStatement(s) as MyStatementNode;
                statementListNode.AddStatement(stmt);

                i++;
            }

            return statementListNode;
        }

        public override MyAbstractNode VisitIfstatement([NotNull] MyLangV4Parser.IfstatementContext ifStmt)
        {
            Console.WriteLine("VisitIfstatement {0}", ifStmt.GetText());
            MyAbstractNode condition = Visit(ifStmt.condition);

            MyStatementListNode thenPart = VisitBlockstatement(ifStmt.thenpart) as MyStatementListNode;
            MyStatementListNode elsePart = ifStmt.elsepart != null ? VisitBlockstatement(ifStmt.elsepart) as MyStatementListNode : null;

            return new MyIfStatement(condition, thenPart, elsePart);
        }

        public override MyAbstractNode VisitPrintstatement([NotNull] MyLangV4Parser.PrintstatementContext context)
        {
            Console.WriteLine("VisitPrintstatement {0}", context.GetText());

            MyAbstractNode expr = null;
            if (context.expression() != null)
                expr = Visit(context.expression());
            else
                expr = new MyStringLiteralNode(string.Empty); 

            MyPrintStatementNode printStmt = new MyPrintStatementNode(expr) as MyPrintStatementNode;

            if (context.EXCLAIM() != null)
            {
                printStmt.Exclaim = true;
            }
            return printStmt;
        }

        public override MyAbstractNode VisitWhilestatement([NotNull] MyLangV4Parser.WhilestatementContext whileStmt)
        {
            Console.WriteLine("VisitWhilestatement {0}", whileStmt.GetText());
            MyAbstractNode condition = Visit(whileStmt.condition);
            MyStatementListNode thenPart = VisitBlockstatement(whileStmt.dopart) as MyStatementListNode;
            return new MyWhileStatement(condition, thenPart);
        }

        public override MyAbstractNode VisitAssignment([NotNull] MyLangV4Parser.AssignmentContext assignStmt)
        {
            Console.WriteLine("VisitAssignment {0}", assignStmt.GetText());
            MyVariableNode myVar = new MyVariableNode(assignStmt.IDENTIFIER().GetText());

            MyAbstractNode expr = Visit(assignStmt.expression());

            Console.WriteLine("VisitAssignment myVar={0}, expr={1}, expression()={2}", myVar, expr, assignStmt.expression().GetText());

            var assignmentNode = new MyAssignmentNode(myVar, expr); ;

            return assignmentNode;
        }


        public override MyAbstractNode VisitReturnstatement([NotNull] MyLangV4Parser.ReturnstatementContext returnStmt)
        {
            Console.WriteLine("VisitReturnstatement {0}", returnStmt.GetText());
            MyAbstractNode expr = Visit(returnStmt.expression());
            return new MyReturnStatement(expr); 
        }


        public override MyAbstractNode VisitFunccall([NotNull] MyLangV4Parser.FunccallContext funcCall)
        {
            Console.WriteLine("VisitFunccall: {0}", funcCall.GetText());
            string funcName = funcCall.funcname.Text;

            List<MyAbstractNode> argNodes = new List<MyAbstractNode>();

            foreach (var arg in funcCall._args)
            {
                MyAbstractNode expr = Visit(arg);
                argNodes.Add(expr);
            }

            MyFunctionCallNode myFuncCall = new MyFunctionCallNode(funcName, argNodes);
            return myFuncCall;
        }

        public override MyAbstractNode VisitEquals([NotNull] MyLangV4Parser.EqualsContext equals)
        {
            Console.WriteLine("VisitEquals: {0}", equals.GetText());
            MyAbstractNode leftExprNode = Visit(equals.left);
            MyAbstractNode rightExprNode = Visit(equals.right);

            MyIsEqualsNode equalsNode = new MyIsEqualsNode(leftExprNode, rightExprNode);
            return equalsNode;
        }

        public override MyAbstractNode VisitGt([NotNull] MyLangV4Parser.GtContext gt)
        {
            Console.WriteLine("VisitGt: {0}", gt.GetText());
            MyAbstractNode leftExprNode = Visit(gt.left);
            MyAbstractNode rightExprNode = Visit(gt.right);

            MyIsGreaterThanNode gtNode = new MyIsGreaterThanNode(leftExprNode, rightExprNode);
            return gtNode;
        }

        public override MyAbstractNode VisitGteq([NotNull] MyLangV4Parser.GteqContext gteq)
        {
            Console.WriteLine("VisitGteq: {0}", gteq.GetText());
            MyAbstractNode leftExprNode = Visit(gteq.left);
            MyAbstractNode rightExprNode = Visit(gteq.right);

            MyIsGreaterThanOrEqualNode gteqNode = new MyIsGreaterThanOrEqualNode(leftExprNode, rightExprNode);
            return gteqNode;
        }

        public override MyAbstractNode VisitLt([NotNull] MyLangV4Parser.LtContext lt)
        {
            Console.WriteLine("VisitLt: {0}", lt.GetText());
            MyAbstractNode leftExprNode = Visit(lt.left);
            MyAbstractNode rightExprNode = Visit(lt.right);

            MyIsLessThanNode ltNode = new MyIsLessThanNode(leftExprNode, rightExprNode);
            return ltNode;
        }

        public override MyAbstractNode VisitLteq([NotNull] MyLangV4Parser.LteqContext lteq)
        {
            Console.WriteLine("VisitLteq: {0}", lteq.GetText());
            MyAbstractNode leftExprNode = Visit(lteq.left);
            MyAbstractNode rightExprNode = Visit(lteq.right);

            MyIsLessThanOrEqualNode lteqNode = new MyIsLessThanOrEqualNode(leftExprNode, rightExprNode);
            return lteqNode;
        }

        public override MyAbstractNode VisitMinus([NotNull] MyLangV4Parser.MinusContext minus)
        {
            Console.WriteLine("VisitMinus: {0}", minus.GetText());
            MyAbstractNode leftExprNode = Visit(minus.left);
            MyAbstractNode rightExprNode = Visit(minus.right);

            MyMinusNode minusNode = new MyMinusNode(leftExprNode, rightExprNode);
            return minusNode;
        }

        public override MyAbstractNode VisitAnd([NotNull] MyLangV4Parser.AndContext and)
        {
            Console.WriteLine("VisitAnd: {0}", and.GetText());
            MyAbstractNode leftExprNode = Visit(and.left);
            MyAbstractNode rightExprNode = Visit(and.right);

            MyLogicalAndNode andNode = new MyLogicalAndNode(leftExprNode, rightExprNode);
            return andNode;
        }

        public override MyAbstractNode VisitDivide([NotNull] MyLangV4Parser.DivideContext divide)
        {
            Console.WriteLine("VisitDivide: {0}, left={1}, right={2}", divide.GetText(), divide.left.GetText(), divide.right.GetText());

            Console.Write("VisitDivide left:");
            MyAbstractNode leftExprNode = Visit(divide.left);

            Console.Write("VisitDivide right:");
            MyAbstractNode rightExprNode = Visit(divide.right);

            MyDivideNode divideNode = new MyDivideNode(leftExprNode, rightExprNode);
            return divideNode;
        }

        public override MyAbstractNode VisitMultiply([NotNull] MyLangV4Parser.MultiplyContext multiply)
        {
            Console.WriteLine("VisitMultiply: {0}, left={1}, right={2}", multiply.GetText(), multiply.left.GetText(), multiply.right.GetText());

            Console.Write("VisitMultiply left:");
            MyAbstractNode leftExprNode = Visit(multiply.left);

            Console.Write("VisitMultiply right:");
            MyAbstractNode rightExprNode = Visit(multiply.right);

            MyMultiplyNode multiplyNode = new MyMultiplyNode(leftExprNode, rightExprNode);
            return multiplyNode;
        }

        public override MyAbstractNode VisitNegate([NotNull] MyLangV4Parser.NegateContext negate)
        {
            Console.WriteLine("VisitNegate: {0}", negate.GetText());
            MyAbstractNode opNode = Visit(negate.negateexpression().expression());
            MyNegateNode negateNode = new MyNegateNode(opNode);
            return negateNode;
        }

        public override MyAbstractNode VisitInt([NotNull] MyLangV4Parser.IntContext number)
        {
            Console.WriteLine("VisitInt: {0}", number.GetText());
            int i = Int32.Parse(number.GetText());
            MyValueNode intNode = new MyValueNode(i);
            return intNode;
        }

        public override MyAbstractNode VisitString([NotNull] MyLangV4Parser.StringContext str)
        {
            Console.WriteLine("VisitString: {0}", str.GetText());
            MyStringLiteralNode strExpr = new MyStringLiteralNode(str.GetText());
            return strExpr;
        }


        public override MyAbstractNode VisitFloat([NotNull] MyLangV4Parser.FloatContext number)
        {
            Console.WriteLine("VisitFloat: {0}", number.GetText());
            double d = Double.Parse(number.GetText());
            MyValueNode floatNode = new MyValueNode(d);
            return floatNode;
        }

        public override MyAbstractNode VisitPlus([NotNull] MyLangV4Parser.PlusContext plus)
        {
            Console.WriteLine("VisitPlus: {0}, left={1}, right={2}", plus.GetText(), plus.left.GetText(), plus.right.GetText());

            MyAbstractNode leftExprNode = Visit(plus.left);
            MyAbstractNode rightExprNode = Visit(plus.right);

            MyAddNode addNode = new MyAddNode(leftExprNode, rightExprNode);
            return addNode;
        }

        public override MyAbstractNode VisitPosate([NotNull] MyLangV4Parser.PosateContext posate)
        {
            Console.WriteLine("VisitPosate: {0}", posate.GetText());
            return Visit(posate.posateexpression().expression());
        }

        public override MyAbstractNode VisitParenexpr([NotNull] MyLangV4Parser.ParenexprContext context)
        {
            Console.WriteLine("VisitParenexpr: {0}", context.GetText());
            return VisitParenexpression(context.parenexpression());
        }

        public override MyAbstractNode VisitParenexpression([NotNull] MyLangV4Parser.ParenexpressionContext context)
        {
            Console.WriteLine("VisitParenexpression: {0}", context.GetText());
            return Visit(context.expression());
        }


        public override MyAbstractNode VisitOr([NotNull] MyLangV4Parser.OrContext or)
        {
            Console.WriteLine("VisitOr: {0}", or.GetText());
            MyAbstractNode leftExprNode = Visit(or.left);
            MyAbstractNode rightExprNode = Visit(or.right);

            MyLogicalOrNode orNode = new MyLogicalOrNode(leftExprNode, rightExprNode);
            return orNode;
        }


        public override MyAbstractNode VisitVarcall([NotNull] MyLangV4Parser.VarcallContext varcall)
        {
            Console.WriteLine("VisitVarcall: {0}", varcall.GetText());
            MyVariableNode varNode = new MyVariableNode(varcall.GetText());
            return varNode;
        }


        public override MyAbstractNode VisitFuncallexpr([NotNull] MyLangV4Parser.FuncallexprContext context)
        {
            Console.WriteLine("VisitFuncallexpr: {0}", context.GetText());
            return VisitFunccall(context.funccall());
        }

        public override MyAbstractNode VisitTerminal(ITerminalNode node)
        {
            Console.WriteLine("VisitTerminal: {0}", node.GetText());
            return base.VisitTerminal(node);
        }

        public override MyAbstractNode VisitExpression([NotNull] MyLangV4Parser.ExpressionContext context)
        {
            Console.WriteLine("VisitExpression: {0}", context.GetText());
            return base.VisitExpression(context);
        }
    }
}
