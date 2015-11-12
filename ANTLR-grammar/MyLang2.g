grammar MyLang2;

options {
  language = CSharp3;
  output = AST;
  backtrack = true;
  ASTLabelType = CommonTree;
  k = 2;
} 


tokens {
    RUNPROG;
    PROGDECL;
    BLOCKSTMT;
    VAR; 
    NEGATE;
    FUNCDECL;  
    FUNCCALL;
}
 
@parser::namespace { PlayingWithDLR.MyScriptRuntime.Script } 

@lexer::namespace { PlayingWithDLR.MyScriptRuntime.Script } 

@header {   
    // turn of unreachable code warnings
    // turn off obsolete code warnings
    // turn off unused variable
    # pragma warning disable 0162,0618,0219 
    using System.Text;
}

@lexer::header {    
    // turn of unreachable code warnings
    // turn off obsolete code warnings
    // turn off unused variable
    # pragma warning disable 0162,0618,0219 
    using System.Text;
}
 
ID  :	('a'..'z'|'A'..'Z'|'_') ('a'..'z'|'A'..'Z'|'0'..'9'|'_')*
    ;

INT :	'0'..'9'+
    ;

FLOAT
    :   ('0'..'9')+ '.' ('0'..'9')* EXPONENT?
    |   '.' ('0'..'9')+ EXPONENT?
    |   ('0'..'9')+ EXPONENT
    ;

COMMENT
    :   '//' ~('\n'|'\r')* '\r'? '\n' {$channel=HIDDEN;}
    |   '/*' ( options {greedy=false;} : . )* '*/' {$channel=HIDDEN;}
    ;

WS  :   ( ' '
        | '\t'
        | '\r'
        | '\n'
        ) {$channel=HIDDEN;}
    ;

fragment
EXPONENT : ('e'|'E') ('+'|'-')? ('0'..'9')+ ;

