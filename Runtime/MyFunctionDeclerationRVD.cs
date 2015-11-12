using System;
using System.Collections.Generic;

namespace MyLang.Runtime
{
    public class MyFunctionDeclerationRVD : MyStatementNode
    {
        public List<string> Params { get; set; }
        public MyStatementListNode Statements { get; set; }
        public string Name { get; set; }

        public MyFunctionDeclerationRVD()
        {
            Params = new List<string>();
        }

        public MyFunctionDeclerationRVD(string name)
        {
            Name = name;
            Params = new List<string>();
        }

        public MyFunctionDeclerationRVD(string name, List<string> pParams)
        {
            Name = name;
            Params = pParams;
        }

        public override object DoStatement(MyContext context)
        {
            return Evaluate(context);
        }

        public override object Evaluate(MyContext context)
        {
            context.AddLog("declaring function: " + Name);
            context.AddLog(Environment.NewLine);

            MyContext ctx = new MyContext();

            List(ctx);

            context.AddLog(ctx.ListLog.ToString());

            context.AddLog(Environment.NewLine);

            context.AddFunctionRVD(Name, this);
            return 0;
        }

        public override void List(MyContext context)
        {
            context.ListLog.Append("declare function: ").Append(Name).Append("( ");

            for (int i = 0; i < Params.Count; i++)
            {
                context.ListLog.Append("@").Append(Params[i]);

                if (i < Params.Count - 1)
                {
                    context.ListLog.Append(", ");
                }
            }

            context.ListLog.Append(" )").NewLine();

            Statements.List(context);

            context.ListLog.NewLine();
        }
    }

}