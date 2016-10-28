parser grammar MyLangV4Parser;

options {
  tokenVocab = MyLangV4Lexer;
}

//identifier 
//    :   IDENTIFIER
//    ;
    
runprogram
    : RUN PROGRAM IDENTIFIER (ENDP | EOF)
    ;

program
    : 
      DECLARE PROGRAM COLON progname=IDENTIFIER LCURLY funcdecl* blockstatement RCURLY (ENDP |  EOF)
    ;

assignment
    : IDENTIFIER ASSIGN expression
    ;

returnstatement
    : RETURN expression
    ;

blockstatement 
    : LCURLY statement* RCURLY 
    ;

ifstatement
    : 
    IF LPAREN condition=expression RPAREN THEN 
        thenpart=blockstatement 
    (
        ELSE
        elsepart=blockstatement
    )?
    ;

whilestatement
    : 
    WHILE LPAREN condition=expression RPAREN dopart=blockstatement
    ;

//statement
//    : assignment #assignment1
//    | returnstatement #returnstatement1
//    | blockstatement #blockstatement1
//    | ifstatement #ifstatement1
//    | whilestatement #whilestatement1
//    ;

statement
    : assignment SEMI
    | returnstatement SEMI
    | blockstatement
    | ifstatement
    | whilestatement
    ;

funcdecl 
    : 
    DECLARE FUNCTION COLON funcname=IDENTIFIER LPAREN (params+=IDENTIFIER (COMMA params+=IDENTIFIER)*)? RPAREN blockstatement
    ;

funccall 
    : 
    CALL funcname=IDENTIFIER LPAREN (args+=expression (COMMA args+=expression)*)? RPAREN 
    ;

parenexpression 
    : LPAREN expression RPAREN
    ;

negateexpression
    : MINUS expression
    ;

posateexpression
    : PLUS expression
    ;

expression
    : parenexpression #parenexpr
    | left=expression MULTIPLY right=expression #multiply
    | left=expression DIVIDE right=expression #divide
    | left=expression PLUS right=expression #plus
    | left=expression MINUS right=expression #minus
    | left=expression AND right=expression #and
    | left=expression OR right=expression #or
    | left=expression EQUALS right=expression #equals
    | left=expression GT right=expression #gt
    | left=expression GTEQ right=expression #gteq
    | left=expression LT right=expression #lt
    | left=expression LTEQ right=expression #lteq
    | INT #int
    | FLOAT #float
    | negateexpression #negate
    | posateexpression #posate
    | IDENTIFIER #varcall
    | funccall #funcallexpr
    ;