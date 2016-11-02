using System;

namespace MyLanguageImpl.Runtime
{
    public class MyExponentNode : MyBinaryOpNode
    {
        public MyExponentNode(MyAbstractNode left, MyAbstractNode right)
            : base(left, right)
        {
        }

        public override string Symbol
        {
            get { return "^"; }
        }

        protected override IConvertible DoBinOp(IConvertible left, IConvertible right)
        {
            TypeCode typeCode = GetValueType(left, right);

            switch (typeCode)
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return Math.Pow(Convert.ToDouble(left),Convert.ToDouble(right));

            }

            throw new Exception("invalid type for EXPONENT");
        }
    }
}