using System;

namespace MyLanguageImpl.Runtime
{
    public class MyWhileStatement : MyStatementNode
    {
        public MyAbstractNode Condition { get; set; }
        public MyStatementListNode Body { get; set; }

        public MyWhileStatement(MyAbstractNode condition, MyStatementListNode body)
        {
            Condition = condition;
            Body = body;
        }

        public override object DoStatement(MyContext context)
        {
            context.AddLog("while cond: ");

            bool cond = (bool)Condition.Evaluate(context);

            context.AddLog(Environment.NewLine);

            object o = null;

            while (cond)
            {
                context.AddLog("then: ");
                o = Body.Evaluate(context);
                context.AddLog(Environment.NewLine);

                if (context.returned)
                {
                    break;
                }

                cond = (bool)Condition.Evaluate(context);
            }

            context.AddLog("end-while");
            return o;
        }

        public override void List(MyContext context)
        {
            context.ListLog.Append("while (");
            Condition.List(context);
            context.ListLog.Append(")").NewLine();
            
            Body.List(context);

        }
    }
}