using System;

namespace MyLanguageImpl.Runtime
{
    public class MyIsLessThanNode : MyBinaryOpNode
    {
        public MyIsLessThanNode(MyAbstractNode left, MyAbstractNode right)
            : base(left, right)
        {
        }

        public override string Symbol
        {
            get { return "<"; }
        }

        protected override IConvertible DoBinOp(IConvertible left, IConvertible right)
        {
            IComparable left1 = (IComparable)left;
            IComparable right1 = (IComparable)right;

            if (left1.CompareTo(right1) < 0)
            {
                return true;
            }
            return false;
        }
    }
}