using System;

namespace MyLanguageImpl.Runtime
{
    public enum IncrOp
    {
        VarPlusPlus,
        VarPlusEquals,
        VarMinusMinus,
        VarMinusEquals,
        PlusPlusVar,
        MinusMinusVar
    }
    public class MyVarIncrementNode : MyStatementListNode
    {
        private string varName;
        private MyAbstractNode incExprNode;
        private MyVariableNode varNode;
        private IncrOp incrOp;

        public MyVarIncrementNode(string varName, IncrOp incrOp, MyAbstractNode incExprNode = null)
        {
            this.varName = varName;
            this.incrOp = incrOp;

            MyAbstractNode expr = null;
            MyBinaryOpNode opNode = null;

            if (incExprNode == null)
            {
                expr = new MyValueNode(1);
            }
            else
            {
                expr = incExprNode;
            }

            varNode = new MyVariableNode(varName);

            this.incExprNode = expr;


            switch (incrOp)
            {
                case IncrOp.VarPlusPlus:
                case IncrOp.PlusPlusVar:
                case IncrOp.VarPlusEquals:
                    opNode = new MyAddNode(varNode, expr);
                    break;

                case IncrOp.VarMinusMinus:
                case IncrOp.MinusMinusVar:
                case IncrOp.VarMinusEquals:
                    opNode = new MyMinusNode(varNode, expr);
                    break;
            }

            string t_name = string.Format("_t_{0}", varName);
            MyStatementListNode stmtList = new MyStatementListNode();

            MyVariableNode t_i = new MyVariableNode(t_name);
            MyAssignmentNode ass_t_i = new MyAssignmentNode(t_i, varNode);
            
            MyAssignmentNode ass_myVar = new MyAssignmentNode(varNode, opNode);
            MyExpressionStatementNode t_i_Stmt = new MyExpressionStatementNode(t_i);

            if (incrOp == IncrOp.PlusPlusVar || incrOp == IncrOp.MinusMinusVar)
            {
                AddStatement(ass_myVar);
            }
            else
            {
                AddStatement(ass_t_i);
                AddStatement(ass_myVar);
                AddStatement(t_i_Stmt);
            }
        }

        public override object Evaluate(MyContext context)
        {
            var result = base.Evaluate(context);
            string t_name = string.Format("_t_{0}", varName);
            context.DestoryVariable(t_name);

            return result;
        }

        public override void List(MyContext context)
        {
            switch (incrOp)
            {
                case IncrOp.VarPlusPlus:
                    varNode.List(context);
                    context.ListLog.Append("++ ");
                    break;
                case IncrOp.VarPlusEquals:
                    varNode.List(context);
                    context.ListLog.Append(" += ");
                    incExprNode.List(context);
                    break;
                case IncrOp.VarMinusMinus:
                    varNode.List(context);
                    context.ListLog.Append("-- ");
                    break;
                case IncrOp.VarMinusEquals:
                    varNode.List(context);
                    context.ListLog.Append(" -= ");
                    incExprNode.List(context);
                    break;
                case IncrOp.PlusPlusVar:
                    context.ListLog.Append(" ++");
                    varNode.List(context);
                    break;
                case IncrOp.MinusMinusVar:
                    context.ListLog.Append(" --");
                    varNode.List(context);
                    break;
            }

        }
    }
}