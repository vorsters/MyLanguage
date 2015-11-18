tree grammar MyLangTree;

options {
  language = CSharp3;
  tokenVocab = MyLangParser;
  ASTLabelType = CommonTree;
  backtrack = true;
  k = 3;
}

@namespace { MyLang.Grammar }


@header {
  //using System.Collections.Generic;
  
  using System;
  using MyLang.Runtime;
}

runprogram returns [MyRunProgramNode result]
    :
    ^(RUNPROG progname=IDENTIFIER) {$result = new MyRunProgramNode(progname.Text);}
    ;

public program returns [MyProgramDeclNode result]
	@init 
		{	    	
			List<MyFunctionDecleration> funcs = new List<MyFunctionDecleration>();
			string programName = String.Empty;
			MyStatementListNode statementListNode = null;
		}
    @after
	    {
            $result = new MyProgramDeclNode(programName, funcs, statementListNode);
	    }    
    :
    ^(PROGDECL progname=identifier {programName = progname;}
    		((f=funcdecl {funcs.Add(f);})*) bs=blockstatement) {statementListNode=bs;}
    ;
    
identifier returns [string result]
	:
	n=IDENTIFIER {$result = $n.Text;}	
	;    

blockstatement  returns [MyStatementListNode result]  
	@init
		{
			$result = new MyStatementListNode();
			int j = 0;
		}
	@after	
		{
		}
    : ^(BLOCKSTMT 
    	(st = statement { $result.AddStatement(st); } )* {j++;})
    ;

assignment returns [MyAssignmentNode result]
    : ^(ASSIGN IDENTIFIER e=expression) 
        {
            MyVariableNode v = new MyVariableNode($IDENTIFIER.Text);
            $result = new MyAssignmentNode(v,e);
        }
    ;

ifstatement returns [MyIfStatement result]
    : 
    IF condition=expression thenpart=blockstatement 
    	{
            $result = new MyIfStatement(condition, thenpart);
    	}
    	(elsepart = blockstatement? {$result.ElsePart = elsepart;})
    ;

whilestatement returns [MyWhileStatement result]
    : 
    WHILE e=expression bs=blockstatement {$result = new MyWhileStatement(e, bs); }
    ;


statement returns [MyStatementNode result]
    :   
    ( 
      a=assignment 		{$result = a;}
    | i=ifstatement 	{$result = i;}
    | r=returnstatement	{$result = r;}
    | w=whilestatement	{$result = w;}
    | bs=blockstatement	{$result = bs;}
    )
    ;
returnstatement returns [MyReturnStatement result]
    : ^(RETURN e=expression {$result = new MyReturnStatement(e);})
    ;


funcdecl returns [MyFunctionDecleration result = new MyFunctionDecleration()]
	@init
		{
			$result = new MyFunctionDecleration();
		}    
	@after
		{
		}    
    : 
    ^(FUNCDECL 
    funcname = IDENTIFIER 
    	{
    		$result.Name = funcname.Text;
    	}
   	( p = IDENTIFIER  {$result.Params.Add(p.Text);} )* 
   	bs=blockstatement
        {
            $result.Statements = bs;
        })
    ;

funccall returns [MyFunctionCallNode result]
	@init
	{
		$result = new MyFunctionCallNode();
	}
    :  ^(FUNCCALL 
    	id = IDENTIFIER 
   		{
		$result.Name = id.Text;
   		}
    	vpl = valueparamlist 
		{
		    $result.Args = vpl;
		}
        )
    ;


valueparamlist returns [List<MyAbstractNode> result]    
    : e = expression { 
                        $result = new List<MyAbstractNode>();
                        $result.Add(e);
                     }
      (e = expression {$result.Add(e);} )*
    ;
    

         

    

expression returns [MyAbstractNode result]
    : ^(PLUS left=expression right=expression	{ $result = new MyAddNode(left, right);})
    | ^(MINUS  left=expression right=expression	{ $result = new MyMinusNode(left, right);})
    | ^(MULTIPLY  left=expression right=expression{ $result = new MyMultiplyNode(left, right);})
    | ^(DIVIDE  left=expression right=expression	{ $result = new MyDivideNode(left, right);})
    | ^(AND left=expression right=expression		{ $result = new MyLogicalAndNode(left, right);})
    | ^(OR left=expression right=expression		{ $result = new MyLogicalOrNode(left, right);})
    | ^(EQUALS left=expression right=expression	{ $result = new MyIsEqualsNode(left, right);})
    | ^(GT left=expression right=expression		{ $result = new MyIsGreaterThanNode(left, right);})
    | ^(GTEQ left=expression right=expression		{ $result = new MyIsGreaterThanOrEqualNode(left, right);})
    | ^(LT left=expression right=expression		{ $result = new MyIsLessThanNode(left, right);})
    | ^(LTEQ left=expression right=expression		{ $result = new MyIsLessThanOrEqualNode(left, right);})
    | ^(NEGATE op=expression						{ $result = new MyNegateNode(op);})
    | i=INT   									{ $result = new MyValueNode(Int32.Parse(i.Text));}
    | f=FLOAT 									{ $result = new MyValueNode(Double.Parse(f.Text));}
    | ^(VAR id=IDENTIFIER 						{ $result = new MyVariableNode(id.Text);})
    | fc=funccall 								{ $result = fc;}
    ;

/*    
primary returns [MyAbstractNode result]
    : INT   {$result = new MyValueNode(Int32.Parse($INT.Text));}
    | FLOAT {$result = new MyValueNode(Double.Parse($FLOAT.Text));}
    | id=IDENTIFIER {$result = new MyVariableNode(id.Text);}
    | fc=funccall {$result = fc;}
    | e=expression {$result = e;}
    ;
*/    
