using System;

namespace MyLanguageImpl.Runtime
{
    public class MyStringLiteralNode : MyValueNode
    {
        public MyStringLiteralNode(IConvertible value) : base(value)
        {
            Value = StripQuotes(value.ToString());
        }

        public override void List(MyContext context)
        {
            context.ListLog.Append("\"");
            context.ListLog.Append(Value.ToString());
            context.ListLog.Append("\"");
        }

        private string StripQuotes(string s)
        {
            return s.Replace("\"", "");
        }

    }

}
