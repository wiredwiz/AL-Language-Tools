parser grammar ALTableParser;

options { tokenVocab=ALLexer; }

import ALCodeParser, ALPropertyParser;

/*
 * Table Keys
 */

keyProperties
   : keyValueProperty*?
   ;

tableKey
   : KEY LEFTPAREN identifier SEMICOLON fieldNames RIGHTPAREN LEFTCBRACE keyProperties RIGHTCBRACE
   ;

tableKeys
   : KEYS LEFTCBRACE tableKey*? RIGHTCBRACE
   ;

/*
 * Properties
 */

tableProperty
   : multiLanguageCaptionProperty
   | permissionsProperty
   | keyIdentifierListProperty
   | keyValueProperty
   ;

tableProperties
   : tableProperty*
   ;

/*
 * Tables
 */

tableFieldId : INTEGER_LITERAL;

tableFieldName : identifier;

tableFieldType
   : INTEGER
   | BIGINTEGER
   | DECIMAL
   | ENUM identifier
   | BOOLEAN
   | BINARY
   | BLOB
   | DATE
   | TIME
   | DATETIME
   | DATEFORMULA
   | DURATION
   | RECORDID
   | TABLEFILTER
   | OPTION
   | GUID
   | CODE sizeDeclaration
   | TEXT sizeDeclaration
   ;

tableFieldProperty
   : TABLERELATION EQUAL tableRelation SEMICOLON
   | CALCFORMULA EQUAL calcForumla SEMICOLON
   | multiLanguageCaptionProperty
   | keyIdentifierListProperty
   | keyValueProperty
   | accessByPermProperty
   | decimalPlacesProperty
   | autoformatExpressionProperty
   ;

tableFieldEntity
   : triggerDeclaration
   | tableFieldProperty
   ;

tableFieldGroupName
   : DROPDOWN
   | {TokenMatches("brick")}? IDENTIFIER
   ;

fieldNames
   : identifier (COMMA identifier)*?
   ;

tableFieldGroup
   : FIELDGROUP LEFTPAREN tableFieldGroupName SEMICOLON fieldNames RIGHTPAREN LEFTCBRACE keyValueProperty*? RIGHTCBRACE
   ;

tableFieldGroups
   : FIELDGROUPS LEFTCBRACE tableFieldGroup*? RIGHTCBRACE
   ;

tableField
   : FIELD LEFTPAREN tableFieldId SEMICOLON tableFieldName SEMICOLON tableFieldType RIGHTPAREN LEFTCBRACE tableFieldEntity*? RIGHTCBRACE
   ;

tableFields
   : FIELDS LEFTCBRACE tableField*? RIGHTCBRACE
   ;

tableEntity
   : tableFields
   | tableKeys
   | tableFieldGroups
   ;

tableEntities
   : tableEntity+
   ;

table
   : namespaceDeclaration? usingDeclarations? TABLE INTEGER_LITERAL identifier LEFTCBRACE tableProperties tableEntities? codeDeclarations? RIGHTCBRACE
   ;

tableExtFieldGroup
   : {TokenMatches("addlast")}? IDENTIFIER LEFTPAREN tableFieldGroupName SEMICOLON fieldNames RIGHTPAREN LEFTCBRACE keyValueProperty*? RIGHTCBRACE
   ;

tableExtFieldGroups
   : FIELDGROUPS LEFTCBRACE tableExtFieldGroup*? RIGHTCBRACE
   ;

tableExtEntity
   : tableFields
   | tableKeys
   | tableExtFieldGroups
   ;

tableExtEntities
   : tableExtEntity+
   ;

tableExtension
   : TABLEEXTENSION INTEGER_LITERAL identifier EXTENDS identifier
      LEFTCBRACE tableProperties tableExtEntities? codeDeclarations? RIGHTCBRACE
   ;