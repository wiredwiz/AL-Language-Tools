parser grammar ALQueryParser;

options { tokenVocab=ALLexer; }

import ALCodeParser, ALPropertyParser;

queryProperty
    : permissionsProperty
    | keyValueProperty
    ;

queryProperties : queryProperty* ;

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

// Properties within a query dataitem block. Structured properties are listed first
// so ANTLR4's LL(*) prediction resolves them before falling through to keyValueProperty.
queryDataItemProperty
    : tableViewProperty           // DataItemTableView = sorting(...) where(...)
    | dataItemLinkProperty        // DataItemLink = field=FIELD(field)
    | dataItemTableFilterProperty // DataItemTableFilter = "F"=CONST(V),...
    | keyValueProperty            // single key = value (SqlJoinType, etc.)
    ;

queryDataItem
    : {TokenMatches("dataitem")}? IDENTIFIER
      LEFTPAREN varName=identifier SEMICOLON tableName=identifier RIGHTPAREN
      LEFTCBRACE
          queryDataItemProperty*?         // 1. properties
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
