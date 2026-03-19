parser grammar ALProfileExtensionParser;

options { tokenVocab=ALLexer; }

import ALProfileParser;

profileextension
    : namespaceDeclaration? usingDeclarations?
      PROFILEEXTENSION identifier EXTENDS identifier
      LEFTCBRACE keyValueProperty* RIGHTCBRACE
    ;
