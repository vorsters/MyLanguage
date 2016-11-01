using System.Diagnostics;

namespace MyLanguageImpl.Runtime
{
    public class MyReturnStatement : MyStatementNode
    {
        public MyAbstractNode ReturnExpression { get; set; }

        public MyReturnStatement(MyAbstractNode returnExpression)
        {
            ReturnExpression = returnExpression;
        }

        public override void List(MyContext context)
        {
            context.ListLog.Append("return ");
            ReturnExpression.List(context);
            //context.ListLog.Append(";");
        }

        public override object DoStatement(MyContext context)
        {
            context.AddLog("returning: ");
            object o = ReturnExpression.Evaluate(context);

            context.returnValue = o;
            context.returned = true;
            //Debug.WriteLine("returning: " + o);

            return o;
        }
    }
}