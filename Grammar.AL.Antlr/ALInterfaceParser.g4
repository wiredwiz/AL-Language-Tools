parser grammar ALInterfaceParser;

options { tokenVocab=ALLexer; }

import ALPropertyParser;

interfaceMethodDeclaration
    : attributes=methodAttribute*?
      PROCEDURE name=identifier
      LEFTPAREN parameters=parameterList? RIGHTPAREN
      returnVal=returnValue? SEMICOLON?
    ;

interface
    : namespaceDeclaration? usingDeclarations?
      INTERFACE identifier
      LEFTCBRACE keyValueProperty* interfaceMethodDeclaration* RIGHTCBRACE
    ;
