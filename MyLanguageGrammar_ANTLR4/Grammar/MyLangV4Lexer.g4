lexer grammar MyLangV4Lexer;

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

WS : (' ' | '\t' | '\n' | '\r' | '\f')+ -> skip;

COMMENT
    :   ( '//' ~('\n'|'\r')* '\r'? '\n' 
        |   '/*' .*? '*/' 
        ) -> skip
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

