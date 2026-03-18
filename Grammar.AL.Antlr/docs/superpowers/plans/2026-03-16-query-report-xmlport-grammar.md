# Query, Report, and XmlPort Grammar Implementation Plan

> **For agentic workers:** REQUIRED: Use superpowers:subagent-driven-development (if subagents available) or superpowers:executing-plans to implement this plan. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add ANTLR4 grammar support for AL Query, Report, and XmlPort object types, and fix a pre-existing declaration ordering bug in Table and Page field grammars.

**Architecture:** One `.g4` file per new object type (`ALQueryParser.g4`, `ALReportParser.g4`, `ALXmlPortParser.g4`); each imports `ALCodeParser` and `ALPropertyParser`, with `ALReport` and `ALXmlPort` also importing `ALPageParser` to reuse the `requestpage` section rules. `ALParser.g4` is updated incrementally as each grammar is added. The ordering bug in `tableField` and `pageField` is fixed first in its own commit.

**Tech Stack:** ANTLR4 grammar (`.g4`), Antlr4 NuGet v4.6.6 (Sam Harwell C# target — runs grammar codegen during `dotnet build`), xUnit 2.4.2, FluentAssertions 6.12.0, .NET 6.0

---

## File Structure

| File | Action | Responsibility |
|------|--------|---------------|
| `Grammar.AL.Antlr/ALTableParser.g4` | Modify | Fix `tableField` ordering: `tableFieldProperty*?` then `triggerDeclaration*?`; remove dead `tableFieldEntity` rule |
| `Grammar.AL.Antlr/ALPageParser.g4` | Modify | Fix `pageField` ordering: `pageFieldProperty*?` then `triggerDeclaration*?`; remove dead `pageFieldEntity` and `pageFieldEntities` rules |
| `Grammar.AL.Antlr/ALParser.g4` | Modify (3×) | Add imports and `alUnit` alternatives incrementally as each grammar is added |
| `Grammar.AL.Antlr/ALQueryParser.g4` | Create | Complete Query object grammar |
| `Grammar.AL.Antlr/ALReportParser.g4` | Create | Complete Report object grammar |
| `Grammar.AL.Antlr/ALXmlPortParser.g4` | Create | Complete XmlPort object grammar |
| `ANTLR AL Parser Tests/Helpers/ALParseHelper.cs` | Modify | Add `ParseAlUnit` helper for parsing complete AL objects (not just codeunit-wrapped snippets) |
| `ANTLR AL Parser Tests/FieldOrderingTests.cs` | Create | Tests for property-before-trigger ordering enforcement in table and page fields |
| `ANTLR AL Parser Tests/QueryParserTests.cs` | Create | Structural parse tests for Query objects |
| `ANTLR AL Parser Tests/ReportParserTests.cs` | Create | Structural parse tests for Report objects |
| `ANTLR AL Parser Tests/XmlPortParserTests.cs` | Create | Structural parse tests for XmlPort objects |

**Working directory for all commands:** `D:\Projects\AL Langauge Tools\Grammar.AL.Antlr\`

**Build command** (regenerates ANTLR C# from `.g4` files):
```
dotnet build
```

**Test command:**
```
dotnet test "..\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj"
```

**Git add paths** (relative to working directory — git root is one level up):
- Grammar files: `ALTableParser.g4`, `ALParser.g4`, etc.
- Test files: `..\ANTLR AL Parser Tests\FieldOrderingTests.cs`, etc.

---

## Chunk 1: Ordering Fix and Test Infrastructure

### Task 1: Add ParseAlUnit helper to ALParseHelper.cs

**Files:**
- Modify: `ANTLR AL Parser Tests/Helpers/ALParseHelper.cs`

`ParseAlUnit` is needed by all new tests and by the ordering fix tests. The existing `ALParser` wrapper (from `Org.Edgerunner.Language.AL.Parsing`) already handles full AL units — just pass the source directly.

- [ ] **Step 1: Add `ParseAlUnit` method to `ALParseHelper.cs`**

Open `ANTLR AL Parser Tests/Helpers/ALParseHelper.cs`. After the `ParseProcedureBodyWithVars` method (before the closing `}`), insert:

```csharp
    /// <summary>
    /// Parses a complete AL unit from source (query, report, xmlport, table, page, codeunit, etc.).
    /// </summary>
    public static (ISyntaxTree Tree, IReadOnlyList<ErrorMessage> Errors) ParseAlUnit(string source)
    {
        var parser = new ALParser();
        var tree = parser.Parse(source);
        return (tree, parser.Errors);
    }
```

- [ ] **Step 2: Build to verify it compiles**

```
dotnet build
```

Expected: Build succeeded, 0 errors.

---

### Task 2: Fix field declaration ordering and commit

**Files:**
- Modify: `Grammar.AL.Antlr/ALTableParser.g4` (lines 79–82, 101–103)
- Modify: `Grammar.AL.Antlr/ALPageParser.g4` (lines 47–54, 108–110)
- Create: `ANTLR AL Parser Tests/FieldOrderingTests.cs`

The AL compiler enforces that properties must precede triggers within a field block. The current grammar uses a mixed `tableFieldEntity*?` / `pageFieldEntity*?` union that permits any order. This task fixes the grammar and writes tests that confirm the ordering is now enforced.

- [ ] **Step 1: Create `FieldOrderingTests.cs` with the tests below**

Create file `ANTLR AL Parser Tests/FieldOrderingTests.cs`:

```csharp
using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

namespace Org.Edgerunner.Language.AL.Grammar.Tests;

/// <summary>
/// Verifies that AL enforces declaration ordering within field blocks:
/// properties must precede triggers.
/// </summary>
public class FieldOrderingTests
{
    // -----------------------------------------------------------------------
    // Table field ordering
    // -----------------------------------------------------------------------

    [Fact]
    public void Table_field_with_property_before_trigger_parses_without_errors()
    {
        var source = @"
table 50000 ""TestTable""
{
    fields
    {
        field(1; Name; Text[100])
        {
            Caption = 'Name';
            trigger OnValidate()
            begin
            end;
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a table field with property before trigger is valid AL");
    }

    [Fact]
    public void Table_field_with_trigger_before_property_produces_parse_errors()
    {
        var source = @"
table 50000 ""TestTable""
{
    fields
    {
        field(1; Name; Text[100])
        {
            trigger OnValidate()
            begin
            end;
            Caption = 'Name';
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().NotBeEmpty("a property declared after a trigger violates AL ordering rules");
    }

    // -----------------------------------------------------------------------
    // Page field ordering
    // -----------------------------------------------------------------------

    [Fact]
    public void Page_field_with_property_before_trigger_parses_without_errors()
    {
        var source = @"
page 50000 ""TestPage""
{
    layout
    {
        area(content)
        {
            group(Group1)
            {
                field(Name; Name)
                {
                    Caption = 'Name';
                    trigger OnValidate()
                    begin
                    end;
                }
            }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a page field with property before trigger is valid AL");
    }

    [Fact]
    public void Page_field_with_trigger_before_property_produces_parse_errors()
    {
        var source = @"
page 50000 ""TestPage""
{
    layout
    {
        area(content)
        {
            group(Group1)
            {
                field(Name; Name)
                {
                    trigger OnValidate()
                    begin
                    end;
                    Caption = 'Name';
                }
            }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().NotBeEmpty("a property declared after a trigger violates AL ordering rules");
    }
}
```

- [ ] **Step 2: Build and verify the two "produces_parse_errors" tests FAIL (no errors produced yet)**

```
dotnet build
dotnet test "..\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" --filter "FieldOrdering"
```

Expected: 2 tests FAIL (errors collection is empty but test asserts NotBeEmpty), 2 tests PASS.

- [ ] **Step 3: Fix `tableField` in `ALTableParser.g4`**

In `Grammar.AL.Antlr/ALTableParser.g4`:

Remove lines 79–82 (the `tableFieldEntity` rule):
```antlr
tableFieldEntity
   : triggerDeclaration
   | tableFieldProperty
   ;
```

Change line 102 (the `tableField` rule body — replace `tableFieldEntity*?` with ordered sections):
```antlr
// Before:
tableField
   : FIELD LEFTPAREN tableFieldId SEMICOLON tableFieldName SEMICOLON tableFieldType RIGHTPAREN LEFTCBRACE tableFieldEntity*? RIGHTCBRACE
   ;

// After:
tableField
   : FIELD LEFTPAREN tableFieldId SEMICOLON tableFieldName SEMICOLON tableFieldType RIGHTPAREN LEFTCBRACE tableFieldProperty*? triggerDeclaration*? RIGHTCBRACE
   ;
```

- [ ] **Step 4: Fix `pageField` in `ALPageParser.g4`**

In `Grammar.AL.Antlr/ALPageParser.g4`:

Remove lines 47–54 (the `pageFieldEntity` and `pageFieldEntities` rules):
```antlr
pageFieldEntity
   : triggerDeclaration
   | pageFieldProperty
   ;

pageFieldEntities
   : pageFieldEntity*
   ;
```

Change line 109 (the `pageField` rule body):
```antlr
// Before:
pageField
   : FIELD LEFTPAREN pageFieldName SEMICOLON pagefieldSource RIGHTPAREN LEFTCBRACE pageFieldEntity*? RIGHTCBRACE
   ;

// After:
pageField
   : FIELD LEFTPAREN pageFieldName SEMICOLON pagefieldSource RIGHTPAREN LEFTCBRACE pageFieldProperty*? triggerDeclaration*? RIGHTCBRACE
   ;
```

- [ ] **Step 5: Build**

```
dotnet build
```

Expected: Build succeeded, 0 errors.

- [ ] **Step 6: Run ordering tests — all 4 should PASS**

```
dotnet test "..\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" --filter "FieldOrdering"
```

Expected: 4/4 PASS.

- [ ] **Step 7: Run full test suite — all existing tests still PASS**

```
dotnet test "..\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj"
```

Expected: all PASS (no regressions in ExpressionPrecedenceTests).

- [ ] **Step 8: Commit (ordering fix — own commit per spec)**

```bash
git add ALTableParser.g4 ALPageParser.g4 "..\ANTLR AL Parser Tests\Helpers\ALParseHelper.cs" "..\ANTLR AL Parser Tests\FieldOrderingTests.cs"
git commit -m "$(cat <<'EOF'
fix: enforce property-before-trigger ordering in table and page field declarations

Replaces mixed tableFieldEntity/pageFieldEntity union rules with ordered
sections (properties then triggers) to match AL compiler enforcement.
Removes the now-unused tableFieldEntity, pageFieldEntity, and
pageFieldEntities rules. Adds ParseAlUnit test helper and FieldOrderingTests.

Co-Authored-By: Claude Sonnet 4.6 <noreply@anthropic.com>
EOF
)"
```

---

## Chunk 2: Query Grammar

### Task 3: Add ALQueryParser.g4 and update ALParser.g4

**Files:**
- Create: `Grammar.AL.Antlr/ALQueryParser.g4`
- Modify: `Grammar.AL.Antlr/ALParser.g4`
- Create: `ANTLR AL Parser Tests/QueryParserTests.cs`

**Soft keywords** (fall back to IDENTIFIER in lexer; use `{TokenMatches("x")}?` predicate):
`elements`, `dataitem`, `column`

**Dedicated tokens** (hard keywords): `QUERY`, `FILTER`

- [ ] **Step 1: Create `QueryParserTests.cs` with failing tests**

Create file `ANTLR AL Parser Tests/QueryParserTests.cs`:

```csharp
using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

namespace Org.Edgerunner.Language.AL.Grammar.Tests;

/// <summary>
/// Verifies structural parsing of AL query objects.
/// </summary>
public class QueryParserTests
{
    [Fact]
    public void Minimal_query_parses_without_errors()
    {
        var source = @"
query 50000 ""MinimalQuery""
{
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a minimal query with no sections should parse without errors");
    }

    [Fact]
    public void Minimal_query_produces_QueryContext()
    {
        var source = @"
query 50000 ""MinimalQuery""
{
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.QueryContext>(tree)
            .Should().NotBeNull("a query object should produce a QueryContext node");
    }

    [Fact]
    public void Query_with_dataitem_and_columns_parses_without_errors()
    {
        var source = @"
query 50000 ""CustomerQuery""
{
    elements
    {
        dataitem(Customer; Customer)
        {
            column(No; ""No."") { }
            column(Name; Name) { }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a query with a dataitem and columns should parse without errors");
    }

    [Fact]
    public void Query_with_filter_parses_without_errors()
    {
        var source = @"
query 50000 ""FilteredQuery""
{
    elements
    {
        dataitem(Customer; Customer)
        {
            column(No; ""No."") { }
            filter(PostingGroup; ""Customer Posting Group"") { }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a query with a filter element should parse without errors");
    }

    [Fact]
    public void Query_with_nested_dataitems_parses_without_errors()
    {
        var source = @"
query 50000 ""NestedQuery""
{
    elements
    {
        dataitem(Customer; Customer)
        {
            column(No; ""No."") { }
            dataitem(SalesLine; ""Sales Line"")
            {
                column(Amount; Amount) { }
            }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a query with nested dataitems should parse without errors");
    }

    [Fact]
    public void API_query_with_entity_properties_parses_without_errors()
    {
        var source = @"
query 50000 ""CustomerAPI""
{
    QueryType = API;
    APIPublisher = 'microsoft';
    APIGroup = 'financials';
    APIVersion = 'v2.0';
    EntityName = 'customer';
    EntitySetName = 'customers';
    elements
    {
        dataitem(Customer; Customer)
        {
            column(id; SystemId) { }
            column(number; ""No."") { }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("an API query with entity properties should parse without errors");
    }

    [Fact]
    public void Query_dataitem_produces_QueryDataItemContext()
    {
        var source = @"
query 50000 ""CustomerQuery""
{
    elements
    {
        dataitem(Customer; Customer)
        {
            column(No; ""No."") { }
        }
    }
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.QueryDataItemContext>(tree)
            .Should().NotBeNull("a query dataitem should produce a QueryDataItemContext node");
    }
}
```

- [ ] **Step 2: Build — verify compile errors for missing query context types**

```
dotnet build
```

Expected: Build FAILS with errors like `'ALParser' does not contain a definition for 'QueryContext'` and `'QueryDataItemContext'`. This is the TDD red state — the tests reference types that don't exist yet.

- [ ] **Step 3: Create `ALQueryParser.g4`**

Create file `Grammar.AL.Antlr/ALQueryParser.g4`:

```antlr
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
```

- [ ] **Step 4: Update `ALParser.g4` to import ALQueryParser and add the `query` alternative**

In `Grammar.AL.Antlr/ALParser.g4`, make these two changes:

```antlr
// Before:
import ALCodeunitParser, ALTableParser, ALPageParser;

alUnit
   : codeunitDeclaration
   | table
   | page
   ;

// After:
import ALCodeunitParser, ALTableParser, ALPageParser, ALQueryParser;

alUnit
   : codeunitDeclaration
   | table
   | page
   | query
   ;
```

- [ ] **Step 5: Build — verify it succeeds (ANTLR regenerates C# with QueryContext etc.)**

```
dotnet build
```

Expected: Build succeeded, 0 errors. ANTLR regenerates `ALParser.cs` with `QueryContext`, `QueryDataItemContext`, `QueryColumnContext`, `QueryFilterContext`, `QueryElementsContext`.

- [ ] **Step 6: Run query tests — all 7 should PASS**

```
dotnet test "..\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" --filter "QueryParser"
```

Expected: 7/7 PASS.

- [ ] **Step 7: Run full test suite**

```
dotnet test "..\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj"
```

Expected: all PASS.

- [ ] **Step 8: Commit**

```bash
git add ALQueryParser.g4 ALParser.g4 "..\ANTLR AL Parser Tests\QueryParserTests.cs"
git commit -m "$(cat <<'EOF'
feat: add ALQueryParser grammar for AL query objects

Adds grammar support for all three query kinds (Normal, API, Reporting).
Kind-specific properties (QueryType, APIPublisher, EntityName, etc.) fall
through to keyValueProperty. Updates ALParser.g4 to include query in alUnit.

Co-Authored-By: Claude Sonnet 4.6 <noreply@anthropic.com>
EOF
)"
```

---

## Chunk 3: Report Grammar

### Task 4: Add ALReportParser.g4 and update ALParser.g4

**Files:**
- Create: `Grammar.AL.Antlr/ALReportParser.g4`
- Modify: `Grammar.AL.Antlr/ALParser.g4`
- Create: `ANTLR AL Parser Tests/ReportParserTests.cs`

**Soft keywords**: `dataset`, `dataitem`, `column`, `requestpage`, `rendering`, `labels`
**Dedicated tokens**: `REPORT`, `LAYOUT`, `LABEL`

The `requestpage` section reuses `pageProperties`, `pageLayoutSection`, `pageActionSection` from `ALPageParser` — zero duplication. `ALReportParser` imports `ALPageParser` directly for this.

- [ ] **Step 1: Create `ReportParserTests.cs` with failing tests**

Create file `ANTLR AL Parser Tests/ReportParserTests.cs`:

```csharp
using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

namespace Org.Edgerunner.Language.AL.Grammar.Tests;

/// <summary>
/// Verifies structural parsing of AL report objects.
/// </summary>
public class ReportParserTests
{
    [Fact]
    public void Minimal_report_parses_without_errors()
    {
        var source = @"
report 50000 ""MinimalReport""
{
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a minimal report with no sections should parse without errors");
    }

    [Fact]
    public void Minimal_report_produces_ReportContext()
    {
        var source = @"
report 50000 ""MinimalReport""
{
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.ReportContext>(tree)
            .Should().NotBeNull("a report object should produce a ReportContext node");
    }

    [Fact]
    public void Report_with_dataset_and_columns_parses_without_errors()
    {
        var source = @"
report 50000 ""CustomerReport""
{
    dataset
    {
        dataitem(Customer; Customer)
        {
            column(No; ""No."") { }
            column(Name; Name) { }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a report with a dataset section should parse without errors");
    }

    [Fact]
    public void Report_with_nested_dataitems_parses_without_errors()
    {
        var source = @"
report 50000 ""NestedReport""
{
    dataset
    {
        dataitem(Customer; Customer)
        {
            column(No; Customer.""No."") { }
            dataitem(SalesLine; ""Sales Line"")
            {
                column(Amount; ""Sales Line"".Amount) { }
            }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a report with nested dataitems should parse without errors");
    }

    [Fact]
    public void Report_with_requestpage_parses_without_errors()
    {
        var source = @"
report 50000 ""ReportWithRequestPage""
{
    dataset
    {
        dataitem(Customer; Customer)
        {
            column(No; ""No."") { }
        }
    }
    requestpage
    {
        layout
        {
            area(content)
            {
                group(Options)
                {
                    field(ShowAll; ShowAll) { }
                }
            }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a report with a requestpage section should parse without errors");
    }

    [Fact]
    public void Report_with_rendering_section_parses_without_errors()
    {
        var source = @"
report 50000 ""ReportWithRendering""
{
    rendering
    {
        layout(RdlcLayout)
        {
            Type = RDLC;
            LayoutFile = 'MyReport.rdlc';
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a report with a rendering section should parse without errors");
    }

    [Fact]
    public void Report_with_labels_parses_without_errors()
    {
        var source = @"
report 50000 ""ReportWithLabels""
{
    labels
    {
        MyLabel = 'Hello', Comment = 'A greeting';
        AmountLabel = 'Amount';
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a report with a labels section should parse without errors");
    }

    [Fact]
    public void Report_dataitem_produces_ReportDataItemContext()
    {
        var source = @"
report 50000 ""CustomerReport""
{
    dataset
    {
        dataitem(Customer; Customer)
        {
            column(No; ""No."") { }
        }
    }
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.ReportDataItemContext>(tree)
            .Should().NotBeNull("a report dataitem should produce a ReportDataItemContext node");
    }
}
```

- [ ] **Step 2: Build — verify compile errors for missing report context types**

```
dotnet build
```

Expected: Build FAILS with errors like `'ALParser' does not contain a definition for 'ReportContext'` and `'ReportDataItemContext'`.

- [ ] **Step 3: Create `ALReportParser.g4`**

Create file `Grammar.AL.Antlr/ALReportParser.g4`:

```antlr
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

reportDataItem
    : {TokenMatches("dataitem")}? IDENTIFIER
      LEFTPAREN varName=identifier SEMICOLON tableName=identifier RIGHTPAREN
      LEFTCBRACE
          keyValueProperty*?      // 1. properties (DataItemLink, DataItemTableFilter, etc.)
          reportColumn*?          // 2. columns
          reportDataItem*?        // 3. nested dataitems
          triggerDeclaration*?    // 4. triggers last (OnPreDataItem, OnAfterGetRecord, OnPostDataItem)
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
```

- [ ] **Step 4: Update `ALParser.g4` to import ALReportParser and add the `report` alternative**

```antlr
// Before:
import ALCodeunitParser, ALTableParser, ALPageParser, ALQueryParser;

alUnit
   : codeunitDeclaration
   | table
   | page
   | query
   ;

// After:
// ALPageParser is kept explicit here: it defines the `page` rule used in alUnit below.
// It also arrives transitively through ALReportParser — ANTLR4 deduplicates imported rules.
import ALCodeunitParser, ALTableParser, ALPageParser, ALQueryParser, ALReportParser;

alUnit
   : codeunitDeclaration
   | table
   | page
   | query
   | report
   ;
```

- [ ] **Step 5: Build — verify it succeeds**

```
dotnet build
```

Expected: Build succeeded, 0 errors. ANTLR regenerates `ALParser.cs` with `ReportContext`, `ReportDataItemContext`, `ReportColumnContext`, `ReportDatasetContext`, `ReportRequestPageContext`, `ReportRenderingContext`, `ReportLabelsContext`.

- [ ] **Step 6: Run report tests — all 8 should PASS**

```
dotnet test "..\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" --filter "ReportParser"
```

Expected: 8/8 PASS.

- [ ] **Step 7: Run full test suite**

```
dotnet test "..\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj"
```

Expected: all PASS.

- [ ] **Step 8: Commit**

```bash
git add ALReportParser.g4 ALParser.g4 "..\ANTLR AL Parser Tests\ReportParserTests.cs"
git commit -m "$(cat <<'EOF'
feat: add ALReportParser grammar for AL report objects

Adds grammar support for report dataset, requestpage (reusing ALPageParser
rules), rendering section (BC 2022+ only), and labels. Ordered declaration
sections enforced in reportDataItem. Updates ALParser.g4.

Co-Authored-By: Claude Sonnet 4.6 <noreply@anthropic.com>
EOF
)"
```

---

## Chunk 4: XmlPort Grammar

### Task 5: Add ALXmlPortParser.g4 and update ALParser.g4

**Files:**
- Create: `Grammar.AL.Antlr/ALXmlPortParser.g4`
- Modify: `Grammar.AL.Antlr/ALParser.g4`
- Create: `ANTLR AL Parser Tests/XmlPortParserTests.cs`

**Soft keywords**: `schema`, `textelement`, `tableelement`, `fieldelement`, `textattribute`, `fieldattribute`, `requestpage`
**Dedicated tokens**: `XMLPORT`

The schema uses a `xmlPortSchemaElement` union rule for recursive nesting. `xmlPortTextElement` and `xmlPortTableElement` reference `xmlPortSchemaElement`, and `xmlPortSchemaElement` references them back — mutual recursion that ANTLR4 handles cleanly (recursion is never left-edge).

- [ ] **Step 1: Create `XmlPortParserTests.cs` with failing tests**

Create file `ANTLR AL Parser Tests/XmlPortParserTests.cs`:

```csharp
using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

namespace Org.Edgerunner.Language.AL.Grammar.Tests;

/// <summary>
/// Verifies structural parsing of AL xmlport objects.
/// </summary>
public class XmlPortParserTests
{
    [Fact]
    public void Minimal_xmlport_parses_without_errors()
    {
        var source = @"
xmlport 50000 ""MinimalXmlPort""
{
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a minimal xmlport with no sections should parse without errors");
    }

    [Fact]
    public void Minimal_xmlport_produces_XmlPortContext()
    {
        var source = @"
xmlport 50000 ""MinimalXmlPort""
{
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.XmlPortContext>(tree)
            .Should().NotBeNull("an xmlport object should produce a XmlPortContext node");
    }

    [Fact]
    public void XmlPort_with_textelement_schema_parses_without_errors()
    {
        var source = @"
xmlport 50000 ""ExportPort""
{
    Direction = Export;
    schema
    {
        textelement(Root)
        {
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("an xmlport with a textelement schema should parse without errors");
    }

    [Fact]
    public void XmlPort_with_tableelement_and_fieldelements_parses_without_errors()
    {
        var source = @"
xmlport 50000 ""CustomerExport""
{
    Direction = Export;
    schema
    {
        textelement(Root)
        {
            tableelement(Customer; Customer)
            {
                fieldelement(No; Customer.""No."") { }
                fieldelement(Name; Customer.Name) { }
            }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("an xmlport with tableelement and fieldelements should parse without errors");
    }

    [Fact]
    public void XmlPort_with_nested_elements_and_attributes_parses_without_errors()
    {
        var source = @"
xmlport 50000 ""NestedXmlPort""
{
    schema
    {
        textelement(Root)
        {
            textelement(Header)
            {
                textattribute(version) { }
            }
            tableelement(Customer; Customer)
            {
                fieldelement(No; Customer.""No."") { }
                fieldattribute(Status; Customer.Status) { }
            }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("an xmlport with nested elements and attributes should parse without errors");
    }

    [Fact]
    public void XmlPort_with_requestpage_parses_without_errors()
    {
        var source = @"
xmlport 50000 ""XmlPortWithRequestPage""
{
    schema
    {
        textelement(Root)
        {
            tableelement(Customer; Customer)
            {
                fieldelement(No; Customer.""No."") { }
            }
        }
    }
    requestpage
    {
        layout
        {
            area(content)
            {
            }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("an xmlport with a requestpage section should parse without errors");
    }

    [Fact]
    public void XmlPort_tableelement_produces_XmlPortTableElementContext()
    {
        var source = @"
xmlport 50000 ""CustomerExport""
{
    schema
    {
        textelement(Root)
        {
            tableelement(Customer; Customer)
            {
                fieldelement(No; Customer.""No."") { }
            }
        }
    }
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.XmlPortTableElementContext>(tree)
            .Should().NotBeNull("a tableelement should produce a XmlPortTableElementContext node");
    }
}
```

- [ ] **Step 2: Build — verify compile errors for missing xmlport context types**

```
dotnet build
```

Expected: Build FAILS with errors like `'ALParser' does not contain a definition for 'XmlPortContext'` and `'XmlPortTableElementContext'`.

- [ ] **Step 3: Create `ALXmlPortParser.g4`**

Create file `Grammar.AL.Antlr/ALXmlPortParser.g4`:

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

// Union rule — referenced recursively by container elements below.
// ANTLR4 handles mutual recursion cleanly: recursion is never left-edge.
xmlPortSchemaElement
    : xmlPortTextElement
    | xmlPortTableElement
    | xmlPortFieldElement
    | xmlPortTextAttribute
    | xmlPortFieldAttribute
    ;

// Container elements — can hold child elements then triggers
xmlPortTextElement
    : {TokenMatches("textelement")}? IDENTIFIER
      LEFTPAREN name=identifier RIGHTPAREN
      LEFTCBRACE
          keyValueProperty*?          // 1. properties
          xmlPortSchemaElement*?      // 2. child elements/attributes (recursive)
          triggerDeclaration*?        // 3. triggers last
      RIGHTCBRACE
    ;

xmlPortTableElement
    : {TokenMatches("tableelement")}? IDENTIFIER
      LEFTPAREN varName=identifier SEMICOLON tableName=identifier RIGHTPAREN
      LEFTCBRACE
          keyValueProperty*?          // 1. properties (DataItemLink, etc.)
          xmlPortSchemaElement*?      // 2. child elements/attributes (recursive)
          triggerDeclaration*?        // 3. triggers last
      RIGHTCBRACE
    ;

xmlPortSchema
    : {TokenMatches("schema")}? IDENTIFIER
      LEFTCBRACE xmlPortSchemaElement* RIGHTCBRACE
    ;

// Reuses pageProperties, pageLayoutSection, pageActionSection from ALPageParser (imported above)
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

- [ ] **Step 4: Update `ALParser.g4` to import ALXmlPortParser and add the `xmlPort` alternative**

```antlr
// Before:
// ALPageParser is kept explicit here: it defines the `page` rule used in alUnit below.
// It also arrives transitively through ALReportParser — ANTLR4 deduplicates imported rules.
import ALCodeunitParser, ALTableParser, ALPageParser, ALQueryParser, ALReportParser;

alUnit
   : codeunitDeclaration
   | table
   | page
   | query
   | report
   ;

// After:
// ALPageParser is kept explicit here: it defines the `page` rule used in alUnit below.
// It also arrives transitively through ALReportParser and ALXmlPortParser — ANTLR4 deduplicates imported rules.
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

- [ ] **Step 5: Build — verify it succeeds**

```
dotnet build
```

Expected: Build succeeded, 0 errors. ANTLR regenerates `ALParser.cs` with `XmlPortContext`, `XmlPortSchemaContext`, `XmlPortTextElementContext`, `XmlPortTableElementContext`, `XmlPortFieldElementContext`, `XmlPortTextAttributeContext`, `XmlPortFieldAttributeContext`, `XmlPortRequestPageContext`.

- [ ] **Step 6: Run xmlport tests — all 7 should PASS**

```
dotnet test "..\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" --filter "XmlPortParser"
```

Expected: 7/7 PASS.

- [ ] **Step 7: Run full test suite**

```
dotnet test "..\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj"
```

Expected: all PASS.

- [ ] **Step 8: Commit**

```bash
git add ALXmlPortParser.g4 ALParser.g4 "..\ANTLR AL Parser Tests\XmlPortParserTests.cs"
git commit -m "$(cat <<'EOF'
feat: add ALXmlPortParser grammar for AL xmlport objects

Adds grammar support for xmlport schema (textelement, tableelement,
fieldelement, textattribute, fieldattribute with recursive nesting) and
requestpage (reusing ALPageParser rules). Updates ALParser.g4.

Co-Authored-By: Claude Sonnet 4.6 <noreply@anthropic.com>
EOF
)"
```
