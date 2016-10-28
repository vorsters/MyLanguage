using Antlr4.Runtime.Misc;
using MyLanguageGrammar_ANTLR4.Grammar;
using MyLanguageImpl.Runtime;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime.Tree;
using System;

namespace MyLanguageImpl_ANTLR4.Impl
{
    public class RuntimeVisitor2 : MyLangV4ParserBaseVisitor<MyAbstractNode>
    {
        int _i = 0;

        public override MyAbstractNode Visit(IParseTree tree)
        {
            _i++;
            Console.WriteLine("Visit ({0}) : {1}", _i, tree.GetText());
            return base.Visit(tree);
        }

        public override MyAbstractNode VisitProgram([NotNull] MyLangV4Parser.ProgramContext context)
        {
            Console.WriteLine("VisitProgram ({0}) : {1}", _i, context.GetText());
            return base.VisitProgram(context);
        }

        public override MyAbstractNode VisitAssignment([NotNull] MyLangV4Parser.AssignmentContext context)
        {
            Console.WriteLine("VisitAssignment ({0}) : {1}", _i, context.GetText());
            return base.VisitAssignment(context);
        }

        public override MyAbstractNode VisitReturnstatement([NotNull] MyLangV4Parser.ReturnstatementContext context)
        {
            Console.WriteLine("VisitAssignment ({0}) : {1}", _i, context.GetText());
            return base.VisitReturnstatement(context);
        }

        public override MyAbstractNode VisitBlockstatement([NotNull] MyLangV4Parser.BlockstatementContext context)
        {
            Console.WriteLine("VisitBlockstatement ({0}) : {1}", _i, context.GetText());
            return base.VisitBlockstatement(context);
        }

        public override MyAbstractNode VisitIfstatement([NotNull] MyLangV4Parser.IfstatementContext context)
        {
            Console.WriteLine("VisitIfstatement ({0}) : {1}", _i, context.GetText());
            return base.VisitIfstatement(context);
        }

        public override MyAbstractNode VisitWhilestatement([NotNull] MyLangV4Parser.WhilestatementContext context)
        {
            Console.WriteLine("VisitWhilestatement ({0}) : {1}", _i, context.GetText());
            return base.VisitWhilestatement(context);
        }

        public override MyAbstractNode VisitStatement([NotNull] MyLangV4Parser.StatementContext context)
        {
            Console.WriteLine("VisitStatement ({0}) : {1}", _i, context.GetText());
            return base.VisitStatement(context);
        }

        public override MyAbstractNode VisitFuncdecl([NotNull] MyLangV4Parser.FuncdeclContext context)
        {
            Console.WriteLine("VisitFuncdecl ({0}) : {1}", _i, context.GetText());
            return base.VisitFuncdecl(context);
        }

        public override MyAbstractNode VisitFunccall([NotNull] MyLangV4Parser.FunccallContext context)
        {
            Console.WriteLine("VisitFunccall ({0}) : {1}", _i, context.GetText());
            return base.VisitFunccall(context);
        }

        public override MyAbstractNode VisitParenexpr([NotNull] MyLangV4Parser.ParenexprContext context)
        {
            Console.WriteLine("VisitParenexpr ({0}) : {1}", _i, context.GetText());
            return base.VisitParenexpr(context);
        }

        public override MyAbstractNode VisitNegateexpression([NotNull] MyLangV4Parser.NegateexpressionContext context)
        {
            Console.WriteLine("VisitNegateexpression ({0}) : {1}", _i, context.GetText());
            return base.VisitNegateexpression(context);
        }

        public override MyAbstractNode VisitExpression([NotNull] MyLangV4Parser.ExpressionContext context)
        {
            Console.WriteLine("VisitExpression ({0}) : {1}", _i, context.GetText());
            return base.VisitExpression(context);
        }

        public override MyAbstractNode VisitTerminal(ITerminalNode node)
        {
            Console.WriteLine("VisitTerminal ({0}) : {1}", _i, node.GetText());
            return base.VisitTerminal(node);
        }


        public override MyAbstractNode VisitInt([NotNull] MyLangV4Parser.IntContext context)
        {
            Console.WriteLine("VisitInt ({0}) : {1}, Parent:{2}-{3}", _i, context.GetText(), context.Parent.GetText(), context.Parent.GetType().Name);
            return new MyValueNode(Int32.Parse(context.INT().GetText()));
        }

        public override MyAbstractNode VisitMultiply([NotNull] MyLangV4Parser.MultiplyContext context)
        {
            Console.WriteLine("VisitMultiply ({0}) : {1}", _i, context.GetText());

            var left = Visit(context.left);
            var right = Visit(context.right);

            MyMultiplyNode node = new MyMultiplyNode(left, right);
            return node;
        }



    }
}
