lexer grammar MyLangLexer;

options {
  language = CSharp3;
}

tokens {
    RUNPROG;
    PROGDECL;
    BLOCKSTMT;
    VAR;
    NEGATE;
    FUNCDECL;
    FUNCCALL;
    WS;
    ENDP; 
}

@namespace { MyLang.Grammar}


@header {   
    // turn of unreachable code warnings
    // turn off obsolete code warnings
    // turn off unused variable
    # pragma warning disable 0162,0618,0219 
    using System.Text;
}

ENDP 	: 'end';

ASSIGN  : ':=';
    
PLUS    : '+';
    
MINUS   : '-';
    
MULTIPLY: '*';
    
DIVIDE  : '/'; 

LPAREN  : '(';
    
RPAREN  : ')';

EQUALS : '==';

GT : '>';

LT : '<';

LTEQ : '<=';

GTEQ : '=>';

OR      : 'OR';

AND     : 'AND';

WHILE   : 'while';

DECLARE : 'declare';

PROGRAM : 'program';

FUNCTION : 'function';

IF : 'if';

RUN : 'run';

THEN : 'then';

ELSE : 'else';

CALL : 'call';

RETURN : 'return';

LCURLY : '{';

RCURLY : '}';

SEMI: ';';

WS : (' ' | '\t' | '\n' | '\r' | '\f')+ {$channel = Hidden;};

COMMENT
    :   '//' ~('\n'|'\r')* '\r'? '\n' {$channel=Hidden;}
    |   '/*' ( options {greedy=false;} : . )* '*/' {$channel=Hidden;}
    ;
  
INT     : DIGIT+;

FLOAT
    :   DIGIT+ DOT DIGIT+
    ;

/*
FLOAT
    :   (DIGIT)+ DOT (DIGIT)* EXPONENT?
    |   DOT (DIGIT)+ EXPONENT?
    |   (DIGIT)+ EXPONENT
    ;
*/

IDENTIFIER  :   (LCLETTER|UCLETTER|USCORE) (LCLETTER|UCLETTER|DIGIT|USCORE)*
    ;

COMMA : ',';


COLON: ':';

fragment
EXPONENT : ('e'|'E') (PLUS|MINUS)? (DIGIT)+ ;

fragment
DOT     : '.';

fragment
USCORE  : '_';

fragment    
DIGIT   : ('0'..'9');

fragment
LCLETTER: ('a'..'z');

fragment
UCLETTER: ('A'..'Z');

