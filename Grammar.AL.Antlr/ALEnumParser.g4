parser grammar ALEnumParser;

options { tokenVocab=ALLexer; }

import ALPropertyParser;

enumValueProperty
    : implementationProperty
    | keyValueProperty
    ;

enumValue
    : VALUE LEFTPAREN INTEGER_LITERAL SEMICOLON identifier RIGHTPAREN
      LEFTCBRACE enumValueProperty*? RIGHTCBRACE
    ;

enumImplementsList
    : IMPLEMENTS identifier (COMMA identifier)*
    ;

enumBodyProperty
    : implementationProperty
    | keyValueProperty
    ;

enum
    : namespaceDeclaration? usingDeclarations?
      ENUM INTEGER_LITERAL identifier enumImplementsList?
      LEFTCBRACE enumBodyProperty* enumValue* RIGHTCBRACE
    ;
