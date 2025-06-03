parser grammar ALCodeParser;

options { tokenVocab=ALLexer; }

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

      // Define helper methods for token text matching

      bool TokenMatches(string text)
      {
         return _input.Lt(1).Text.ToLowerInvariant() == text;
      }

      bool TokenMatches(List<string> values)
      {
         return values.Contains(_input.Lt(1).Text.ToLowerInvariant());
      }
}

comment
   : IDENTIFIER
   ;

maxLength
   : IDENTIFIER
   ;

locked
   : IDENTIFIER
   ;

keyValueProperty
   : IDENTIFIER EQUAL (STRING_LITERAL | INTEGER_LITERAL | FLOAT_LITERAL | IDENTIFIER | booleanLiteral) SEMICOLON
   ;

identifierList
   : IDENTIFIER (COMMA IDENTIFIER)*?
   ;

keyIdentifierListProperty
   : IDENTIFIER EQUAL identifierList? SEMICOLON
   ;

permissionSpeficier
   : {TokenMatches("tabledata")}? IDENTIFIER objectId EQUAL IDENTIFIER
   ;

permissionsProperty
   : {TokenMatches("permissions")}? IDENTIFIER EQUAL permissionSpeficier (COMMA permissionSpeficier)*? SEMICOLON
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

methodDeclaration
   : methodAttribute*? PROCEDURE IDENTIFIER LEFTPAREN parameterList? RIGHTPAREN returnValue? varBlock? statementBlock SEMICOLON;

/*
 * Method attributes
 */

attributeArgument
   : IDENTIFIER COLON builtinType;

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
   : maxLength EQUAL INTEGER_LITERAL
   ;

labelComment
   : comment EQUAL STRING_LITERAL
   ;

labelLocked
   : locked EQUAL (TRUE | FALSE)
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

objectId
   : IDENTIFIER
   | INTEGER_LITERAL
   ;

dimensions
   : INTEGER_LITERAL (COMMA INTEGER_LITERAL)*?
   ;

variableTypeDeclaration
   : {TokenMatches("dictionary")}? IDENTIFIER OF LEFTBRACKET dictionaryKey COMMA dictionaryDataType RIGHTBRACKET #DictionaryVariable
   | {TokenMatches("list")}? IDENTIFIER OF LEFTBRACKET variableTypeDeclaration RIGHTBRACKET #ListVariable
   | ARRAY LEFTBRACKET dimensions RIGHTBRACKET OF variableTypeDeclaration #ArrayVariable
   | {TokenMatches(VarAppObjects)}? IDENTIFIER objectId #ApplicationObjectVariable
   | {TokenMatches("label")}? IDENTIFIER labelText (COMMA labelArgs)? #LabelVariable   
   | {TokenMatches("record")}? IDENTIFIER objectId TEMPORARY? #RecordVariable   
   | {TokenMatches("text")}? IDENTIFIER sizeDeclaration? #TextVariable
   | {TokenMatches("code")}? IDENTIFIER sizeDeclaration #CodeVariable
   | {TokenMatches("textconst")}? IDENTIFIER IDENTIFIER EQUAL STRING_LITERAL #TextConstantVariable
   | {TokenMatches(VariableTypes)}? IDENTIFIER #SimpleVariable
   ;

parameterName
   : IDENTIFIER
   ;

parameterDeclaration
   : VAR? parameterName COLON variableTypeDeclaration
   ;

parameterList
   : parameterDeclaration (SEMICOLON parameterDeclaration)*?
   ;
   
variableNameList
	: IDENTIFIER (COMMA IDENTIFIER)*?
   ;

variableDeclaration
   : variableNameList COLON variableTypeDeclaration SEMICOLON
   ;

variableDeclarations
   : variableDeclaration (variableDeclaration)*?
   ;

varBlock
   : VAR variableDeclarations
   ;

protectedVarBlock
   : PROTECTED VAR variableDeclaration
   ;

returnValue
   : COLON IDENTIFIER
   ;

/*
 * Trigger declarations
 */

triggerName : IDENTIFIER;

triggerDeclaration
   : TRIGGER triggerName LEFTPAREN parameterList? RIGHTPAREN returnValue? varBlock? statementBlock SEMICOLON;

/*
 * Code delcaration elements
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

ifConditionStatement
   : IF expression THEN;

elseStatement
   : ELSE statement;

ifStatement
   : ifConditionStatement statement (elseStatement)?;

/*
 * AL WHILE statement logic
 */

whileConditionalStatement
   : WHILE expression DO;

whileStatement
   : whileConditionalStatement statement;

/*
 * AL FOR statement logic
 */

forValue
   : expression
   ;

forControlStatement
   : FOR IDENTIFIER ASSGN expression (TO | DOWNTO) expression DO;

forStatement
   : forControlStatement statement;

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

caseValueStatement
   : (caseSet | caseRange) COLON statement;

caseElseStatement
   : ELSE statement;

caseBody
   : (caseValueStatement (SEMICOLON caseValueStatement)*?)? caseElseStatement?;

caseControlStatement
   : CASE expression OF;

caseStatement
   : caseControlStatement caseBody END;

/*
 * AL REPEAT UNTIL statement logic
 */

untilCondition
   : UNTIL expression;

repeatUntilStatement
   : REPEAT statement untilCondition;

/*
 * AL WITH statement logic
 * (DEPRECATED in Dynamics 365 Business Central 2020, release wave 2)
 */

withControlStatement
   : WITH IDENTIFIER DO;

withStatement
   : withControlStatement statement;

/* 
 *AL Exit statement logic
 */

 exitStatement
   : EXIT (expression)?;

/*
 * AL generic statement statement logic
 */

statementLine
   : ifStatement
   | forStatement
   | caseStatement
   | withStatement
   | whileStatement
   | repeatUntilStatement
   | exitStatement
   | expression;
   
statementBlock
   : BEGIN statementList END;

statement
   : (statementLine | statementBlock) SEMICOLON?
   ;

statementList
   : (statementLine (SEMICOLON statementLine)*?)? SEMICOLON?;

/*
 * AL expression logic
 */

indexAccessorValue
   : INTEGER_LITERAL
   | IDENTIFIER
   ;

indexAccessorSet
   : indexAccessorValue (COMMA indexAccessorValue)*?;

valueSet
   : expression (COMMA expression)*?;

expression
   : LEFTPAREN expression RIGHTPAREN #ParenthesisExpression
   | expression (ASTERISK | BACKSLASH | MOD) expression #DivMultExpression
   | expression (PLUS | MINUS) expression #AddSubtractExpression
   | expression (LESSTHAN | GREATERTHAN | LESSTHANEQUAL | GREATERTHANEQUAL | NOTEQUAL | EQUAL) expression #ComparisonExpression
   | NOT expression #negationExpression
   | expression (AND | OR) expression #LogicalComparisonExpression
   | expression (ASSGN | DIV_ASSGN | MULTIPLY_ASSGN | ADD_ASSGN | MINUS_ASSGN) expression #AssignmentExpression
   | expression SCOPE IDENTIFIER #ScopeExpression
   | expression LEFTBRACKET indexAccessorSet RIGHTBRACKET #IndexExpression
   | LEFTBRACKET valueSet? RIGHTBRACKET #SetExpression
   | IDENTIFIER (PERIOD IDENTIFIER)*? LEFTPAREN methodCallArguments? RIGHTPAREN #MethodCallExpression
   | expression PERIOD IDENTIFIER (PERIOD IDENTIFIER)*? #MemberAccessExpression   
   | IDENTIFIER RIGHTPAREN #FunctionCallExpression
   | expression IN LEFTBRACKET valueSet? RIGHTBRACKET #InSetExpression
   | GUIALLOWED #GuiAllowedFunctionExpression
   | booleanLiteral #BooleanLiteralExpression
   | DATE_LITERAL #DateLiteralExpression
   | TIME_LITERAL #TimeLiteralExpression
   | DATETIME_LITERAL #DatetimeLiteralExpression
   | IDENTIFIER #IdentifierExpression
   | STRING_LITERAL #StringLiteralExpression
   | FLOAT_LITERAL #FloatLiteralExpression
   | INTEGER_LITERAL #IntegerLiteralExpression
   | optionLiteral #OptionLiteralExpression
   | MINUS expression #NegativeExpression
   ;

methodCallArguments
   : expression (COMMA expression)*?
   ;

optionLiteral
   : IDENTIFIER SCOPE IDENTIFIER;

booleanLiteral
   : TRUE
   | FALSE
   ;

numberLiteral
   : FLOAT_LITERAL
   | INTEGER_LITERAL
   ;