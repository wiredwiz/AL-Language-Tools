parser grammar ALTableParser;

options { tokenVocab=ALLexer; }

import ALCodeParser;

/*
 * Field related rules
 */

fieldValue
   : IDENTIFIER 
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
   : IDENTIFIER
   ;

fieldReference
   : IDENTIFIER
   ;

qualifiedFieldReference
   : tableReference PERIOD fieldReference
   ;

/*
 * Table relations
 */

tableRelationFilter
   : IDENTIFIER EQUAL 
        ({TokenMatches("field")}? IDENTIFIER LEFTPAREN IDENTIFIER RIGHTPAREN 
        | {TokenMatches("const")}? IDENTIFIER LEFTPAREN fieldValue RIGHTPAREN
        | {TokenMatches("filter")}? IDENTIFIER LEFTPAREN (EQUAL|NOTEQUAL|LESSTHAN|GREATERTHAN|LESSTHANEQUAL|GREATERTHANEQUAL) fieldValue RIGHTPAREN)
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
   | FIELD LEFTPAREN IDENTIFIER RIGHTPAREN
   | FIELD LEFTPAREN UPPERLIMIT LEFTPAREN IDENTIFIER RIGHTPAREN RIGHTPAREN
   | FIELD LEFTPAREN FILTER LEFTPAREN IDENTIFIER RIGHTPAREN RIGHTPAREN
   | FIELD LEFTPAREN UPPERLIMIT LEFTPAREN FILTER LEFTPAREN IDENTIFIER RIGHTPAREN RIGHTPAREN RIGHTPAREN
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
   : KEY LEFTPAREN fieldNames RIGHTPAREN LEFTCBRACE keyProperties RIGHTCBRACE
   ;

tableKeys
   : KEY LEFTCBRACE tableKey*? RIGHTCBRACE
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

tableFieldName : IDENTIFIER;

tableFieldType
   : {TokenMatches(SimpleFieldTypes)}? IDENTIFIER
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
   : IDENTIFIER (SEMICOLON IDENTIFIER)*?
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
   : TABLE INTEGER_LITERAL IDENTIFIER LEFTCBRACE tableProperties tableEntities? codeDeclarations? RIGHTCBRACE
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
   : TABLEEXTENSION INTEGER_LITERAL IDENTIFIER EXTENDS IDENTIFIER 
      LEFTCBRACE tableProperties tableExtEntities? codeDeclarations? RIGHTCBRACE
   ;