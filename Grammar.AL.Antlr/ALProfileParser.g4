parser grammar ALProfileParser;

options { tokenVocab=ALLexer; }

import ALPropertyParser;

profile
    : namespaceDeclaration? usingDeclarations?
      PROFILE identifier
      LEFTCBRACE keyValueProperty* RIGHTCBRACE
    ;
