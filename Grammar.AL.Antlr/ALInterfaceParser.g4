parser grammar ALInterfaceParser;

options { tokenVocab=ALLexer; }

import ALPropertyParser;

interfaceMethodDeclaration
    : attributes=methodAttribute*?
      PROCEDURE name=identifier
      LEFTPAREN parameters=parameterList? RIGHTPAREN
      returnVal=returnValue? SEMICOLON?
    ;

interfaceExtendsList
    : EXTENDS identifier (COMMA identifier)*
    ;

interface
    : namespaceDeclaration? usingDeclarations?
      INTERFACE identifier interfaceExtendsList?
      LEFTCBRACE keyValueProperty* interfaceMethodDeclaration* RIGHTCBRACE
    ;
