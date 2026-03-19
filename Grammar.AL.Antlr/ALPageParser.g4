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
   | captionProperty
   | keyIdentifierListProperty
   | keyValueProperty
   | accessByPermProperty
   | decimalPlacesProperty
   | autoformatExpressionProperty
   | expressionProperty
   ;

pageGenericProperty
   : multiLanguageCaptionProperty
   | captionProperty
   | keyIdentifierListProperty
   | runObjectProperty
   | runPageLinkProperty
   | runPageViewProperty
   | keyValueProperty
   | accessByPermProperty
   | decimalPlacesProperty
   | autoformatExpressionProperty
   | expressionProperty
   ;

/*
 * Layout
 */

pageRepeaterEntity
   : pageGenericProperty
   | pageField
   ;

pageRepeaterEntities
   : pageRepeaterEntity*
   ;

pageCueEntity
   : pageGenericProperty
   | pageField
   ;

pageCueEntities
   : pageCueEntity*
   ;

pagePartProperty
   : subPageLinkProperty
   | pageGenericProperty
   ;

pagePartEntity
   : pagePartProperty
   ;

pagePartEntities
   : pagePartEntity*
   ;

pageFieldId 
    : INTEGER_LITERAL
    ;

pageFieldName 
    : identifier
    ;

pagefieldSource
    : expression
    ;

pageEntityName
    : identifier
    ;

pagePartType
    : identifier
    ;

pageField
   : FIELD LEFTPAREN pageFieldName SEMICOLON pagefieldSource RIGHTPAREN LEFTCBRACE pageFieldProperty*? triggerDeclaration*? RIGHTCBRACE
   ;

pageFields
    : pageField*
    ;

pageLayoutGroup
    : GROUP LEFTPAREN pageEntityName RIGHTPAREN LEFTCBRACE pageGenericProperty* pageLayoutEntities RIGHTCBRACE
    ;

pageLayoutRepeater
    : REPEATER LEFTPAREN pageEntityName RIGHTPAREN LEFTCBRACE pageRepeaterEntities RIGHTCBRACE
    ;

pageCuePart
    : CUEGROUP LEFTPAREN pageEntityName RIGHTPAREN LEFTCBRACE pageCueEntities RIGHTCBRACE
    ;

pagePart
    : PART LEFTPAREN pageEntityName SEMICOLON pagePartType RIGHTPAREN LEFTCBRACE pagePartEntities RIGHTCBRACE
    ;

pageSystemPart
    : SYSTEMPART LEFTPAREN pageEntityName SEMICOLON pagePartType RIGHTPAREN LEFTCBRACE pagePartEntities RIGHTCBRACE
    ;

pageLayoutEntity
    : pageLayoutGroup
    | pageLayoutRepeater
    | pageCuePart
    | pagePart
    | pageSystemPart
    | pageField
    ;

pageLayoutEntities
    : pageLayoutEntity*
    ;

pageLayoutArea
    : AREA LEFTPAREN pageEntityName RIGHTPAREN LEFTCBRACE pageLayoutEntities RIGHTCBRACE
    ;

pageLayoutAreas
    : pageLayoutArea*
    ;

pageLayoutSection
    : LAYOUT LEFTCBRACE pageLayoutAreas RIGHTCBRACE
    ;

/*
 * Actions
 */

pageActionEntity
    : triggerDeclaration
    | pageGenericProperty
    ;

pageAction
    : ACTION LEFTPAREN identifier RIGHTPAREN LEFTCBRACE pageActionEntity* RIGHTCBRACE
    ;

pageActionRef
    : ACTIONREF LEFTPAREN identifier SEMICOLON identifier RIGHTPAREN LEFTCBRACE pageGenericProperty* RIGHTCBRACE
    ;

pageActions
    : pageAction*
    ;

pageActionArea
    : AREA LEFTPAREN identifier RIGHTPAREN LEFTCBRACE pageActions RIGHTCBRACE
    ;

pageActionAreas
    : pageActionArea*
    ;

actionGroup
    : GROUP LEFTPAREN pageEntityName RIGHTPAREN LEFTCBRACE actionGroupEntities RIGHTCBRACE
    ;

actionGroupEntity
    : actionGroup
    | pageAction
    | pageActionRef
    | pageGenericProperty
    ;

actionGroupEntities
    : actionGroupEntity*
    ;

pageActionSection
    : ACTIONS LEFTCBRACE pageActionAreas RIGHTCBRACE
    ;

/*
 * Page
 */

pageEntity
    : pageLayoutSection
    | pageActionSection
    ;

pageEntities
    : pageEntity*
    ;

page
   : namespaceDeclaration? usingDeclarations? PAGE INTEGER_LITERAL identifier LEFTCBRACE pageProperties pageEntities codeDeclarations? RIGHTCBRACE
   ;

pageLayoutModification
    : {TokenMatches("addafter") || TokenMatches("addbefore") ||
       TokenMatches("addfirst") || TokenMatches("addlast")}?
      identifier LEFTPAREN identifier RIGHTPAREN
      LEFTCBRACE pageLayoutEntity* RIGHTCBRACE
    | {TokenMatches("modify")}?
      identifier LEFTPAREN identifier RIGHTPAREN
      LEFTCBRACE pageGenericProperty* triggerDeclaration* RIGHTCBRACE
    | {TokenMatches("movebefore") || TokenMatches("moveafter") ||
       TokenMatches("movefirst") || TokenMatches("movelast")}?
      identifier LEFTPAREN identifier SEMICOLON identifier RIGHTPAREN
    ;

pageActionModification
    : {TokenMatches("addafter") || TokenMatches("addbefore") ||
       TokenMatches("addfirst") || TokenMatches("addlast")}?
      identifier LEFTPAREN identifier RIGHTPAREN
      LEFTCBRACE (actionGroup | pageAction | pageActionRef)* RIGHTCBRACE
    | {TokenMatches("modify")}?
      identifier LEFTPAREN identifier RIGHTPAREN
      LEFTCBRACE pageActionEntity* RIGHTCBRACE
    | {TokenMatches("movebefore") || TokenMatches("moveafter") ||
       TokenMatches("movefirst") || TokenMatches("movelast")}?
      identifier LEFTPAREN identifier SEMICOLON identifier RIGHTPAREN
    ;
