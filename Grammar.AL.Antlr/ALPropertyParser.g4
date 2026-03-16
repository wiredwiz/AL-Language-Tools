parser grammar ALPropertyParser;

options { tokenVocab=ALLexer; }

import ALCodeParser;

keyValueProperty
   : identifier EQUAL (STRING_LITERAL | INTEGER_LITERAL | FLOAT_LITERAL | identifier | booleanLiteral) SEMICOLON
   ;

expressionProperty
   : identifier EQUAL expression SEMICOLON
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

propExpression
   : booleanLiteral #BooleanPropExpression
   | STRING_LITERAL #StringPropExpression
   | FLOAT_LITERAL #FloatPropExpression
   | INTEGER_LITERAL #IntegerPropExpression
   | identifier #IdentifierPropExpression
   // | systemEnumerationLiteral #SystemEnumerationPropExpression
   // | expression SCOPE identifier #ScopePropExpression
   // | expression PERIOD identifier #MemberAccessPropExpression
   ;

/*
 * Field related rules
 */

fieldEnumLiteral
   : identifier SCOPE identifier
   ;

fieldValue
   : identifier
   | INTEGER_LITERAL
   | FLOAT_LITERAL
   | DATE_LITERAL
   | TIME_LITERAL
   | DATETIME_LITERAL
   | STRING_LITERAL
   | booleanLiteral
   | systemEnumerationLiteral
   | fieldEnumLiteral
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
 * Sub Page Links
 */

subPageLinkTableFilter
   : CONST LEFTPAREN fieldValue RIGHTPAREN
   | FILTER LEFTPAREN compoundFilterRule RIGHTPAREN
   | FIELD LEFTPAREN identifier RIGHTPAREN
   | FIELD LEFTPAREN UPPERLIMIT LEFTPAREN FILTER LEFTPAREN identifier RIGHTPAREN RIGHTPAREN RIGHTPAREN
   | FIELD LEFTPAREN UPPERLIMIT LEFTPAREN identifier RIGHTPAREN RIGHTPAREN
   | FIELD LEFTPAREN FILTER LEFTPAREN identifier RIGHTPAREN RIGHTPAREN
   ;

subPageLink
   : identifier EQUAL subPageLinkTableFilter
   ;

subPageLinks
   : subPageLink ( COMMA subPageLink)*
   ;

subPageLinkProperty
   : SUBPAGELINK EQUAL subPageLinks SEMICOLON
   ;

/*
 * Table relations
 */

tableRelationFilterSegment
   : (EQUAL|NOTEQUAL|LESSTHAN|GREATERTHAN|LESSTHANEQUAL|GREATERTHANEQUAL)? fieldValue
   ;

tableRelationFilterCompound
   : tableRelationFilterSegment ( AMPERSAND tableRelationFilterSegment | PIPE tableRelationFilterSegment )*
   ;

tableRelationFilter
   : identifier EQUAL
        (FIELD LEFTPAREN identifier RIGHTPAREN
        | CONST LEFTPAREN fieldValue RIGHTPAREN
        | FILTER LEFTPAREN tableRelationFilterCompound RIGHTPAREN)
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

calcFormula
   : calcFormulaExist
   | calcFormulaCount
   | calcFormulaSum
   | calcFormulaAverage
   | calcFormulaMin
   | calcFormulaMax
   | calcFormulaLookup
   ;

/*
 * Language properties
 */

languageCaption
   : IDENTIFIER EQUAL STRING_LITERAL
   ;

multiLanguageCaptionProperty
   : CAPTIONML EQUAL languageCaption (COMMA languageCaption)*? SEMICOLON
   ;

accessByPermProperty
   : {TokenMatches("AccessByPermission")}? identifier EQUAL TABLEDATA objectId EQUAL identifier SEMICOLON
   ;

decimalPlacesProperty
   : {TokenMatches("DecimalPlaces")}? identifier EQUAL INTEGER_LITERAL COLON INTEGER_LITERAL SEMICOLON
   ;

/*
 * Format Properties
 */

autoformatExpressionProperty
    : {TokenMatches("AutoFormatExpression")}? identifier EQUAL expression SEMICOLON
    ;