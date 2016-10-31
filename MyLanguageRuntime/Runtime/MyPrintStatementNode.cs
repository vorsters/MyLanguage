using System;

namespace MyLanguageImpl.Runtime
{
    public class MyPrintStatementNode : MyStatementNode
    {
        public MyAbstractNode Expr { get; private set; }
        public bool Exclaim { get; set; }

        public MyPrintStatementNode(MyAbstractNode expr)
        {
            this.Expr = expr;
        }

        public override object DoStatement(MyContext context)
        {
            IConvertible o = (IConvertible)Expr.Evaluate(context);

            ConsoleColor fg = Console.ForegroundColor;
            ConsoleColor bg = Console.BackgroundColor;

            Console.ForegroundColor = ConsoleColor.Blue;

            if (Exclaim)
            {
                Console.BackgroundColor = ConsoleColor.White;
            }

            Console.WriteLine(o);

            Console.ForegroundColor = fg;
            Console.BackgroundColor= bg;

            return o;
        }

        public override void List(MyContext context)
        {
            context.ListLog.Append("print ");
            if (Exclaim)
            {
                context.ListLog.Append("! ");
            }

            Expr.List(context);
            context.ListLog.Append(" ;");
        }
    }

}
