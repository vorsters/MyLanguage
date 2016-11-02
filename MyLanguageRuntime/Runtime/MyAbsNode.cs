using System;

namespace MyLanguageImpl.Runtime
{
    public class MyAbsNode : MyAbstractNode
    {
        public MyAbstractNode Op { get; set; }

        public MyAbsNode(MyAbstractNode op)
        {
            Op = op;
        }

        public override object Evaluate(MyContext context)
        {
            IConvertible o = (IConvertible) Op.Evaluate(context);

            switch (o.GetTypeCode())
            {
                case TypeCode.Char:
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                    return Math.Abs(Convert.ToInt32(o));

                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                
                case TypeCode.Single:
                case TypeCode.Double:
                    return Math.Abs(Convert.ToDouble(o));

                case TypeCode.Decimal:
                    return Math.Abs((decimal) o);

                default:
                    return o;
            }

        }

        public override void List(MyContext context)
        {
            context.ListLog.Append(" `");
            Op.List(context);
        }
    }
}