parser grammar ALEnumExtensionParser;

options { tokenVocab=ALLexer; }

import ALEnumParser;

enumextension
    : namespaceDeclaration? usingDeclarations?
      ENUMEXTENSION INTEGER_LITERAL identifier EXTENDS identifier
      LEFTCBRACE enumValue* RIGHTCBRACE
    ;
