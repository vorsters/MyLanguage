using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MyLanguageImpl.Runtime
{
    public class MyStatementListNode : MyStatementNode
    {
        List<MyStatementNode> statements = new List<MyStatementNode>();

        public MyStatementListNode()
        {

        }

        public override object Evaluate(MyContext context)
        {
            object o = DoStatement(context);
            return o;
        }


        public MyStatementListNode AddStatement(MyStatementNode statement)
        {
            statements.Add(statement);
            return this;
        }

        public override object DoStatement(MyContext context)
        {
            object o = 0;

            foreach (var statementNode in statements)
            {
                o = statementNode.Evaluate(context);
                context.AddLog(Environment.NewLine);

                if (context.returned)
                {
                    return o;
                }
            }

            return o;
        }

        public override void List(MyContext context)
        {
            context.ListLog.Append("{");
            context.ListLog.NewLine();
            context.ListLog.StepDown();
            foreach (var statementNode in statements)
            {
                statementNode.List(context);

                if (statementNode is MyAssignmentNode || 
                    statementNode is MyReturnStatement || 
                    statementNode is MyVarIncrementNode)
                {
                    context.ListLog.Append(";");
                }

                context.ListLog.NewLine();
            }
            context.ListLog.StepUp();
            context.ListLog.Append("}");
        }
    }
}