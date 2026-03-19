parser grammar ALPageCustomizationParser;

options { tokenVocab=ALLexer; }

import ALPageParser;

// Restrict modify blocks to properties only — no triggers, no code
pageCustomizationLayoutEntity
    : pageGenericProperty
    ;

pageCustomizationActionEntity
    : pageGenericProperty
    ;

pageCustomizationLayoutModification
    : {TokenMatches("modify")}?
      identifier LEFTPAREN identifier RIGHTPAREN
      LEFTCBRACE pageCustomizationLayoutEntity* RIGHTCBRACE
    ;

pageCustomizationActionModification
    : {TokenMatches("modify")}?
      identifier LEFTPAREN identifier RIGHTPAREN
      LEFTCBRACE pageCustomizationActionEntity* RIGHTCBRACE
    ;

pageCustomizationLayoutSection
    : LAYOUT LEFTCBRACE pageCustomizationLayoutModification* RIGHTCBRACE
    ;

pageCustomizationActionSection
    : ACTIONS LEFTCBRACE pageCustomizationActionModification* RIGHTCBRACE
    ;

pagecustomization
    : namespaceDeclaration? usingDeclarations?
      PAGECUSTOMIZATION identifier
      {TokenMatches("customizes")}? IDENTIFIER identifier
      LEFTCBRACE
          pageCustomizationLayoutSection?
          pageCustomizationActionSection?
      RIGHTCBRACE
    ;
