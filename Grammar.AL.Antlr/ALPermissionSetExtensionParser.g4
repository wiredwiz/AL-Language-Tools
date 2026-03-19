parser grammar ALPermissionSetExtensionParser;

options { tokenVocab=ALLexer; }

import ALPermissionSetParser;

permissionsetextension
    : namespaceDeclaration? usingDeclarations?
      PERMISSIONSETEXTENSION INTEGER_LITERAL identifier EXTENDS identifier
      LEFTCBRACE permissionSetProperty* RIGHTCBRACE
    ;
