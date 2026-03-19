parser grammar ALTableExtensionParser;

options { tokenVocab=ALLexer; }

import ALTableParser;

tableExtFieldGroup
    : {TokenMatches("addlast")}? identifier LEFTPAREN tableFieldGroupName SEMICOLON fieldNames RIGHTPAREN
      LEFTCBRACE keyValueProperty*? RIGHTCBRACE
    ;

tableExtFieldGroups
    : FIELDGROUPS LEFTCBRACE tableExtFieldGroup*? RIGHTCBRACE
    ;

tableExtFieldModification
    : {TokenMatches("modify")}? identifier LEFTPAREN identifier RIGHTPAREN
      LEFTCBRACE tableFieldProperty*? RIGHTCBRACE
    ;

tableExtFields
    : FIELDS LEFTCBRACE (tableField | tableExtFieldModification)*? RIGHTCBRACE
    ;

tableExtEntity
    : tableExtFields
    | tableKeys
    | tableExtFieldGroups
    ;

tableExtEntities
    : tableExtEntity+
    ;

tableExtension
    : namespaceDeclaration? usingDeclarations?
      TABLEEXTENSION INTEGER_LITERAL identifier EXTENDS identifier
      LEFTCBRACE tableProperties tableExtEntities? codeDeclarations? RIGHTCBRACE
    ;
