lexer grammar AntlrEntityLexer;


options { caseInsensitive=true; }

// keywords
KW_CLASS: 'class';
MODULE	: 'module';
IMPORT	: 'import';
TYPE	: 'type';

// symbols
COLON	: ':';
SEMI	: ';';
COMMA	: ',';
DOT		: '.';
LSQRD	: '[';
RSQRD	: ']';
LCRLY	: '{';
RCRLY	: '}';
LPAREN	: '(';
RPAREN	: ')';

// operators
ADD		: '+';
SUB		: '-';
MUL		: '*';
DIV		: '/';
EQ		: '=';

// literals
STRING	: '"' .*? '"';
INTEGER	: '0'|[1-9][0-9]*;
BOOLEAN	: 'true'|'false';
REAL	: INTEGER DOT INTEGER;

// identifier
ID		: [A-Z]+;

// comments
COMMENT : '//' ~[\r\n]* '\r'? '\n' -> skip;

// whitespaces
WS: [ \r\n\t]+ -> channel(HIDDEN);
