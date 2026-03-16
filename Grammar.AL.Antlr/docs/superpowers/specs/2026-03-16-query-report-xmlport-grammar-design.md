# Design: Query, Report, and XmlPort Grammar

**Date:** 2026-03-16
**Branch:** AntlrGrammar

---

## Problem

`ALParser.g4` currently supports only three AL object types: Codeunit, Table, and Page. Query, Report, and XmlPort are common object types in Business Central that are entirely unrecognised by the parser — any AL file containing them fails to parse.

---

## Goals

1. Add grammar support for `query`, `report`, and `xmlport` object types.
2. One `.g4` file per object type, following the established project pattern.
3. Fix the pre-existing ordering bug in `tableField` and `pageField` where triggers and properties could appear interleaved, contrary to AL compiler requirements.
4. Make no unrelated changes.

---

## Declaration Ordering

Microsoft's AL extension parser enforces a strict ordering of declarations within member blocks. Placing a trigger before columns or nested dataitems produces a parser error (the error message confusingly reports an expected `}`, but is caused by the ordering violation). No formal grammar is published by Microsoft, but empirical testing confirms enforcement.

**Required order within any member block:**
1. Properties (`keyValueProperty`)
2. Data declarations (columns, nested dataitems, child schema elements)
3. Trigger declarations

This ordering applies to:
- Report and Query dataitem blocks
- XmlPort schema element blocks
- Table field blocks (pre-existing bug — fixed in this work)
- Page field blocks (pre-existing bug — fixed in this work)

---

## Files Changed

| File | Change |
|------|--------|
| `ALParser.g4` | Add three imports; add `query`, `report`, `xmlPort` alternatives to `alUnit` |
| `ALTableParser.g4` | Fix `tableField`: replace mixed `tableFieldEntity*?` with `tableFieldProperty*? triggerDeclaration*?` |
| `ALPageParser.g4` | Fix `pageField`: replace mixed `pageFieldEntity*?` with `pageFieldProperty*? triggerDeclaration*?` |
| `ALQueryParser.g4` | New file — Query object grammar |
| `ALReportParser.g4` | New file — Report object grammar |
| `ALXmlPortParser.g4` | New file — XmlPort object grammar |

---

## Design

### Change 1: Fix `tableField` and `pageField` Ordering (own commit)

#### `ALTableParser.g4`

```antlr
// Before
tableField
   : FIELD LEFTPAREN tableFieldId SEMICOLON tableFieldName SEMICOLON tableFieldType RIGHTPAREN
     LEFTCBRACE tableFieldEntity*? RIGHTCBRACE
   ;

// After
tableField
   : FIELD LEFTPAREN tableFieldId SEMICOLON tableFieldName SEMICOLON tableFieldType RIGHTPAREN
     LEFTCBRACE tableFieldProperty*? triggerDeclaration*? RIGHTCBRACE
   ;
```

The `tableFieldEntity` and `tableFieldEntities` rules become unused and are removed in the same commit.

#### `ALPageParser.g4`

```antlr
// Before
pageField
   : FIELD LEFTPAREN pageFieldName SEMICOLON pagefieldSource RIGHTPAREN
     LEFTCBRACE pageFieldEntity*? RIGHTCBRACE
   ;

// After
pageField
   : FIELD LEFTPAREN pageFieldName SEMICOLON pagefieldSource RIGHTPAREN
     LEFTCBRACE pageFieldProperty*? triggerDeclaration*? RIGHTCBRACE
   ;
```

The `pageFieldEntity` and `pageFieldEntities` rules become unused and are removed in the same commit.

---

### Change 2: Update `ALParser.g4`

```antlr
// ALPageParser is kept explicit here: it defines the `page` rule used in alUnit below.
// It also arrives transitively through ALReportParser and ALXmlPortParser — ANTLR4
// deduplicates imported rules so the redundancy is harmless.
import ALCodeunitParser, ALTableParser, ALPageParser, ALQueryParser, ALReportParser, ALXmlPortParser;

alUnit
   : codeunitDeclaration
   | table
   | page
   | query
   | report
   | xmlPort
   ;
```

---

### Change 3: `ALQueryParser.g4`

**Imports:** `ALCodeParser`, `ALPropertyParser`

**Soft keywords** (no dedicated lexer token): `elements`, `dataitem`, `column`
**Dedicated tokens**: `QUERY`, `FILTER`

All three query kinds (Normal, API, Reporting) share the same structural grammar. Kind-specific properties (`QueryType`, `APIPublisher`, `EntityName`, `ODataKeyFields`, etc.) fall through to `keyValueProperty`.

```antlr
parser grammar ALQueryParser;

options { tokenVocab=ALLexer; }

import ALCodeParser, ALPropertyParser;

queryProperties : keyValueProperty* ;

queryColumnSource : identifier ;

queryColumn
    : {TokenMatches("column")}? IDENTIFIER
      LEFTPAREN name=identifier SEMICOLON source=queryColumnSource RIGHTPAREN
      LEFTCBRACE keyValueProperty*? triggerDeclaration*? RIGHTCBRACE
    ;

// FILTER is a dedicated lexer token
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
```

**Notes:**
- `queryColumnSource` is a simple `identifier` — field names within a query dataitem are unqualified since the table is established by the enclosing dataitem.
- `queryDataItem` uses ordered sections, not a flat mixed union.
- `queryColumn` leaf blocks use `keyValueProperty*? triggerDeclaration*?` — consistent with the ordered-sections principle applied throughout this spec and with all other leaf blocks (`reportColumn`, `xmlPortFieldElement`, etc.).

---

### Change 4: `ALReportParser.g4`

**Imports:** `ALCodeParser`, `ALPropertyParser`, `ALPageParser`

**Soft keywords**: `dataset`, `dataitem`, `column`, `requestpage`, `rendering`, `labels`
**Dedicated tokens**: `REPORT`, `LAYOUT`, `LABEL`

```antlr
parser grammar ALReportParser;

options { tokenVocab=ALLexer; }

import ALCodeParser, ALPropertyParser, ALPageParser;

reportProperties : keyValueProperty* ;

reportColumnSource
    : identifier PERIOD identifier
    | identifier
    ;

reportColumn
    : {TokenMatches("column")}? IDENTIFIER
      LEFTPAREN name=identifier SEMICOLON source=reportColumnSource RIGHTPAREN
      LEFTCBRACE keyValueProperty*? triggerDeclaration*? RIGHTCBRACE
    ;

reportDataItem
    : {TokenMatches("dataitem")}? IDENTIFIER
      LEFTPAREN varName=identifier SEMICOLON tableName=identifier RIGHTPAREN
      LEFTCBRACE
          keyValueProperty*?      // 1. properties
          reportColumn*?          // 2. columns
          reportDataItem*?        // 3. nested dataitems
          triggerDeclaration*?    // 4. triggers last
      RIGHTCBRACE
    ;

reportDataset
    : {TokenMatches("dataset")}? IDENTIFIER
      LEFTCBRACE reportDataItem*? RIGHTCBRACE
    ;

// Reuses pageProperties, pageLayoutSection, pageActionSection from ALPageParser
reportRequestPage
    : {TokenMatches("requestpage")}? IDENTIFIER
      LEFTCBRACE pageProperties pageLayoutSection? pageActionSection? codeDeclarations? RIGHTCBRACE
    ;

// Modern rendering section only (BC 2022+)
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
```

**Notes:**
- `reportRequestPage` references `pageProperties`, `pageLayoutSection`, `pageActionSection` directly from `ALPageParser` — zero duplication.
- `reportColumnSource` allows `identifier` or `identifier PERIOD identifier` — columns can reference a bare field name or a dataitem-qualified field.
- Legacy layout properties (`RdlcLayout`, `WordLayout`) are not modelled explicitly; they fall through to `keyValueProperty` if present in older code.
- `reportLabelModifier` uses `expression` for its value so `MaxLength = 100`, `Comment = 'text'`, and `Locked = true` all parse correctly.
- `ALReportExtensionParser.g4` (future) will follow the same import pattern.

---

### Change 5: `ALXmlPortParser.g4`

**Imports:** `ALCodeParser`, `ALPropertyParser`, `ALPageParser`

**Soft keywords**: `schema`, `textelement`, `tableelement`, `fieldelement`, `textattribute`, `fieldattribute`, `requestpage`
**Dedicated tokens**: `XMLPORT`

```antlr
parser grammar ALXmlPortParser;

options { tokenVocab=ALLexer; }

import ALCodeParser, ALPropertyParser, ALPageParser;

xmlPortProperties : keyValueProperty* ;

// fieldelement and fieldattribute source is always TableVar.Field
xmlPortFieldSource : identifier PERIOD identifier ;

// Leaf elements — no child schema elements
xmlPortFieldElement
    : {TokenMatches("fieldelement")}? IDENTIFIER
      LEFTPAREN name=identifier SEMICOLON source=xmlPortFieldSource RIGHTPAREN
      LEFTCBRACE keyValueProperty*? triggerDeclaration*? RIGHTCBRACE
    ;

xmlPortTextAttribute
    : {TokenMatches("textattribute")}? IDENTIFIER
      LEFTPAREN name=identifier RIGHTPAREN
      LEFTCBRACE keyValueProperty*? triggerDeclaration*? RIGHTCBRACE
    ;

xmlPortFieldAttribute
    : {TokenMatches("fieldattribute")}? IDENTIFIER
      LEFTPAREN name=identifier SEMICOLON source=xmlPortFieldSource RIGHTPAREN
      LEFTCBRACE keyValueProperty*? triggerDeclaration*? RIGHTCBRACE
    ;

// Union rule — referenced recursively by container elements
xmlPortSchemaElement
    : xmlPortTextElement
    | xmlPortTableElement
    | xmlPortFieldElement
    | xmlPortTextAttribute
    | xmlPortFieldAttribute
    ;

// Container elements — hold child elements then triggers
xmlPortTextElement
    : {TokenMatches("textelement")}? IDENTIFIER
      LEFTPAREN name=identifier RIGHTPAREN
      LEFTCBRACE
          keyValueProperty*?          // 1. properties
          xmlPortSchemaElement*?      // 2. child elements/attributes
          triggerDeclaration*?        // 3. triggers last
      RIGHTCBRACE
    ;

xmlPortTableElement
    : {TokenMatches("tableelement")}? IDENTIFIER
      LEFTPAREN varName=identifier SEMICOLON tableName=identifier RIGHTPAREN
      LEFTCBRACE
          keyValueProperty*?          // 1. properties (DataItemLink etc.)
          xmlPortSchemaElement*?      // 2. child elements/attributes
          triggerDeclaration*?        // 3. triggers last
      RIGHTCBRACE
    ;

xmlPortSchema
    : {TokenMatches("schema")}? IDENTIFIER
      LEFTCBRACE xmlPortSchemaElement* RIGHTCBRACE
    ;

// Reuses pageProperties, pageLayoutSection, pageActionSection from ALPageParser
xmlPortRequestPage
    : {TokenMatches("requestpage")}? IDENTIFIER
      LEFTCBRACE pageProperties pageLayoutSection? pageActionSection? codeDeclarations? RIGHTCBRACE
    ;

xmlPort
    : namespaceDeclaration? usingDeclarations?
      XMLPORT INTEGER_LITERAL identifier
      LEFTCBRACE
          xmlPortProperties
          xmlPortSchema?
          xmlPortRequestPage?
          codeDeclarations?
      RIGHTCBRACE
    ;
```

**Notes:**
- `xmlPortSchemaElement` and the container rules form mutual recursion — ANTLR4 handles this cleanly since the recursion is never left-edge.
- `xmlPortFieldSource` requires qualification (`TableVar.Field`). The validator enforces that the table variable matches an enclosing `tableelement`; the grammar only requires the dot-notation form.
- The grammar permits any `xmlPortSchemaElement` at the schema root. The validator enforces that the root must be a `textelement` or `tableelement`.
- `requestpage` in XmlPort is structurally identical to `requestpage` in Report — both import `ALPageParser` for the shared rules.

---

## Out of Scope

- `reportextension` — deferred; will follow the same import pattern as `ALReportParser`
- `queryextension` — not a BC AL object type
- Enum, Interface, PermissionSet, Profile, PageExtension, TableExtension, ControlAddIn, Entitlement — separate chunk
- Semantic validation of XmlPort field source table variable references (the grammar requires `identifier PERIOD identifier` form; enforcing that the left side matches an enclosing `tableelement` variable is a validator concern)
- XmlPort schema root element type enforcement — the grammar accepts any `xmlPortSchemaElement` at the root; the validator enforces it must be `textelement` or `tableelement`
- Legacy Report layout properties (`RdlcLayout`, `WordLayout`)
