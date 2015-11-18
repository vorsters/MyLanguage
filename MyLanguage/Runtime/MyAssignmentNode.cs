using System;

namespace MyLang.Runtime
{
    public class MyAssignmentNode : MyStatementNode
    {
        public MyVariableNode VariableNode { get; set; }
        public MyAbstractNode ValueNode { get; set; }

        public MyAssignmentNode(MyVariableNode variableNode, MyAbstractNode valueNode)
        {
            VariableNode = variableNode;
            ValueNode = valueNode;
        }

        public override object DoStatement(MyContext context)
        {
            context.AddLog(VariableNode.Name + " := ");

            IConvertible value = (IConvertible)ValueNode.Evaluate(context);

            if (!context.ContainsVariable(VariableNode.Name))
            {
                context.AddVariable(VariableNode.Name, value);
            }
            else
            {
                context.SetVariable(VariableNode.Name, value);
            }

            return value;
        }

        public override void List(MyContext context)
        {
            VariableNode.List(context);
            context.ListLog.Append(" := ");
            ValueNode.List(context);
            context.ListLog.Append(";");
        }
    }
}