parser grammar ALCodeParser;

options { tokenVocab=ALLexer; }

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
 * Generic constructs
 */

namespaceIdentifier
   : IDENTIFIER namespaceAccessor?
   ;

namespaceAccessor
   : PERIOD IDENTIFIER namespaceAccessor?
   ;

objectId
   : simpleObjectId
   | qualifiedObjectId;

objectName
   : IDENTIFIER
   ;

simpleObjectId
   : IDENTIFIER
   | INTEGER_LITERAL;

qualifiedObjectId
   : namespaceIdentifier PERIOD IDENTIFIER
   ;

booleanLiteral
   : TRUE
   | FALSE
   ;

numberLiteral
   : FLOAT_LITERAL
   | INTEGER_LITERAL
   ;

objectLiteral
   : (namespaceIdentifier PERIOD)? IDENTIFIER
   ;

enumerationLiteral
   : objectLiteral SCOPE IDENTIFIER
   ;

systemEnumerationLiteral
   : OBJECTTYPE SCOPE IDENTIFIER #ObjectTypeLiteral
   | TRANSACTIONTYPE SCOPE IDENTIFIER #TransactionTypeLiteral
   | EXECUTIONMODE SCOPE IDENTIFIER #ExecutionModeLiteral
   | SECURITYFILTER SCOPE IDENTIFIER #SecurityFilterLiteral
   | TEXTENCODING SCOPE IDENTIFIER #TextEncodingLiteral
   | REPORTFORMAT SCOPE IDENTIFIER #ReportFormatLiteral
   | DEFAULTLAYOUT SCOPE IDENTIFIER #DefaultLayoutLiteral
   | CONNECTIONTYPE SCOPE IDENTIFIER #ConnectionTypeLiteral
   | CLIENTTYPE SCOPE IDENTIFIER #ClientTypeLiteral
   | COMMITBEHAVIOR SCOPE IDENTIFIER #CommitBehaviorLiteral
   | DATASCOPE SCOPE IDENTIFIER #DataScopeLiteral
   | DATACLASSIFICATION SCOPE IDENTIFIER #DataClassificationLiteral
   | ACTION SCOPE IDENTIFIER #ActionLiteral
   | AUDITCATEGORY SCOPE IDENTIFIER #AuditCategoryLiteral
   | EXECUTIONCONTEXT SCOPE IDENTIFIER #ExecutionContextLiteral
   | ERRORBEHAVIOR SCOPE IDENTIFIER #ErrorBehaviorLiteral
   | ERRORTYPE SCOPE IDENTIFIER #ErrorTypeLiteral
   | ERRORCONTEXT SCOPE IDENTIFIER #ErrorContextLiteral
   | FIELDCLASS SCOPE IDENTIFIER #FieldClassLiteral
   | FIELDTYPE SCOPE IDENTIFIER #FieldTypeLiteral
   | INHERENTPERMISSIONSSCOPE SCOPE IDENTIFIER #InherentPermissionsScopeLiteral
   | ISOLATIONLEVEL SCOPE IDENTIFIER #IsolationLevelLiteral
   | NOTIFICATIONSCOPE SCOPE IDENTIFIER #NotificationScopeLiteral
   | OBJECTTYPE SCOPE IDENTIFIER #ObjectTypeLiteral
   | PAGEBACKGROUNDTASKERRORLEVEL SCOPE IDENTIFIER #PageBackgroundTaskErrorLevelLiteral
   | PERMISSIONOBJECTTYPE SCOPE IDENTIFIER #PermissionObjectTypeLiteral
   | REPORTLAYOUTTYPE SCOPE IDENTIFIER #ReportLayoutTypeLiteral
   | SECURITYOPERATIONRESULT SCOPE IDENTIFIER #SecurityOperationResultLiteral
   | TABLECONNECTIONTYPE SCOPE IDENTIFIER #TableConnectionTypeLiteral
   | TELEMETRYSCOPE SCOPE IDENTIFIER #TelemetryScopeLiteral
   | TESTPERMISSIONS SCOPE IDENTIFIER #TestPermissionsLiteral
   | TRANSACTIONMODEL SCOPE IDENTIFIER #TransactionModelLiteral
   | TRANSACTIONTYPE SCOPE IDENTIFIER #TransactionTypeLiteral
   | VERBOSITY SCOPE IDENTIFIER #VerbosityLiteral
   | WEBSERVICEACTIONRESULTCODE SCOPE IDENTIFIER #WebServiceActionResultCodeLiteral
   | CODEUNIT SCOPE objectLiteral #CodeunitLiteral
   | PAGE SCOPE objectLiteral #PageLiteral
   | PAGEEXTENSION SCOPE objectLiteral #PageExtensionLiteral
   | QUERY SCOPE objectLiteral #QueryLiteral
   | REPORT SCOPE objectLiteral #ReportLiteral
   | XMLPORT SCOPE objectLiteral #XmlPortLiteral
   | DATABASE SCOPE objectLiteral #DatabaseLiteral
   | ENUM SCOPE objectLiteral #EnumLiteral
   | TESTPAGE SCOPE objectLiteral #TestPageLiteral
   | TESTREQUESTPAGE SCOPE objectLiteral #TestRequestPageLiteral
   ;

/*
 * Property declarations
 */

keyValueProperty
   : IDENTIFIER EQUAL (STRING_LITERAL | INTEGER_LITERAL | FLOAT_LITERAL | IDENTIFIER | booleanLiteral) SEMICOLON
   ;

identifierList
   : IDENTIFIER (COMMA IDENTIFIER)*
   ;

keyIdentifierListProperty
   : IDENTIFIER EQUAL identifierList? SEMICOLON
   ;

permissionSpecifier
   : TABLEDATA objectId EQUAL IDENTIFIER
   ;

permissionSpecifiers
   : permissionSpecifier (COMMA permissionSpecifier)*;

permissionsProperty
   : PERMISSIONS EQUAL permissionSpecifiers SEMICOLON
   ;

/*
 * Type declarations
 */

sizeDeclaration : LEFTBRACKET INTEGER_LITERAL RIGHTBRACKET;

builtinType
   : IDENTIFIER;
   /*
   : ACTION | ARRAY | AUTOMATION | BIGINTEGER | BIGTEXT | BLOB | BOOLEAN | BYTE | CHAR | CLIENTTYPE | CODE
   | CODEUNIT | COMPLETIONTRIGGERERRORLEVEL | CONNECTIONTYPE | DATABASE | DATACLASSIFICATION | DATASCOPE
   | DATE | DATEFORMULA | DATETIME | DECIMAL | DEFAULTLAYOUT | DIALOG | DICTIONARY | DOTNET | DOTNETASSEMBLY
   | DOTNETTYPEDECLARATION | DURATION | ENUM | ERRORINFO | ERRORTYPE | EXECUTIONCONTEXT | EXECUTIONMODE
   | FIELDCLASS | FIELDREF | FIELDTYPE | FILE | FILTERPAGEBUILDER | GUID | INSTREAM | INTEGER | JOKER
   | KEYREF | LIST | MODULEDEPENDENCYINFO | MODULEINFO | NONE | NOTIFICATION | NOTIFICATIONSCOPE | OBJECTTYPE
   | OPTION | OUTSTREAM | PAGE | PAGERESULT | QUERY | RECORD | RECORDID | RECORDREF | REPORT | REPORTFORMAT
   | SECURITYFILTER | SECURITYFILTERING | TABLE | TABLECONNECTIONTYPE | TABLEFILTER | TESTACTION | TESTFIELD
   | TESTFILTERFIELD | TESTPAGE | TESTPERMISSIONS | TESTREQUESTPAGE | TEXT | TEXTBUILDER | TEXTCONST
   | TEXTENCODING | TIME | TRANSACTIONMODEL | TRANSACTIONTYPE | VARIANT | VERBOSITY | VERSION | XMLPORT
   | HTTPCONTENT | HTTPHEADERS | HTTPCLIENT | HTTPREQUESTMESSAGE | HTTPRESPONSEMESSAGE | JSONTOKEN
   | JSONVALUE | JSONARRAY | JSONOBJECT | VIEW | VIEWS | XMLATTRIBUTE | XMLATTRIBUTECOLLECTION | XMLCOMMENT
   | XMLCDATA | XMLDECLARATION | XMLDOCUMENT | XMLDOCUMENTTYPE | XMLELEMENT | XMLNAMESPACEMANAGER
   | XMLNAMETABLE | XMLNODE | XMLNODELIST | XMLPROCESSINGINSTRUCTION | XMLREADOPTIONS | XMLTEXT
   | XMLWRITEOPTIONS | WEBSERVICEACTIONCONTEXT | WEBSERVICEACTIONRESULTCODE | SESSIONSETTINGS;
*/

/*
 * Method
 */

methodName
   : IDENTIFIER;

methodDeclaration
   : methodAttribute*? LOCAL? PROCEDURE IDENTIFIER LEFTPAREN parameterList? RIGHTPAREN returnValue? varBlock? statementBlock SEMICOLON;

/*
 * Method attributes
 */

attributeIdentifier
   : IDENTIFIER COLON;

attributeArgument
   : (attributeIdentifier)? (STRING_LITERAL | INTEGER_LITERAL	| FLOAT_LITERAL | booleanLiteral | systemEnumerationLiteral | enumerationLiteral);

attributeArgumentList
   : attributeArgument (COMMA attributeArgument)*?;

methodAttribute
   : LEFTBRACKET IDENTIFIER (LEFTPAREN attributeArgumentList? RIGHTPAREN)? RIGHTBRACKET;

/*
 * Variables
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
   : {TokenMatches("MaxLength")}? IDENTIFIER EQUAL INTEGER_LITERAL
   ;

labelComment
   : {TokenMatches("Comment")}? IDENTIFIER EQUAL STRING_LITERAL
   ;

labelLocked
   : {TokenMatches("Locked")}? IDENTIFIER EQUAL (TRUE | FALSE)
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
   : IDENTIFIER
   ;

optionValueList
   : optionValue (COMMA optionValue)*?
   ;

dimensions
   : INTEGER_LITERAL (COMMA INTEGER_LITERAL)*?
   ;

variableTypeDeclaration
   : DICTIONARY OF LEFTBRACKET dictionaryKey COMMA dictionaryDataType RIGHTBRACKET #DictionaryVariable
   | LIST OF LEFTBRACKET variableTypeDeclaration RIGHTBRACKET #ListVariable
   | ARRAY LEFTBRACKET dimensions RIGHTBRACKET OF variableTypeDeclaration #ArrayVariable
   | LABEL labelText (COMMA labelArgs)? #LabelVariable
   | RECORD objectId TEMPORARY? #RecordVariable
   | TEXT sizeDeclaration? #TextVariable
   | CODE sizeDeclaration #CodeVariable
   | TEXTCONST IDENTIFIER EQUAL STRING_LITERAL #TextConstantVariable
   | INTEGER #IntegerVariable
   | DECIMAL #DecimalVariable
   | BOOLEAN #BooleanVariable
   | CHAR #CharVariable
   | DATE #DateVariable
   | TIME #TimeVariable
   | DATETIME #DatetimeVariable
   | DURATION #DurationVariable
   | GUID #GuidVariable
   | OPTION #OptionVariable
   | INSTREAM #InStreamVariable
   | OUTSTREAM #OutStreamVariable
   | FILE #FileVariable
   | LABEL #LabelVariable
   | VARIANT #VariantVariable
   | BIGINTEGER #BigIntegerVariable
   | BYTE #ByteVariable
   | RECORD #RecordVariable
   | RECORDREF #RecordRefVariable
   | FIELDREF #FieldRefVariable
   | KEYREF #KeyRefVariable
   | RECORDID #RecordIdVariable
   | HTTPCLIENT #HttpClientVariable
   | HTTPHEADERS #HttpHeadersVariable
   | HTTPREQUESTMESSAGE #HttpRequestMessageVariable
   | HTTPRESPONSEMESSAGE #HttpResponseMessageVariable
   | HTTPCONTENT #HttpContentVariable
   | JSONOBJECT #JsonObjectVariable
   | JSONARRAY #JsonArrayVariable
   | JSONVALUE #JsonValueVariable
   | JSONTOKEN #JsonTokenVariable
   | XMLDOCUMENT #XmlDocumentVariable
   | XMLELEMENT #XmlElementVariable
   | XMLATTRIBUTE #XmlAttributeVariable
   | XMLNODE #XmlNodeVariable
   | TEXTBUILDER #TextBuilderVariable
   | FILTERPAGEBUILDER #FilterPageBuilderVariable
   | NOTIFICATION #NotificationVariable
   | SESSIONSETTINGS #SessionSettingsVariable
   | CODEUNIT objectId #CodeunitVariable
   | PAGE objectId #PageVariable
   | DOTNET objectId #DotNetVariable
   | ENUM objectId #EnumVariable
   | QUERY objectId #QueryVariable
   | REPORT objectId #ReportVariable
   | XMLPORT objectId #XmlPortVariable
   ;

parameterName
   : IDENTIFIER
   ;

parameterDeclaration
   : VAR? parameterName COLON variableTypeDeclaration
   ;

parameterList
   : parameterDeclaration (SEMICOLON parameterDeclaration)*
   ;

variableName
   : IDENTIFIER;

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
   : IDENTIFIER? returnType
   ;

returnType
   : COLON variableTypeDeclaration
   ;

/*
 * Trigger declarations
 */

triggerName : IDENTIFIER;

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
   : FOR IDENTIFIER ASSGN expression (TO | DOWNTO) expression DO;

forStatement
   : forControl statement;

/*
 * AL FOREACH statement logic
 */

forEachControl:
	FOREACH IDENTIFIER IN expression DO;

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
   : WITH IDENTIFIER DO;

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
   | IDENTIFIER
   ;

indexAccessorSet
   : indexAccessorValue (COMMA indexAccessorValue)*;

valueSet
   : expression (COMMA expression)*;

expression
   : LEFTPAREN expression RIGHTPAREN #ParenthesisExpression
   | booleanLiteral #BooleanLiteralExpression
   | DATE_LITERAL #DateLiteralExpression
   | TIME_LITERAL #TimeLiteralExpression
   | DATETIME_LITERAL #DatetimeLiteralExpression
   | STRING_LITERAL #StringLiteralExpression
   | FLOAT_LITERAL #FloatLiteralExpression
   | INTEGER_LITERAL	#IntegerLiteralExpression
   | IDENTIFIER #IdentifierExpression
   | expression PERIOD methodName LEFTPAREN methodCallArguments? RIGHTPAREN #MethodCallExpression
   | methodName LEFTPAREN methodCallArguments? RIGHTPAREN #MethodCallExpression
   | expression SCOPE IDENTIFIER # ScopeExpression
   | expression PERIOD IDENTIFIER #MemberAccessExpression
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

methodCallArguments
   : expression (COMMA expression)*
   ;

/*
* Codeunit structure
*/

namespaceDeclaration
   : {TokenMatches("namespace")}? IDENTIFIER namespaceIdentifier SEMICOLON;

usingDeclaration
   : {TokenMatches("using")}? IDENTIFIER namespaceIdentifier SEMICOLON;

usingDeclarations
   : usingDeclaration*;

codeunitProperty
   : {TokenMatches("Obsolete")}? IDENTIFIER EQUAL STRING_LITERAL SEMICOLON
   | {TokenMatches("ObsoleteState")}? IDENTIFIER EQUAL {TokenMatches(ObsoleteStateValues)}? IDENTIFIER SEMICOLON
   | {TokenMatches("ObsoleteTag")}? IDENTIFIER EQUAL STRING_LITERAL SEMICOLON
   | permissionsProperty
   | keyValueProperty
   | keyIdentifierListProperty
   ;

codeunitProperties
   : codeunitProperty*;

codeunitDeclaration
   : namespaceDeclaration? usingDeclarations? CODEUNIT objectId? objectName LEFTCBRACE codeunitProperties? codeDeclarations? RIGHTCBRACE;