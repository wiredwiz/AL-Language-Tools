parser grammar ALPageExtensionParser;

options { tokenVocab=ALLexer; }

import ALPageParser;

pageExtensionLayoutSection
    : LAYOUT LEFTCBRACE pageLayoutModification* RIGHTCBRACE
    ;

pageExtensionActionSection
    : ACTIONS LEFTCBRACE pageActionModification* RIGHTCBRACE
    ;

pageextension
    : namespaceDeclaration? usingDeclarations?
      PAGEEXTENSION INTEGER_LITERAL identifier EXTENDS identifier
      LEFTCBRACE
          pageProperties
          pageExtensionLayoutSection?
          pageExtensionActionSection?
          codeDeclarations?
      RIGHTCBRACE
    ;
