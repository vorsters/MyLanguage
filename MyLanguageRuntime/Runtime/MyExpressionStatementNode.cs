using System;

namespace MyLanguageImpl.Runtime
{
    public class MyExpressionStatementNode : MyStatementNode
    {
        public MyAbstractNode Expression { get; private set; }
        public MyExpressionStatementNode(MyAbstractNode expr)
        {
            this.Expression = expr;
        }


        public override object DoStatement(MyContext context)
        {
            return Expression.Evaluate(context);
        }

        public override void List(MyContext context)
        {
            Expression.List(context);
        }
    }
}