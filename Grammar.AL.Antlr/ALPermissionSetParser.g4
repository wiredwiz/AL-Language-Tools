parser grammar ALPermissionSetParser;

options { tokenVocab=ALLexer; }

import ALPropertyParser;

includedPermissionSetsProperty
    : {TokenMatches("IncludedPermissionSets")}? identifier EQUAL identifierList SEMICOLON
    ;

permissionSetProperty
    : permissionsProperty
    | includedPermissionSetsProperty
    | keyValueProperty
    ;

permissionset
    : namespaceDeclaration? usingDeclarations?
      PERMISSIONSET INTEGER_LITERAL identifier
      LEFTCBRACE permissionSetProperty* RIGHTCBRACE
    ;
