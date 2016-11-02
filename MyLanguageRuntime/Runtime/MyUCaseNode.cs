using System;

namespace MyLanguageImpl.Runtime
{
    public class MyUCaseNode : MyAbstractNode
    {
        public MyAbstractNode Operand { get; set; }

        public MyUCaseNode(MyAbstractNode operand)
        {
            Operand = operand;
        }

        public override object Evaluate(MyContext context)
        {
            var o = (IConvertible)Operand.Evaluate(context).ToString().ToUpper();
            return o;
        }

        public override void List(MyContext context)
        {
            context.ListLog.Append(" ^");
            Operand.List(context);
        }
    }
}