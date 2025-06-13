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
   : {TokenMatches("where")}? IDENTIFIER LEFTPAREN tableRelationFilters RIGHTPAREN
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
   : {TokenMatches("const")}? IDENTIFIER LEFTPAREN fieldValue RIGHTPAREN
   | {TokenMatches("filter")}? IDENTIFIER LEFTPAREN compoundFilterRule RIGHTPAREN
   | {TokenMatches("field")}? IDENTIFIER LEFTPAREN IDENTIFIER RIGHTPAREN
   | {TokenMatches("field")}? IDENTIFIER LEFTPAREN {TokenMatches("upperlimit")}? IDENTIFIER LEFTPAREN IDENTIFIER RIGHTPAREN RIGHTPAREN
   | {TokenMatches("field")}? IDENTIFIER LEFTPAREN {TokenMatches("filter")}? IDENTIFIER LEFTPAREN IDENTIFIER RIGHTPAREN RIGHTPAREN
   | {TokenMatches("field")}? IDENTIFIER LEFTPAREN {TokenMatches("upperlimit")}? IDENTIFIER LEFTPAREN {TokenMatches("filter")}? IDENTIFIER LEFTPAREN IDENTIFIER RIGHTPAREN RIGHTPAREN RIGHTPAREN
   ;

calcFormulaTableFilter
   : fieldReference EQUAL calcFormulaTableFilterValue
   ;

calcFormulaTableFilters
   : calcFormulaTableFilter (COMMA calcFormulaTableFilter)*?
   ;

calcFormulaWhereClause
   : {TokenMatches("where")}? IDENTIFIER LEFTPAREN calcFormulaTableFilters RIGHTPAREN
   ;

calcFormulaExist
   : MINUS? {TokenMatches("exist")}? IDENTIFIER LEFTPAREN tableReference calcFormulaWhereClause? RIGHTPAREN
   ;

calcFormulaCount
   : {TokenMatches("count")}? IDENTIFIER LEFTPAREN tableReference calcFormulaWhereClause? RIGHTPAREN
   ;

calcFormulaSum
   : MINUS? {TokenMatches("sum")}? IDENTIFIER LEFTPAREN qualifiedFieldReference calcFormulaWhereClause? RIGHTPAREN
   ;

calcFormulaAverage
   : MINUS? {TokenMatches("average")}? IDENTIFIER LEFTPAREN qualifiedFieldReference calcFormulaWhereClause? RIGHTPAREN
   ;
   
calcFormulaMin
   : {TokenMatches("min")}? IDENTIFIER LEFTPAREN qualifiedFieldReference calcFormulaWhereClause? RIGHTPAREN
   ;

calcFormulaMax
   : {TokenMatches("max")}? IDENTIFIER LEFTPAREN qualifiedFieldReference calcFormulaWhereClause? RIGHTPAREN
   ;

calcFormulaLookup
   : {TokenMatches("lookup")}? IDENTIFIER LEFTPAREN qualifiedFieldReference calcFormulaWhereClause? RIGHTPAREN
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
   : {TokenMatches("key")}? IDENTIFIER LEFTPAREN fieldNames RIGHTPAREN LEFTCBRACE keyProperties RIGHTCBRACE
   ;

tableKeys
   : {TokenMatches("keys")}? IDENTIFIER LEFTCBRACE tableKey*? RIGHTCBRACE
   ;

/*
 * Properties
 */

languageCaption
   : IDENTIFIER EQUAL STRING_LITERAL
   ;

multiLangaugeCaptionPropertty
   : {TokenMatches("captionml")}? IDENTIFIER EQUAL languageCaption (COMMA languageCaption)*? SEMICOLON
   ;

tableProperty
   : multiLangaugeCaptionPropertty
   | permissionsProperty
   | keyIdentifierListProperty
   | keyValueProperty   
   ;

tableProperties
   : tableProperty*?
   ;

/*
 * Tables
 */

tableFieldId : INTEGER_LITERAL;

tableFieldName : IDENTIFIER;

tableFieldType
   : {TokenMatches(SimpleFieldTypes)}? IDENTIFIER
   | {TokenMatches("code")}? IDENTIFIER sizeDeclaration
   | {TokenMatches("text")}? IDENTIFIER sizeDeclaration
   ;

tableFieldProperty
   : {TokenMatches("tablerelation")}? IDENTIFIER EQUAL tableRelation SEMICOLON
   | {TokenMatches("calcformula")}? IDENTIFIER EQUAL calcForumla SEMICOLON
   | multiLangaugeCaptionPropertty
   | keyIdentifierListProperty
   | keyValueProperty   
   ;

tableFieldEntity
   : triggerDeclaration
   | tableFieldProperty
   ;

tableFieldGroupName
   : {TokenMatches("dropdown")}? IDENTIFIER
   | {TokenMatches("brick")}? IDENTIFIER
   ;

fieldNames
   : IDENTIFIER (SEMICOLON IDENTIFIER)*?
   ;

tableFieldGroup
   : {TokenMatches("fieldgroup")}? IDENTIFIER LEFTPAREN tableFieldGroupName SEMICOLON fieldNames RIGHTPAREN LEFTCBRACE keyValueProperty*? RIGHTCBRACE
   ;

tableFieldGroups
   : {TokenMatches("fieldgroups")}? IDENTIFIER LEFTCBRACE tableFieldGroup*? RIGHTCBRACE
   ;

tableField
   : {TokenMatches("field")}? IDENTIFIER LEFTPAREN tableFieldId SEMICOLON tableFieldName SEMICOLON tableFieldType RIGHTPAREN LEFTCBRACE tableFieldEntity*? RIGHTCBRACE
   ;

tableFields
   : {TokenMatches("fields")}? IDENTIFIER LEFTCBRACE tableField*? RIGHTCBRACE
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
   : {TokenMatches("table")}? IDENTIFIER INTEGER_LITERAL IDENTIFIER LEFTCBRACE tableProperties tableEntities? codeDeclarations? RIGHTCBRACE
   ;

tableExtFieldGroup
   : {TokenMatches("addlast")}? IDENTIFIER LEFTPAREN tableFieldGroupName SEMICOLON fieldNames RIGHTPAREN LEFTCBRACE keyValueProperty*? RIGHTCBRACE
   ;

tableExtFieldGroups
   : {TokenMatches("fieldgroups")}? IDENTIFIER LEFTCBRACE tableExtFieldGroup*? RIGHTCBRACE
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
   : {TokenMatches("tableextension")}? IDENTIFIER INTEGER_LITERAL IDENTIFIER {TokenMatches("extends")}? IDENTIFIER IDENTIFIER 
      LEFTCBRACE tableProperties tableExtEntities? codeDeclarations? RIGHTCBRACE
   ;