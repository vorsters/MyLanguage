parser grammar MyLangParser;

options {
  language = CSharp3;
  output = AST;
  tokenVocab = MyLangLexer;
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


@namespace { MyLanguageImpl.Grammar } 

identifier 
    : IDENTIFIER^;

public runprogram
    : WS? RUN WS PROGRAM WS IDENTIFIER WS? (ENDP | EOF) -> ^(RUNPROG IDENTIFIER);

public program
    : 
      WS? DECLARE WS PROGRAM WS? COLON WS identifier WS? LCURLY WS? funcdecl* WS? blockstatement WS? RCURLY WS? (ENDP |  EOF) -> ^(PROGDECL identifier funcdecl* blockstatement);

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
    )?
    ;

whilestatement 
    : 
    WHILE LPAREN! expression RPAREN! blockstatement
    ;

returnstatement
    : RETURN^ expression
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
    WS? DECLARE WS FUNCTION WS COLON WS IDENTIFIER WS LPAREN (IDENTIFIER (COMMA IDENTIFIER)*)? RPAREN blockstatement -> ^(FUNCDECL IDENTIFIER IDENTIFIER* blockstatement)
    ;

funccall 
    : 
CALL IDENTIFIER LPAREN (expression (COMMA expression)*)? RPAREN -> ^(FUNCCALL IDENTIFIER expression*); 

   
    
primary 
    : INT 
    | FLOAT
    | IDENTIFIER -> ^(VAR IDENTIFIER)
    | funccall
    | LPAREN! expression RPAREN!
    ;

unaryexpr
//    : primary
    : (PLUS! | negation^)* primary
    ;
         
negation
    :MINUS -> NEGATE
    ;         
         
multexpr 
    : unaryexpr ((MULTIPLY|DIVIDE)^ unaryexpr)*
    ;

    
expression 
//    : multexpr ((PLUS|MINUS|EQUALS|GT|GTEQ|LT|LTEQ|AND|OR)^ multexpr)*
    : multexpr ((PLUS|MINUS|EQUALS|GT|GTEQ|LT|LTEQ|AND|OR)^ multexpr)*
    ;



