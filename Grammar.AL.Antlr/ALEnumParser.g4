parser grammar ALEnumParser;

options { tokenVocab=ALLexer; }

import ALPropertyParser;

enumValue
    : VALUE LEFTPAREN INTEGER_LITERAL SEMICOLON identifier RIGHTPAREN
      LEFTCBRACE keyValueProperty*? RIGHTCBRACE
    ;

enum
    : namespaceDeclaration? usingDeclarations?
      ENUM INTEGER_LITERAL identifier
      LEFTCBRACE keyValueProperty* enumValue* RIGHTCBRACE
    ;
