parser grammar ALQueryParser;

options { tokenVocab=ALLexer; }

import ALCodeParser, ALPropertyParser;

queryProperties : keyValueProperty* ;

// Column source is a bare field name — table is established by the enclosing dataitem
queryColumnSource : identifier ;

// FILTER is a dedicated lexer token; column is a soft keyword
queryColumn
    : {TokenMatches("column")}? IDENTIFIER
      LEFTPAREN name=identifier SEMICOLON source=queryColumnSource RIGHTPAREN
      LEFTCBRACE keyValueProperty*? triggerDeclaration*? RIGHTCBRACE
    ;

queryFilter
    : FILTER LEFTPAREN name=identifier SEMICOLON source=queryColumnSource RIGHTPAREN
      LEFTCBRACE keyValueProperty*? RIGHTCBRACE
    ;

queryDataItem
    : {TokenMatches("dataitem")}? IDENTIFIER
      LEFTPAREN varName=identifier SEMICOLON tableName=identifier RIGHTPAREN
      LEFTCBRACE
          keyValueProperty*?              // 1. properties
          (queryColumn | queryFilter)*?   // 2. columns and filters
          queryDataItem*?                 // 3. nested dataitems
          triggerDeclaration*?            // 4. triggers last
      RIGHTCBRACE
    ;

queryElements
    : {TokenMatches("elements")}? IDENTIFIER
      LEFTCBRACE queryDataItem*? RIGHTCBRACE
    ;

query
    : namespaceDeclaration? usingDeclarations?
      QUERY INTEGER_LITERAL identifier
      LEFTCBRACE queryProperties queryElements? codeDeclarations? RIGHTCBRACE
    ;
