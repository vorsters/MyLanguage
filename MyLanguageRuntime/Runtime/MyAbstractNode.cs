using System;

namespace MyLanguageImpl.Runtime
{
    public abstract class MyAbstractNode
    {
        public abstract object Evaluate(MyContext context);
        public abstract void List(MyContext context);
    }
}