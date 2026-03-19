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

tableExtEntity
    : tableFields
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
