parser grammar ALDotNetParser;

options { tokenVocab=ALLexer; }

import ALPropertyParser;

dotNetName
    : STRING_LITERAL
    | identifier (PERIOD identifier)*
    ;

dotNetType
    : TYPE
      LEFTPAREN dotNetName SEMICOLON dotNetName RIGHTPAREN
      LEFTCBRACE keyValueProperty*? RIGHTCBRACE
    ;

dotNetAssemblyItem
    : dotNetType
    | keyValueProperty
    ;

dotNetAssembly
    : {TokenMatches("assembly")}? IDENTIFIER
      LEFTPAREN dotNetName RIGHTPAREN
      LEFTCBRACE dotNetAssemblyItem* RIGHTCBRACE
    ;

dotnet
    : namespaceDeclaration? usingDeclarations?
      DOTNET
      LEFTCBRACE dotNetAssembly* RIGHTCBRACE
    ;
