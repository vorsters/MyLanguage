using System;

namespace MyLanguageImpl.Runtime
{
    public class MyLogicalOrNode : MyBinaryOpNode
    {
        public MyLogicalOrNode(MyAbstractNode left, MyAbstractNode right)
            : base(left, right)
        {
        }

        public override string Symbol
        {
            get { return "OR"; }
        }

        protected override IConvertible DoBinOp(IConvertible left, IConvertible right)
        {
            Boolean left1 = Convert.ToBoolean(left);
            Boolean right1 = Convert.ToBoolean(right);

            return left1 || right1;
        }
    }
}