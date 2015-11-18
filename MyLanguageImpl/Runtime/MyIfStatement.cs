using System;

namespace MyLanguageImpl.Runtime
{
    public class MyIfStatement : MyStatementNode
    {
        public MyAbstractNode Condition { get; set; }
        public MyStatementListNode ThenPart { get; set; }
        public MyStatementListNode ElsePart { get; set; }

        public MyIfStatement(MyAbstractNode condition, MyStatementListNode thenPart)
        {
            Condition = condition;
            ThenPart = thenPart;
            ElsePart = null;
        }

        public MyIfStatement(MyAbstractNode condition, MyStatementListNode thenPart, MyStatementListNode elsePart)
        {
            Condition = condition;
            ThenPart = thenPart;
            ElsePart = elsePart;
        }

        public override object DoStatement(MyContext context)
        {
            context.AddLog("if cond: ");

            bool cond = (bool)Condition.Evaluate(context);

            context.AddLog(Environment.NewLine);

            object o = null;

            if (cond)
            {
                context.AddLog("then: ");
                o = ThenPart.Evaluate(context);
                context.AddLog(Environment.NewLine);
            }
            else
            {

                if (ElsePart != null)
                {
                    context.AddLog("else: ");
                    o = ElsePart.Evaluate(context);
                    context.AddLog(Environment.NewLine);
                }
                else
                {
                    context.AddLog("nothing: ");
                    context.AddLog("end-if");
                }
            }

            return o;
        }

        public override void List(MyContext context)
        {
            context.ListLog.Append("if (");
            Condition.List(context);
            context.ListLog.Append(") then").NewLine();
            
            ThenPart.List(context);

            if (ElsePart != null)
            {
                context.ListLog.NewLine().Append("else").NewLine();
                ElsePart.List(context);
            }
        }
    }
}