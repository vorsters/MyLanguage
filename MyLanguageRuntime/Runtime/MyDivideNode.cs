using System;

namespace MyLanguageImpl.Runtime
{
    public class MyDivideNode : MyBinaryOpNode
    {
        public MyDivideNode(MyAbstractNode left, MyAbstractNode right)
            : base(left, right)
        {
        }

        public override string Symbol
        {
            get { return "/"; }
        }

        protected override IConvertible DoBinOp(IConvertible left, IConvertible right)
        {
            TypeCode typeCode = GetValueType(left, right);

            switch (typeCode)
            {
                case TypeCode.SByte:
                    return (Convert.ToSByte(left) / Convert.ToSByte(right));

                case TypeCode.Byte:
                    return (Convert.ToByte(left) / Convert.ToByte(right));

                case TypeCode.Int16:
                    return (Convert.ToInt16(left) / Convert.ToInt16(right));

                case TypeCode.UInt16:
                    return (Convert.ToUInt16(left) / Convert.ToUInt16(right));

                case TypeCode.Int32:
                    return (Convert.ToInt32(left) / Convert.ToInt32(right));

                case TypeCode.UInt32:
                    return (Convert.ToUInt32(left) / Convert.ToUInt32(right));

                case TypeCode.Int64:
                    return (Convert.ToInt64(left) / Convert.ToInt64(right));

                case TypeCode.UInt64:
                    return (Convert.ToUInt64(left) / Convert.ToUInt64(right));

                case TypeCode.Double:
                    return (Convert.ToDouble(left) / Convert.ToDouble(right));

                case TypeCode.Decimal:
                    return (Convert.ToDecimal(left) / Convert.ToDecimal(right));

            }

            throw new Exception("invalid type for DIVIDE");
        }
    }
}