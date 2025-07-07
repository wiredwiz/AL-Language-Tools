lexer grammar ALLexer;

channels {
   COMMENTS,
   DIRECTIVE
}
/*
 * fragments
 */

// Because AL is case insensitive, we define fragments for each letter that we use further down

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

fragment LOWERCASELETTER
	: [a-z] ;

fragment UPPERCASELETTER
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

fragment WHITESPACE
   : UNICODE_CLASS_ZS //'<Any Character With Unicode Class Zs>'
   | '\u0009'     //'<Horizontal Tab Character (U+0009)>'
   | '\u000B'     //'<Vertical Tab Character (U+000B)>'
   | '\u000C'     //'<Form Feed Character (U+000C)>'
   ;

fragment UNICODE_CLASS_ZS
   : '\u0020'   // SPACE
   | '\u00A0' // NO_BREAK SPACE
   | '\u1680' // OGHAM SPACE MARK
   | '\u180E' // MONGOLIAN VOWEL SEPARATOR
   | '\u2000' // EN QUAD
   | '\u2001' // EM QUAD
   | '\u2002' // EN SPACE
   | '\u2003' // EM SPACE
   | '\u2004' // THREE_PER_EM SPACE
   | '\u2005' // FOUR_PER_EM SPACE
   | '\u2006' // SIX_PER_EM SPACE
   | '\u2008' // PUNCTUATION SPACE
   | '\u2009' // THIN SPACE
   | '\u200A' // HAIR SPACE
   | '\u202F' // NARROW NO_BREAK SPACE
   | '\u3000' // IDEOGRAPHIC SPACE
   | '\u205F' // MEDIUM MATHEMATICAL SPACE
   ;

fragment NEWLINE
   : '\r\n'
   | '\r'
   | '\n'
   | '\u0085' // <Next Line CHARACTER (U+0085)>'
   | '\u2028' //'<Line Separator CHARACTER (U+2028)>'
   | '\u2029' //'<Paragraph Separator CHARACTER (U+2029)>'
   ;

/*
 * Whitespace
 */

WHITE_SPACE
	:	WHITESPACE+ -> channel(HIDDEN)
	;

NEW_LINE
   :	NEWLINE+ -> channel(HIDDEN)
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
 * comments
 */

SINGLE_LINE_COMMENT
	: '//' INPUT_CHARACTER* -> channel(COMMENTS);

DELIMITED_COMMENT
	: '/*' .*? '*/' -> channel(COMMENTS);

/*
 * Symbols
 */

HASH
   : '#' -> mode(DIRECTIVE_MODE), skip;

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

/*
 * strings
 */

STRING_LITERAL
	: '\'' ( ESC | ~['\r\n])* '\'';

/*
 * keywords
 */

ABS
   : A B S
   ;

ACTION
   : A C T I O N
   ;

ACTIONREF
   : A C T I O N R E F
   ;

ACTIVATE
   : A C T I V A T E
   ;

ACTIVE
   : A C T I V E
   ;

ACTIVECONTROLONOPEN
   : A C T I V E C O N T R O L O N O P E N
   ;

ADDLINK
   : A D D L I N K
   ;

ADDTEXT
   : A D D T E X T
   ;

ALTSEARCHFIELD
   : A L T S E A R C H F I E L D
   ;

APPLICATIONPATH
   : A P P L I C A T I O N P A T H
   ;

ARRAY
   : A R R A Y
   ;

ARRAYLEN
   : A R R A Y L E N
   ;

ASCENDING
   : A S C E N D I N G
   ;

ASSERTERROR
   : A S S E R T E R R O R
   ;

ASSISTEDIT
   : A S S I S T E D I T
   ;

AUDITCATEGORY
   : A U D I T C A T E G O R Y
   ;

AUTOCALCFIELD
   : A U T O C A L C F I E L D
   ;

AUTOENTER
   : A U T O E N T E R
   ;

AUTOFORMATEXPR
   : A U T O F O R M A T E X P R
   ;

AUTOFORMATTYPE
   : A U T O F O R M A T T Y P E
   ;

AUTOINCREMENT
   : A U T O I N C R E M E N T
   ;

AUTOMATION
   : A U T O M A T I O N
   ;

AUTOPOSITION
   : A U T O P O S I T I O N
   ;

AUTOREPEAT
   : A U T O R E P E A T
   ;

AUTOREPLACE
   : A U T O R E P L A C E
   ;

AUTOSAVE
   : A U T O S A V E
   ;

AUTOSPLITKEY
   : A U T O S P L I T K E Y
   ;

AUTOUPDATE
   : A U T O U P D A T E
   ;

AVERAGE
   : A V E R A G E
   ;

BACKCOLOR
   : B A C K C O L O R
   ;

BACKTRANSPARENT
   : B A C K T R A N S P A R E N T
   ;

BACKUPKEY
   : B A C K U P K E Y
   ;

BEEP
   : B E E P
   ;

BEGIN
   : B E G I N
   ;

BIGINTEGER
   : B I G I N T E G E R
   ;

BIGTEXT
   : B I G T E X T
   ;

BINARY
   : B I N A R Y
   ;

BITMAP
   : B I T M A P
   ;

BITMAPLIST
   : B I T M A P L I S T
   ;

BITMAPPOS
   : B I T M A P P O S
   ;

BLANKNUMBERS
   : B L A N K N U M B E R S
   ;

BLANKZERO
   : B L A N K Z E R O
   ;

BLOB
   : B L O B
   ;

BOOLEAN
   : B O O L E A N
   ;

BORDER
   : B O R D E R
   ;

BORDERCOLOR
   : B O R D E R C O L O R
   ;

BORDERSTYLE
   : B O R D E R S T Y L E
   ;

BORDERWIDTH
   : B O R D E R W I D T H
   ;

BOTTOMMARGIN
   : B O T T O M M A R G I N
   ;

BREAK
   : B R E A K
   ;

BYTE
   : B Y T E
   ;

CALCDATE
   : C A L C D A T E
   ;

CALCFIELD
   : C A L C F I E L D
   ;

CALCFIELDS
   : C A L C F I E L D S
   ;

CALCFORMULA
   : C A L C F O R M U L A
   ;

CALCSUM
   : C A L C S U M
   ;

CALCSUMS
   : C A L C S U M S
   ;

CALLFIELDVALIDATE
   : C A L L F I E L D V A L I D A T E
   ;

CANCEL
   : C A N C E L
   ;

CAPTION
   : C A P T I O N
   ;

CAPTIONBAR
   : C A P T I O N B A R
   ;

CAPTIONCLASS
   : C A P T I O N C L A S S
   ;

CAPTIONML
   : C A P T I O N M L
   ;

CARDPAGEID
   : C A R D P A G E I D
   ;

CASE
   : C A S E
   ;

CHANGECOMPANY
   : C H A N G E C O M P A N Y
   ;

CHAR
   : C H A R
   ;

CHARALLOWED
   : C H A R A L L O W E D
   ;

CHARTPARTID
   : C H A R T P A R T I D
   ;

CHECKLICENSEFILE
   : C H E C K L I C E N S E F I L E
   ;

CLASS
   : C L A S S
   ;

CLEAR
   : C L E A R
   ;

CLEARALL
   : C L E A R A L L
   ;

CLEARLASTERROR
   : C L E A R L A S T E R R O R
   ;

CLEARMARKS
   : C L E A R M A R K S
   ;

CLEARONLOOKUP
   : C L E A R O N L O O K U P
   ;

CLIENTTYPE
   : C L I E N T T Y P E
   ;

CLOSE
   : C L O S E
   ;

CLOSINGDATE
   : C L O S I N G D A T E
   ;

CLOSINGDATES
   : C L O S I N G D A T E S
   ;

CLUSTERED
   : C L U S T E R E D
   ;

CODE
   : C O D E
   ;

CODECOVERAGELOG
   : C O D E C O V E R A G E L O G
   ;

CODEUNIT
   : C O D E U N I T
   ;

COMMANDLINE
   : C O M M A N D L I N E
   ;

COMMIT
   : C O M M I T
   ;

COMMITBEHAVIOR
   : C O M M I T B E H A V I O R
   ;

COMPANYNAME
   : C O M P A N Y N A M E
   ;

COMPLETIONTRIGGERERRORLEVEL
   : C O M P L E T I O N T R I G G E R E R R O R L E V E L
   ;

COMPRESSARRAY
   : C O M P R E S S A R R A Y
   ;

COMPRESSED
   : C O M P R E S S E D
   ;

CONFIRM
   : C O N F I R M
   ;

CONNECTIONTYPE
   : C O N N E C T I O N T Y P E
   ;

CONSISTENT
   : C O N S I S T E N T
   ;

CONST
   : C O N S T
   ;

CONTAINERTYPE
   : C O N T A I N E R T Y P E
   ;

CONTEXTURL
   : C O N T E X T U R L
   ;

CONTINUE
   : C O N T I N U E
   ;

CONTROLADDIN
   : C O N T R O L A D D I N
   ;

CONTROLTYPE
   : C O N T R O L T Y P E
   ;

CONVERTSTR
   : C O N V E R T S T R
   ;

COPY
   : C O P Y
   ;

COPYARRAY
   : C O P Y A R R A Y
   ;

COPYFILTER
   : C O P Y F I L T E R
   ;

COPYFILTERS
   : C O P Y F I L T E R S
   ;

COPYLINKS
   : C O P Y L I N K S
   ;

COPYSTR
   : C O P Y S T R
   ;

COPYSTREAM
   : C O P Y S T R E A M
   ;

COUNT
   : C O U N T
   ;

COUNTAPPROX
   : C O U N T A P P R O X
   ;

CREATE
   : C R E A T E
   ;

CREATEDATETIME
   : C R E A T E D A T E T I M E
   ;

CREATEGUID
   : C R E A T E G U I D
   ;

CREATEINSTREAM
   : C R E A T E I N S T R E A M
   ;

CREATEOUTSTREAM
   : C R E A T E O U T S T R E A M
   ;

CREATETEMPFILE
   : C R E A T E T E M P F I L E
   ;

CURRENTDATETIME
   : C U R R E N T D A T E T I M E
   ;

CURRENTEXECUTIONMODE
   : C U R R E N T E X E C U T I O N M O D E
   ;

CURRENTKEY
   : C U R R E N T K E Y
   ;

CURRENTKEYINDEX
   : C U R R E N T K E Y I N D E X
   ;

CURRENTTRANSACTIONTYPE
   : C U R R E N T T R A N S A C T I O N T Y P E
   ;

CURRPAGE
   : C U R R P A G E
   ;

CURRREPORT
   : C U R R R E P O R T
   ;

CUSTOMACTION
   : C U S T O M A C T I O N
   ;

DATABASE
   : D A T A B A S E
   ;

DATACAPTIONEXPR
   : D A T A C A P T I O N E X P R
   ;

DATACAPTIONFIELDS
   : D A T A C A P T I O N F I E L D S
   ;

DATACLASSIFICATION
   : D A T A C L A S S I F I C A T I O N
   ;

DATAITEMINDENT
   : D A T A I T E M I N D E N T
   ;

DATAITEMLINK
   : D A T A I T E M L I N K
   ;

DATAITEMLINKREFERENCE
   : D A T A I T E M L I N K R E F E R E N C E
   ;

DATAITEMSEPARATOR
   : D A T A I T E M S E P A R A T O R
   ;

DATAITEMTABLE
   : D A T A I T E M T A B L E
   ;

DATAITEMTABLEVIEW
   : D A T A I T E M T A B L E V I E W
   ;

DATALENGTH
   : D A T A L E N G T H
   ;

DATAPERCOMPANY
   : D A T A P E R C O M P A N Y
   ;

DATASCOPE
   : D A T A S C O P E
   ;

DATATRANSFER
   : D A T A T R A N S F E R
   ;

DATE
   : D A T E
   ;

DATE2DMY
   : D A T E [2] D M Y
   ;

DATE2DWY
   : D A T E [2] D W Y
   ;

DATEFORMULA
   : D A T E F O R M U L A
   ;

DATETIME
   : D A T E T I M E
   ;

DEBUGGER
   : D E B U G G E R
   ;

DECIMAL
   : D E C I M A L
   ;

DECIMALPLACES
   : D E C I M A L P L A C E S
   ;

DECIMALPLACESMAX
   : D E C I M A L P L A C E S M A X
   ;

DECIMALPLACESMIN
   : D E C I M A L P L A C E S M I N
   ;

DEFAULT
   : D E F A U L T
   ;

DEFAULTFIELDSVALIDATION
   : D E F A U L T F I E L D S V A L I D A T I O N
   ;

DEFAULTLAYOUT
   : D E F A U L T L A Y O U T
   ;

DELAYEDINSERT
   : D E L A Y E D I N S E R T
   ;

DELCHR
   : D E L C H R
   ;

DELETE
   : D E L E T E
   ;

DELETEALL
   : D E L E T E A L L
   ;

DELETEALLOWED
   : D E L E T E A L L O W E D
   ;

DELETELINK
   : D E L E T E L I N K
   ;

DELETELINKS
   : D E L E T E L I N K S
   ;

DELSTR
   : D E L S T R
   ;

DESCRIPTION
   : D E S C R I P T I O N
   ;

DEVICEFONTNAME
   : D E V I C E F O N T N A M E
   ;

DIALOG
   : D I A L O G
   ;

DICTIONARY
   : D I C T I O N A R Y
   ;

DIMENSIONS
   : D I M E N S I O N S
   ;

DIRECTION
   : D I R E C T I O N
   ;

DIVISOR
   : D I V I S O R
   ;

DMY2DATE
   : D M Y [2] D A T E
   ;

DO
   : D O
   ;

DOTNET
   : D O T N E T
   ;

DOTNETASSEMBLY
   : D O T N E T A S S E M B L Y
   ;

DOTNETTYPEDECLARATION
   : D O T N E T T Y P E D E C L A R A T I O N
   ;

DOWNLOAD
   : D O W N L O A D
   ;

DOWNLOADFROMSTREAM
   : D O W N L O A D F R O M S T R E A M
   ;

DOWNTO
   : D O W N T O
   ;

DRILLDOWN
   : D R I L L D O W N
   ;

DRILLDOWNPAGEID
   : D R I L L D O W N P A G E I D
   ;

DROPDOWN
   : D R O P D O W N
   ;

DT2DATE
   : D T [2] D A T E
   ;

DT2TIME
   : D T [2] T I M E
   ;

DUPLICATE
   : D U P L I C A T E
   ;

DURATION
   : D U R A T I O N
   ;

DWY2DATE
   : D W Y [2] D A T E
   ;

EDITABLE
   : E D I T A B L E
   ;

ELLIPSIS
   : E L L I P S I S
   ;

ELSE
   : E L S E
   ;

ENABLED
   : E N A B L E D
   ;

ENABLEDIFERROR
   : E N A B L E D I F E R R O R
   ;

ENABLEDIFINSERTION
   : E N A B L E D I F I N S E R T I O N
   ;

ENABLEDIFSELECTION
   : E N A B L E D I F S E L E C T I O N
   ;

ENCODING
   : E N C O D I N G
   ;

END
   : E N D
   ;

ENTITLEMENT
   : E N T I T L E M E N T
   ;

ENUM
   : E N U M
   ;

ENUMEXTENSION
   : E N U M E X T E N S I O N
   ;

ENVIRON
   : E N V I R O N
   ;

EOS
   : E O S
   ;

ERASE
   : E R A S E
   ;

ERROR
   : E R R O R
   ;

ERRORBEHAVIOR
   : E R R O R B E H A V I O R
   ;

ERRORCONTEXT
   : E R R O R C O N T E X T
   ;

ERRORINFO
   : E R R O R I N F O
   ;

ERRORTYPE
   : E R R O R T Y P E
   ;

EVALUATE
   : E V A L U A T E
   ;

EXECUTIONCONTEXT
   : E X E C U T I O N C O N T E X T
   ;

EXECUTIONMODE
   : E X E C U T I O N M O D E
   ;

EVENT
   : E V E N T
   ;

EXIST
   : E X I S T
   ;

EXISTS
   : E X I S T S
   ;

EXIT
   : E X I T
   ;

EXPORT
   : E X P O R T
   ;

EXPORTOBJECTS
   : E X P O R T O B J E C T S
   ;

EXTENDEDDATATYPE
   : E X T E N D E D D A T A T Y P E
   ;

EXTENDS
   : E X T E N D S
   ;

FIELD
   : F I E L D
   ;

FIELDACTIVE
   : F I E L D A C T I V E
   ;

FIELDCAPTION
   : F I E L D C A P T I O N
   ;

FIELDCLASS
   : F I E L D C L A S S
   ;

FIELDCOUNT
   : F I E L D C O U N T
   ;

FIELDENDDELIMITER
   : F I E L D E N D D E L I M I T E R
   ;

FIELDERROR
   : F I E L D E R R O R
   ;

FIELDEXIST
   : F I E L D E X I S T
   ;

FIELDGROUP
   : F I E L D G R O U P
   ;

FIELDGROUPS
   : F I E L D G R O U P S
   ;

FIELDINDEX
   : F I E L D I N D E X
   ;

FIELDNAME
   : F I E L D N A M E
   ;

FIELDNO
   : F I E L D N O
   ;

FIELDREF
   : F I E L D R E F
   ;

FIELDS
   : F I E L D S
   ;

FIELDSEPARATOR
   : F I E L D S E P A R A T O R
   ;

FIELDTYPE
   : F I E L D T Y P E
   ;

FIELDSTARTDELIMITER
   : F I E L D S T A R T D E L I M I T E R
   ;

FIELDVALIDATE
   : F I E L D V A L I D A T E
   ;

FILE
   : F I L E
   ;

FILEFORMAT
   : F I L E F O R M A T
   ;

FILENAME
   : F I L E N A M E
   ;

FILTER
   : F I L T E R
   ;

FILTERGROUP
   : F I L T E R G R O U P
   ;

FILTERPAGEBUILDER
   : F I L T E R P A G E B U I L D E R
   ;

FIND
   : F I N D
   ;

FINDFIRST
   : F I N D F I R S T
   ;

FINDLAST
   : F I N D L A S T
   ;

FINDSET
   : F I N D S E T
   ;

FOCUSABLE
   : F O C U S A B L E
   ;

FOCUSONCLICK
   : F O C U S O N C L I C K
   ;

FONTBOLD
   : F O N T B O L D
   ;

FONTITALIC
   : F O N T I T A L I C
   ;

FONTNAME
   : F O N T N A M E
   ;

FONTSIZE
   : F O N T S I Z E
   ;

FONTSTRIKETHRU
   : F O N T S T R I K E T H R U
   ;

FONTUNDERLINE
   : F O N T U N D E R L I N E
   ;

FOR
   : F O R
   ;

FOREACH
   : F O R E A C H
   ;

FORECOLOR
   : F O R E C O L O R
   ;

FORMAT
   : F O R M A T
   ;

FREEZECOLUMNID
   : F R E E Z E C O L U M N I D
   ;

FUNCTION
   : F U N C T I O N
   ;

FUNCTIONTYPE
   : F U N C T I O N T Y P E
   ;

GET
   : G E T
   ;

GETFILTER
   : G E T F I L T E R
   ;

GETFILTERS
   : G E T F I L T E R S
   ;

GETLASTERRORTEXT
   : G E T L A S T E R R O R T E X T
   ;

GETPOSITION
   : G E T P O S I T I O N
   ;

GETRANGEMAX
   : G E T R A N G E M A X
   ;

GETRANGEMIN
   : G E T R A N G E M I N
   ;

GETRECORD
   : G E T R E C O R D
   ;

GETSTAMP
   : G E T S T A M P
   ;

GETSUBTEXT
   : G E T S U B T E X T
   ;

GETTABLE
   : G E T T A B L E
   ;

GETURL
   : G E T U R L
   ;

GETVIEW
   : G E T V I E W
   ;

GLOBALLANGUAGE
   : G L O B A L L A N G U A G E
   ;

GROUPTOTALFIELDS
   : G R O U P T O T A L F I E L D S
   ;

GROUPTYPE
   : G R O U P T Y P E
   ;

GUIALLOWED
   : G U I A L L O W E D
   ;

GUID: G U I D;

HANDLERFUNCTIONS
   : H A N D L E R F U N C T I O N S
   ;

HASFILTER
   : H A S F I L T E R
   ;

HASLINKS
   : H A S L I N K S
   ;

HASVALUE
   : H A S V A L U E
   ;

HEADINGHEIGHT
   : H E A D I N G H E I G H T
   ;

HEIGHT
   : H E I G H T
   ;

HIDEVALUE
   : H I D E V A L U E
   ;

HORZALIGN
   : H O R Z A L I G N
   ;

HORZGLUE
   : H O R Z G L U E
   ;

HORZGRID
   : H O R Z G R I D
   ;

HTTPCONTENT
   : H T T P C O N T E N T
   ;

HTTPHEADERS
   : H T T P H E A D E R S
   ;

HTTPCLIENT
   : H T T P C L I E N T
   ;

HTTPREQUESTMESSAGE
   : H T T P R E Q U E S T M E S S A G E
   ;

HTTPRESPONSEMESSAGE
   : H T T P R E S P O N S E M E S S A G E
   ;

HYPERLINK
   : H Y P E R L I N K
   ;

ID
   : I D
   ;

IF
   : I F
   ;

IMAGE
   : I M A G E
   ;

IMPLEMENTS
   : I M P L E M E N T S
   ;

IMPORT
   : I M P O R T
   ;

IMPORTANCE
   : I M P O R T A N C E
   ;

IMPORTOBJECTS
   : I M P O R T O B J E C T S
   ;

IN
   : I N
   ;

INCLUDEINDATASET
   : I N C L U D E I N D A T A S E T
   ;

INCOLUMN
   : I N C O L U M N
   ;

INCOLUMNHEADING
   : I N C O L U M N H E A D I N G
   ;

INCSTR
   : I N C S T R
   ;

INDATASET
   : I N D A T A S E T
   ;

INDENT
   : I N D E N T
   ;

INDENTATION
   : I N D E N T A T I O N
   ;

INDENTATIONCOLUMNNAME
   : I N D E N T A T I O N C O L U M N N A M E
   ;

INDENTATIONCONTROLS
   : I N D E N T A T I O N C O N T R O L S
   ;

INFOOTERBAR
   : I N F O O T E R B A R
   ;

INFRAME
   : I N F R A M E
   ;

INHERENTPERMISSIONSSCOPE
   : I N H E R E N T P E R M I S S I O N S S C O P E
   ;

INIT
   : I N I T
   ;

INITVALUE
   : I N I T V A L U E
   ;

INLINEEDITING
   : I N L I N E E D I T I N G
   ;

INMATRIX
   : I N M A T R I X
   ;

INMATRIXHEADING
   : I N M A T R I X H E A D I N G
   ;

INPAGE
   : I N P A G E
   ;

INPUT
   : I N P U T
   ;

INSERT
   : I N S E R T
   ;

INSERTALLOWED
   : I N S E R T A L L O W E D
   ;

INSSTR
   : I N S S T R
   ;

INSTREAM
   : I N S T R E A M
   ;

INSTRUCTIONALTEXTML
   : I N S T R U C T I O N A L T E X T M L
   ;

INTEGER
   : I N T E G E R
   ;

INTERFACE
   : I N T E R F A C E
   ;

INTERNAL
   : I N T E R N A L
   ;

INVALIDACTIONAPPEARANCE
   : I N V A L I D A C T I O N A P P E A R A N C E
   ;

ISACTION
   : I S A C T I O N
   ;

ISAUTOMATION
   : I S A U T O M A T I O N
   ;

ISBINARY
   : I S B I N A R Y
   ;

ISBOOLEAN
   : I S B O O L E A N
   ;

ISCHAR
   : I S C H A R
   ;

ISCLEAR
   : I S C L E A R
   ;

ISCODE
   : I S C O D E
   ;

ISCODEUNIT
   : I S C O D E U N I T
   ;

ISDATE
   : I S D A T E
   ;

ISDATEFORMULA
   : I S D A T E F O R M U L A
   ;

ISDECIMAL
   : I S D E C I M A L
   ;

ISEMPTY
   : I S E M P T Y
   ;

ISFILE
   : I S F I L E
   ;

ISINSTREAM
   : I S I N S T R E A M
   ;

ISINTEGER
   : I S I N T E G E R
   ;

ISNULLGUID
   : I S N U L L G U I D
   ;

ISOLATIONLEVEL
   : I S O L A T I O N L E V E L
   ;

ISOPTION
   : I S O P T I O N
   ;

ISOUTSTREAM
   : I S O U T S T R E A M
   ;

ISRECORD
   : I S R E C O R D
   ;

ISTEXT
   : I S T E X T
   ;

ISTIME
   : I S T I M E
   ;

ISTRANSACTIONTYPE
   : I S T R A N S A C T I O N T Y P E
   ;

JOKER
   : J O K E R
   ;

JSONTOKEN
   : J S O N T O K E N
   ;

JSONVALUE
   : J S O N V A L U E
   ;

JSONARRAY
   : J S O N A R R A Y
   ;

JSONOBJECT
   : J S O N O B J E C T
   ;

KEEPWITHNEXT
   : K E E P W I T H N E X T
   ;

KEY
   : K E Y
   ;

KEYS
   : K E Y S
   ;

KEYCOUNT
   : K E Y C O U N T
   ;

KEYGROUPDISABLE
   : K E Y G R O U P D I S A B L E
   ;

KEYGROUPENABLE
   : K E Y G R O U P E N A B L E
   ;

KEYGROUPENABLED
   : K E Y G R O U P E N A B L E D
   ;

KEYGROUPS
   : K E Y G R O U P S
   ;

KEYINDEX
   : K E Y I N D E X
   ;

KEYREF
   : K E Y R E F
   ;

LABEL
   : L A B E L
   ;

LANGUAGE
   : L A N G U A G E
   ;

LEADERDOTS
   : L E A D E R D O T S
   ;

LEFTMARGIN
   : L E F T M A R G I N
   ;

LEN
   : L E N
   ;

LENGTH
   : L E N G T H
   ;

LINKEDINTRANSACTION
   : L I N K E D I N T R A N S A C T I O N
   ;

LINKEDOBJECT
   : L I N K E D O B J E C T
   ;

LINKFIELDS
   : L I N K F I E L D S
   ;

LINKSALLOWED
   : L I N K S A L L O W E D
   ;

LINKTABLE
   : L I N K T A B L E
   ;

LINKTABLEFORCEINSERT
   : L I N K T A B L E F O R C E I N S E R T
   ;

LIST
   : L I S T
   ;

LOCAL
   : L O C A L
   ;

LOCKTABLE
   : L O C K T A B L E
   ;

LOCKTIMEOUT
   : L O C K T I M E O U T
   ;

LOGHEIGHT
   : L O G H E I G H T
   ;

LOGWIDTH
   : L O G W I D T H
   ;

LOOKUP
   : L O O K U P
   ;

LOOKUPMODE
   : L O O K U P M O D E
   ;

LOOKUPPAGEID
   : L O O K U P P A G E I D
   ;

LOWERCASE
   : L O W E R C A S E
   ;

MAINTAINSIFTINDEX
   : M A I N T A I N S I F T I N D E X
   ;

MAINTAINSQLINDEX
   : M A I N T A I N S Q L I N D E X
   ;

MARK
   : M A R K
   ;

MARKEDONLY
   : M A R K E D O N L Y
   ;

MATRIXCOLUMNWIDTH
   : M A T R I X C O L U M N W I D T H
   ;

MATRIXSOURCETABLE
   : M A T R I X S O U R C E T A B L E
   ;

MAX
   : M A X
   ;

MAXIMIZABLE
   : M A X I M I Z A B L E
   ;

MAXIMIZEDONOPEN
   : M A X I M I Z E D O N O P E N
   ;

MAXITERATION
   : M A X I T E R A T I O N
   ;

MAXLENGTH
   : M A X L E N G T H
   ;

MAXOCCURS
   : M A X O C C U R S
   ;

MAXSTRLEN
   : M A X S T R L E N
   ;

MAXVALUE
   : M A X V A L U E
   ;

MEDIA
   : M E D I A
   ;

MEDIASET
   : M E D I A S E T
   ;

MENUITEMTYPE
   : M E N U I T E M T Y P E
   ;

MENULEVEL
   : M E N U L E V E L
   ;

MESSAGE
   : M E S S A G E
   ;

MIN
   : M I N
   ;

MINIMIZABLE
   : M I N I M I Z A B L E
   ;

MINIMIZEDONOPEN
   : M I N I M I Z E D O N O P E N
   ;

MINOCCURS
   : M I N O C C U R S
   ;

MINVALUE
   : M I N V A L U E
   ;

MODIFY
   : M O D I F Y
   ;

MODIFYALL
   : M O D I F Y A L L
   ;

MODIFYALLOWED
   : M O D I F Y A L L O W E D
   ;

MODULEDEPENDENCYINFO
   : M O D U L E D E P E N D E N C Y I N F O
   ;

MODULEINFO
   : M O D U L E I N F O
   ;

MULTILINE
   : M U L T I L I N E
   ;

MULTIPLENEWLINES
   : M U L T I P L E N E W L I N E S
   ;

NAME
   : N A M E
   ;

NAMESPACE
   : N A M E S P A C E
   ;

NEWPAGEPERGROUP
   : N E W P A G E P E R G R O U P
   ;

NEXT
   : N E X T
   ;

NEXTCONTROL
   : N E X T C O N T R O L
   ;

NODENAME
   : N O D E N A M E
   ;

NODETYPE
   : N O D E T Y P E
   ;

NONE
   : N O N E
   ;

NOTIFICATION
   : N O T I F I C A T I O N
   ;

NOTIFICATIONSCOPE
   : N O T I F I C A T I O N S C O P E
   ;

NORMALDATE
   : N O R M A L D A T E
   ;

NOTBLANK
   : N O T B L A N K
   ;

NUMBER
   : N U M B E R
   ;

NUMERIC
   : N U M E R I C
   ;

OBJECTTYPE
   : O B J E C T T Y P E
   ;

OCCURRENCE
   : O C C U R R E N C E
   ;

OCX
   : O C X
   ;

OF
   : O F
   ;

ONACTION
   : O N A C T I O N
   ;

ONACTIVATE
   : O N A C T I V A T E
   ;

ONAFTERASSIGNFIELD
   : O N A F T E R A S S I G N F I E L D
   ;

ONAFTERASSIGNVARIABLE
   : O N A F T E R A S S I G N V A R I A B L E
   ;

ONAFTEREXPORTRECORD
   : O N A F T E R E X P O R T R E C O R D
   ;

ONAFTERFORMATFIELD
   : O N A F T E R F O R M A T F I E L D
   ;

ONAFTERGETFIELD
   : O N A F T E R G E T F I E L D
   ;

ONAFTERGETRECORD
   : O N A F T E R G E T R E C O R D
   ;

ONAFTERIMPORTRECORD
   : O N A F T E R I M P O R T R E C O R D
   ;

ONAFTERINITRECORD
   : O N A F T E R I N I T R E C O R D
   ;

ONAFTERINPUT
   : O N A F T E R I N P U T
   ;

ONAFTERINSERTRECORD
   : O N A F T E R I N S E R T R E C O R D
   ;

ONAFTERMODIFYRECORD
   : O N A F T E R M O D I F Y R E C O R D
   ;

ONAFTERTESTRUN
   : O N A F T E R T E S T R U N
   ;

ONAFTERVALIDATE
   : O N A F T E R V A L I D A T E
   ;

ONASSISTEDIT
   : O N A S S I S T E D I T
   ;

ONBEFOREEVALUATEFIELD
   : O N B E F O R E E V A L U A T E F I E L D
   ;

ONBEFOREEXPORTRECORD
   : O N B E F O R E E X P O R T R E C O R D
   ;

ONBEFOREIMPORTRECORD
   : O N B E F O R E I M P O R T R E C O R D
   ;

ONBEFOREINPUT
   : O N B E F O R E I N P U T
   ;

ONBEFOREINSERTRECORD
   : O N B E F O R E I N S E R T R E C O R D
   ;

ONBEFOREMODIFYRECORD
   : O N B E F O R E M O D I F Y R E C O R D
   ;

ONBEFOREPASSFIELD
   : O N B E F O R E P A S S F I E L D
   ;

ONBEFOREPASSVARIABLE
   : O N B E F O R E P A S S V A R I A B L E
   ;

ONBEFOREPUTRECORD
   : O N B E F O R E P U T R E C O R D
   ;

ONBEFORETESTRUN
   : O N B E F O R E T E S T R U N
   ;

ONCLOSEPAGE
   : O N C L O S E P A G E
   ;

ONCONTROLADDIN
   : O N C O N T R O L A D D I N
   ;

ONDEACTIVATE
   : O N D E A C T I V A T E
   ;

ONDELETE
   : O N D E L E T E
   ;

ONDELETERECORD
   : O N D E L E T E R E C O R D
   ;

ONDRILLDOWN
   : O N D R I L L D O W N
   ;

ONFINDRECORD
   : O N F I N D R E C O R D
   ;

ONFORMAT
   : O N F O R M A T
   ;

ONINIT
   : O N I N I T
   ;

ONINITREPORT
   : O N I N I T R E P O R T
   ;

ONINITXMLPORT
   : O N I N I T X M L P O R T
   ;

ONINPUTCHANGE
   : O N I N P U T C H A N G E
   ;

ONINSERT
   : O N I N S E R T
   ;

ONINSERTRECORD
   : O N I N S E R T R E C O R D
   ;

ONLOOKUP
   : O N L O O K U P
   ;

ONMODIFY
   : O N M O D I F Y
   ;

ONMODIFYRECORD
   : O N M O D I F Y R E C O R D
   ;

ONNEWRECORD
   : O N N E W R E C O R D
   ;

ONNEXTRECORD
   : O N N E X T R E C O R D
   ;

ONOPENPAGE
   : O N O P E N P A G E
   ;

ONPOSTDATAITEM
   : O N P O S T D A T A I T E M
   ;

ONPOSTREPORT
   : O N P O S T R E P O R T
   ;

ONPOSTSECTION
   : O N P O S T S E C T I O N
   ;

ONPOSTXMLPORT
   : O N P O S T X M L P O R T
   ;

ONPREDATAITEM
   : O N P R E D A T A I T E M
   ;

ONPREREPORT
   : O N P R E R E P O R T
   ;

ONPRESECTION
   : O N P R E S E C T I O N
   ;

ONPREXMLITEM
   : O N P R E X M L I T E M
   ;

ONPREXMLPORT
   : O N P R E X M L P O R T
   ;

ONPUSH
   : O N P U S H
   ;

ONQUERYCLOSEPAGE
   : O N Q U E R Y C L O S E P A G E
   ;

ONRENAME
   : O N R E N A M E
   ;

ONTIMER
   : O N T I M E R
   ;

ONVALIDATE
   : O N V A L I D A T E
   ;

OPEN
   : O P E N
   ;

OPTION
   : O P T I O N
   ;

OPTIONCAPTION
   : O P T I O N C A P T I O N
   ;

OPTIONCAPTIONML
   : O P T I O N C A P T I O N M L
   ;

OPTIONSTRING
   : O P T I O N S T R I N G
   ;

OPTIONVALUE
   : O P T I O N V A L U E
   ;

ORDER
   : O R D E R
   ;

ORIENTATION
   : O R I E N T A T I O N
   ;

OSVERSION
   : O S V E R S I O N
   ;

OUTSTREAM
   : O U T S T R E A M
   ;

PADCHAR
   : P A D C H A R
   ;

PADSTR
   : P A D S T R
   ;

PAGE
   : P A G E
   ;

PAGEBACKGROUNDTASKERRORLEVEL
   : P A G E B A C K G R O U N D T A S K E R R O R L E V E L
   ;

PAGECUSTOMIZATION
   : P A G E C U S T O M I Z A T I O N
   ;

PAGEEXTENSION
   : P A G E E X T E N S I O N
   ;

PAGENAMES
   : P A G E N A M E S
   ;

PAGENAMESML
   : P A G E N A M E S M L
   ;

PAGENO
   : P A G E N O
   ;

PAGERESULT
   : P A G E R E S U L T
   ;

PAGEPARTID
   : P A G E P A R T I D
   ;

PAGETYPE
   : P A G E T Y P E
   ;

PAPERSIZE
   : P A P E R S I Z E
   ;

PAPERSOURCE
   : P A P E R S O U R C E
   ;

PAPERSOURCEDEFAULTPAGE
   : P A P E R S O U R C E D E F A U L T P A G E
   ;

PAPERSOURCEFIRSTPAGE
   : P A P E R S O U R C E F I R S T P A G E
   ;

PAPERSOURCELASTPAGE
   : P A P E R S O U R C E L A S T P A G E
   ;

PAPERSOURCEOTHERPAGES
   : P A P E R S O U R C E O T H E R P A G E S
   ;

PARENTCONTROL
   : P A R E N T C O N T R O L
   ;

PARTTYPE
   : P A R T T Y P E
   ;

PASSWORDTEXT
   : P A S S W O R D T E X T
   ;

PASTEISVALID
   : P A S T E I S V A L I D
   ;

PERCENTAGE
   : P E R C E N T A G E
   ;

PERMANENTASSIST
   : P E R M A N E N T A S S I S T
   ;

PERMISSIONOBJECTTYPE
   : P E R M I S S I O N O B J E C T T Y P E
   ;

PERMISSIONS
   : P E R M I S S I O N S
   ;

PERMISSIONSET
   : P E R M I S S I O N S E T
   ;

PERMISSIONSETEXTENSION
   : P E R M I S S I O N S E T E X T E N S I O N
   ;

PLACEINBOTTOM
   : P L A C E I N B O T T O M
   ;

POPULATEALLFIELDS
   : P O P U L A T E A L L F I E L D S
   ;

POS
   : P O S
   ;

POWER
   : P O W E R
   ;

PREVIEW
   : P R E V I E W
   ;

PRINTONEVERYPAGE
   : P R I N T O N E V E R Y P A G E
   ;

PRINTONLYIFDETAIL
   : P R I N T O N L Y I F D E T A I L
   ;

PROCEDURE
   : P R O C E D U R E
   ;

PROCESSINGONLY
   : P R O C E S S I N G O N L Y
   ;

PROFILE
   : P R O F I L E
   ;

PROGRAM
   : P R O G R A M
   ;

PROMOTED
   : P R O M O T E D
   ;

PROMOTEDCATEGORY
   : P R O M O T E D C A T E G O R Y
   ;

PROMOTEDISBIG
   : P R O M O T E D I S B I G
   ;

PROTECTED
   : P R O T E C T E D
   ;

PROVIDERID
   : P R O V I D E R I D
   ;

PUSHACTION
   : P U S H A C T I O N
   ;

QUERY
   : Q U E R Y
   ;

QUERYREPLACE
   : Q U E R Y R E P L A C E
   ;

QUIT
   : Q U I T
   ;

RANDOM
   : R A N D O M
   ;

RANDOMIZE
   : R A N D O M I Z E
   ;

READ
   : R E A D
   ;

READCONSISTENCY
   : R E A D C O N S I S T E N C Y
   ;

READPERMISSION
   : R E A D P E R M I S S I O N
   ;

READTEXT
   : R E A D T E X T
   ;

REC
   : R E C
   ;

RECORD
   : R E C O R D
   ;

RECORDID
   : R E C O R D I D
   ;

RECORDLEVELLOCKING
   : R E C O R D L E V E L L O C K I N G
   ;

RECORDREF
   : R E C O R D R E F
   ;

RECORDSEPARATOR
   : R E C O R D S E P A R A T O R
   ;

RELATION
   : R E L A T I O N
   ;

RENAME
   : R E N A M E
   ;

REPEAT
   : R E P E A T
   ;

REPORT
   : R E P O R T
   ;

REPORTEXTENSION
   : R E P O R T E X T E N S I O N
   ;

REPORTFORMAT
   : R E P O R T F O R M A T
   ;

REPORTLAYOUTTYPE
   : R E P O R T L A Y O U T T Y P E
   ;

REQFILTERFIELDS
   : R E Q F I L T E R F I E L D S
   ;

REQFILTERHEADING
   : R E Q F I L T E R H E A D I N G
   ;

REQFILTERHEADINGML
   : R E Q F I L T E R H E A D I N G M L
   ;

REQUESTOPTIONSPAGE
   : R E Q U E S T O P T I O N S P A G E
   ;

RESET
   : R E S E T
   ;

RIGHTMARGIN
   : R I G H T M A R G I N
   ;

ROUND
   : R O U N D
   ;

ROUNDDATETIME
   : R O U N D D A T E T I M E
   ;

ROWHEIGHT
   : R O W H E I G H T
   ;

RUN
   : R U N
   ;

RUNCOMMAND
   : R U N C O M M A N D
   ;

RUNMODAL
   : R U N M O D A L
   ;

RUNOBJECT
   : R U N O B J E C T
   ;

RUNONCLIENT
   : R U N O N C L I E N T
   ;

RUNPAGELINK
   : R U N P A G E L I N K
   ;

RUNPAGEONREC
   : R U N P A G E O N R E C
   ;

RUNPAGEVIEW
   : R U N P A G E V I E W
   ;

SAVEASEXCEL
   : S A V E A S E X C E L
   ;

SAVEASHTML
   : S A V E A S H T M L
   ;

SAVEASPDF
   : S A V E A S P D F
   ;

SAVEASXML
   : S A V E A S X M L
   ;

SAVECOLUMNWIDTHS
   : S A V E C O L U M N W I D T H S
   ;

SAVECONTROLINFO
   : S A V E C O N T R O L I N F O
   ;

SAVEPOSANDSIZE
   : S A V E P O S A N D S I Z E
   ;

SAVERECORD
   : S A V E R E C O R D
   ;

SAVEVALUES
   : S A V E V A L U E S
   ;

SECRETTEXT
   : S E C R E T T E X T
   ;

SECTIONHEIGHT
   : S E C T I O N H E I G H T
   ;

SECTIONWIDTH
   : S E C T I O N W I D T H
   ;

SECURITYFILTER
   : S E C U R I T Y F I L T E R
   ;

SECURITYFILTERING
   : S E C U R I T Y F I L T E R I N G
   ;

SECURITYOPERATIONRESULT
   : S E C U R I T Y O P E R A T I O N R E S U L T
   ;

SEEK
   : S E E K
   ;

SELECTLATESTVERSION
   : S E L E C T L A T E S T V E R S I O N
   ;

SELECTSTR
   : S E L E C T S T R
   ;

SERIALNUMBER
   : S E R I A L N U M B E R
   ;

SESSIONSETTINGS
   : S E S S I O N S E T T I N G S
   ;

SETAUTOCALCFIELDS
   : S E T A U T O C A L C F I E L D S
   ;

SETCURRENTKEY
   : S E T C U R R E N T K E Y
   ;

SETFILTER
   : S E T F I L T E R
   ;

SETPERMISSIONFILTER
   : S E T P E R M I S S I O N F I L T E R
   ;

SETPOSITION
   : S E T P O S I T I O N
   ;

SETRANGE
   : S E T R A N G E
   ;

SETRECFILTER
   : S E T R E C F I L T E R
   ;

SETRECORD
   : S E T R E C O R D
   ;

SETSELECTIONFILTER
   : S E T S E L E C T I O N F I L T E R
   ;

SETSTAMP
   : S E T S T A M P
   ;

SETTABLE
   : S E T T A B L E
   ;

SETTABLEVIEW
   : S E T T A B L E V I E W
   ;

SETVIEW
   : S E T V I E W
   ;

SHAPESTYLE
   : S H A P E S T Y L E
   ;

SHELL
   : S H E L L
   ;

SHORTCUTKEY
   : S H O R T C U T K E Y
   ;

SHOWASTREE
   : S H O W A S T R E E
   ;

SHOWCAPTION
   : S H O W C A P T I O N
   ;

SHOWPRINTSTATUS
   : S H O W P R I N T S T A T U S
   ;

SHOWSTATUS
   : S H O W S T A T U S
   ;

SID
   : S I D
   ;

SIGNDISPLACEMENT
   : S I G N D I S P L A C E M E N T
   ;

SINGLEINSTANCE
   : S I N G L E I N S T A N C E
   ;

SIZABLE
   : S I Z A B L E
   ;

SLEEP
   : S L E E P
   ;

SORTING
   : S O R T I N G
   ;

SOURCEEXPR
   : S O U R C E E X P R
   ;

SOURCEFIELD
   : S O U R C E F I E L D
   ;

SOURCETABLE
   : S O U R C E T A B L E
   ;

SOURCETABLEPLACEMENT
   : S O U R C E T A B L E P L A C E M E N T
   ;

SOURCETABLERECORD
   : S O U R C E T A B L E R E C O R D
   ;

SOURCETABLETEMPORARY
   : S O U R C E T A B L E T E M P O R A R Y
   ;

SOURCETABLEVIEW
   : S O U R C E T A B L E V I E W
   ;

SOURCETYPE
   : S O U R C E T Y P E
   ;

SQLINDEX
   : S Q L I N D E X
   ;

STARTPOS
   : S T A R T P O S
   ;

STARTSESSION
   : S T A R T S E S S I O N
   ;

STOPSESSION
   : S T O P S E S S I O N
   ;

STRCHECKSUM
   : S T R C H E C K S U M
   ;

STRLEN
   : S T R L E N
   ;

STRMENU
   : S T R M E N U
   ;

STRPOS
   : S T R P O S
   ;

STRSUBSTNO
   : S T R S U B S T N O
   ;

STYLE
   : S T Y L E
   ;

STYLEEXPR
   : S T Y L E E X P R
   ;

SUBPAGELINK
   : S U B P A G E L I N K
   ;

SUBPAGEVIEW
   : S U B P A G E V I E W
   ;

SUBTYPE
   : S U B T Y P E
   ;

SUM
   : S U M
   ;

SUMINDEXFIELDS
   : S U M I N D E X F I E L D S
   ;

SUPPRESSDISPOSE
   : S U P P R E S S D I S P O S E
   ;

SYNCHRONIZEALLLOGINS
   : S Y N C H R O N I Z E A L L L O G I N S
   ;

SYNCHRONIZESINGLELOGIN
   : S Y N C H R O N I Z E S I N G L E L O G I N
   ;

SYSTEM
   : S Y S T E M
   ;

SYSTEMACTION
   : S Y S T E M A C T I O N
   ;

SYSTEMPARTID
   : S Y S T E M P A R T I D
   ;

TABLE
   : T A B L E
   ;

TABLEBOXID
   : T A B L E B O X I D
   ;

TABLECAPTION
   : T A B L E C A P T I O N
   ;

TABLECONNECTIONTYPE
   : T A B L E C O N N E C T I O N T Y P E
   ;

TABLEDATA
   : T A B L E D A T A
   ;

TABLEEXTENSION
   : T A B L E E X T E N S I O N
   ;

TABLEFILTER
   : T A B L E F I L T E R
   ;

TABLEIDEXPR
   : T A B L E I D E X P R
   ;

TABLENAME
   : T A B L E N A M E
   ;

TABLENO
   : T A B L E N O
   ;

TABLERELATION
   : T A B L E R E L A T I O N
   ;

TELEMETRYSCOPE
   : T E L E M E T R Y S C O P E
   ;

TEMPORARY
   : T E M P O R A R Y
   ;

TEMPORARYPATH
   : T E M P O R A R Y P A T H
   ;

TESTACTION
   : T E S T A C T I O N
   ;

TESTFIELD
   : T E S T F I E L D
   ;

TESTPAGE
   : T E S T P A G E
   ;

TESTPERMISSIONS
   : T E S T P E R M I S S I O N S
   ;

TESTREQUESTPAGE
   : T E S T R E Q U E S T P A G E
   ;

TESTTABLERELATION
   : T E S T T A B L E R E L A T I O N
   ;

TEXT
   : T E X T
   ;

TEXTBUILDER
   : T E X T B U I L D E R
   ;

TEXTCONST
   : T E X T C O N S T
   ;

TEXTENCODING
   : T E X T E N C O D I N G
   ;

TEXTMODE
   : T E X T M O D E
   ;

TEXTPOS
   : T E X T P O S
   ;

TEXTTYPE
   : T E X T T Y P E
   ;

THEN
   : T H E N
   ;

TIME
   : T I M E
   ;

TIMERINTERVAL
   : T I M E R I N T E R V A L
   ;

TITLE
   : T I T L E
   ;

TO
   : T O
   ;

TODAY
   : T O D A Y
   ;

TOOLTIP
   : T O O L T I P
   ;

TOOLTIPML
   : T O O L T I P M L
   ;

TOPLINEONLY
   : T O P L I N E O N L Y
   ;

TOPMARGIN
   : T O P M A R G I N
   ;

TOTALFIELDS
   : T O T A L F I E L D S
   ;

TOTALSCAUSEDBY
   : T O T A L S C A U S E D B Y
   ;

TRANSACTIONMODEL
   : T R A N S A C T I O N M O D E L
   ;

TRANSACTIONTYPE
   : T R A N S A C T I O N T Y P E
   ;

TRANSFERFIELDS
   : T R A N S F E R F I E L D S
   ;

TRIGGER
   : T R I G G E R
   ;

UNTIL
   : U N T I L
   ;

TRUNC
   : T R U N C
   ;

TYPE
   : T Y P E
   ;

UPDATE
   : U P D A T E
   ;

UPDATECONTROLS
   : U P D A T E C O N T R O L S
   ;

UPDATEEDITABLE
   : U P D A T E E D I T A B L E
   ;

UPDATEFONTBOLD
   : U P D A T E F O N T B O L D
   ;

UPDATEFORECOLOR
   : U P D A T E F O R E C O L O R
   ;

UPDATEINDENT
   : U P D A T E I N D E N T
   ;

UPDATEONACTION
   : U P D A T E O N A C T I O N
   ;

UPDATEONACTIVATE
   : U P D A T E O N A C T I V A T E
   ;

UPDATESELECTED
   : U P D A T E S E L E C T E D
   ;

UPLOAD
   : U P L O A D
   ;

UPLOADINTOSTREAM
   : U P L O A D I N T O S T R E A M
   ;

UPPERCASE
   : U P P E R C A S E
   ;

UPPERLIMIT
   : U P P E R L I M I T
   ;

USEREQUESTPAGE
   : U S E R E Q U E S T P A G E
   ;

USERID
   : U S E R I D
   ;

USING
   : U S I N G
   ;

USESYSTEMPRINTER
   : U S E S Y S T E M P R I N T E R
   ;

VALIDATE
   : V A L I D A T E
   ;

VALIDATETABLERELATION
   : V A L I D A T E T A B L E R E L A T I O N
   ;

VALUE
   : V A L U E
   ;

VALUESALLOWED
   : V A L U E S A L L O W E D
   ;

VAR
   : V A R
   ;

VARIABLEACTIVE
   : V A R I A B L E A C T I V E
   ;

VARIABLENAME
   : V A R I A B L E N A M E
   ;

VARIANT
   : V A R I A N T
   ;

VARIANT2DATE
   : V A R I A N T [2] D A T E
   ;

VARIANT2TIME
   : V A R I A N T [2] T I M E
   ;

VERBOSITY
   : V E R B O S I T Y
   ;

VERSION
   : V E R S I O N
   ;

VERTALIGN
   : V E R T A L I G N
   ;

VERTGLUE
   : V E R T G L U E
   ;

VERTGRID
   : V E R T G R I D
   ;

VIEW
   : V I E W
   ;

VIEWS
   : V I E W S
   ;

VISIBLE
   : V I S I B L E
   ;

WEBSERVICEACTIONCONTEXT
   : W E B S E R V I C E A C T I O N C O N T E X T
   ;

WEBSERVICEACTIONRESULTCODE
   : W E B S E R V I C E A C T I O N R E S U L T C O D E
   ;

WHERE
   : W H E R E
   ;

WHILE
   : W H I L E
   ;

WIDTH
   : W I D T H
   ;

WINDOWSLANGUAGE
   : W I N D O W S L A N G U A G E
   ;

WITH
   : W I T H
   ;

WITHEVENTS
   : W I T H E V E N T S
   ;

WORDDATE
   : W O R D D A T E
   ;

WRITE
   : W R I T E
   ;

WRITEMODE
   : W R I T E M O D E
   ;

WRITEPERMISSION
   : W R I T E P E R M I S S I O N
   ;

WRITETEXT
   : W R I T E T E X T
   ;

XMLATTRIBUTE
   : X M L A T T R I B U T E
   ;

XMLATTRIBUTECOLLECTION
   : X M L A T T R I B U T E C O L L E C T I O N
   ;

XMLCOMMENT
   : X M L C O M M E N T
   ;

XMLCDATA
   : X M L C D A T A
   ;

XMLDECLARATION
   : X M L D E C L A R A T I O N
   ;

XMLDOCUMENT
   : X M L D O C U M E N T
   ;

XMLDOCUMENTTYPE
   : X M L D O C U M E N T T Y P E
   ;

XMLELEMENT
   : X M L E L E M E N T
   ;

XMLNAMESPACEMANAGER
   : X M L N A M E S P A C E M A N A G E R
   ;

XMLNAMETABLE
   : X M L N A M E T A B L E
   ;

XMLNODE
   : X M L N O D E
   ;

XMLNODELIST
   : X M L N O D E L I S T
   ;

XMLPORT
   : X M L P O R T
   ;

XMLPROCESSINGINSTRUCTION
   : X M L P R O C E S S I N G I N S T R U C T I O N
   ;

XMLREADOPTIONS
   : X M L R E A D O P T I O N S
   ;

XMLTEXT
   : X M L T E X T
   ;

XMLVERSIONNO
   : X M L V E R S I O N N O
   ;

XMLWRITEOPTIONS
   : X M L W R I T E O P T I O N S
   ;

XPOS
   : X P O S
   ;

XREC
   : X R E C
   ;

YIELD
   : Y I E L D
   ;

YPOS
   : Y P O S
   ;

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
	: LOWERCASELETTER
	| UPPERCASELETTER
	;

/*
 * preprocessor directives
 */

mode DIRECTIVE_MODE;

DIRECTIVE_WHITESPACES  : WHITE_SPACE+                     -> channel(HIDDEN);
DEFINE                 : 'define'                -> channel(DIRECTIVE);
UNDEF                  : 'undef'                 -> channel(DIRECTIVE);
DIRECTIVE_IF           : 'if'                    -> channel(DIRECTIVE), type(IF);
ELIF                   : 'elif'                  -> channel(DIRECTIVE);
DIRECTIVE_ELSE         : 'else'                  -> channel(DIRECTIVE), type(ELSE);
ENDIF                  : 'endif'                 -> channel(DIRECTIVE);
REGION                 : 'region' WHITE_SPACE*            -> channel(DIRECTIVE), mode(DIRECTIVE_TEXT_MODE);
ENDREGION              : 'endregion' WHITE_SPACE*         -> channel(DIRECTIVE), mode(DIRECTIVE_TEXT_MODE);
PRAGMA                 : 'pragma' WHITE_SPACE+            -> channel(DIRECTIVE), mode(DIRECTIVE_TEXT_MODE);
DIRECTIVE_OPEN_PARENS  : '('                     -> channel(DIRECTIVE), type(LEFTPAREN);
DIRECTIVE_CLOSE_PARENS : ')'                     -> channel(DIRECTIVE), type(RIGHTPAREN);
BANG                   : '!'                     -> channel(DIRECTIVE);
OP_AND                 : '&&'                    -> channel(DIRECTIVE);
OP_OR                  : '||'                    -> channel(DIRECTIVE);

DIRECTIVE_IDENT        : (LETTER | DIGIT | UNDERSCORE)+ -> channel(DIRECTIVE), type(IDENTIFIER);
DIRECTIVE_NEW_LINE     : NEW_LINE+ -> channel(DIRECTIVE), mode(DEFAULT_MODE);

mode DIRECTIVE_TEXT_MODE;

DIRECTIVE_TEXT         : ~[\r\n\u0085\u2028\u2029]+ -> channel(DIRECTIVE);
TEXT_NEW_LINE          : NEW_LINE+ -> channel(DIRECTIVE), type(DIRECTIVE_NEW_LINE), mode(DEFAULT_MODE);
