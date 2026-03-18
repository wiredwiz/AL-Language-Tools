parser grammar ALReportParser;

options { tokenVocab=ALLexer; }

import ALCodeParser, ALPropertyParser, ALPageParser;

reportProperties : keyValueProperty* ;

// Column source: bare field name, or DataItem.Field
reportColumnSource
    : identifier PERIOD identifier
    | identifier
    ;

reportColumn
    : {TokenMatches("column")}? IDENTIFIER
      LEFTPAREN name=identifier SEMICOLON source=reportColumnSource RIGHTPAREN
      LEFTCBRACE keyValueProperty*? triggerDeclaration*? RIGHTCBRACE
    ;

// Properties within a dataitem block: single-value (keyValueProperty) or
// comma-separated field list (keyIdentifierListProperty) for properties
// like RequestFilterFields = "No.","Name";
reportDataItemProperty
    : keyValueProperty
    | keyIdentifierListProperty
    ;

reportDataItem
    : {TokenMatches("dataitem")}? IDENTIFIER
      LEFTPAREN varName=identifier SEMICOLON tableName=identifier RIGHTPAREN
      LEFTCBRACE
          reportDataItemProperty*?  // 1. properties (DataItemLink, DataItemTableFilter, RequestFilterFields, etc.)
          reportColumn*?            // 2. columns
          reportDataItem*?          // 3. nested dataitems
          triggerDeclaration*?      // 4. triggers last (OnPreDataItem, OnAfterGetRecord, OnPostDataItem)
      RIGHTCBRACE
    ;

reportDataset
    : {TokenMatches("dataset")}? IDENTIFIER
      LEFTCBRACE reportDataItem*? RIGHTCBRACE
    ;

// Reuses pageProperties, pageLayoutSection, pageActionSection from ALPageParser (imported above)
reportRequestPage
    : {TokenMatches("requestpage")}? IDENTIFIER
      LEFTCBRACE pageProperties pageLayoutSection? pageActionSection? codeDeclarations? RIGHTCBRACE
    ;

// Modern rendering section only (BC 2022+); LAYOUT is a dedicated token
reportRenderingLayout
    : LAYOUT LEFTPAREN identifier RIGHTPAREN
      LEFTCBRACE keyValueProperty*? RIGHTCBRACE
    ;

reportRendering
    : {TokenMatches("rendering")}? IDENTIFIER
      LEFTCBRACE reportRenderingLayout*? RIGHTCBRACE
    ;

reportLabelModifier : identifier EQUAL expression ;

// The primary label value is always a string literal (AL requirement).
// Modifier values (Comment, MaxLength, Locked) use expression to handle
// strings, integers, and booleans without separate typed alternatives.
reportLabel
    : name=identifier EQUAL value=STRING_LITERAL
      (COMMA reportLabelModifier)* SEMICOLON
    ;

reportLabels
    : {TokenMatches("labels")}? IDENTIFIER
      LEFTCBRACE reportLabel*? RIGHTCBRACE
    ;

report
    : namespaceDeclaration? usingDeclarations?
      REPORT INTEGER_LITERAL identifier
      LEFTCBRACE
          reportProperties
          reportDataset?
          reportRequestPage?
          reportRendering?
          reportLabels?
          codeDeclarations?
      RIGHTCBRACE
    ;
