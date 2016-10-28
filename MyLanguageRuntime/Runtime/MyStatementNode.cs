namespace MyLanguageImpl.Runtime
{
    public abstract class MyStatementNode : MyNonTerminatingNode
    {
        public abstract object DoStatement(MyContext context);

        public override object Evaluate(MyContext context)
        {
            context.AddLog("> ");
            object o = DoStatement(context);
            context.AddLog(";");
            return o;
        }

    }
}