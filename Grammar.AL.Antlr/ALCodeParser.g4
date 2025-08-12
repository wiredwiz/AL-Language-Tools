parser grammar ALCodeParser;

options { tokenVocab=ALLexer; }

import ALCoreParser;

/*
 * AL Code Parser
 This parser is designed to parse AL code, including variable declarations,
 * method declarations, control structures, expressions and codeunits.
 */

/*
 * Method
 */

methodAccessModifier
   : LOCAL
   | PROTECTED
   | INTERNAL
   ;

methodName
   : identifier;

methodDeclaration
   : attributes=methodAttribute*? accessModifier=methodAccessModifier* PROCEDURE name=identifier LEFTPAREN parameters=parameterList? RIGHTPAREN returnVal=returnValue? SEMICOLON? variables=varBlock? codeBlock=statementBlock SEMICOLON;

/*
 * Method attributes
 */

attributeIdentifier
   : identifier COLON;

attributeArgument
   : (attributeIdentifier)? (STRING_LITERAL | INTEGER_LITERAL	| FLOAT_LITERAL | identifier | booleanLiteral | systemEnumerationLiteral | enumerationLiteral);

attributeArgumentList
   : attributeArgument (COMMA attributeArgument)*?;

methodAttribute
   : LEFTBRACKET identifier (LEFTPAREN attributeArgumentList? RIGHTPAREN)? RIGHTBRACKET;

/*
 * AL Variables, parameters and return types
 */

dictionaryKey
   : variableTypeDeclaration
   ;

dictionaryDataType
   : variableTypeDeclaration
   ;

labelText
   : STRING_LITERAL
   ;

labelMaxLength
   : {TokenMatches("MaxLength")}? identifier EQUAL INTEGER_LITERAL
   ;

labelComment
   : {TokenMatches("Comment")}? identifier EQUAL STRING_LITERAL
   ;

labelLocked
   : {TokenMatches("Locked")}? identifier EQUAL (TRUE | FALSE)
   ;

labelArgument
   : labelMaxLength
   | labelComment
   | labelLocked
   ;

labelArgs
   : labelArgument (COMMA labelArgument)*?
   ;

optionValue
   : identifier
   ;

optionValueList
   : optionValue (COMMA optionValue)*?
   ;

variableTypeDeclaration
   : ARRAY LEFTBRACKET dimensions=arrayDimensions RIGHTBRACKET OF typeDecl=variableTypeDeclaration #ArrayVariable
   | BIGINTEGER #BigIntegerVariable
   | BLOB #BlobVariable
   | BOOLEAN #BooleanVariable
   | BYTE #ByteVariable
   | CHAR #CharVariable
   | CODE sizeDecl=sizeDeclaration #CodeVariable
   | CODEUNIT objId=objectId #CodeunitVariable
   | DATE #DateVariable
   | DATEFORMULA #DateFormulaVariable
   | DATETIME #DatetimeVariable
   | DECIMAL #DecimalVariable
   | DIALOG #DialogVariable
   | DICTIONARY OF LEFTBRACKET keyDecl=dictionaryKey COMMA typeDecl=dictionaryDataType RIGHTBRACKET #DictionaryVariable
   | DOTNET objId=objectId #DotNetVariable
   | DURATION #DurationVariable
   | ENUM objId=objectId #EnumVariable
   | FIELDREF #FieldRefVariable
   | FILE #FileVariable
   | FILTERPAGEBUILDER #FilterPageBuilderVariable
   | GUID #GuidVariable
   | HTTPCLIENT #HttpClientVariable
   | HTTPCONTENT #HttpContentVariable
   | HTTPHEADERS #HttpHeadersVariable
   | HTTPREQUESTMESSAGE #HttpRequestMessageVariable
   | HTTPRESPONSEMESSAGE #HttpResponseMessageVariable
   | INSTREAM #InStreamVariable
   | INTEGER #IntegerVariable
   | INTERFACE objId=objectId #InterfaceVariable
   | JSONARRAY #JsonArrayVariable
   | JSONOBJECT #JsonObjectVariable
   | JSONTOKEN #JsonTokenVariable
   | JSONVALUE #JsonValueVariable
   | KEYREF #KeyRefVariable
   | LABEL text=labelText (COMMA arguments=labelArgs)? #LabelVariable
   | LIST OF LEFTBRACKET typeDecl=variableTypeDeclaration RIGHTBRACKET #ListVariable
   | MODULEDEPENDENCYINFO #ModuleDependencyInfoVariable
   | MODULEINFO #ModuleInfoVariable
   | NOTIFICATION #NotificationVariable
   | OPTION options=optionValueList? #OptionVariable
   | OUTSTREAM #OutStreamVariable
   | PAGE objId=objectId #PageVariable
   | QUERY objId=objectId #QueryVariable
   | RECORD objId=objectId TEMPORARY? #RecordVariable
   | RECORDID #RecordIdVariable
   | RECORDREF #RecordRefVariable
   | REPORT objId=objectId #ReportVariable
   | SESSIONSETTINGS #SessionSettingsVariable
   | TEXT sizeDecl=sizeDeclaration? #TextVariable
   | TEXTBUILDER #TextBuilderVariable
   | TEXTCONST name=identifier EQUAL literalValue=STRING_LITERAL #TextConstantVariable
   | TIME #TimeVariable
   | VARIANT #VariantVariable
   | VERSION #VersionVariable
   | XMLATTRIBUTE #XmlAttributeVariable
   | XMLDOCUMENT #XmlDocumentVariable
   | XMLELEMENT #XmlElementVariable
   | XMLNODE #XmlNodeVariable
   | XMLPORT objId=objectId #XmlPortVariable
   ;

parameterName
   : identifier
   ;

parameterDeclaration
   : VAR? parameterName COLON variableTypeDeclaration
   ;

parameterList
   : parameterDeclaration (SEMICOLON parameterDeclaration)*
   ;

variableAttribute
   : LEFTBRACKET identifier (LEFTPAREN attributeArgumentList? RIGHTPAREN)? RIGHTBRACKET;

variableName
   : identifier;

variableNameList
	: variableName (COMMA variableName)*
   ;

variableDeclaration
   : variableAttribute? variableNameList COLON variableTypeDeclaration SEMICOLON
   ;

variableDeclarations
   : variableDeclaration*
   ;

varBlock
   : VAR variableDeclarations
   ;

protectedVarBlock
   : PROTECTED VAR variableDeclarations
   ;

returnValue
   : identifier? returnType
   ;

returnType
   : COLON variableTypeDeclaration
   ;

/*
 * Trigger declarations
 */

triggerName : identifier;

triggerDeclaration
   : TRIGGER triggerName LEFTPAREN parameterList? RIGHTPAREN returnValue? varBlock? statementBlock SEMICOLON;

/*
 * Code declaration elements
 */

codeEntity
    : varBlock
    | protectedVarBlock
    | triggerDeclaration
    | methodDeclaration
    ;

codeDeclarations
    : codeEntity+
    ;

/*
 * AL IF statement logic
 */

ifCondition
   : IF expression THEN;

ifElse
   : ELSE statement?;

ifStatement
   : ifCondition (statement ifElse?)?;

/*
 * AL WHILE statement logic
 */

whileConditional
   : WHILE expression DO;

whileStatement
   : whileConditional statement?;

/*
 * AL FOR statement logic
 */

forValue
   : expression
   ;

forControl
   : FOR identifier ASSGN expression (TO | DOWNTO) expression DO;

forStatement
   : forControl statement?;

/*
 * AL FOREACH statement logic
 */

enumeratorExpression
   : expression
   ;

forEachControl:
	FOREACH identifier IN enumeratorExpression DO;

forEachStatement: forEachControl statement?;

/*
 * AL CASE statement logic
 */

caseValue
   : expression
   ;

caseSet
   : caseValue (COMMA caseValue)*?;

caseRange
   : caseValue RANGE caseValue;

caseValueCondition
   : (caseSet | caseRange) COLON statement?;

caseElse
   : ELSE statement?;

caseBody
   : caseValueCondition (SEMICOLON caseValueCondition)*? (SEMICOLON caseElse)?;

caseControl
   : CASE expression OF;

caseStatement
   : caseControl caseBody? SEMICOLON? END;

/*
 * AL CONTINUE statement logic
 */

continueStatement
   : CONTINUE;

/*
 * AL BREAK statement logic
 */

breakStatement: BREAK;

/*
 * AL REPEAT UNTIL statement logic
 */

untilCondition
   : UNTIL expression;

repeatUntilStatement
   : REPEAT statementList? SEMICOLON? untilCondition;

/*
 * AL WITH statement logic
 * (DEPRECATED in Dynamics 365 Business Central 2020, release wave 2)
 */

withControl
   : WITH identifier DO;

withStatement
   : withControl statement?;

/*
 *AL Exit statement logic
 */

exitStatement
   : EXIT (LEFTPAREN expression RIGHTPAREN)?;

/*
 * AL generic statement statement logic
 */

statementLine
   : ifStatement
   | forStatement
   | forEachStatement
   | caseStatement
   | withStatement
   | whileStatement
   | repeatUntilStatement
   | exitStatement
   | continueStatement
   | breakStatement
   | expression;

statementBlock
   : BEGIN statementList? SEMICOLON? END;

statement
   : (statementLine | statementBlock)
   ;

statementList
   : statementLine (SEMICOLON statementLine)*?;

/*
 * AL expression logic
 */

indexAccessorValue
   : INTEGER_LITERAL
   | identifier
   ;

indexAccessorSet
   : indexAccessorValue (COMMA indexAccessorValue)*;

valueSet
   : expression (COMMA expression)*;

methodCallArguments
   : expression (COMMA expression)*
   ;

expression
   : LEFTPAREN expr=expression RIGHTPAREN #ParenthesisExpression
   | booleanLiteral #BooleanLiteralExpression
   | DATE_LITERAL #DateLiteralExpression
   | TIME_LITERAL #TimeLiteralExpression
   | DATETIME_LITERAL #DatetimeLiteralExpression
   | STRING_LITERAL #StringLiteralExpression
   | FLOAT_LITERAL #FloatLiteralExpression
   | INTEGER_LITERAL	#IntegerLiteralExpression
   | systemEnumerationLiteral #SystemEnumerationLiteralExpression
   | identifier #IdentifierExpression
   | expression PERIOD name=methodName LEFTPAREN arguments=methodCallArguments? RIGHTPAREN #MethodCallExpression
   | name=methodName LEFTPAREN arguments=methodCallArguments? RIGHTPAREN #MethodCallExpression
   | expr=expression SCOPE scope=identifier #ScopeExpression
   | expr=expression PERIOD member=identifier #MemberAccessExpression
   | expr=expression LEFTBRACKET indexValue=expression RIGHTBRACKET #IndexExpression
   | LEFTBRACKET setExpr=valueSet? RIGHTBRACKET #SetExpression
   | NOT expr=expression #NotExpression
   | MINUS expr=expression #NegativeExpression
   | lhs=expression ASTERISK rhs=expression	#MultiplyExpression
   | lhs=expression BACKSLASH rhs=expression #DivideExpression
   | lhs=expression DIV rhs=expression #IntegerDivideExpression
   | lhs=expression MOD rhs=expression #ModulusExpression
   | lhs=expression PLUS rhs=expression #AddExpression
   | lhs=expression MINUS rhs=expression #SubtractExpression
   | lhs=expression (LESSTHAN | GREATERTHAN | LESSTHANEQUAL | GREATERTHANEQUAL | NOTEQUAL | EQUAL) rhs=expression #ComparisonExpression
   | lhs=expression (AND | OR | XOR) rhs=expression #LogicalComparisonExpression
   | condition=expression CONDITION trueExpr=expression COLON falseExpr=expression #TernaryExpression
   | lhs=expression (ASSGN | DIV_ASSGN | MULTIPLY_ASSGN | ADD_ASSGN | MINUS_ASSGN) rhs=expression #AssignmentExpression
   | expr=expression IN LEFTBRACKET setExpr=valueSet? RIGHTBRACKET #InRangeExpression
   | GUIALLOWED #GuiAllowedFunctionExpression
   ;
