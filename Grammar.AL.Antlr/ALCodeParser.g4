parser grammar ALCodeParser;

options { tokenVocab=ALLexer; }

import ALCoreParser;

/*
 * AL Code Parser
 This parser is designed to parse AL code, including variable declarations,
 * method declarations, control structures, expressions and codeunits.
 */

@parser::members
{  // Define our member text lists
   List<string> Builtins = new List<string>()
      {
         "action", "array", "automation", "biginteger", "bigtext", "blob", "boolean", "byte",
         "char", "clienttype", "code", "codeunit", "completiontriggererrorlevel", "connectiontype",
         "database", "dataclassification", "datascope", "date", "dateformula", "datetime", "decimal",
         "defaultlayout", "dialog", "dictionary", "dotnet", "dotnetassembly", "dotnettypedeclaration",
         "duration", "enum", "errorinfo", "errortype", "executioncontext", "executionmode", "fieldclass",
         "fieldref", "fieldtype", "file", "filterpagebuilder", "guid", "instream", "integer", "joker",
         "keyref", "list", "moduledependencyinfo", "moduleinfo", "none", "notification",
         "notificationscope", "objecttype", "option", "outstream", "page", "pageresult", "query",
         "record", "recordid", "recordref", "report", "reportformat", "securityfilter", "securityfiltering",
         "table", "tableconnectiontype", "tablefilter", "testaction", "testfield", "testfilterfield",
         "testpage", "testpermissions", "testrequestpage", "text", "textbuilder", "textconst", "textencoding",
         "time", "transactionmodel", "transactiontype", "variant", "verbosity", "version", "xmlport",
         "httpcontent", "httpheaders", "httpclient", "httprequestmessage", "httpresponsemessage",
         "jsontoken", "jsonvalue", "jsonarray", "jsonobject", "view", "views", "xmlattribute",
         "xmlattributecollection", "xmlcomment", "xmlcdata", "xmldeclaration", "xmldocument",
         "xmldocumenttype", "xmlelement", "xmlnamespacemanager", "xmlnametable", "xmlnode",
         "xmlnodelist", "xmlprocessinginstruction", "xmlreadoptions", "xmltext", "xmlwriteoptions",
         "webserviceactioncontext", "webserviceactionresultcode", "sessionsettings;"
      };

      List<string> VariableTypes = new List<string>()
      {
         "any", "biginteger", "bigtext", "blob", "boolean", "byte", "char", "code",
         "codeunit", "companyproperty", "database", "datatransfer", "date", "dateformula",
         "datetime", "debugger", "decimal", "dialog", "dictionary", "dotnet", "duration",
         "enum", "errorinfo", "fieldref", "file", "filterpagebuilder", "guid", "httpclient",
         "httpcontent", "httpheaders", "httprequestmessage", "httpresponsemessage",
         "instream", "integer", "isolatedstorage", "jsonarray", "jsonobject", "jsontoken", "jsonvalue",
         "keyref", "label", "list", "media", "mediaset", "moduledependencyinfo", "moduleinfo",
         "navapp", "none", "notification", "numbersequence", "option", "outstream", "page", "productname", "query",
         "record", "recordid", "recordref", "report", "requestpage", "session", "sessioninformation",
         "sessionsettings", "system", "taskscheduler", "testaction", "testfield",
         "testfilter", "testfilterfield", "testpage", "testpart", "testrequestpage", "text",
         "textbuilder", "textconst", "time", "variant", "version", "webserviceactioncontext",
         "xmlattribute", "xmlattributecollection", "xmlcdata", "xmlcomment", "xmldeclaration",
         "xmldocument", "xmldocumenttype", "xmlelement", "xmlnamespacemanager", "xmlnametable",
         "xmlnode", "xmlnodelist", "xmlport", "xmlprocessinginstruction", "xmlreadoptions", "xmltext",
         "xmlwriteoptions", "action", "auditcategory", "clienttype", "commitbehavior", "dataclassification",
         "datascope", "defaultlayout", "errorbehavior", "errortype", "executioncontext", "executionmode",
         "fieldclass", "fieldtype", "inherentpermissionsscope", "isolationlevel", "notificationscope",
         "objecttype", "pagebackgroundtaskerrorlevel", "permissionobjecttype", "reportformat", "reportlayouttype",
         "securityfilter", "securityoperationresult", "tableconnectiontype", "telemetryscope", "testpermissions",
         "textencoding", "transactionmodel", "transactiontype", "verbosity", "webserviceactionresultcode"
      };

      List<string> VarAppObjects = new List<string>() {"codeunit", "page", "requestpage", "dotnet", "enum", "query", "report", "xmlport"};

      List<string> SimpleFieldTypes = new List<string>()
      {
         "integer", "biginteger", "decimal", "enum", "boolean", "binary", "blob", "date", "time", "datetime", "dateformula", "duration", "recordid", "tablefilter", "option", "guid"
      };

      // Positioning keywords for various entities (Groups, Areas, Actions, Controls)
      List<string> GroupPosKeywords = new List<string>()
      {
         "addfirst", "addlast", "addafter", "addbefore", "movefirst", "movelast", "moveafter", "movebefore", "modify"
      };

      List<string> AreaPosKeywords = new List<string>()
      {
         "addfirst", "addlast", "movefirst", "movelast"
      };

      List<string> ActionPosKeywords = new List<string>()
      {
         "addafter", "addbefore", "moveafter", "movebefore", "modify"
      };

      List<string> ControlPosKeywords = new List<string>()
      {
         "addafter", "addbefore", "moveafter", "movebefore", "modify"
      };

      List<string> ObsoleteStateValues = new List<string>()
      {
         "pending", "no"
      };


      // Define helper methods for token text matching

      bool TokenMatches(string text)
      {
         return _input.Lt(1).Text.ToLowerInvariant() == text.ToLowerInvariant();
      }

      bool TokenMatches(List<string> values)
      {
         return values.Contains(_input.Lt(1).Text.ToLowerInvariant());
      }
}


/*
 * Property declarations
 */

keyValueProperty
   : identifier EQUAL (STRING_LITERAL | INTEGER_LITERAL | FLOAT_LITERAL | identifier | booleanLiteral) SEMICOLON
   ;

identifierList
   : identifier (COMMA identifier)*
   ;

keyIdentifierListProperty
   : identifier EQUAL identifierList? SEMICOLON
   ;

permissionSpecifier
   : TABLEDATA objectId EQUAL identifier
   ;

permissionSpecifiers
   : permissionSpecifier (COMMA permissionSpecifier)*;

permissionsProperty
   : PERMISSIONS EQUAL permissionSpecifiers SEMICOLON
   ;

/*
 * Method
 */

methodName
   : identifier;

methodDeclaration
   : methodAttribute*? LOCAL? PROCEDURE identifier LEFTPAREN parameterList? RIGHTPAREN returnValue? varBlock? statementBlock SEMICOLON;

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
   : DICTIONARY OF LEFTBRACKET dictionaryKey COMMA dictionaryDataType RIGHTBRACKET #DictionaryVariable
   | LIST OF LEFTBRACKET variableTypeDeclaration RIGHTBRACKET #ListVariable
   | ARRAY LEFTBRACKET dimensions RIGHTBRACKET OF variableTypeDeclaration #ArrayVariable
   | BIGINTEGER #BigIntegerVariable
   | BLOB #BlobVariable
   | BOOLEAN #BooleanVariable
   | BYTE #ByteVariable
   | CHAR #CharVariable
   | CODE sizeDeclaration #CodeVariable
   | CODEUNIT objectId #CodeunitVariable
   | DATE #DateVariable
   | DATETIME #DatetimeVariable
   | DECIMAL #DecimalVariable
   | DOTNET objectId #DotNetVariable
   | DURATION #DurationVariable
   | ENUM objectId #EnumVariable
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
   | JSONARRAY #JsonArrayVariable
   | JSONOBJECT #JsonObjectVariable
   | JSONTOKEN #JsonTokenVariable
   | JSONVALUE #JsonValueVariable
   | KEYREF #KeyRefVariable
   | LABEL labelText (COMMA labelArgs)? #LabelVariable
   | LABEL #LabelVariable
   | MODULEDEPENDENCYINFO #ModuleDependencyInfoVariable
   | MODULEINFO #ModuleInfoVariable
   | NOTIFICATION #NotificationVariable
   | OPTION #OptionVariable
   | OUTSTREAM #OutStreamVariable
   | PAGE objectId #PageVariable
   | QUERY objectId #QueryVariable
   | RECORD objectId TEMPORARY? #RecordVariable
   | RECORD #RecordVariable
   | RECORDID #RecordIdVariable
   | RECORDREF #RecordRefVariable
   | REPORT objectId #ReportVariable
   | SESSIONSETTINGS #SessionSettingsVariable
   | TEXT sizeDeclaration? #TextVariable
   | TEXTBUILDER #TextBuilderVariable
   | TEXTCONST identifier EQUAL STRING_LITERAL #TextConstantVariable
   | TIME #TimeVariable
   | VARIANT #VariantVariable
   | VERSION #VersionVariable
   | XMLATTRIBUTE #XmlAttributeVariable
   | XMLDOCUMENT #XmlDocumentVariable
   | XMLELEMENT #XmlElementVariable
   | XMLNODE #XmlNodeVariable
   | XMLPORT objectId #XmlPortVariable
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

variableName
   : identifier;

variableNameList
	: variableName (COMMA variableName)*
   ;

variableDeclaration
   : variableNameList COLON variableTypeDeclaration SEMICOLON
   ;

variableDeclarations
   : variableDeclaration*
   ;

varBlock
   : VAR variableDeclarations
   ;

protectedVarBlock
   : PROTECTED VAR variableDeclaration
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
   : ifCondition statement (ifElse)?;

/*
 * AL WHILE statement logic
 */

whileConditional
   : WHILE expression DO;

whileStatement
   : whileConditional statement;

/*
 * AL FOR statement logic
 */

forValue
   : expression
   ;

forControl
   : FOR identifier ASSGN expression (TO | DOWNTO) expression DO;

forStatement
   : forControl statement;

/*
 * AL FOREACH statement logic
 */

forEachControl:
	FOREACH identifier IN expression DO;

forEachStatement: forEachControl statement;

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
   : ELSE statement? SEMICOLON?;

caseBody
   : (caseValueCondition (SEMICOLON caseValueCondition?)*?)? caseElse?;

caseControl
   : CASE expression OF;

caseStatement
   : caseControl caseBody END;

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
   : REPEAT statementList SEMICOLON? untilCondition;

/*
 * AL WITH statement logic
 * (DEPRECATED in Dynamics 365 Business Central 2020, release wave 2)
 */

withControl
   : WITH identifier DO;

withStatement
   : withControl statement;

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
   : BEGIN statementList END;

statement
   : (statementLine | statementBlock)
   ;

statementList
   : (statementLine (SEMICOLON statementLine?)*?)?;

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
   : LEFTPAREN expression RIGHTPAREN #ParenthesisExpression
   | booleanLiteral #BooleanLiteralExpression
   | DATE_LITERAL #DateLiteralExpression
   | TIME_LITERAL #TimeLiteralExpression
   | DATETIME_LITERAL #DatetimeLiteralExpression
   | STRING_LITERAL #StringLiteralExpression
   | FLOAT_LITERAL #FloatLiteralExpression
   | INTEGER_LITERAL	#IntegerLiteralExpression
   | identifier #IdentifierExpression
   | expression PERIOD methodName LEFTPAREN methodCallArguments? RIGHTPAREN #MethodCallExpression
   | methodName LEFTPAREN methodCallArguments? RIGHTPAREN #MethodCallExpression
   | expression SCOPE identifier # ScopeExpression
   | expression PERIOD identifier #MemberAccessExpression
   | expression LEFTBRACKET indexAccessorSet RIGHTBRACKET #IndexExpression
   | LEFTBRACKET valueSet? RIGHTBRACKET #SetExpression
   | NOT expression #NotExpression
   | MINUS expression #NegativeExpression
   | expression ASTERISK expression	#MultiplyExpression
   | expression BACKSLASH expression #DivideExpression
   | expression DIV expression #IntegerDivideExpression
   | expression MOD expression #ModulusExpression
   | expression PLUS expression #AddExpression
   | expression MINUS expression #SubtractExpression
   | expression (LESSTHAN | GREATERTHAN | LESSTHANEQUAL | GREATERTHANEQUAL | NOTEQUAL | EQUAL) expression #ComparisonExpression
   | expression (AND | OR | XOR) expression #LogicalComparisonExpression
   | expression CONDITION expression COLON expression #TernaryExpression
   | expression (ASSGN | DIV_ASSGN | MULTIPLY_ASSGN | ADD_ASSGN | MINUS_ASSGN) expression #AssignmentExpression
   | expression IN LEFTBRACKET valueSet? RIGHTBRACKET #InRangeExpression
   | GUIALLOWED #GuiAllowedFunctionExpression
   ;
