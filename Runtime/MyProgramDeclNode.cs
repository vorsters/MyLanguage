using System;
using System.Collections.Generic;

namespace MyLang.Runtime
{
    public class MyProgramDeclNode : MyNonTerminatingNode
    {
        public MyProgramDeclNode(string name, List<MyFunctionDeclerationRVD> funcs, MyStatementListNode programStatements)
        {
            Name = name;
            Funcs = funcs; 
            Statements = programStatements;
        }

        public List<MyFunctionDeclerationRVD> Funcs { get; set; }

        public MyStatementListNode Statements { get; set; }

        public string Name { get; set; }

        public override object Evaluate(MyContext context)
        {
            context.AddLog("declaring program: " + Name);
            context.AddLog(Environment.NewLine);

            MyContext ctx = new MyContext();

            List(ctx);

            context.AddLog(ctx.ListLog.ToString());

            context.AddLog(Environment.NewLine);

            context.AddProgram(Name, this);
            return 0;
        }

        public override void List(MyContext context)
        {
            context.ListLog.Append("declare program: ").Append(Name).NewLine().Append("{").NewLine();

            context.ListLog.StepDown();
            foreach (var func in Funcs)
            {
                func.List(context);
                context.ListLog.NewLine();
            }

            Statements.List(context);

            context.ListLog.StepUp();
            context.ListLog.NewLine().Append("}").NewLine();
        }

    }
}