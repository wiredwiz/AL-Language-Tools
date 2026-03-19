parser grammar ALControlAddInParser;

options { tokenVocab=ALLexer; }

import ALPropertyParser;

controlAddInEventDeclaration
    : EVENT identifier LEFTPAREN parameterList? RIGHTPAREN SEMICOLON?
    ;

controlAddInProcedureDeclaration
    : PROCEDURE identifier LEFTPAREN parameterList? RIGHTPAREN returnValue? SEMICOLON?
    ;

controlAddInMember
    : controlAddInEventDeclaration
    | controlAddInProcedureDeclaration
    | keyValueProperty
    ;

controladdin
    : namespaceDeclaration? usingDeclarations?
      CONTROLADDIN identifier
      LEFTCBRACE controlAddInMember* RIGHTCBRACE
    ;
