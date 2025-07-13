parser grammar ALPageParser;

options { tokenVocab=ALLexer; }

import ALCodeParser, ALPropertyParser;

/*
 * Properties
 */

pageProperty
   : multiLanguageCaptionProperty
   | permissionsProperty
   | keyIdentifierListProperty
   | keyValueProperty
   ;

pageProperties
   : pageProperty*
   ;

pageFieldProperty
   : TABLERELATION EQUAL tableRelation SEMICOLON
   | multiLanguageCaptionProperty
   | keyIdentifierListProperty
   | keyValueProperty
   | accessByPermProperty
   | decimalPlacesProperty
   | autoformatExpressionProperty
   | expressionProperty
   ;

pageActionProperty
   : TABLERELATION EQUAL tableRelation SEMICOLON
   | multiLanguageCaptionProperty
   | keyIdentifierListProperty
   | keyValueProperty
   | accessByPermProperty
   | decimalPlacesProperty
   | autoformatExpressionProperty
   | expressionProperty
   ;

/*
 * Layout
 */

pageFieldEntity
   : triggerDeclaration
   | pageFieldProperty
   ;

pageFieldId 
    : INTEGER_LITERAL
    ;

pageFieldName 
    : identifier
    ;

pagefieldSource
    : identifier
    | identifier PERIOD identifier
    ;

pageField
   : FIELD LEFTPAREN pageFieldName SEMICOLON pagefieldSource RIGHTPAREN LEFTCBRACE pageFieldEntity*? RIGHTCBRACE
   ;

pageLayoutGroup
    : GROUP LEFTPAREN identifier RIGHTPAREN LEFTCBRACE pageField* RIGHTCBRACE
    ;

pageLayoutRepeater
    : REPEATER LEFTPAREN identifier RIGHTPAREN LEFTCBRACE pageField* RIGHTCBRACE
    ;

pageCuePart
    : CUEGROUP LEFTPAREN identifier RIGHTPAREN LEFTCBRACE pageField* RIGHTCBRACE
    ;

pageLayoutEntity
    : pageLayoutGroup
    | pageLayoutRepeater
    | pageCuePart
    ;

pageLayoutArea
    : AREA LEFTPAREN identifier RIGHTPAREN LEFTCBRACE pageLayoutEntity* RIGHTCBRACE
    ;

pageLayout
    : LAYOUT LEFTCBRACE pageLayoutArea* RIGHTCBRACE
    ;

/*
 * Actions
 */

pageActionEntity
    : triggerDeclaration
    | pageActionProperty
    ;

pageAction
    : ACTION LEFTPAREN identifier RIGHTPAREN LEFTCBRACE pageActionEntity* RIGHTCBRACE
    ;

pageActionArea
    : AREA LEFTPAREN identifier RIGHTPAREN LEFTCBRACE pageAction* RIGHTCBRACE
    ;

pageActions
    : ACTIONS LEFTCBRACE pageActionArea* RIGHTCBRACE
    ;

/*
 * Page
 */

pageEntities
    : pageLayout
    ;

page
   : namespaceDeclaration? usingDeclarations? PAGE INTEGER_LITERAL identifier LEFTCBRACE pageProperties pageEntities? codeDeclarations? RIGHTCBRACE
   ;