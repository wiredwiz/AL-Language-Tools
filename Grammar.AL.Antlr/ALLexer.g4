lexer grammar ALLexer;

channels { COMMENTS_CHANNEL }

SINGLE_LINE_COMMENT
	: '//' INPUT_CHARACTER* -> channel(COMMENTS_CHANNEL);

DELIMITED_COMMENT
	: '/*' .*? '*/' -> channel(COMMENTS_CHANNEL);

WS
	:	[ \t\r\n] -> channel(HIDDEN)
	;

// SYMBOLS

SCOPE
   : '::';

RANGE
   : '..';

SEMICOLON
   : ';';

COLON
   : ':';

COMMA
   : ',';

PERIOD
   : '.';

GREATERTHANEQUAL
   : '>=';

LESSTHANEQUAL
   : '<=';

NOTEQUAL
   : '<>';

EQUAL
   : '=';

ASSGN
   : ':=';

MULTIPLY_ASSGN
   : '*=';

DIV_ASSGN
   : '/=';

ADD_ASSGN
   : '+=';

MINUS_ASSGN
   : '-=';

ASTERISK
   : '*';

BACKSLASH
   : '/';

PLUS
   : '+';

MINUS
   : '-';

LESSTHAN
   : '<';

GREATERTHAN
   : '>';

LEFTPAREN
   : '(';

RIGHTPAREN
   : ')';

LEFTBRACKET
   : '[';

RIGHTBRACKET
   : ']';

LEFTCBRACE
   : '{';

RIGHTCBRACE
   : '}';

PIPE
   : '|';

AMPERSAND
   : '&';

CONDITION
   : '?' ;

/*
 * operator keywords
 */

AND
   : A N D
   ;

DIV
   : D I V
   ;

MOD
   : M O D
   ;

NOT
   : N O T
   ;

OR
   : O R
   ;

XOR
   : X O R
   ;

// WORDS

ARRAY
   : A R R A Y
   ;

ASSERTERROR
   : A S S E R T E R R O R
   ;

BEGIN
   : B E G I N
   ;

BREAK
   : B R E A K
   ;

CASE
   : C A S E
   ;

CONTINUE
   : C O N T I N U E
   ;

DO
   : D O
   ;

DOWNTO
   : D O W N T O
   ;

ELSE
   : E L S E
   ;

END
   : E N D
   ;

EXIT
   : E X I T
   ;

FOR
   : F O R
   ;

FOREACH
   : F O R E A C H
   ;

FUNCTION
   : F U N C T I O N
   ;

GUIALLOWED
   : G U I A L L O W E D
   ;

IF
   : I F
   ;

IMPLEMENTS
   : I M P L E M E N T S
   ;

IN
   : I N
   ;

INDATASET
   : I N D A T A S E T
   ;

INTERFACE
   : I N T E R F A C E
   ;

INTERNAL
   : I N T E R N A L
   ;

LOCAL
   : L O C A L
   ;

OF
   : O F
   ;

PROCEDURE
   : P R O C E D U R E
   ;

PROGRAM
   : P R O G R A M
   ;

PROTECTED
   : P R O T E C T E D
   ;

REPEAT
   : R E P E A T
   ;

RUNONCLIENT
   : R U N O N C L I E N T
   ;

SECURITYFILTERING
   : S E C U R I T Y F I L T E R I N G
   ;

SUPPRESSDISPOSE
   : S U P P R E S S D I S P O S E
   ;

TEMPORARY
   : T E M P O R A R Y
   ;

THEN
   : T H E N
   ;

TO
   : T O
   ;

TRIGGER
   : T R I G G E R
   ;

UNTIL
   : U N T I L
   ;

VAR
   : V A R
   ;

WHILE
   : W H I L E
   ;

WITH
   : W I T H
   ;

WITHEVENTS
   : W I T H E V E N T S
   ;

/*
 * boolean
 */

TRUE
   : T R U E;

FALSE
   : F A L S E;

/*
 * date
 */

DATE_LITERAL
   : DIGIT+ D;

/*
 * time
 */

TIME_LITERAL
   : DIGIT+ ([.] DIGIT+)? T;

/*
 * datetime
 */

DATETIME_LITERAL
   : DIGIT+ D T;

/*
 * numbers
 */

INTEGER_LITERAL
   : DIGIT+
   | ('0' X HEXDIGIT*? | DIGIT+) (EXPONENT_NOTATION EXPONENT_SIGN DIGIT+)? (INTEGER_SUFFIX | FLOAT_SUFFIX)?
   ;

FLOAT_LITERAL
	: (DIGIT+ [.] (DIGIT*)? {_input.La(1) != '.'}? (EXPONENT_NOTATION EXPONENT_SIGN DIGIT+)?
	| [.] DIGIT+ (EXPONENT_NOTATION EXPONENT_SIGN DIGIT+)?
	| DIGIT+ EXPONENT_NOTATION EXPONENT_SIGN DIGIT+) (INTEGER_SUFFIX | FLOAT_SUFFIX)?
	;

/*
 * strings
 */

STRING_LITERAL
	: '\'' ( ESC | ~['\r\n])* '\'';

/*
 * identifiers
 */

IDENTIFIER
	: (LETTER | DIGIT | UNDERSCORE)+
   | '"' ~["]+ '"'
	;

UNDERSCORE
	: '_';

LETTER
	: LOWERCASE
	| UPPERCASE
	;

/*
 * preprocessor directives
 */

PREPROCESSOR_DIRECTIVE
   : '#' ~[\r\n]* '\r'? '\n';

/*
 * fragments
 */

fragment LOWERCASE
	: [a-z] ;

fragment UPPERCASE
	: [A-Z] ;

fragment EXPONENT_NOTATION
	: ('E' | 'e');

fragment EXPONENT_SIGN
	: ('-' | '+');

fragment DIGIT
	: [0-9] ;

fragment HEXDIGIT : [0-9] | [A-F] | [a-f];

fragment INTEGER_SUFFIX
   : [uU] [lL]? [lL]? | [lL] [lL]?;

fragment FLOAT_SUFFIX
   : [fF];

fragment ESC
	: '\'\'' ;

fragment INPUT_CHARACTER
	: ~[\r\n\u0085\u2028\u2029];

fragment A : [aA];
fragment B : [bB];
fragment C : [cC];
fragment D : [dD];
fragment E : [eE];
fragment F : [fF];
fragment G : [gG];
fragment H : [hH];
fragment I : [iI];
fragment J : [jJ];
fragment K : [kK];
fragment L : [lL];
fragment M : [mM];
fragment N : [nN];
fragment O : [oO];
fragment P : [pP];
fragment Q : [qQ];
fragment R : [rR];
fragment S : [sS];
fragment T : [tT];
fragment U : [uU];
fragment V : [vV];
fragment W : [wW];
fragment X : [xX];
fragment Y : [yY];
fragment Z : [zZ];