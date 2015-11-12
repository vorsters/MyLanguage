using System;

namespace MyLang.Runtime
{
    public class MyValueNode : MyTerminalNode
    {
        public IConvertible Value { get; set; }

        public MyValueNode(IConvertible value)
        {
            Value = value;
        }

        public override object Evaluate(MyContext context)
        {
            context.AddLog(Value.ToString() + " ");
            return Value;
        }

        public override void List(MyContext context)
        {
            context.ListLog.Append(Value.ToString()).Append(" ");
        }
    }
}