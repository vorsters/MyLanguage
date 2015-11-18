using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Antlr.Runtime;
using Antlr.Runtime.Tree;
using MyLanguageImpl.Grammar;
using MyLanguageImpl.Runtime;

//using 

namespace TestMyLanguageApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private static MyProgramDeclNode GetProg1ProgramDeclNode()
        {
            var func1Decl = GetMyFuncDecl_func1();
            var func2Decl = GetMyFuncDecl_func2();
            var func3Decl = GetMyFuncDecl_func3();
            var fib = GetMyFuncDecl_fibonaci();

            //*****************************************//

            MyAssignmentNode assignmentNodePF =
                new MyAssignmentNode(
                    new MyVariableNode("PF"),
                    new MyValueNode(10)
                    );

            MyAssignmentNode assignmentNodeA =
                new MyAssignmentNode(
                    new MyVariableNode("A"),
                    new MyAddNode(
                        new MyValueNode(10),
                        new MyValueNode(15)
                        )
                    );

            MyAssignmentNode assignmentNodeB =
                new MyAssignmentNode(
                    new MyVariableNode("B"),
                    new MyAddNode(
                        new MyValueNode(10),
                        new MyVariableNode("A")
                        )
                    );

            //  ( +  ( +  ( + 1 A[25] )  ( * 3 B[35] )  )  ( +  ( +  ( + 70 80  ) 5  )  ( + 6  ( + 9  ( /  ( - 20 10  ) 5  )  )  )  )  ) ;
            MyAddNode expressionTree =
                new MyAddNode(
                    new MyAddNode(
                        new MyAddNode(
                            new MyValueNode(1),
                            new MyVariableNode("A")
                            ),
                        new MyMultiplyNode(
                            new MyValueNode(3),
                            new MyVariableNode("B")
                            )
                        ),
                    new MyAddNode(
                        new MyAddNode(
                            new MyAddNode(
                                new MyValueNode(70),
                                new MyValueNode(80)
                                ),
                            new MyValueNode(5)
                            ),
                        new MyAddNode(
                            new MyValueNode(6),
                            new MyAddNode(
                                new MyValueNode(9),
                                new MyDivideNode(
                                    new MyMinusNode(
                                        new MyValueNode(20),
                                        new MyValueNode(10)
                                        ),
                                    new MyValueNode(5)
                                    )
                                )
                            )
                        )
                    );

            MyAssignmentNode assignmentNodeC =
                new MyAssignmentNode(
                    new MyVariableNode("C"),
                    expressionTree);

            MyAssignmentNode assignmentNodeD =
                new MyAssignmentNode(
                    new MyVariableNode("D"),
                    new MyFunctionCallNode(
                        "func1",
                        new List<MyAbstractNode>()
                            {
                                new MyVariableNode("A"),
                                new MyVariableNode("B"),
                                new MyAddNode(
                                    new MyValueNode(99),
                                    new MyValueNode(101)
                                    )
                            }
                        )
                    );

            MyAssignmentNode assignmentNodeE =
                new MyAssignmentNode(
                    new MyVariableNode("E"),
                    new MyFunctionCallNode(
                        "func1",
                        new List<MyAbstractNode>()
                            {
                                new MyVariableNode("C"),
                                new MyVariableNode("D"),
                                new MyAddNode(
                                    new MyValueNode(1000),
                                    new MyValueNode(2000)
                                    )
                            }
                        )
                    );

            MyAssignmentNode assignmentNodeF =
                new MyAssignmentNode(
                    new MyVariableNode("F"),
                    new MyFunctionCallNode(
                        "fib",
                        new List<MyAbstractNode>()
                            {
                                new MyValueNode(10)
                            }
                        )
                    );

            MyIfStatement ifA_GTEQ_B =
                new MyIfStatement(
                    new MyIsLessThanOrEqualNode(
                        new MyVariableNode("A"),
                        new MyVariableNode("B")
                        ),
                    new MyStatementListNode()
                        .AddStatement(
                            new MyAssignmentNode(
                                new MyVariableNode("AA"),
                                new MyVariableNode("A")
                                )
                        )
                        .AddStatement(
                            new MyAssignmentNode(
                                new MyVariableNode("BB"),
                                new MyVariableNode("B")
                                )
                        ).AddStatement(
                            new MyIfStatement(
                                new MyIsEqualsNode(
                                        new MyVariableNode("A"),
                                        new MyValueNode(25)
                                    ),
                                new MyStatementListNode()
                                .AddStatement(
                                    new MyAssignmentNode(
                                        new MyVariableNode("MMM"),
                                        new MyValueNode(99999)
                                        )
                                ).AddStatement(
                                    new MyAssignmentNode(
                                        new MyVariableNode("NNN"),
                                        new MyValueNode(998888)
                                        )
                                ),
                                new MyStatementListNode()
                                .AddStatement(
                                    new MyAssignmentNode(
                                        new MyVariableNode("MMM"),
                                        new MyValueNode(-99999)
                                        )
                                 ).AddStatement(
                                    new MyAssignmentNode(
                                        new MyVariableNode("MMM"),
                                        new MyValueNode(-99999)
                                        )
                                    )
                                )
                        ),
                    new MyStatementListNode()
                        .AddStatement(
                            new MyAssignmentNode(
                                new MyVariableNode("AA"),
                                new MyVariableNode("B")
                                )
                        )
                        .AddStatement(
                            new MyAssignmentNode(
                                new MyVariableNode("BB"),
                                new MyVariableNode("A")
                                )
                        )
                    );

            MyAssignmentNode assignmentNodeH =
                new MyAssignmentNode(
                    new MyVariableNode("H"),
                    new MyFunctionCallNode(
                        "func2",
                        new List<MyAbstractNode>()
                            {
                                new MyValueNode(-2)
                            }

                        )
                    );

            MyAssignmentNode assignmentNodeI =
                new MyAssignmentNode(
                    new MyVariableNode("I"),
                    new MyFunctionCallNode(
                        "func2",
                        new List<MyAbstractNode>()
                            {
                                new MyValueNode(80)
                            }

                        )
                    );

            MyAssignmentNode assignmentNodeJ =
                new MyAssignmentNode(
                    new MyVariableNode("J"),
                    new MyFunctionCallNode(
                        "func2",
                        new List<MyAbstractNode>()
                            {
                                new MyValueNode(120)
                            }

                        )
                    );

            MyAssignmentNode assignmentNodeK =
                new MyAssignmentNode(
                    new MyVariableNode("K"),
                    new MyFunctionCallNode(
                        "func3",
                        new List<MyAbstractNode>()
                            {
                                new MyValueNode(12)
                            }

                        )
                    );

            MyAssignmentNode assignmentNodeL =
                new MyAssignmentNode(
                    new MyVariableNode("L"),
                    new MyFunctionCallNode(
                        "func3",
                        new List<MyAbstractNode>()
                            {
                                new MyValueNode(120)
                            }

                        )
                    );


            List<MyFunctionDecleration> funcs = new List<MyFunctionDecleration>()
                                                    {
                                                        func1Decl,
                                                        func2Decl,
                                                        func3Decl,
                                                        fib
                                                    };

            MyStatementListNode programStatements = new MyStatementListNode();

            programStatements.AddStatement(assignmentNodePF);

            programStatements.AddStatement(assignmentNodeA);
            programStatements.AddStatement(assignmentNodeB);
            programStatements.AddStatement(assignmentNodeC);
            programStatements.AddStatement(assignmentNodeD);
            programStatements.AddStatement(assignmentNodeE);

            programStatements.AddStatement(assignmentNodeF);


            programStatements.AddStatement(ifA_GTEQ_B);

            programStatements.AddStatement(assignmentNodeH);
            programStatements.AddStatement(assignmentNodeI);
            programStatements.AddStatement(assignmentNodeJ);

            programStatements.AddStatement(assignmentNodeK);
            programStatements.AddStatement(assignmentNodeL);


            var prog1ProgramDecl = new MyProgramDeclNode(
                "prog1_myl",
                funcs,
                programStatements);

            return prog1ProgramDecl;
        }

        private static MyFunctionDecleration GetMyFuncDecl_func2()
        {

            MyStatementListNode stmtlist2t = new MyStatementListNode()
                .AddStatement(
                    new MyReturnStatement(
                        new MyMultiplyNode(
                            new MyParamValue("p1"),
                            new MyParamValue("p1")
                            )
                         )
                    );

            MyStatementListNode stmtlist2e = new MyStatementListNode()
                .AddStatement(
                    new MyReturnStatement(
                        new MyDivideNode(
                            new MyParamValue("p1"),
                            new MyValueNode(2)
                            )
                         )
                );


            MyIfStatement if2 = new MyIfStatement(
                new MyIsGreaterThanNode(
                    new MyParamValue("p1"),
                    new MyValueNode(100)),
                stmtlist2t,
                stmtlist2e
                );



            ////////////////////////////////////////////////////////////////////////////////

            MyStatementListNode stmtlist1t = new MyStatementListNode()
                .AddStatement(
                    new MyReturnStatement(
                        new MyParamValue("p1")
                        )
                    );

            MyStatementListNode stmtlist1e = new MyStatementListNode()
                .AddStatement(
                    if2
                );


            MyIfStatement if1 =
            new MyIfStatement(
                new MyIsLessThanOrEqualNode(
                    new MyParamValue("p1"),
                    new MyValueNode(0)
                    ),
                    stmtlist1t,
                    stmtlist1e
                );


            MyReturnStatement defaultReturnStmt = new MyReturnStatement(
                new MyValueNode(-1)
                );

            MyStatementListNode func2Stmts = new MyStatementListNode()
                 .AddStatement(if1)
                 .AddStatement(defaultReturnStmt);

            var func2 = new MyFunctionDecleration(
                "func2",
                new List<string>() { "p1" }
                );

            func2.Statements = func2Stmts;

            return func2;
        }

        private static MyFunctionDecleration GetMyFuncDecl_func3()
        {

            MyIfStatement if1 = new MyIfStatement(
                new MyIsLessThanNode(
                    new MyParamValue("p1"),
                    new MyValueNode(0)),
                new MyStatementListNode().AddStatement(
                    new MyReturnStatement(
                        new MyValueNode(0)
                        )
                    ),
                    null
                );

            MyAssignmentNode VV = new MyAssignmentNode(
                new MyVariableNode("VV"),
                new MyValueNode(0)
                );


            MyStatementListNode while1Body = new MyStatementListNode()
                .AddStatement(
                    new MyAssignmentNode(
                            new MyVariableNode("VV"),
                            new MyAddNode(
                                new MyVariableNode("VV"),
                                new MyValueNode(1)
                                )
                            )
                        );

            MyWhileStatement while1 =
            new MyWhileStatement(
                new MyIsLessThanOrEqualNode(
                    new MyVariableNode("VV"),
                    new MyParamValue("p1")
                    ),
                    while1Body
                );


            MyReturnStatement defaultReturnStmt = new MyReturnStatement(
                new MyVariableNode("VV")
                );

            MyStatementListNode func3Stmts = new MyStatementListNode()
                 .AddStatement(if1)
                 .AddStatement(VV)
                 .AddStatement(while1)
                 .AddStatement(defaultReturnStmt);

            var func3 = new MyFunctionDecleration(
                "func3",
                new List<string>() { "p1" }
                );

            func3.Statements = func3Stmts;

            return func3;
        }


        private static MyFunctionDecleration GetMyFuncDecl_func1()
        {

            MyAssignmentNode assignmentNodeF1 =
                new MyAssignmentNode(
                    new MyVariableNode("XX"),
                    new MyAddNode(
                        new MyParamValue("p1"),
                        new MyValueNode(15)
                        )
                    );

            MyAssignmentNode assignmentNodeF2 =
                new MyAssignmentNode(
                    new MyVariableNode("B"),
                    new MyAddNode(
                        new MyParamValue("p3"),
                        new MyParamValue("p2")
                        )
                    );

            MyReturnStatement func1ReturnStatement =
                new MyReturnStatement(
                    new MyAddNode(
                        new MyVariableNode("B"),
                        new MyVariableNode("XX")
                        )
                    );


            MyFunctionDecleration func1Decl =
                new MyFunctionDecleration(
                    "func1",
                    new List<string>() { "p1", "p2", "p3" }
                    );


            MyStatementListNode func1StmtList = new MyStatementListNode();

            func1StmtList.AddStatement(assignmentNodeF1);
            func1StmtList.AddStatement(assignmentNodeF2);
            func1StmtList.AddStatement(func1ReturnStatement);

            func1Decl.Statements = func1StmtList;
            return func1Decl;
        }

        private static ulong fib(ulong n)
        {
            if (n == 0 || n == 1)
            {
                return n;
            }
            else
            {
                return fib(n - 1) + fib(n - 2);
            }
        }

        private static MyFunctionDecleration GetMyFuncDecl_fibonaci()
        {
            /*
             * fib(n)
                {
                    if (n == 0 || n == 1)
                    {
                      return n;
                    }
                    else
                    {
                      return fib(n - 1) + fib(n - 2);
                    }
                }
             */

            MyIfStatement ifStatement =
                new MyIfStatement(
                    new MyLogicalOrNode(
                        new MyIsEqualsNode(
                            new MyParamValue("n"),
                            new MyValueNode(0)
                            ),
                        new MyIsEqualsNode(
                            new MyParamValue("n"),
                            new MyValueNode(1)
                            )
                        ),
                    new MyStatementListNode(
                        ).AddStatement(
                            new MyReturnStatement(
                                new MyParamValue("n")
                                )
                        ),
                    new MyStatementListNode(
                        ).AddStatement(
                            new MyReturnStatement(
                                new MyAddNode(
                                    new MyFunctionCallNode("fib",
                                        new List<MyAbstractNode>()
                                            {
                                                new MyMinusNode(
                                                    new MyParamValue("n"), 
                                                    new MyValueNode(1))
                                            }
                                        ),
                                     new MyFunctionCallNode("fib",
                                        new List<MyAbstractNode>()
                                            {
                                                new MyMinusNode(
                                                    new MyParamValue("n"), 
                                                    new MyValueNode(2))
                                            }
                                        )
                                    )
                                )
                        )
                );

            MyFunctionDecleration fib =
                new MyFunctionDecleration(
                    "fib",
                    new List<string>() { "n" }
                    );


            MyStatementListNode fibonaciStmtList = new MyStatementListNode();

            fibonaciStmtList.AddStatement(ifStatement);

            fib.Statements = fibonaciStmtList;
            return fib;
        }

        MyContext _context = new MyContext();
        private AstParserRuleReturnScope<CommonTree, IToken> _ast;
        private MyProgramDeclNode _currentProgram;

        private void Form1_Load(object sender, EventArgs e)
        {
            var c = new MyContext();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //*****************************************//
            try
            {
                _context.sb.Clear();

                var runProgramNode = new MyRunProgramNode(_currentProgram.Name);

                runProgramNode.Evaluate(_context);

                foreach (var varkey in _context.variables.Keys)
                {
                    textBox3.AppendText(varkey + " => " + _context.GetVariable(varkey));
                    textBox3.AppendText(Environment.NewLine);
                }

                textBox2.AppendText(_context.sb.ToString());

            }
            catch (Exception ex)
            {
                textBox2.AppendText(Environment.NewLine);

                textBox2.AppendText(ex.Message);
                textBox2.AppendText(ex.StackTrace);

                textBox2.AppendText(Environment.NewLine);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _currentProgram.List(_context);
            textBox1.AppendText(_context.ListLog.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            _context = new MyContext();
        }

        private void button4_Click(object sender, EventArgs e)  
        {
            // declare program
            _currentProgram = GetProg1ProgramDeclNode();
            _currentProgram.Evaluate(_context);
            textBox2.AppendText(">>> " + _currentProgram.Name + " loaded.");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                ANTLRStringStream stream = new ANTLRStringStream(textBox1.Text);
                MyLangLexer lexer = new MyLangLexer(stream);
                CommonTokenStream tokenStream = new CommonTokenStream(lexer);
                MyLangParser parser = new MyLangParser(tokenStream);

                _ast = parser.program();
                textBox2.Text = _ast.Tree.ToStringTree();
            }
            catch (Exception exception)
            {
                textBox2.AppendText(Environment.NewLine);
                textBox2.AppendText(exception.ToString());
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            var nodes = new CommonTreeNodeStream(_ast.Tree);

            MyLangTree walker = new MyLangTree(nodes);

            MyProgramDeclNode prog = walker.program();

            if (prog == null)
            {
                textBox2.AppendText(Environment.NewLine);
                textBox2.AppendText("prog is null! Tree walk failed.");
                return;
            }

            _currentProgram = prog;

            textBox2.AppendText(Environment.NewLine);
            textBox2.AppendText("prog: " + prog.Name + " walked!");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (_context.programs.ContainsKey(_currentProgram.Name))
            {
                _context = new MyContext();
            }

            textBox2.AppendText(Environment.NewLine);
            textBox2.AppendText("evaluation prog " + _currentProgram.Name);

            _currentProgram.Evaluate(_context);

            textBox2.AppendText(Environment.NewLine);
            textBox2.AppendText(_currentProgram.Name + " loaded");

        }
    }

    //public partial class MyLangParser : 

}
