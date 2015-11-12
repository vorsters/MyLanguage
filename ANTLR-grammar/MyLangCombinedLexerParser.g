grammar MyLangCombinedLexerParser;


options {
  language = CSharp3;
  output = AST;
  backtrack = true;
  ASTLabelType = CommonTree;
  k = 2;   
}

tokens{
    RUNPROG;
    PROGDECL;
    BLOCKSTMT;
    VAR;
    NEGATE='!';
    FUNCDECL; 
    FUNCCALL; 
} 

@parser::namespace { MyLang.Runtime.Script } 

@lexer::namespace { MyLang.Runtime.Script } 


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




public runprogram
    : WS?! RUN WS! PROGRAM WS! IDENTIFIER (SEMI|EOF) -> ^(RUNPROG IDENTIFIER)
    ;

public program
    : 
      WS?! DECLARE WS! PROGRAM COLON  WS! IDENTIFIER LCURLY funcdecl* blockstatement RCURLY EOF -> ^(PROGDECL IDENTIFIER funcdecl* blockstatement)
    ; 


blockstatement 
    : LCURLY statement* RCURLY -> ^(BLOCKSTMT statement*)
    ;

assignment
    :  
    IDENTIFIER ASSIGN expression -> ^(ASSIGN IDENTIFIER expression) 
    ;

ifstatement 
    : 
    IF LPAREN! expression RPAREN! THEN! 
        blockstatement 
    (
        ELSE! 
        blockstatement
    )
    ;

whilestatement 
    : 
    WHILE LPAREN! expression RPAREN! blockstatement
    ;

returnstatement
    : RETURN expression
    ;

statement
    :   
    ( 
      assignment 
    | ifstatement
    | returnstatement
    | whilestatement
    | blockstatement
    )        
    SEMI!;

funcdecl 
    : 
    DECLARE FUNCTION COLON IDENTIFIER LPAREN (IDENTIFIER (COMMA IDENTIFIER)*)? RPAREN blockstatement -> ^(FUNCDECL IDENTIFIER IDENTIFIER* blockstatement)
    ;

funccall 
    : 
CALL IDENTIFIER LPAREN (expression (COMMA expression)*)? RPAREN -> ^(CALL IDENTIFIER expression*); 
   
    
primary 
    : INT 
    | FLOAT
    | IDENTIFIER -> ^(VAR IDENTIFIER)
    | funccall
    | LPAREN! expression RPAREN!
    ;

unaryexpr
    : (NEGATE)* primary
    ;
         
multexpr 
    : unaryexpr ((MULTIPLY|DIVIDE)^ unaryexpr)*
    ;

    
expression 
    : multexpr ((PLUS|MINUS|EQUALS|GT|GTEQ|LT|LTEQ|AND|OR)^ multexpr)* 
    ;


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

RUN : 'run';

PROGRAM : 'program';

DECLARE : 'declare';

FUNCTION : 'function';

IF : 'if';



THEN : 'then';

ELSE : 'else';

CALL : 'call';

RETURN : 'return';

LCURLY : '{';

RCURLY : '}';

SEMI: ';';

WS : (' ' | '\t' | '\n' | '\r' )+ {$channel = Hidden;};

IDENTIFIER  :   (LCLETTER|UCLETTER|USCORE) (LCLETTER|UCLETTER|DIGIT|USCORE)*
    ;
// \'f'
/*
STRING
    :  '"' ( ESC_SEQ | ~('\\'|'"') )* '"'
    ;

CHAR:  '\'' ( ESC_SEQ | ~('\''|'\\') ) '\''
    ;
*/
 
COMMENT 
    :   '//' ~('\n'|'\r')* '\r'? '\n' {$channel=Hidden;}
    |   '/*' ( options {greedy=false;} : . )* '*/' {$channel=Hidden;}
    ;
  
INT     : DIGIT+;

FLOAT
    :   DIGIT+ DOT DIGIT+
    ;


COMMA : ',';


COLON: ':';

fragment
HEX_DIGIT : ('0'..'9'|'a'..'f'|'A'..'F') ;

fragment
ESC_SEQ
    :   '\\' ('b'|'t'|'n'|'f'|'r'|'\"'|'\''|'\\')
    |   UNICODE_ESC
    |   OCTAL_ESC
    ;

fragment
OCTAL_ESC
    :   '\\' ('0'..'3') ('0'..'7') ('0'..'7')
    |   '\\' ('0'..'7') ('0'..'7')
    |   '\\' ('0'..'7')
    ;

fragment
UNICODE_ESC
    :   '\\' 'u' HEX_DIGIT HEX_DIGIT HEX_DIGIT HEX_DIGIT
    ;


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



