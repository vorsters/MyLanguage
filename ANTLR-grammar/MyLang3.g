grammar MyLang3;


options {
  language = CSharp3;
  output=AST;
  ASTLabelType=CommonTree;
}

evaluator
	:	assignment* INT EOF!
	;
	
assignment
	:	IDENT ':='^ INT ';'!
	;

IDENT 
	:	 'a'..'z'+
	;

INT :	'0'..'9'+
    ;

WS  :   ( ' '
        | '\t'
        | '\r'
        | '\n'
        ) {$channel=HIDDEN;}
    ;

