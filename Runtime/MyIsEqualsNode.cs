using System;

namespace MyLang.Runtime
{
    public class MyIsEqualsNode : MyBinaryOpNode
    {
        public MyIsEqualsNode(MyAbstractNode left, MyAbstractNode right)
            : base(left, right)
        {
        }

        public override string Symbol
        {
            get { return "=="; }
        }

        protected override IConvertible DoBinOp(IConvertible left, IConvertible right)
        {
            return Equals(left, right);
        }
    }
}