using System;

namespace MyLang.Runtime
{
    public abstract class MyBinaryOpNode : MyNonTerminatingNode
    {
        public abstract string Symbol { get; }

        public MyAbstractNode Left { get; set; }
        public MyAbstractNode Right { get; set; }

        protected MyBinaryOpNode(MyAbstractNode left, MyAbstractNode right)
        {
            Left = left;
            Right = right;
        }

        public override void List(MyContext context)
        {
            context.ListLog.Append(" ( ");
            Left.List(context);
            context.ListLog.Append(" ").Append(Symbol).Append(" ");
            Right.List(context);
            context.ListLog.Append(" ) ");
        }

        public override object Evaluate(MyContext context)
        {
            context.AddLog(" ( ");

            context.AddLog(Symbol + " ");

            IConvertible left = (IConvertible)Left.Evaluate(context);
            IConvertible right = (IConvertible)Right.Evaluate(context);

            context.AddLog(" ) ");

            IConvertible result = DoBinOp(left, right);

            return result;
        }

        protected abstract IConvertible DoBinOp(IConvertible left, IConvertible right);

        protected TypeCode GetValueType(IConvertible left, IConvertible right)
        {
            const int INTEGRAL_8 = 0x0001;
            const int UINTEGRAL_8 = 0x0002;
            const int INTEGRAL_16 = 0x0004;
            const int UINTEGRAL_16 = 0x0008;

            const int INTEGRAL_32 = 0x0010;
            const int UINTEGRAL_32 = 0x0020;
            const int INTEGRAL_64 = 0x0040;
            const int UINTEGRAL_64 = 0x0080;

            const int FLOATING_POINT = 0x0100;
            const int DECIMAL = 0x0200;

            const int STRING = 0x0400;

            int leftResult = 0;

            switch (left.GetTypeCode())
            {
                case TypeCode.Boolean:
                    leftResult = INTEGRAL_8;
                    break;

                case TypeCode.Char:
                    leftResult = UINTEGRAL_16;
                    break;

                case TypeCode.SByte:
                    leftResult = INTEGRAL_8;
                    break;

                case TypeCode.Byte:
                    leftResult = UINTEGRAL_8;
                    break;

                case TypeCode.Int16:
                    leftResult = INTEGRAL_16;
                    break;

                case TypeCode.UInt16:
                    leftResult = UINTEGRAL_16;
                    break;

                case TypeCode.Int32:
                    leftResult = INTEGRAL_32;
                    break;

                case TypeCode.UInt32:
                    leftResult = UINTEGRAL_32;
                    break;

                case TypeCode.Int64:
                    leftResult = INTEGRAL_64;
                    break;

                case TypeCode.UInt64:
                    leftResult = UINTEGRAL_64;
                    break;

                case TypeCode.Single:
                case TypeCode.Double:
                    leftResult = FLOATING_POINT;
                    break;

                case TypeCode.Decimal:
                    leftResult = DECIMAL;
                    break;

                default:
                    leftResult = STRING;
                    break;
            }

            int rightResult = 0;
            switch (right.GetTypeCode())
            {
                case TypeCode.Boolean:
                    rightResult = INTEGRAL_8;
                    break;

                case TypeCode.Char:
                    rightResult = UINTEGRAL_16;
                    break;

                case TypeCode.SByte:
                    rightResult = INTEGRAL_8;
                    break;

                case TypeCode.Byte:
                    rightResult = UINTEGRAL_8;
                    break;

                case TypeCode.Int16:
                    rightResult = INTEGRAL_16;
                    break;

                case TypeCode.UInt16:
                    rightResult = UINTEGRAL_16;
                    break;

                case TypeCode.Int32:
                    rightResult = INTEGRAL_32;
                    break;

                case TypeCode.UInt32:
                    rightResult = UINTEGRAL_32;
                    break;

                case TypeCode.Int64:
                    rightResult = INTEGRAL_64;
                    break;

                case TypeCode.UInt64:
                    rightResult = UINTEGRAL_64;
                    break;

                case TypeCode.Single:
                case TypeCode.Double:
                    rightResult = FLOATING_POINT;
                    break;

                case TypeCode.Decimal:
                    rightResult = DECIMAL;
                    break;

                default:
                    rightResult = STRING;
                    break;
            }

            int result = leftResult | rightResult;

            if ((int)(result & STRING) == STRING)
            {
                return TypeCode.String;
            }

            if ((int)(result & DECIMAL) == DECIMAL)
            {
                return TypeCode.Decimal;
            }

            if ((int)(result & FLOATING_POINT) == FLOATING_POINT)
            {
                return TypeCode.Double;
            }

            if ((int)(result & UINTEGRAL_64) == UINTEGRAL_64)
            {
                return TypeCode.UInt64;
            }

            if ((int)(result & INTEGRAL_64) == INTEGRAL_64)
            {
                return TypeCode.Int64;
            }

            if ((int)(result & UINTEGRAL_32) == UINTEGRAL_32)
            {
                return TypeCode.UInt32;
            }

            if ((int)(result & INTEGRAL_32) == INTEGRAL_32)
            {
                return TypeCode.Int32;
            }

            if ((int)(result & UINTEGRAL_16) == UINTEGRAL_16)
            {
                return TypeCode.UInt16;
            }

            if ((int)(result & INTEGRAL_16) == INTEGRAL_16)
            {
                return TypeCode.Int16;
            }

            if ((int)(result & UINTEGRAL_8) == UINTEGRAL_8)
            {
                return TypeCode.Byte;
            }

            if ((int)(result & INTEGRAL_8) == INTEGRAL_8)
            {
                return TypeCode.SByte;
            }

            return TypeCode.String;
        }
    }

}