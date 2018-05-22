using Antlr4.Runtime.Misc;
using MyLanguageGrammar_ANTLR4.Grammar;
using MyLanguageImpl.Runtime;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Tree;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace MyLanguageImpl_ANTLR4.Impl
{
    public class RuntimeVisitorDLR : MyLangV4ParserBaseVisitor<Expression>
    {
        private bool _debug;

        public RuntimeVisitorDLR(bool debug)
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
        public override Expression VisitProgram([NotNull] MyLangV4Parser.ProgramContext program)
        {
            DebugLine("VisitProgram: {0}", program.GetText());
            string programName = program.progname.Text;

            List<Expression> funcs = new List<Expression>();
            foreach (MyLangV4Parser.FuncdeclContext f in program.funcdecl())
            {
                Expression myFuncDecl = VisitFuncdecl(f);
                if (myFuncDecl != null)
                {
                    funcs.Add(myFuncDecl);
                }
            }

            MyLangV4Parser.BlockstatementContext bsc = program.blockstatement();
            Expression bs = VisitBlockstatement(bsc);

            List<Expression> all = new List<Expression>();

            all.AddRange(funcs);
            all.Add(bs);

            Expression programBlock = Expression.Block(all);

            return programBlock;
        }


        public override Expression VisitFuncdecl([NotNull] MyLangV4Parser.FuncdeclContext funcdecl)
        {
            DebugLine("VisitFuncdecl: {0}", funcdecl.GetText());
            string funcName = funcdecl.funcname.Text;
            var paramz = funcdecl._params.Select(x => x.Text).ToList();

            List<Expression> expressions = new List<Expression>();
            foreach(var param in paramz)
            {
                Expression paramExpr = Expression.Parameter(typeof(IConvertible), param);
                expressions.Add(paramExpr);
            }

            Expression body = VisitBlockstatement(funcdecl.blockstatement());

            expressions.Add(body);

            Expression funcDeclExpr = Expression.Block(typeof(IConvertible), expressions);

            return funcDeclExpr;
        }

        public override Expression VisitStatement([NotNull] MyLangV4Parser.StatementContext stmt)
        {
            DebugLine("VisitStatement: {0}", stmt.GetText());

            Expression s = null;

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

            if (stmt.expression() != null)
            {
                var expressionNode = Visit(stmt.expression());
                return expressionNode;
            }

            return s;
        }

        protected override bool ShouldVisitNextChild(IRuleNode node, Expression currentResult)
        {
            bool b = base.ShouldVisitNextChild(node, currentResult);
            return b;
        }

        public override Expression VisitBlockstatement([NotNull] MyLangV4Parser.BlockstatementContext context)
        {
            DebugLine("VisitBlockstatement: {0}", context.GetText()); 

            int i = 0;
            List<Expression> statements = new List<Expression>();

            foreach (MyLangV4Parser.StatementContext s in context.statement())
            {
                DebugLine("$$$: {0}, {1}", i, s.GetText());

                Expression stmt = VisitStatement(s);
                statements.Add(stmt);

                i++;
            }

            BlockExpression statementListNode = Expression.Block(statements);

            return statementListNode;
        }

        public override Expression VisitIfstatement([NotNull] MyLangV4Parser.IfstatementContext ifStmt)
        {
            DebugLine("VisitIfstatement {0}", ifStmt.GetText());
            Expression condition = Visit(ifStmt.condition);


            Expression ifStmtExpr = null;
            Expression thenPart = VisitBlockstatement(ifStmt.thenpart);
            Expression elsePart = ifStmt.elsepart != null ? VisitBlockstatement(ifStmt.elsepart)  : null;

            if (ifStmt.elsepart != null)
            {
                ifStmtExpr = Expression.IfThenElse(condition, thenPart, elsePart);
            }
            else
            {
                ifStmtExpr = Expression.IfThen(condition, thenPart);
            }


            return ifStmtExpr;
        }

        public static void print(string s)
        {
            Console.WriteLine(s);
        }

        public override Expression VisitPrintstatement([NotNull] MyLangV4Parser.PrintstatementContext context)
        {
            DebugLine("VisitPrintstatement {0}", context.GetText());



            Expression expr = null;
            if (context.expression() != null)
            {
                expr =  Visit(context.expression());
                expr = Expression.Call(expr, typeof(object).GetMethod("ToString", new Type[] { }));
            }
            else
            {
                expr = Expression.Constant(string.Empty);
            }
            Expression colsoleWriteExpr = Expression.Call(typeof(Console).GetMethod("WriteLine", new Type[] { typeof(string) }), expr);

            //if (context.EXCLAIM() != null)
            //{
            //    printStmt.Exclaim = true;
            //}

            return colsoleWriteExpr;
        }

        public override Expression VisitWhilestatement([NotNull] MyLangV4Parser.WhilestatementContext whileStmt)
        {
            DebugLine("VisitWhilestatement {0}", whileStmt.GetText());

            Expression condition = Visit(whileStmt.condition);

            Expression body = VisitBlockstatement(whileStmt.dopart);

            Expression loop = Expression.Loop(body, Expression.Label());
            return loop;
        }

        public override Expression VisitAssignment([NotNull] MyLangV4Parser.AssignmentContext assignStmt)
        {
            DebugLine("VisitAssignment {0}", assignStmt.GetText());
            Expression myVar = Expression.Variable(typeof(IConvertible), assignStmt.IDENTIFIER().GetText());

            Expression expr = Visit(assignStmt.expression());

            DebugLine("VisitAssignment myVar={0}, expr={1}, expression()={2}", myVar, expr, assignStmt.expression().GetText());

            //Expression.

            //Expression varExpression = Expression.Variable();

            Expression assignmentNode = Expression.Assign(myVar, expr);

            return assignmentNode;
        }


        public override Expression VisitReturnstatement([NotNull] MyLangV4Parser.ReturnstatementContext returnStmt)
        {
            DebugLine("VisitReturnstatement {0}", returnStmt.GetText());

            Expression expr = Visit(returnStmt.expression());

            Expression returnExpr = Expression.Return(Expression.Label(), expr);

            return returnExpr;
        }


        public override Expression VisitFunccall([NotNull] MyLangV4Parser.FunccallContext funcCall)
        {
            DebugLine("VisitFunccall: {0}", funcCall.GetText());
            string funcName = funcCall.funcname.Text;

            List<Expression> argNodes = new List<Expression>();

            foreach (var arg in funcCall._args)
            {
                Expression expr = Visit(arg);
                argNodes.Add(expr);
            }

            //MethodInfo
            System.Reflection.MethodInfo func = null;
            Expression funcCallExpr = Expression.Call(func, argNodes);
            return funcCallExpr;
        }

        public override Expression VisitEquals([NotNull] MyLangV4Parser.EqualsContext equals)
        {
            DebugLine("VisitEquals: {0}", equals.GetText());
            Expression leftExprNode = Visit(equals.left);
            Expression rightExprNode = Visit(equals.right);

            Expression equalsNode = Expression.Equal(leftExprNode, rightExprNode);
            return equalsNode;
        }

        public override Expression VisitGt([NotNull] MyLangV4Parser.GtContext gt)
        {
            DebugLine("VisitGt: {0}", gt.GetText());
            Expression leftExprNode = Visit(gt.left);
            Expression rightExprNode = Visit(gt.right);

            Expression gtNode = Expression.GreaterThan(leftExprNode, rightExprNode);
            return gtNode;
        }

        public override Expression VisitGteq([NotNull] MyLangV4Parser.GteqContext gteq)
        {
            DebugLine("VisitGteq: {0}", gteq.GetText());
            Expression leftExprNode = Visit(gteq.left);
            Expression rightExprNode = Visit(gteq.right);

            Expression gteqNode = Expression.GreaterThanOrEqual(leftExprNode, rightExprNode);
            return gteqNode;
        }

        public override Expression VisitLt([NotNull] MyLangV4Parser.LtContext lt)
        {
            DebugLine("VisitLt: {0}", lt.GetText());
            Expression leftExprNode = Visit(lt.left);
            Expression rightExprNode = Visit(lt.right);

            Expression ltNode = Expression.LessThan(leftExprNode, rightExprNode);
            return ltNode;
        }

        public override Expression VisitLteq([NotNull] MyLangV4Parser.LteqContext lteq)
        {
            DebugLine("VisitLteq: {0}", lteq.GetText());
            Expression leftExprNode = Visit(lteq.left);
            Expression rightExprNode = Visit(lteq.right);

            Expression lteqNode = Expression.LessThanOrEqual(leftExprNode, rightExprNode);
            return lteqNode;
        }

        public override Expression VisitMinus([NotNull] MyLangV4Parser.MinusContext minus)
        {
            DebugLine("VisitMinus: {0}", minus.GetText());
            Expression leftExprNode = Visit(minus.left);
            Expression rightExprNode = Visit(minus.right);

            Expression minusNode = Expression.Subtract(leftExprNode, rightExprNode);
            return minusNode;
        }

        public override Expression VisitAnd([NotNull] MyLangV4Parser.AndContext and)
        {
            DebugLine("VisitAnd: {0}", and.GetText());
            Expression leftExprNode = Visit(and.left);
            Expression rightExprNode = Visit(and.right);

            Expression andNode = Expression.AndAlso(leftExprNode, rightExprNode);
            return andNode;
        }

        public override Expression VisitDivide([NotNull] MyLangV4Parser.DivideContext divide)
        {
            DebugLine("VisitDivide: {0}, left={1}, right={2}", divide.GetText(), divide.left.GetText(), divide.right.GetText());

            Debug("VisitDivide left:");
            Expression leftExprNode = Visit(divide.left);

            Debug("VisitDivide right:");
            Expression rightExprNode = Visit(divide.right);

            Expression divideNode = Expression.Divide(leftExprNode, rightExprNode);
            return divideNode;
        }

        public override Expression VisitMultiply([NotNull] MyLangV4Parser.MultiplyContext multiply)
        {
            DebugLine("VisitMultiply: {0}, left={1}, right={2}", multiply.GetText(), multiply.left.GetText(), multiply.right.GetText());

            Debug("VisitMultiply left:");
            Expression leftExprNode = Visit(multiply.left);

            Debug("VisitMultiply right:");
            Expression rightExprNode = Visit(multiply.right);

            Expression multiplyNode = Expression.Multiply(leftExprNode, rightExprNode);
            return multiplyNode;
        }

        public override Expression VisitExp([NotNull] MyLangV4Parser.ExpContext context)
        {
            DebugLine("VisitExp: {0}, left={1}, right={2}", context.GetText(), context.left.GetText(), context.right.GetText());

            Debug("VisitExp left:");
            Expression leftExprNode = Visit(context.left);

            Debug("VisitExp right:");

            Expression rightExprNode = Visit(context.right);
            Expression mathPowExpr = Expression.Call(typeof(Math).GetMethod("Pow", new Type[] { typeof(double), typeof(double) }), leftExprNode, rightExprNode);

            return mathPowExpr;

        }

        public override Expression VisitMod([NotNull] MyLangV4Parser.ModContext mod)
        {
            DebugLine("VisitMod: {0}, left={1}, right={2}", mod.GetText(), mod.left.GetText(), mod.right.GetText());

            Debug("VisitMod left:");
            Expression leftExprNode = Visit(mod.left);

            Debug("VisitMod right:");
            Expression rightExprNode = Visit(mod.right);

            Expression ModNode = Expression.Modulo(leftExprNode, rightExprNode);
            return ModNode;
        }

        public override Expression VisitNegate([NotNull] MyLangV4Parser.NegateContext negate)
        {
            DebugLine("VisitNegate: {0}", negate.GetText());


            Expression opNode = Visit(negate.negateexpression().expression());
            Expression expr = Expression.Negate(opNode);
            return expr;
        }

        public override Expression VisitAbs([NotNull] MyLangV4Parser.AbsContext context)
        {
            DebugLine("VisitAbs: {0}", context.GetText());
            Expression opNode = Visit(context.abs_expression());

            Expression mathPowExpr = Expression.Call(typeof(Math).GetMethod("Pow", new Type[] { typeof(double)}), opNode);

            return opNode;
        }

        public override Expression VisitInt([NotNull] MyLangV4Parser.IntContext number)
        {
            DebugLine("VisitInt: {0}", number.GetText());
            int i = Int32.Parse(number.GetText());

            Expression expr = Expression.Constant(i, typeof(int));
            return expr;
        }

        public override Expression VisitString([NotNull] MyLangV4Parser.StringContext str)
        {
            DebugLine("VisitString: {0}", str.GetText());
            Expression strExpr = Expression.Constant(str.GetText(), typeof(string));
            return strExpr;
        }


        public override Expression VisitStringucase([NotNull] MyLangV4Parser.StringucaseContext context)
        {
            DebugLine("VisitStringucase: {0}", context.GetText());

            Expression str = Visit(context.ucase().expression());

            Expression expr = Expression.Call(str, typeof(object).GetMethod("ToString", new Type[] { }));

            Expression strToUpperExpr = Expression.Call(expr, typeof(string).GetMethod("ToUpper", new Type[] { }));

            return strToUpperExpr;
        }


        public override Expression VisitFloat([NotNull] MyLangV4Parser.FloatContext number)
        {
            DebugLine("VisitFloat: {0}", number.GetText());
            double d = Double.Parse(number.GetText());

            Expression expr = Expression.Constant(d, typeof(double));
            return expr;
        }

        public override Expression VisitPlus([NotNull] MyLangV4Parser.PlusContext plus)
        {
            DebugLine("VisitPlus: {0}, left={1}, right={2}", plus.GetText(), plus.left.GetText(), plus.right.GetText());

            DebugLine("VisitOr: {0}", plus.GetText());
            Expression leftExprNode = Visit(plus.left);
            Expression rightExprNode = Visit(plus.right);

            Expression expr = Expression.Add(leftExprNode, rightExprNode);

            return expr;
        }

        public override Expression VisitPosate([NotNull] MyLangV4Parser.PosateContext posate)
        {
            DebugLine("VisitPosate: {0}", posate.GetText());
            return Visit(posate.posateexpression().expression());
        }

        public override Expression VisitParenexpr([NotNull] MyLangV4Parser.ParenexprContext context)
        {
            DebugLine("VisitParenexpr: {0}", context.GetText());
            return VisitParenexpression(context.parenexpression());
        }

        public override Expression VisitParenexpression([NotNull] MyLangV4Parser.ParenexpressionContext context)
        {
            DebugLine("VisitParenexpression: {0}", context.GetText());
            return Visit(context.expression());
        }


        public override Expression VisitOr([NotNull] MyLangV4Parser.OrContext or)
        {
            DebugLine("VisitOr: {0}", or.GetText());
            Expression leftExprNode = Visit(or.left);
            Expression rightExprNode = Visit(or.right);

            Expression expr = Expression.Or(leftExprNode, rightExprNode);

            return expr;
        }


        public override Expression VisitVarcall([NotNull] MyLangV4Parser.VarcallContext context)
        {
            DebugLine("VisitVarcall: {0}", context.GetText());

            string varName = context.IDENTIFIER().GetText();

            Expression expr = Expression.Variable(typeof(IConvertible), varName);
            return expr;
        }


        public override Expression VisitFuncallexpr([NotNull] MyLangV4Parser.FuncallexprContext context)
        {
            DebugLine("VisitFuncallexpr: {0}", context.GetText());
            return VisitFunccall(context.funccall());
        }

        public override Expression VisitTerminal(ITerminalNode node)
        {
            DebugLine("VisitTerminal: {0}", node.GetText());
            return base.VisitTerminal(node);
        }

        public override Expression VisitExpression([NotNull] MyLangV4Parser.ExpressionContext context)
        {
            DebugLine("VisitExpression: {0}", context.GetText());
            return base.VisitExpression(context);
        }

        public override Expression VisitVarinc([NotNull] MyLangV4Parser.VarincContext context)
        {
            DebugLine("VisitVarinc: {0}", context.GetText());
            return base.VisitVarinc(context);
        }

        public override Expression VisitIncvar([NotNull] MyLangV4Parser.IncvarContext context)
        {
            DebugLine("VisitIncvar: {0}", context.GetText());
            return base.VisitIncvar(context);
        }


        /*  perhaps we can get the parser to rewrite i++, i--, i+=val, i-=val as this:
         {
                _t_i := i;
                i := i [+|-] [1|val];
                _t_i := t_i;
         }
         */


        public override Expression VisitVarPlusPlus([NotNull] MyLangV4Parser.VarPlusPlusContext context)
        {
            DebugLine("VisitVarPlusPlus: {0}", context.GetText());

            string varName = context.IDENTIFIER().GetText();

            Expression expr =
                Expression.PostIncrementAssign(
                    Expression.Variable(typeof(IConvertible), varName)
                );

            return expr;
        }

        public override Expression VisitVarMinusMinus([NotNull] MyLangV4Parser.VarMinusMinusContext context)
        {
            DebugLine("VisitVarMinusMinus: {0}", context.GetText());

            string varName = context.IDENTIFIER().GetText();

            Expression expr =
                Expression.PostDecrementAssign(
                    Expression.Variable(typeof(IConvertible), varName)
                );

            return expr;
        }

        public override Expression VisitVarPlusEquals([NotNull] MyLangV4Parser.VarPlusEqualsContext context)
        {
            DebugLine("VisitVarPlusEquals: {0}", context.GetText());

            string varName = context.IDENTIFIER().GetText();
            Expression rightExpr = Visit(context.expression());

            Expression expr =
                Expression.AddAssign(
                    Expression.Variable(typeof(IConvertible), varName),
                    rightExpr
                );

            return expr;
        }

        public override Expression VisitVarMinusEquals([NotNull] MyLangV4Parser.VarMinusEqualsContext context)
        {
            DebugLine("VisitVarMinusEquals: {0}", context.GetText());

            string varName = context.IDENTIFIER().GetText();
            Expression rightExpr = Visit(context.expression());

            Expression expr =
                Expression.SubtractAssign(
                    Expression.Variable(typeof(IConvertible), varName),
                    rightExpr
                );

            return expr;
        }

        public override Expression VisitPlusPlusVar([NotNull] MyLangV4Parser.PlusPlusVarContext context)
        {
            DebugLine("VisitPlusPlusVar: {0}", context.GetText());

            string varName = context.IDENTIFIER().GetText();

            Expression expr =
                Expression.PreIncrementAssign(
                    Expression.Variable( typeof(IConvertible), varName)
                );

            return expr;
        }

        public override Expression VisitMinusMinusVar([NotNull] MyLangV4Parser.MinusMinusVarContext context)
        {
            DebugLine("VisitMinusMinusVar: {0}", context.GetText());

            string varName = context.IDENTIFIER().GetText();

            Expression expr =
                Expression.PreDecrementAssign(
                    Expression.Variable( typeof(IConvertible), varName)
                );

            return expr;
        }

        
    }
}
