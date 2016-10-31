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
        private bool _debug;

        public RuntimeVisitor(bool debug)
        {
            _debug = debug;
        }
        
        private void DebugLine(string s1, object arg0 = null, object arg1=null, object arg2 = null)
        {
            if (_debug)
                Console.WriteLine(s1, arg0, arg1, arg2);
        }

        private void Debug(string s1, object arg0 = null, object arg1 = null, object arg2 = null)
        {
            if (_debug)
                Console.Write(s1, arg0, arg1, arg2);
        }
        public override MyAbstractNode VisitProgram([NotNull] MyLangV4Parser.ProgramContext program)
        {
            DebugLine("VisitProgram: {0}", program.GetText());
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
            DebugLine("VisitFuncdecl: {0}", funcdecl.GetText());
            string funcName = funcdecl.funcname.Text;
            var paramz = funcdecl._params.Select(x => x.Text).ToList();
            MyFunctionDecleration myFuncDecl = new MyFunctionDecleration(funcName, paramz);
            myFuncDecl.Statements = VisitBlockstatement(funcdecl.blockstatement()) as MyStatementListNode;
            return myFuncDecl;
        }

        public override MyAbstractNode VisitStatement([NotNull] MyLangV4Parser.StatementContext stmt)
        {
            DebugLine("VisitStatement: {0}", stmt.GetText());

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
            //    DebugLine("ShouldVisitNextChild: node={0}, currentResult={1}, b={2}", node.GetText(), currentResult, !b);
            //    return false;
            //}

            //DebugLine("ShouldVisitNextChild: node={0}, currentResult={1}, b={2}", node.GetText(), currentResult, b);
            return b;
        }

        public override MyAbstractNode VisitBlockstatement([NotNull] MyLangV4Parser.BlockstatementContext context)
        {
            DebugLine("VisitBlockstatement: {0}", context.GetText()); 

            var statementListNode = new MyStatementListNode();

            //DebugLine("VisitBlockstatement 1: " + context.GetText());
            //DebugLine("VisitBlockstatement 2: " + context.statement());

            int i = 0;

            foreach (MyLangV4Parser.StatementContext s in context.statement())
            {
                DebugLine("$$$: {0}, {1}", i, s.GetText());

                MyStatementNode stmt = VisitStatement(s) as MyStatementNode;
                statementListNode.AddStatement(stmt);

                i++;
            }

            return statementListNode;
        }

        public override MyAbstractNode VisitIfstatement([NotNull] MyLangV4Parser.IfstatementContext ifStmt)
        {
            DebugLine("VisitIfstatement {0}", ifStmt.GetText());
            MyAbstractNode condition = Visit(ifStmt.condition);

            MyStatementListNode thenPart = VisitBlockstatement(ifStmt.thenpart) as MyStatementListNode;
            MyStatementListNode elsePart = ifStmt.elsepart != null ? VisitBlockstatement(ifStmt.elsepart) as MyStatementListNode : null;

            return new MyIfStatement(condition, thenPart, elsePart);
        }

        public override MyAbstractNode VisitPrintstatement([NotNull] MyLangV4Parser.PrintstatementContext context)
        {
            DebugLine("VisitPrintstatement {0}", context.GetText());

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
            DebugLine("VisitWhilestatement {0}", whileStmt.GetText());
            MyAbstractNode condition = Visit(whileStmt.condition);
            MyStatementListNode thenPart = VisitBlockstatement(whileStmt.dopart) as MyStatementListNode;
            return new MyWhileStatement(condition, thenPart);
        }

        public override MyAbstractNode VisitAssignment([NotNull] MyLangV4Parser.AssignmentContext assignStmt)
        {
            DebugLine("VisitAssignment {0}", assignStmt.GetText());
            MyVariableNode myVar = new MyVariableNode(assignStmt.IDENTIFIER().GetText());

            MyAbstractNode expr = Visit(assignStmt.expression());

            DebugLine("VisitAssignment myVar={0}, expr={1}, expression()={2}", myVar, expr, assignStmt.expression().GetText());

            var assignmentNode = new MyAssignmentNode(myVar, expr); ;

            return assignmentNode;
        }


        public override MyAbstractNode VisitReturnstatement([NotNull] MyLangV4Parser.ReturnstatementContext returnStmt)
        {
            DebugLine("VisitReturnstatement {0}", returnStmt.GetText());
            MyAbstractNode expr = Visit(returnStmt.expression());
            return new MyReturnStatement(expr); 
        }


        public override MyAbstractNode VisitFunccall([NotNull] MyLangV4Parser.FunccallContext funcCall)
        {
            DebugLine("VisitFunccall: {0}", funcCall.GetText());
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
            DebugLine("VisitEquals: {0}", equals.GetText());
            MyAbstractNode leftExprNode = Visit(equals.left);
            MyAbstractNode rightExprNode = Visit(equals.right);

            MyIsEqualsNode equalsNode = new MyIsEqualsNode(leftExprNode, rightExprNode);
            return equalsNode;
        }

        public override MyAbstractNode VisitGt([NotNull] MyLangV4Parser.GtContext gt)
        {
            DebugLine("VisitGt: {0}", gt.GetText());
            MyAbstractNode leftExprNode = Visit(gt.left);
            MyAbstractNode rightExprNode = Visit(gt.right);

            MyIsGreaterThanNode gtNode = new MyIsGreaterThanNode(leftExprNode, rightExprNode);
            return gtNode;
        }

        public override MyAbstractNode VisitGteq([NotNull] MyLangV4Parser.GteqContext gteq)
        {
            DebugLine("VisitGteq: {0}", gteq.GetText());
            MyAbstractNode leftExprNode = Visit(gteq.left);
            MyAbstractNode rightExprNode = Visit(gteq.right);

            MyIsGreaterThanOrEqualNode gteqNode = new MyIsGreaterThanOrEqualNode(leftExprNode, rightExprNode);
            return gteqNode;
        }

        public override MyAbstractNode VisitLt([NotNull] MyLangV4Parser.LtContext lt)
        {
            DebugLine("VisitLt: {0}", lt.GetText());
            MyAbstractNode leftExprNode = Visit(lt.left);
            MyAbstractNode rightExprNode = Visit(lt.right);

            MyIsLessThanNode ltNode = new MyIsLessThanNode(leftExprNode, rightExprNode);
            return ltNode;
        }

        public override MyAbstractNode VisitLteq([NotNull] MyLangV4Parser.LteqContext lteq)
        {
            DebugLine("VisitLteq: {0}", lteq.GetText());
            MyAbstractNode leftExprNode = Visit(lteq.left);
            MyAbstractNode rightExprNode = Visit(lteq.right);

            MyIsLessThanOrEqualNode lteqNode = new MyIsLessThanOrEqualNode(leftExprNode, rightExprNode);
            return lteqNode;
        }

        public override MyAbstractNode VisitMinus([NotNull] MyLangV4Parser.MinusContext minus)
        {
            DebugLine("VisitMinus: {0}", minus.GetText());
            MyAbstractNode leftExprNode = Visit(minus.left);
            MyAbstractNode rightExprNode = Visit(minus.right);

            MyMinusNode minusNode = new MyMinusNode(leftExprNode, rightExprNode);
            return minusNode;
        }

        public override MyAbstractNode VisitAnd([NotNull] MyLangV4Parser.AndContext and)
        {
            DebugLine("VisitAnd: {0}", and.GetText());
            MyAbstractNode leftExprNode = Visit(and.left);
            MyAbstractNode rightExprNode = Visit(and.right);

            MyLogicalAndNode andNode = new MyLogicalAndNode(leftExprNode, rightExprNode);
            return andNode;
        }

        public override MyAbstractNode VisitDivide([NotNull] MyLangV4Parser.DivideContext divide)
        {
            DebugLine("VisitDivide: {0}, left={1}, right={2}", divide.GetText(), divide.left.GetText(), divide.right.GetText());

            Debug("VisitDivide left:");
            MyAbstractNode leftExprNode = Visit(divide.left);

            Debug("VisitDivide right:");
            MyAbstractNode rightExprNode = Visit(divide.right);

            MyDivideNode divideNode = new MyDivideNode(leftExprNode, rightExprNode);
            return divideNode;
        }

        public override MyAbstractNode VisitMultiply([NotNull] MyLangV4Parser.MultiplyContext multiply)
        {
            DebugLine("VisitMultiply: {0}, left={1}, right={2}", multiply.GetText(), multiply.left.GetText(), multiply.right.GetText());

            Debug("VisitMultiply left:");
            MyAbstractNode leftExprNode = Visit(multiply.left);

            Debug("VisitMultiply right:");
            MyAbstractNode rightExprNode = Visit(multiply.right);

            MyMultiplyNode multiplyNode = new MyMultiplyNode(leftExprNode, rightExprNode);
            return multiplyNode;
        }

        public override MyAbstractNode VisitNegate([NotNull] MyLangV4Parser.NegateContext negate)
        {
            DebugLine("VisitNegate: {0}", negate.GetText());
            MyAbstractNode opNode = Visit(negate.negateexpression().expression());
            MyNegateNode negateNode = new MyNegateNode(opNode);
            return negateNode;
        }

        public override MyAbstractNode VisitInt([NotNull] MyLangV4Parser.IntContext number)
        {
            DebugLine("VisitInt: {0}", number.GetText());
            int i = Int32.Parse(number.GetText());
            MyValueNode intNode = new MyValueNode(i);
            return intNode;
        }

        public override MyAbstractNode VisitString([NotNull] MyLangV4Parser.StringContext str)
        {
            DebugLine("VisitString: {0}", str.GetText());
            MyStringLiteralNode strExpr = new MyStringLiteralNode(str.GetText());
            return strExpr;
        }


        public override MyAbstractNode VisitFloat([NotNull] MyLangV4Parser.FloatContext number)
        {
            DebugLine("VisitFloat: {0}", number.GetText());
            double d = Double.Parse(number.GetText());
            MyValueNode floatNode = new MyValueNode(d);
            return floatNode;
        }

        public override MyAbstractNode VisitPlus([NotNull] MyLangV4Parser.PlusContext plus)
        {
            DebugLine("VisitPlus: {0}, left={1}, right={2}", plus.GetText(), plus.left.GetText(), plus.right.GetText());

            MyAbstractNode leftExprNode = Visit(plus.left);
            MyAbstractNode rightExprNode = Visit(plus.right);

            MyAddNode addNode = new MyAddNode(leftExprNode, rightExprNode);
            return addNode;
        }

        public override MyAbstractNode VisitPosate([NotNull] MyLangV4Parser.PosateContext posate)
        {
            DebugLine("VisitPosate: {0}", posate.GetText());
            return Visit(posate.posateexpression().expression());
        }

        public override MyAbstractNode VisitParenexpr([NotNull] MyLangV4Parser.ParenexprContext context)
        {
            DebugLine("VisitParenexpr: {0}", context.GetText());
            return VisitParenexpression(context.parenexpression());
        }

        public override MyAbstractNode VisitParenexpression([NotNull] MyLangV4Parser.ParenexpressionContext context)
        {
            DebugLine("VisitParenexpression: {0}", context.GetText());
            return Visit(context.expression());
        }


        public override MyAbstractNode VisitOr([NotNull] MyLangV4Parser.OrContext or)
        {
            DebugLine("VisitOr: {0}", or.GetText());
            MyAbstractNode leftExprNode = Visit(or.left);
            MyAbstractNode rightExprNode = Visit(or.right);

            MyLogicalOrNode orNode = new MyLogicalOrNode(leftExprNode, rightExprNode);
            return orNode;
        }


        public override MyAbstractNode VisitVarcall([NotNull] MyLangV4Parser.VarcallContext varcall)
        {
            DebugLine("VisitVarcall: {0}", varcall.GetText());
            MyVariableNode varNode = new MyVariableNode(varcall.GetText());
            return varNode;
        }


        public override MyAbstractNode VisitFuncallexpr([NotNull] MyLangV4Parser.FuncallexprContext context)
        {
            DebugLine("VisitFuncallexpr: {0}", context.GetText());
            return VisitFunccall(context.funccall());
        }

        public override MyAbstractNode VisitTerminal(ITerminalNode node)
        {
            DebugLine("VisitTerminal: {0}", node.GetText());
            return base.VisitTerminal(node);
        }

        public override MyAbstractNode VisitExpression([NotNull] MyLangV4Parser.ExpressionContext context)
        {
            DebugLine("VisitExpression: {0}", context.GetText());
            return base.VisitExpression(context);
        }
    }
}
