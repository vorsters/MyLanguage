using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MyLang.Runtime
{
    public class MyFunctionRoepNode : MyNonTerminatingNode
    {
        public string Name { get; set; }
        public List<MyAbstractNode> Args;

        public MyFunctionRoepNode()
        {
            Args = new List<MyAbstractNode>();
        }

        public MyFunctionRoepNode(string name)
        {
            Name = name;
            Args = new List<MyAbstractNode>() ;
        }

        public MyFunctionRoepNode(string name, List<MyAbstractNode> args)
        {
            Name = name;
            Args = args;
        }

        public override object Evaluate(MyContext context)
        {
            MyFunctionDeclerationRVD func = context.GetFunctionRVD(Name);
            
            MyContext ctx = new MyContext();

            ctx.functionsRVD = context.functionsRVD;

            for (int i = 0; i < func.Params.Count ; i++)
            {
                IConvertible value = (IConvertible) Args[i].Evaluate(context);
                string param = func.Params[i];
                ctx.AddVariable(param, value);

                if (i < func.Params.Count - 1)
                {
                    context.AddLog(", ");
                }

                //Debug.WriteLine(Name + "," + param + "=" + value);

                context.AddLog(value.ToString());
            }

            object o = func.Statements.Evaluate(ctx);

            object returnValue = ctx.returnValue;
            ctx.returned = false;

            //Debug.WriteLine(Name + ".returnValue =" + returnValue);

            context.AddLog(ctx.sb.ToString());

            return returnValue;
        }

        public override void List(MyContext context)
        {
            context.ListLog.Append("call ").Append(Name).Append("(");

            for (int i = 0; i < Args.Count; i++)
            {
                Args[i].List(context);
                if (i < Args.Count - 1)
                {
                    context.ListLog.Append(", ");
                }
            }

            context.ListLog.Append(")");
        }
    }
}