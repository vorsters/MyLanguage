using System;
using System.Diagnostics;

namespace MyLanguageImpl.Runtime
{
    public class MyVariableNode : MyTerminalNode
    {
        public string Name { get; set; }
   
        public MyVariableNode(string variableName)
        {
            Name = variableName;
        }

        public override object Evaluate(MyContext context)
        {
            IConvertible value = (IConvertible) context.GetVariable(Name);

            context.AddLog(" $" + Name + "[" + value + "] ");

            return value;
        }

        public override void List(MyContext context)
        {
            context.ListLog.Append("$").Append(Name);
        }
    }

    public class MyParamValue : MyTerminalNode
    {
        public string Name { get; set; }

        public MyParamValue(string variableName)
        {
            Name = variableName;
        }

        public override object Evaluate(MyContext context)
        {
            IConvertible value = (IConvertible)context.GetVariable(Name);

            context.AddLog(" @" + Name + "[" + value + "] ");

            //Debug.WriteLine(" @" + Name + "[" + value + "] ");

            return value;
        }

        public override void List(MyContext context)
        {
            context.ListLog.Append("@").Append(Name);
        }
    }

}