parser grammar ALTableParser;

options { tokenVocab=ALLexer; }

import ALCodeParser;

/*
 * Field related rules
 */

fieldValue
   : identifier
   | INTEGER_LITERAL
   | FLOAT_LITERAL
   | DATE_LITERAL
   | TIME_LITERAL
   | DATETIME_LITERAL
   | STRING_LITERAL
   | booleanLiteral
   ;

comparisonFilter
   : (NOTEQUAL | EQUAL | LESSTHAN | GREATERTHAN | LESSTHANEQUAL | GREATERTHANEQUAL ) fieldValue
   ;

filterRule
   : (fieldValue | comparisonFilter)
   ;

compoundFilterRule
   : filterRule ((PIPE | AMPERSAND) filterRule)*?
   ;

tableReference
   : identifier
   ;

fieldReference
   : identifier
   ;

qualifiedFieldReference
   : tableReference PERIOD fieldReference
   ;

/*
 * Table relations
 */

tableRelationFilter
   : IDENTIFIER EQUAL
        (FIELD LEFTPAREN identifier RIGHTPAREN
        | CONST LEFTPAREN fieldValue RIGHTPAREN
        | FILTER LEFTPAREN (EQUAL|NOTEQUAL|LESSTHAN|GREATERTHAN|LESSTHANEQUAL|GREATERTHANEQUAL) fieldValue RIGHTPAREN)
   ;

tableRelationFilters
   : tableRelationFilter (COMMA tableRelationFilter)*?
   ;

tableRelationWhereClause
   : WHERE LEFTPAREN tableRelationFilters RIGHTPAREN
   ;

fieldRelationClause
   : (tableReference | qualifiedFieldReference) tableRelationWhereClause?
   ;

tableRelationIfCondition
   : IF LEFTPAREN tableRelationFilters RIGHTPAREN fieldRelationClause tableRelationElseCondition?
   ;

tableRelationElseCondition
   : ELSE tableRelationIfCondition
   ;

tableRelation
   : fieldRelationClause?
   | tableRelationIfCondition
   ;

/*
 * Flow fields
 */

calcFormulaTableFilterValue
   : CONST LEFTPAREN fieldValue RIGHTPAREN
   | FILTER LEFTPAREN compoundFilterRule RIGHTPAREN
   | FIELD LEFTPAREN identifier RIGHTPAREN
   | FIELD LEFTPAREN UPPERLIMIT LEFTPAREN identifier RIGHTPAREN RIGHTPAREN
   | FIELD LEFTPAREN FILTER LEFTPAREN identifier RIGHTPAREN RIGHTPAREN
   | FIELD LEFTPAREN UPPERLIMIT LEFTPAREN FILTER LEFTPAREN identifier RIGHTPAREN RIGHTPAREN RIGHTPAREN
   ;

calcFormulaTableFilter
   : fieldReference EQUAL calcFormulaTableFilterValue
   ;

calcFormulaTableFilters
   : calcFormulaTableFilter (COMMA calcFormulaTableFilter)*?
   ;

calcFormulaWhereClause
   : WHERE LEFTPAREN calcFormulaTableFilters RIGHTPAREN
   ;

calcFormulaExist
   : MINUS? EXIST LEFTPAREN tableReference calcFormulaWhereClause? RIGHTPAREN
   ;

calcFormulaCount
   : COUNT LEFTPAREN tableReference calcFormulaWhereClause? RIGHTPAREN
   ;

calcFormulaSum
   : MINUS? SUM LEFTPAREN qualifiedFieldReference calcFormulaWhereClause? RIGHTPAREN
   ;

calcFormulaAverage
   : MINUS? AVERAGE LEFTPAREN qualifiedFieldReference calcFormulaWhereClause? RIGHTPAREN
   ;

calcFormulaMin
   : MIN LEFTPAREN qualifiedFieldReference calcFormulaWhereClause? RIGHTPAREN
   ;

calcFormulaMax
   : MAX LEFTPAREN qualifiedFieldReference calcFormulaWhereClause? RIGHTPAREN
   ;

calcFormulaLookup
   : LOOKUP LEFTPAREN qualifiedFieldReference calcFormulaWhereClause? RIGHTPAREN
   ;

calcForumla
   : calcFormulaExist
   | calcFormulaCount
   | calcFormulaSum
   | calcFormulaAverage
   | calcFormulaMin
   | calcFormulaMax
   | calcFormulaLookup
   ;

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

languageCaption
   : IDENTIFIER EQUAL STRING_LITERAL
   ;

multiLangaugeCaptionPropertty
   : CAPTIONML EQUAL languageCaption (COMMA languageCaption)*? SEMICOLON
   ;

tableProperty
   : multiLangaugeCaptionPropertty
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
   | multiLangaugeCaptionPropertty
   | keyIdentifierListProperty
   | keyValueProperty
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