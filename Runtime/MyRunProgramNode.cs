namespace MyLang.Runtime
{
    class MyRunProgramNode : MyNonTerminatingNode
    {
        public MyRunProgramNode(string name)
        {
            Name = name;
        }

        public string Name { get; set; }    

        public override object Evaluate(MyContext context)
        {
            var p = context.GetProgramDeclNode(Name);

            foreach (var func in p.Funcs)
            {
                func.Evaluate(context);
            }

            var o = p.Statements.Evaluate(context);
            return o;
        }

        public override void List(MyContext context)
        {
            context.ListLog.Append("run program " + Name);
        }
    }
}