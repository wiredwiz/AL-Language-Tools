# Remaining AL Object Types Implementation Plan

> **For agentic workers:** REQUIRED: Use superpowers:subagent-driven-development (if subagents available) or superpowers:executing-plans to implement this plan. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add grammar support for all 14 remaining AL object types (enum, enumextension, interface, tableextension, pageextension, pagecustomization, reportextension, profile, profileextension, permissionset, permissionsetextension, entitlement, dotnet, controladdin) to reach full BC26 coverage.

**Architecture:** Each new object type gets its own `.g4` parser grammar file; extension types import their base type's grammar (inheriting all rules) rather than redefining them. All new parsers are wired into `ALParser.g4` via its `import` list and `alUnit` union rule. Every new grammar file gets a corresponding `*ParserTests.cs` with inline unit tests; once all types are wired, real-file batch tests (`[Theory]`/`[MemberData]`) are added to validate against up to 40 BC26 source files per type.

**Tech Stack:** ANTLR4 C# target (Sam Harwell, NuGet v4.6.6), auto-generates C# from `.g4` on `dotnet build`. Tests: xUnit 2.4.2 + FluentAssertions 6.12.0, `net6.0`. Grammar project: `netstandard2.0`.

---

## File Structure

### Files to Modify
| File | Change |
|---|---|
| `Grammar.AL.Antlr/ALLexer.g4` | Add `PROFILEEXTENSION` token after `PROFILE` |
| `Grammar.AL.Antlr/ALPropertyParser.g4` | Expand `permissionSpecifier` to support non-tabledata types and `*` |
| `Grammar.AL.Antlr/ALPageParser.g4` | Add `pageLayoutModification` and `pageActionModification` rules |
| `Grammar.AL.Antlr/ALTableParser.g4` | Remove `tableExtension` and related rules (moved to `ALTableExtensionParser.g4`) |
| `Grammar.AL.Antlr/ALParser.g4` | Add imports for all 14 new parsers; expand `alUnit` with all 14 new alternatives |

### Files to Create (Grammar)
| File | Object type |
|---|---|
| `Grammar.AL.Antlr/ALEnumParser.g4` | `enum` |
| `Grammar.AL.Antlr/ALEnumExtensionParser.g4` | `enumextension` |
| `Grammar.AL.Antlr/ALInterfaceParser.g4` | `interface` |
| `Grammar.AL.Antlr/ALTableExtensionParser.g4` | `tableextension` (hosts moved rules) |
| `Grammar.AL.Antlr/ALPageExtensionParser.g4` | `pageextension` |
| `Grammar.AL.Antlr/ALPageCustomizationParser.g4` | `pagecustomization` |
| `Grammar.AL.Antlr/ALReportExtensionParser.g4` | `reportextension` |
| `Grammar.AL.Antlr/ALProfileParser.g4` | `profile` |
| `Grammar.AL.Antlr/ALProfileExtensionParser.g4` | `profileextension` |
| `Grammar.AL.Antlr/ALPermissionSetParser.g4` | `permissionset` |
| `Grammar.AL.Antlr/ALPermissionSetExtensionParser.g4` | `permissionsetextension` |
| `Grammar.AL.Antlr/ALEntitlementParser.g4` | `entitlement` |
| `Grammar.AL.Antlr/ALDotNetParser.g4` | `dotnet` |
| `Grammar.AL.Antlr/ALControlAddInParser.g4` | `controladdin` |

### Files to Create (Tests)
`ANTLR AL Parser Tests/EnumParserTests.cs`, `InterfaceParserTests.cs`, `TableExtensionParserTests.cs`, `PageExtensionParserTests.cs`, `PageCustomizationParserTests.cs`, `ReportExtensionParserTests.cs`, `ProfileParserTests.cs`, `PermissionSetParserTests.cs`, `EntitlementParserTests.cs`, `DotNetParserTests.cs`, `ControlAddInParserTests.cs`

---

## Chunk 1: Foundation + Enum Types

### Task 1: Foundation — shared grammar changes

**Files:**
- Modify: `Grammar.AL.Antlr/ALLexer.g4` (after PROFILE token, ~line 2533)
- Modify: `Grammar.AL.Antlr/ALPropertyParser.g4` (permissionSpecifier rule)
- Modify: `Grammar.AL.Antlr/ALPageParser.g4` (add modification verb rules after pageActionSection)

- [ ] **Step 1: Add `PROFILEEXTENSION` to `ALLexer.g4`**

  In `ALLexer.g4`, find the `PROFILE` token definition (currently around line 2531):
  ```antlr
  PROFILE
     : P R O F I L E
     ;
  ```
  Insert `PROFILEEXTENSION` immediately after it (before `PROGRAM`):
  ```antlr
  PROFILE
     : P R O F I L E
     ;

  PROFILEEXTENSION
     : P R O F I L E E X T E N S I O N
     ;
  ```

- [ ] **Step 2: Expand `permissionSpecifier` in `ALPropertyParser.g4`**

  Find and replace the current `permissionSpecifier` rule:
  ```antlr
  permissionSpecifier
     : TABLEDATA objectId EQUAL identifier
     ;
  ```
  With:
  ```antlr
  permissionSpecifier
      : TABLEDATA objectId EQUAL identifier
      | {!TokenMatches("tabledata")}? identifier (objectId | ASTERISK) EQUAL identifier
      ;
  ```
  The `{!TokenMatches("tabledata")}?` predicate prevents ANTLR4 from trying the second alternative when the current token text is "tabledata" — without it, TABLEDATA (which also lexes as a valid `identifier`) would create an ambiguity that causes incorrect parses.

- [ ] **Step 3: Add modification verb rules to `ALPageParser.g4`**

  At the end of `ALPageParser.g4`, append two new rules after the `page` rule. These are inherited by `ALPageExtensionParser`, `ALPageCustomizationParser`, and `ALReportExtensionParser` via ANTLR4's import mechanism:
  ```antlr
  pageLayoutModification
      : {TokenMatches("addafter") || TokenMatches("addbefore") ||
         TokenMatches("addfirst") || TokenMatches("addlast")}?
        identifier LEFTPAREN identifier RIGHTPAREN
        LEFTCBRACE pageLayoutEntity* RIGHTCBRACE
      | {TokenMatches("modify")}?
        identifier LEFTPAREN identifier RIGHTPAREN
        LEFTCBRACE pageGenericProperty* triggerDeclaration* RIGHTCBRACE
      | {TokenMatches("movebefore") || TokenMatches("moveafter") ||
         TokenMatches("movefirst") || TokenMatches("movelast")}?
        identifier LEFTPAREN identifier SEMICOLON identifier RIGHTPAREN
      ;

  pageActionModification
      : {TokenMatches("addafter") || TokenMatches("addbefore") ||
         TokenMatches("addfirst") || TokenMatches("addlast")}?
        identifier LEFTPAREN identifier RIGHTPAREN
        LEFTCBRACE pageAction* RIGHTCBRACE
      | {TokenMatches("modify")}?
        identifier LEFTPAREN identifier RIGHTPAREN
        LEFTCBRACE pageActionEntity* RIGHTCBRACE
      | {TokenMatches("movebefore") || TokenMatches("moveafter") ||
         TokenMatches("movefirst") || TokenMatches("movelast")}?
        identifier LEFTPAREN identifier SEMICOLON identifier RIGHTPAREN
      ;
  ```

- [ ] **Step 4: Build to verify the shared changes compile**

  ```
  dotnet build "D:\Projects\AL Langauge Tools\Grammar.AL.Antlr\Grammar.AL.Antlr.csproj"
  ```
  Expected: Build succeeded, 0 Error(s). ANTLR4 will regenerate all parser/lexer files. If warnings appear about unreachable rules, they are non-fatal.

- [ ] **Step 5: Run existing tests to confirm nothing regressed**

  ```
  dotnet test "D:\Projects\AL Langauge Tools\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" -v n
  ```
  Expected: All previously passing tests still pass.

- [ ] **Step 6: Commit**

  ```
  git add "Grammar.AL.Antlr/ALLexer.g4" "Grammar.AL.Antlr/ALPropertyParser.g4" "Grammar.AL.Antlr/ALPageParser.g4"
  git commit -m "feat: add PROFILEEXTENSION token, expand permissionSpecifier, add page modification verb rules"
  ```

---

### Task 2: `enum` parser + tests

**Files:**
- Create: `Grammar.AL.Antlr/ALEnumParser.g4`
- Create: `ANTLR AL Parser Tests/EnumParserTests.cs`
- Modify: `Grammar.AL.Antlr/ALParser.g4`

- [ ] **Step 1: Create `ALEnumParser.g4`**

  ```antlr
  parser grammar ALEnumParser;

  options { tokenVocab=ALLexer; }

  import ALPropertyParser;

  enumValue
      : VALUE LEFTPAREN INTEGER_LITERAL SEMICOLON identifier RIGHTPAREN
        LEFTCBRACE keyValueProperty*? RIGHTCBRACE
      ;

  enum
      : namespaceDeclaration? usingDeclarations?
        ENUM INTEGER_LITERAL identifier
        LEFTCBRACE keyValueProperty* enumValue* RIGHTCBRACE
      ;
  ```

- [ ] **Step 2: Create `ANTLR AL Parser Tests/EnumParserTests.cs`**

  ```csharp
  using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

  namespace Org.Edgerunner.Language.AL.Grammar.Tests;

  /// <summary>
  /// Verifies structural parsing of AL enum objects.
  /// </summary>
  public class EnumParserTests
  {
      [Fact]
      public void Minimal_enum_parses_without_errors()
      {
          var source = @"
  enum 50000 ""MyEnum""
  {
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a minimal enum with no values should parse without errors");
      }

      [Fact]
      public void Minimal_enum_produces_EnumContext()
      {
          var source = @"
  enum 50000 ""MyEnum""
  {
  }";
          var (tree, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty();
          ParseTreeSearch.FindFirst<ALParser.EnumContext>(tree)
              .Should().NotBeNull("an enum object should produce an EnumContext node");
      }

      [Fact]
      public void Enum_with_values_parses_without_errors()
      {
          var source = @"
  enum 50000 ""DocumentType""
  {
      Extensible = true;
      value(0; "" "") { Caption = 'blank'; }
      value(1; Invoice) { Caption = 'Invoice'; }
      value(2; ""Credit Memo"") { Caption = 'Credit Memo'; }
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("an enum with values and properties should parse without errors");
      }

      [Fact]
      public void Enum_with_obsolete_properties_parses_without_errors()
      {
          var source = @"
  enum 50001 ""ObsoleteEnum""
  {
      ObsoleteState = Pending;
      ObsoleteReason = 'Use NewEnum instead';
      value(0; Default) { }
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("an enum with obsolete properties should parse without errors");
      }
  }
  ```

- [ ] **Step 3: Update `ALParser.g4` — add enum import and alUnit alternative**

  In `ALParser.g4`, add `ALEnumParser` to the import list:
  ```antlr
  import ALCodeunitParser, ALTableParser, ALPageParser, ALQueryParser, ALReportParser, ALXmlPortParser,
         ALEnumParser;
  ```
  Add `enum` to `alUnit`:
  ```antlr
  alUnit
     : codeunitDeclaration
     | table
     | page
     | query
     | report
     | xmlPort
     | enum
     ;
  ```

- [ ] **Step 4: Build**

  ```
  dotnet build "D:\Projects\AL Langauge Tools\Grammar.AL.Antlr\Grammar.AL.Antlr.csproj"
  ```
  Expected: Build succeeded, 0 Error(s). The `ALParser.EnumContext` class is now generated.

- [ ] **Step 5: Run enum tests**

  ```
  dotnet test "D:\Projects\AL Langauge Tools\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" --filter "FullyQualifiedName~EnumParserTests" -v n
  ```
  Expected: 4 tests pass.

- [ ] **Step 6: Commit**

  ```
  git add "Grammar.AL.Antlr/ALEnumParser.g4" "ANTLR AL Parser Tests/EnumParserTests.cs" "Grammar.AL.Antlr/ALParser.g4"
  git commit -m "feat: add ALEnumParser grammar and EnumParserTests"
  ```

---

### Task 3: `enumextension` parser + tests

**Files:**
- Create: `Grammar.AL.Antlr/ALEnumExtensionParser.g4`
- Modify: `ANTLR AL Parser Tests/EnumParserTests.cs` (add enumextension tests to same file)
- Modify: `Grammar.AL.Antlr/ALParser.g4`

- [ ] **Step 1: Create `ALEnumExtensionParser.g4`**

  Imports `ALEnumParser`, inheriting the `enumValue` rule.
  ```antlr
  parser grammar ALEnumExtensionParser;

  options { tokenVocab=ALLexer; }

  import ALEnumParser;

  enumextension
      : namespaceDeclaration? usingDeclarations?
        ENUMEXTENSION INTEGER_LITERAL identifier EXTENDS identifier
        LEFTCBRACE enumValue* RIGHTCBRACE
      ;
  ```

- [ ] **Step 2: Add `enumextension` tests to `EnumParserTests.cs`**

  Insert the following methods before the final closing `}` of the `EnumParserTests` class:
  ```csharp
      [Fact]
      public void Minimal_enumextension_parses_without_errors()
      {
          var source = @"
  enumextension 50000 ""DocumentTypeExt"" extends ""Document Type""
  {
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a minimal enumextension with no new values should parse without errors");
      }

      [Fact]
      public void Minimal_enumextension_produces_EnumextensionContext()
      {
          var source = @"
  enumextension 50000 ""DocumentTypeExt"" extends ""Document Type""
  {
  }";
          var (tree, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty();
          ParseTreeSearch.FindFirst<ALParser.EnumextensionContext>(tree)
              .Should().NotBeNull("an enumextension object should produce an EnumextensionContext node");
      }

      [Fact]
      public void Enumextension_with_values_parses_without_errors()
      {
          var source = @"
  enumextension 50000 ""DocumentTypeExt"" extends ""Document Type""
  {
      value(100; ""New Value"") { Caption = 'New Value'; }
      value(101; AnotherValue) { Caption = 'Another Value'; }
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("an enumextension with new values should parse without errors");
      }
  ```

- [ ] **Step 3: Update `ALParser.g4`**

  Add `ALEnumExtensionParser` to imports:
  ```antlr
  import ALCodeunitParser, ALTableParser, ALPageParser, ALQueryParser, ALReportParser, ALXmlPortParser,
         ALEnumParser, ALEnumExtensionParser;
  ```
  Add `enumextension` to `alUnit`:
  ```antlr
  alUnit
     : codeunitDeclaration
     | table
     | page
     | query
     | report
     | xmlPort
     | enum
     | enumextension
     ;
  ```

- [ ] **Step 4: Build**

  ```
  dotnet build "D:\Projects\AL Langauge Tools\Grammar.AL.Antlr\Grammar.AL.Antlr.csproj"
  ```
  Expected: Build succeeded, 0 Error(s).

- [ ] **Step 5: Run enum + enumextension tests**

  ```
  dotnet test "D:\Projects\AL Langauge Tools\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" --filter "FullyQualifiedName~EnumParserTests" -v n
  ```
  Expected: 7 tests pass.

- [ ] **Step 6: Commit**

  ```
  git add "Grammar.AL.Antlr/ALEnumExtensionParser.g4" "ANTLR AL Parser Tests/EnumParserTests.cs" "Grammar.AL.Antlr/ALParser.g4"
  git commit -m "feat: add ALEnumExtensionParser grammar and enumextension tests"
  ```

---

## Chunk 2: Interface + Table/Page Extension Types

### Task 4: `interface` parser + tests

**Files:**
- Create: `Grammar.AL.Antlr/ALInterfaceParser.g4`
- Create: `ANTLR AL Parser Tests/InterfaceParserTests.cs`
- Modify: `Grammar.AL.Antlr/ALParser.g4`

- [ ] **Step 1: Create `ALInterfaceParser.g4`**

  Imports `ALPropertyParser` (not `ALCodeParser`) so `keyValueProperty` is available. `ALPropertyParser` transitively imports `ALCodeParser`, so `methodAttribute`, `parameterList`, and `returnValue` are accessible.
  ```antlr
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
  ```

- [ ] **Step 2: Create `ANTLR AL Parser Tests/InterfaceParserTests.cs`**

  ```csharp
  using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

  namespace Org.Edgerunner.Language.AL.Grammar.Tests;

  /// <summary>
  /// Verifies structural parsing of AL interface objects.
  /// </summary>
  public class InterfaceParserTests
  {
      [Fact]
      public void Minimal_interface_parses_without_errors()
      {
          var source = @"
  interface ""IMyInterface""
  {
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a minimal interface with no methods should parse without errors");
      }

      [Fact]
      public void Minimal_interface_produces_InterfaceContext()
      {
          var source = @"
  interface ""IMyInterface""
  {
  }";
          var (tree, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty();
          ParseTreeSearch.FindFirst<ALParser.InterfaceContext>(tree)
              .Should().NotBeNull("an interface object should produce an InterfaceContext node");
      }

      [Fact]
      public void Interface_with_procedure_signatures_parses_without_errors()
      {
          var source = @"
  interface ""IMyInterface""
  {
      procedure DoSomething(Value: Integer)
      procedure GetName(): Text[100];
      procedure Process(Rec: Record Customer; var Result: Text[250])
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("an interface with procedure signatures (with and without semicolons) should parse without errors");
      }

      [Fact]
      public void Interface_with_obsolete_properties_parses_without_errors()
      {
          var source = @"
  interface ""IMyInterface""
  {
      ObsoleteState = Pending;
      ObsoleteReason = 'Use INewInterface instead';
      procedure DoSomething()
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("an interface with obsolete state properties should parse without errors");
      }

      [Fact]
      public void Interface_with_method_attributes_parses_without_errors()
      {
          var source = @"
  interface ""IMyInterface""
  {
      [Obsolete('Use NewMethod', '24.0')]
      procedure OldMethod()
      procedure NewMethod()
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("an interface with method attributes should parse without errors");
      }
  }
  ```

- [ ] **Step 3: Update `ALParser.g4`**

  Replace the import list and `alUnit` with:
  ```antlr
  import ALCodeunitParser, ALTableParser, ALPageParser, ALQueryParser, ALReportParser, ALXmlPortParser,
         ALEnumParser, ALEnumExtensionParser, ALInterfaceParser;
  ```
  ```antlr
  alUnit
     : codeunitDeclaration
     | table | page | query | report | xmlPort
     | enum | enumextension
     | interface
     ;
  ```

- [ ] **Step 4: Build**

  ```
  dotnet build "D:\Projects\AL Langauge Tools\Grammar.AL.Antlr\Grammar.AL.Antlr.csproj"
  ```
  Expected: Build succeeded, 0 Error(s).

- [ ] **Step 5: Run interface tests**

  ```
  dotnet test "D:\Projects\AL Langauge Tools\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" --filter "FullyQualifiedName~InterfaceParserTests" -v n
  ```
  Expected: 5 tests pass.

- [ ] **Step 6: Commit**

  ```
  git add "Grammar.AL.Antlr/ALInterfaceParser.g4" "ANTLR AL Parser Tests/InterfaceParserTests.cs" "Grammar.AL.Antlr/ALParser.g4"
  git commit -m "feat: add ALInterfaceParser grammar and InterfaceParserTests"
  ```

---

### Task 5: `tableextension` parser + tests

This task moves `tableExtension` and its supporting rules from `ALTableParser.g4` into the new `ALTableExtensionParser.g4`.

**Files:**
- Create: `Grammar.AL.Antlr/ALTableExtensionParser.g4`
- Modify: `Grammar.AL.Antlr/ALTableParser.g4` (remove moved rules)
- Create: `ANTLR AL Parser Tests/TableExtensionParserTests.cs`
- Modify: `Grammar.AL.Antlr/ALParser.g4`

- [ ] **Step 1: Create `ALTableExtensionParser.g4`**

  Contains the rules previously in `ALTableParser.g4` starting at `tableExtFieldGroup`:
  ```antlr
  parser grammar ALTableExtensionParser;

  options { tokenVocab=ALLexer; }

  import ALTableParser;

  tableExtFieldGroup
      : {TokenMatches("addlast")}? IDENTIFIER LEFTPAREN tableFieldGroupName SEMICOLON fieldNames RIGHTPAREN
        LEFTCBRACE keyValueProperty*? RIGHTCBRACE
      ;

  tableExtFieldGroups
      : FIELDGROUPS LEFTCBRACE tableExtFieldGroup*? RIGHTCBRACE
      ;

  tableExtEntity
      : tableFields
      | tableKeys
      | tableExtFieldGroups
      ;

  tableExtEntities
      : tableExtEntity+
      ;

  tableExtension
      : namespaceDeclaration? usingDeclarations?
        TABLEEXTENSION INTEGER_LITERAL identifier EXTENDS identifier
        LEFTCBRACE tableProperties tableExtEntities? codeDeclarations? RIGHTCBRACE
      ;
  ```

- [ ] **Step 2: Remove moved rules from `ALTableParser.g4`**

  Delete the following rules from `ALTableParser.g4` (they were the last rules in the file):
  - `tableExtFieldGroup`
  - `tableExtFieldGroups`
  - `tableExtEntity`
  - `tableExtEntities`
  - `tableExtension`

  The file should end after the `table` rule.

- [ ] **Step 3: Create `ANTLR AL Parser Tests/TableExtensionParserTests.cs`**

  ```csharp
  using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

  namespace Org.Edgerunner.Language.AL.Grammar.Tests;

  /// <summary>
  /// Verifies structural parsing of AL tableextension objects.
  /// </summary>
  public class TableExtensionParserTests
  {
      [Fact]
      public void Minimal_tableextension_parses_without_errors()
      {
          var source = @"
  tableextension 50000 ""CustomerExt"" extends Customer
  {
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a minimal tableextension with no sections should parse without errors");
      }

      [Fact]
      public void Minimal_tableextension_produces_TableExtensionContext()
      {
          var source = @"
  tableextension 50000 ""CustomerExt"" extends Customer
  {
  }";
          var (tree, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty();
          ParseTreeSearch.FindFirst<ALParser.TableExtensionContext>(tree)
              .Should().NotBeNull("a tableextension object should produce a TableExtensionContext node");
      }

      [Fact]
      public void Tableextension_with_fields_parses_without_errors()
      {
          var source = @"
  tableextension 50000 ""CustomerExt"" extends Customer
  {
      fields
      {
          field(50000; ""My Field""; Text[50])
          {
              Caption = 'My Field';
          }
      }
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a tableextension with new fields should parse without errors");
      }

      [Fact]
      public void Tableextension_with_keys_parses_without_errors()
      {
          var source = @"
  tableextension 50000 ""CustomerExt"" extends Customer
  {
      keys
      {
          key(MyKey; Name, ""No."") { }
      }
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a tableextension with new keys should parse without errors");
      }

      [Fact]
      public void Tableextension_with_fieldgroups_parses_without_errors()
      {
          var source = @"
  tableextension 50000 ""CustomerExt"" extends Customer
  {
      fieldgroups
      {
          addlast(DropDown; Name, ""No."") { }
      }
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a tableextension with addlast fieldgroup should parse without errors");
      }

      [Fact]
      public void Tableextension_with_procedure_parses_without_errors()
      {
          var source = @"
  tableextension 50000 ""CustomerExt"" extends Customer
  {
      procedure MyProcedure()
      begin
      end;
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a tableextension with a procedure should parse without errors");
      }
  }
  ```

- [ ] **Step 4: Update `ALParser.g4`**

  Add `ALTableExtensionParser` to imports and `tableExtension` to `alUnit`. Keep `ALTableParser` explicit (it defines the `table` rule used in `alUnit`):
  ```antlr
  import ALCodeunitParser, ALTableParser, ALTableExtensionParser,
         ALPageParser, ALQueryParser, ALReportParser, ALXmlPortParser,
         ALEnumParser, ALEnumExtensionParser, ALInterfaceParser;
  ```
  ```antlr
  alUnit
     : codeunitDeclaration
     | table | tableExtension
     | page | query | report | xmlPort
     | enum | enumextension
     | interface
     ;
  ```

- [ ] **Step 5: Build**

  ```
  dotnet build "D:\Projects\AL Langauge Tools\Grammar.AL.Antlr\Grammar.AL.Antlr.csproj"
  ```
  Expected: Build succeeded, 0 Error(s).

- [ ] **Step 6: Run all tests to confirm both table and tableextension work**

  ```
  dotnet test "D:\Projects\AL Langauge Tools\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" -v n
  ```
  Expected: All tests pass. Confirm `TableExtensionParserTests` (6 tests) pass and no existing tests regressed.

- [ ] **Step 7: Commit**

  ```
  git add "Grammar.AL.Antlr/ALTableExtensionParser.g4" "Grammar.AL.Antlr/ALTableParser.g4" "ANTLR AL Parser Tests/TableExtensionParserTests.cs" "Grammar.AL.Antlr/ALParser.g4"
  git commit -m "feat: move tableExtension to ALTableExtensionParser, add TableExtensionParserTests"
  ```

---

### Task 6: `pageextension` parser + tests

**Files:**
- Create: `Grammar.AL.Antlr/ALPageExtensionParser.g4`
- Create: `ANTLR AL Parser Tests/PageExtensionParserTests.cs`
- Modify: `Grammar.AL.Antlr/ALParser.g4`

- [ ] **Step 1: Create `ALPageExtensionParser.g4`**

  Inherits `pageProperties`, `pageLayoutEntity`, `pageAction`, `pageLayoutModification`, `pageActionModification` from `ALPageParser`:
  ```antlr
  parser grammar ALPageExtensionParser;

  options { tokenVocab=ALLexer; }

  import ALPageParser;

  pageExtensionLayoutSection
      : LAYOUT LEFTCBRACE pageLayoutModification* RIGHTCBRACE
      ;

  pageExtensionActionSection
      : ACTIONS LEFTCBRACE pageActionModification* RIGHTCBRACE
      ;

  pageextension
      : namespaceDeclaration? usingDeclarations?
        PAGEEXTENSION INTEGER_LITERAL identifier EXTENDS identifier
        LEFTCBRACE
            pageProperties
            pageExtensionLayoutSection?
            pageExtensionActionSection?
            codeDeclarations?
        RIGHTCBRACE
      ;
  ```

- [ ] **Step 2: Create `ANTLR AL Parser Tests/PageExtensionParserTests.cs`**

  ```csharp
  using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

  namespace Org.Edgerunner.Language.AL.Grammar.Tests;

  /// <summary>
  /// Verifies structural parsing of AL pageextension objects.
  /// </summary>
  public class PageExtensionParserTests
  {
      [Fact]
      public void Minimal_pageextension_parses_without_errors()
      {
          var source = @"
  pageextension 50000 ""CustomerCardExt"" extends ""Customer Card""
  {
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a minimal pageextension with no sections should parse without errors");
      }

      [Fact]
      public void Minimal_pageextension_produces_PageextensionContext()
      {
          var source = @"
  pageextension 50000 ""CustomerCardExt"" extends ""Customer Card""
  {
  }";
          var (tree, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty();
          ParseTreeSearch.FindFirst<ALParser.PageextensionContext>(tree)
              .Should().NotBeNull("a pageextension object should produce a PageextensionContext node");
      }

      [Fact]
      public void Pageextension_with_addlast_layout_parses_without_errors()
      {
          var source = @"
  pageextension 50000 ""CustomerCardExt"" extends ""Customer Card""
  {
      layout
      {
          addlast(General)
          {
              field(MyField; Rec.""My Field"") { }
          }
      }
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a pageextension with addlast layout should parse without errors");
      }

      [Fact]
      public void Pageextension_with_modify_layout_parses_without_errors()
      {
          var source = @"
  pageextension 50000 ""CustomerCardExt"" extends ""Customer Card""
  {
      layout
      {
          modify(Name)
          {
              Visible = false;
          }
      }
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a pageextension with modify layout should parse without errors");
      }

      [Fact]
      public void Pageextension_with_addafter_actions_parses_without_errors()
      {
          var source = @"
  pageextension 50000 ""CustomerCardExt"" extends ""Customer Card""
  {
      actions
      {
          addafter(""&Customer"")
          {
              action(MyAction)
              {
                  Caption = 'My Action';
              }
          }
      }
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a pageextension with addafter actions should parse without errors");
      }

      [Fact]
      public void Pageextension_with_movebefore_layout_parses_without_errors()
      {
          var source = @"
  pageextension 50000 ""CustomerCardExt"" extends ""Customer Card""
  {
      layout
      {
          movebefore(Name; ""No."")
      }
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a pageextension with movebefore layout verb should parse without errors");
      }

      [Fact]
      public void Pageextension_with_procedure_parses_without_errors()
      {
          var source = @"
  pageextension 50000 ""CustomerCardExt"" extends ""Customer Card""
  {
      procedure MyHelper()
      begin
      end;
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a pageextension with a procedure should parse without errors");
      }
  }
  ```

- [ ] **Step 3: Update `ALParser.g4`**

  Add `ALPageExtensionParser` to imports and `pageextension` to `alUnit`:
  ```antlr
  import ALCodeunitParser, ALTableParser, ALTableExtensionParser,
         ALPageParser, ALPageExtensionParser,
         ALQueryParser, ALReportParser, ALXmlPortParser,
         ALEnumParser, ALEnumExtensionParser, ALInterfaceParser;
  ```
  ```antlr
  alUnit
     : codeunitDeclaration
     | table | tableExtension
     | page | pageextension | query | report | xmlPort
     | enum | enumextension
     | interface
     ;
  ```

- [ ] **Step 4: Build**

  ```
  dotnet build "D:\Projects\AL Langauge Tools\Grammar.AL.Antlr\Grammar.AL.Antlr.csproj"
  ```
  Expected: Build succeeded, 0 Error(s).

- [ ] **Step 5: Run pageextension tests**

  ```
  dotnet test "D:\Projects\AL Langauge Tools\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" --filter "FullyQualifiedName~PageExtensionParserTests" -v n
  ```
  Expected: 7 tests pass.

- [ ] **Step 6: Commit**

  ```
  git add "Grammar.AL.Antlr/ALPageExtensionParser.g4" "ANTLR AL Parser Tests/PageExtensionParserTests.cs" "Grammar.AL.Antlr/ALParser.g4"
  git commit -m "feat: add ALPageExtensionParser grammar and PageExtensionParserTests"
  ```

---

### Task 7: `pagecustomization` parser + tests

**Files:**
- Create: `Grammar.AL.Antlr/ALPageCustomizationParser.g4`
- Create: `ANTLR AL Parser Tests/PageCustomizationParserTests.cs`
- Modify: `Grammar.AL.Antlr/ALParser.g4`

- [ ] **Step 1: Create `ALPageCustomizationParser.g4`**

  Imports `ALPageParser` directly (NOT `ALPageExtensionParser`) — `pagecustomization` only supports `modify`, no `add*`/`move*` verbs, no code. Uses `customizes` soft keyword instead of `extends`. No object ID.
  ```antlr
  parser grammar ALPageCustomizationParser;

  options { tokenVocab=ALLexer; }

  import ALPageParser;

  // Restrict modify blocks to properties only — no triggers, no code
  pageCustomizationLayoutEntity : pageGenericProperty ;

  pageCustomizationActionEntity : pageGenericProperty ;

  pageCustomizationLayoutModification
      : {TokenMatches("modify")}?
        identifier LEFTPAREN identifier RIGHTPAREN
        LEFTCBRACE pageCustomizationLayoutEntity* RIGHTCBRACE
      ;

  pageCustomizationActionModification
      : {TokenMatches("modify")}?
        identifier LEFTPAREN identifier RIGHTPAREN
        LEFTCBRACE pageCustomizationActionEntity* RIGHTCBRACE
      ;

  pageCustomizationLayoutSection
      : LAYOUT LEFTCBRACE pageCustomizationLayoutModification* RIGHTCBRACE
      ;

  pageCustomizationActionSection
      : ACTIONS LEFTCBRACE pageCustomizationActionModification* RIGHTCBRACE
      ;

  pagecustomization
      : namespaceDeclaration? usingDeclarations?
        PAGECUSTOMIZATION identifier
        {TokenMatches("customizes")}? IDENTIFIER identifier
        LEFTCBRACE
            pageCustomizationLayoutSection?
            pageCustomizationActionSection?
        RIGHTCBRACE
      ;
  ```

- [ ] **Step 2: Create `ANTLR AL Parser Tests/PageCustomizationParserTests.cs`**

  ```csharp
  using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

  namespace Org.Edgerunner.Language.AL.Grammar.Tests;

  /// <summary>
  /// Verifies structural parsing of AL pagecustomization objects.
  /// </summary>
  public class PageCustomizationParserTests
  {
      [Fact]
      public void Minimal_pagecustomization_parses_without_errors()
      {
          var source = @"
  pagecustomization MyCustomization customizes ""Customer Card""
  {
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a minimal pagecustomization should parse without errors");
      }

      [Fact]
      public void Minimal_pagecustomization_produces_PagecustomizationContext()
      {
          var source = @"
  pagecustomization MyCustomization customizes ""Customer Card""
  {
  }";
          var (tree, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty();
          ParseTreeSearch.FindFirst<ALParser.PagecustomizationContext>(tree)
              .Should().NotBeNull("a pagecustomization object should produce a PagecustomizationContext node");
      }

      [Fact]
      public void Pagecustomization_with_modify_layout_parses_without_errors()
      {
          var source = @"
  pagecustomization MyCustomization customizes ""Customer Card""
  {
      layout
      {
          modify(Name)
          {
              Visible = false;
          }
          modify(""No."")
          {
              Importance = Promoted;
          }
      }
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a pagecustomization with modify layout blocks should parse without errors");
      }

      [Fact]
      public void Pagecustomization_with_modify_actions_parses_without_errors()
      {
          var source = @"
  pagecustomization MyCustomization customizes ""Customer Card""
  {
      actions
      {
          modify(MyAction)
          {
              Visible = false;
          }
      }
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a pagecustomization with modify actions should parse without errors");
      }
  }
  ```

- [ ] **Step 3: Update `ALParser.g4`**

  Add `ALPageCustomizationParser` to imports and `pagecustomization` to `alUnit`:
  ```antlr
  import ALCodeunitParser, ALTableParser, ALTableExtensionParser,
         ALPageParser, ALPageExtensionParser, ALPageCustomizationParser,
         ALQueryParser, ALReportParser, ALXmlPortParser,
         ALEnumParser, ALEnumExtensionParser, ALInterfaceParser;
  ```
  ```antlr
  alUnit
     : codeunitDeclaration
     | table | tableExtension
     | page | pageextension | pagecustomization | query | report | xmlPort
     | enum | enumextension
     | interface
     ;
  ```

- [ ] **Step 4: Build**

  ```
  dotnet build "D:\Projects\AL Langauge Tools\Grammar.AL.Antlr\Grammar.AL.Antlr.csproj"
  ```
  Expected: Build succeeded, 0 Error(s).

- [ ] **Step 5: Run pagecustomization tests**

  ```
  dotnet test "D:\Projects\AL Langauge Tools\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" --filter "FullyQualifiedName~PageCustomizationParserTests" -v n
  ```
  Expected: 4 tests pass.

- [ ] **Step 6: Commit**

  ```
  git add "Grammar.AL.Antlr/ALPageCustomizationParser.g4" "ANTLR AL Parser Tests/PageCustomizationParserTests.cs" "Grammar.AL.Antlr/ALParser.g4"
  git commit -m "feat: add ALPageCustomizationParser grammar and PageCustomizationParserTests"
  ```

---

## Chunk 3: ReportExtension + Profile + PermissionSet Types

### Task 8: `reportextension` parser + tests

**Files:**
- Create: `Grammar.AL.Antlr/ALReportExtensionParser.g4`
- Create: `ANTLR AL Parser Tests/ReportExtensionParserTests.cs`
- Modify: `Grammar.AL.Antlr/ALParser.g4`

- [ ] **Step 1: Create `ALReportExtensionParser.g4`**

  Imports both `ALReportParser` (for `reportDataItem`, `reportColumn`, `reportDataItemProperty`) and `ALPageExtensionParser` (for `pageExtensionLayoutSection`, `pageExtensionActionSection` in the requestpage). Note: `reportextension` has no object-level `reportProperties` section — this is intentional and consistent with BC26 behavior.
  ```antlr
  parser grammar ALReportExtensionParser;

  options { tokenVocab=ALLexer; }

  import ALReportParser, ALPageExtensionParser;

  reportExtensionDatasetModification
      : {TokenMatches("addafter") || TokenMatches("addbefore") ||
         TokenMatches("addfirst") || TokenMatches("addlast")}?
        identifier LEFTPAREN identifier RIGHTPAREN
        LEFTCBRACE reportDataItem* RIGHTCBRACE
      | {TokenMatches("modify")}?
        identifier LEFTPAREN identifier RIGHTPAREN
        LEFTCBRACE reportDataItemProperty* RIGHTCBRACE
      ;

  reportExtensionDataset
      : {TokenMatches("dataset")}? IDENTIFIER
        LEFTCBRACE reportExtensionDatasetModification* RIGHTCBRACE
      ;

  reportExtensionRequestPage
      : {TokenMatches("requestpage")}? IDENTIFIER
        LEFTCBRACE pageExtensionLayoutSection? pageExtensionActionSection? RIGHTCBRACE
      ;

  reportextension
      : namespaceDeclaration? usingDeclarations?
        REPORTEXTENSION INTEGER_LITERAL identifier EXTENDS identifier
        LEFTCBRACE
            reportExtensionDataset?
            reportExtensionRequestPage?
            codeDeclarations?
        RIGHTCBRACE
      ;
  ```

- [ ] **Step 2: Create `ANTLR AL Parser Tests/ReportExtensionParserTests.cs`**

  ```csharp
  using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

  namespace Org.Edgerunner.Language.AL.Grammar.Tests;

  /// <summary>
  /// Verifies structural parsing of AL reportextension objects.
  /// </summary>
  public class ReportExtensionParserTests
  {
      [Fact]
      public void Minimal_reportextension_parses_without_errors()
      {
          var source = @"
  reportextension 50000 ""CustomerReportExt"" extends ""Customer - List""
  {
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a minimal reportextension with no sections should parse without errors");
      }

      [Fact]
      public void Minimal_reportextension_produces_ReportextensionContext()
      {
          var source = @"
  reportextension 50000 ""CustomerReportExt"" extends ""Customer - List""
  {
  }";
          var (tree, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty();
          ParseTreeSearch.FindFirst<ALParser.ReportextensionContext>(tree)
              .Should().NotBeNull("a reportextension object should produce a ReportextensionContext node");
      }

      [Fact]
      public void Reportextension_with_addlast_dataset_parses_without_errors()
      {
          var source = @"
  reportextension 50000 ""CustomerReportExt"" extends ""Customer - List""
  {
      dataset
      {
          addlast(Customer)
          {
              column(MyField; ""My Field"") { }
          }
      }
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a reportextension with addlast dataset should parse without errors");
      }

      [Fact]
      public void Reportextension_with_modify_dataset_parses_without_errors()
      {
          var source = @"
  reportextension 50000 ""CustomerReportExt"" extends ""Customer - List""
  {
      dataset
      {
          modify(Customer)
          {
              DataItemTableView = sorting(Name);
          }
      }
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a reportextension with modify dataset should parse without errors");
      }

      [Fact]
      public void Reportextension_with_requestpage_parses_without_errors()
      {
          var source = @"
  reportextension 50000 ""CustomerReportExt"" extends ""Customer - List""
  {
      requestpage
      {
          layout
          {
              addlast(content)
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
          errors.Should().BeEmpty("a reportextension with a requestpage layout modification should parse without errors");
      }
  }
  ```

- [ ] **Step 3: Update `ALParser.g4`**

  Add `ALReportExtensionParser` to imports and `reportextension` to `alUnit`:
  ```antlr
  import ALCodeunitParser, ALTableParser, ALTableExtensionParser,
         ALPageParser, ALPageExtensionParser, ALPageCustomizationParser,
         ALQueryParser, ALReportParser, ALReportExtensionParser, ALXmlPortParser,
         ALEnumParser, ALEnumExtensionParser, ALInterfaceParser;
  ```
  ```antlr
  alUnit
     : codeunitDeclaration
     | table | tableExtension
     | page | pageextension | pagecustomization
     | query | report | reportextension | xmlPort
     | enum | enumextension
     | interface
     ;
  ```

- [ ] **Step 4: Build**

  ```
  dotnet build "D:\Projects\AL Langauge Tools\Grammar.AL.Antlr\Grammar.AL.Antlr.csproj"
  ```
  Expected: Build succeeded, 0 Error(s).

- [ ] **Step 5: Run reportextension tests**

  ```
  dotnet test "D:\Projects\AL Langauge Tools\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" --filter "FullyQualifiedName~ReportExtensionParserTests" -v n
  ```
  Expected: 5 tests pass.

- [ ] **Step 6: Commit**

  ```
  git add "Grammar.AL.Antlr/ALReportExtensionParser.g4" "ANTLR AL Parser Tests/ReportExtensionParserTests.cs" "Grammar.AL.Antlr/ALParser.g4"
  git commit -m "feat: add ALReportExtensionParser grammar and ReportExtensionParserTests"
  ```

---

### Task 9: `profile` parser + tests

**Files:**
- Create: `Grammar.AL.Antlr/ALProfileParser.g4`
- Create: `ANTLR AL Parser Tests/ProfileParserTests.cs`
- Modify: `Grammar.AL.Antlr/ALParser.g4`

- [ ] **Step 1: Create `ALProfileParser.g4`**

  Pure key/value properties: `Caption`, `ProfileDescription`, `RoleCenter`, `Enabled`, `Customizations`.
  ```antlr
  parser grammar ALProfileParser;

  options { tokenVocab=ALLexer; }

  import ALPropertyParser;

  profile
      : namespaceDeclaration? usingDeclarations?
        PROFILE identifier
        LEFTCBRACE keyValueProperty* RIGHTCBRACE
      ;
  ```

- [ ] **Step 2: Create `ANTLR AL Parser Tests/ProfileParserTests.cs`**

  ```csharp
  using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

  namespace Org.Edgerunner.Language.AL.Grammar.Tests;

  /// <summary>
  /// Verifies structural parsing of AL profile and profileextension objects.
  /// </summary>
  public class ProfileParserTests
  {
      [Fact]
      public void Minimal_profile_parses_without_errors()
      {
          var source = @"
  profile MyProfile
  {
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a minimal profile with no properties should parse without errors");
      }

      [Fact]
      public void Minimal_profile_produces_ProfileContext()
      {
          var source = @"
  profile MyProfile
  {
  }";
          var (tree, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty();
          ParseTreeSearch.FindFirst<ALParser.ProfileContext>(tree)
              .Should().NotBeNull("a profile object should produce a ProfileContext node");
      }

      [Fact]
      public void Profile_with_properties_parses_without_errors()
      {
          var source = @"
  profile MyProfile
  {
      Caption = 'My Profile';
      RoleCenter = ""Business Manager Role Center"";
      Enabled = true;
      Customizations = MyCustomization;
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a profile with Caption, RoleCenter, Enabled, and Customizations properties should parse without errors");
      }
  }
  ```

- [ ] **Step 3: Update `ALParser.g4`**

  Replace the import list and `alUnit` with:
  ```antlr
  import ALCodeunitParser,
         ALTableParser, ALTableExtensionParser,
         ALPageParser, ALPageExtensionParser, ALPageCustomizationParser,
         ALQueryParser,
         ALReportParser, ALReportExtensionParser,
         ALXmlPortParser,
         ALEnumParser, ALEnumExtensionParser,
         ALInterfaceParser,
         ALProfileParser;
  ```
  ```antlr
  alUnit
     : codeunitDeclaration
     | table | tableExtension
     | page | pageextension | pagecustomization
     | query | report | reportextension | xmlPort
     | enum | enumextension
     | interface
     | profile
     ;
  ```

- [ ] **Step 4: Build**

  ```
  dotnet build "D:\Projects\AL Langauge Tools\Grammar.AL.Antlr\Grammar.AL.Antlr.csproj"
  ```
  Expected: Build succeeded, 0 Error(s).

- [ ] **Step 5: Run profile tests**

  ```
  dotnet test "D:\Projects\AL Langauge Tools\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" --filter "FullyQualifiedName~ProfileParserTests" -v n
  ```
  Expected: 3 tests pass.

- [ ] **Step 6: Commit**

  ```
  git add "Grammar.AL.Antlr/ALProfileParser.g4" "ANTLR AL Parser Tests/ProfileParserTests.cs" "Grammar.AL.Antlr/ALParser.g4"
  git commit -m "feat: add ALProfileParser grammar and profile tests"
  ```

---

### Task 10: `profileextension` parser + tests

**Files:**
- Create: `Grammar.AL.Antlr/ALProfileExtensionParser.g4`
- Modify: `ANTLR AL Parser Tests/ProfileParserTests.cs` (add profileextension tests)
- Modify: `Grammar.AL.Antlr/ALParser.g4`

- [ ] **Step 1: Create `ALProfileExtensionParser.g4`**

  Uses the `PROFILEEXTENSION` token added to `ALLexer.g4` in Task 1.
  ```antlr
  parser grammar ALProfileExtensionParser;

  options { tokenVocab=ALLexer; }

  import ALProfileParser;

  profileextension
      : namespaceDeclaration? usingDeclarations?
        PROFILEEXTENSION identifier EXTENDS identifier
        LEFTCBRACE keyValueProperty* RIGHTCBRACE
      ;
  ```

- [ ] **Step 2: Add `profileextension` tests to `ProfileParserTests.cs`**

  Insert the following methods before the final closing `}` of the `ProfileParserTests` class:
  ```csharp
      [Fact]
      public void Minimal_profileextension_parses_without_errors()
      {
          var source = @"
  profileextension MyProfileExt extends MyProfile
  {
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a minimal profileextension with no properties should parse without errors");
      }

      [Fact]
      public void Minimal_profileextension_produces_ProfileextensionContext()
      {
          var source = @"
  profileextension MyProfileExt extends MyProfile
  {
  }";
          var (tree, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty();
          ParseTreeSearch.FindFirst<ALParser.ProfileextensionContext>(tree)
              .Should().NotBeNull("a profileextension object should produce a ProfileextensionContext node");
      }

      [Fact]
      public void Profileextension_with_customization_parses_without_errors()
      {
          var source = @"
  profileextension MyProfileExt extends MyProfile
  {
      Customizations = AdditionalCustomization;
      Enabled = true;
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a profileextension with properties should parse without errors");
      }
  ```

- [ ] **Step 3: Update `ALParser.g4`**

  Replace the import list and `alUnit` with:
  ```antlr
  import ALCodeunitParser,
         ALTableParser, ALTableExtensionParser,
         ALPageParser, ALPageExtensionParser, ALPageCustomizationParser,
         ALQueryParser,
         ALReportParser, ALReportExtensionParser,
         ALXmlPortParser,
         ALEnumParser, ALEnumExtensionParser,
         ALInterfaceParser,
         ALProfileParser, ALProfileExtensionParser;
  ```
  ```antlr
  alUnit
     : codeunitDeclaration
     | table | tableExtension
     | page | pageextension | pagecustomization
     | query | report | reportextension | xmlPort
     | enum | enumextension
     | interface
     | profile | profileextension
     ;
  ```

- [ ] **Step 4: Build**

  ```
  dotnet build "D:\Projects\AL Langauge Tools\Grammar.AL.Antlr\Grammar.AL.Antlr.csproj"
  ```
  Expected: Build succeeded, 0 Error(s).

- [ ] **Step 5: Run profile + profileextension tests**

  ```
  dotnet test "D:\Projects\AL Langauge Tools\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" --filter "FullyQualifiedName~ProfileParserTests" -v n
  ```
  Expected: 6 tests pass.

- [ ] **Step 6: Commit**

  ```
  git add "Grammar.AL.Antlr/ALProfileExtensionParser.g4" "ANTLR AL Parser Tests/ProfileParserTests.cs" "Grammar.AL.Antlr/ALParser.g4"
  git commit -m "feat: add ALProfileExtensionParser grammar and profileextension tests"
  ```

---

### Task 11: `permissionset` parser + tests

**Files:**
- Create: `Grammar.AL.Antlr/ALPermissionSetParser.g4`
- Create: `ANTLR AL Parser Tests/PermissionSetParserTests.cs`
- Modify: `Grammar.AL.Antlr/ALParser.g4`

- [ ] **Step 1: Create `ALPermissionSetParser.g4`**

  Uses expanded `permissionsProperty` from `ALPropertyParser.g4` (Task 1). The `includedPermissionSetsProperty` handles the comma-separated identifier list form.
  ```antlr
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
  ```

- [ ] **Step 2: Create `ANTLR AL Parser Tests/PermissionSetParserTests.cs`**

  ```csharp
  using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

  namespace Org.Edgerunner.Language.AL.Grammar.Tests;

  /// <summary>
  /// Verifies structural parsing of AL permissionset and permissionsetextension objects.
  /// </summary>
  public class PermissionSetParserTests
  {
      [Fact]
      public void Minimal_permissionset_parses_without_errors()
      {
          var source = @"
  permissionset 50000 ""MyPermSet""
  {
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a minimal permissionset with no properties should parse without errors");
      }

      [Fact]
      public void Minimal_permissionset_produces_PermissionsetContext()
      {
          var source = @"
  permissionset 50000 ""MyPermSet""
  {
  }";
          var (tree, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty();
          ParseTreeSearch.FindFirst<ALParser.PermissionsetContext>(tree)
              .Should().NotBeNull("a permissionset object should produce a PermissionsetContext node");
      }

      [Fact]
      public void Permissionset_with_tabledata_permissions_parses_without_errors()
      {
          var source = @"
  permissionset 50000 ""MyPermSet""
  {
      Assignable = true;
      Permissions = tabledata Customer = RIMD,
                    tabledata ""Sales Header"" = R,
                    tabledata ""Sales Line"" = R;
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a permissionset with tabledata permissions should parse without errors");
      }

      [Fact]
      public void Permissionset_with_object_type_permissions_parses_without_errors()
      {
          var source = @"
  permissionset 50000 ""MyPermSet""
  {
      Permissions = codeunit ""My Codeunit"" = X,
                    page ""My Page"" = X,
                    report * = X;
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a permissionset with codeunit/page/report object permissions and wildcard should parse without errors");
      }

      [Fact]
      public void Permissionset_with_included_sets_parses_without_errors()
      {
          var source = @"
  permissionset 50000 ""MyPermSet""
  {
      IncludedPermissionSets = ""BaseSet"", ""AnotherSet"";
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a permissionset with IncludedPermissionSets property should parse without errors");
      }
  }
  ```

- [ ] **Step 3: Update `ALParser.g4`**

  Replace the import list and `alUnit` with:
  ```antlr
  import ALCodeunitParser,
         ALTableParser, ALTableExtensionParser,
         ALPageParser, ALPageExtensionParser, ALPageCustomizationParser,
         ALQueryParser,
         ALReportParser, ALReportExtensionParser,
         ALXmlPortParser,
         ALEnumParser, ALEnumExtensionParser,
         ALInterfaceParser,
         ALProfileParser, ALProfileExtensionParser,
         ALPermissionSetParser;
  ```
  ```antlr
  alUnit
     : codeunitDeclaration
     | table | tableExtension
     | page | pageextension | pagecustomization
     | query | report | reportextension | xmlPort
     | enum | enumextension
     | interface
     | profile | profileextension
     | permissionset
     ;
  ```

- [ ] **Step 4: Build**

  ```
  dotnet build "D:\Projects\AL Langauge Tools\Grammar.AL.Antlr\Grammar.AL.Antlr.csproj"
  ```
  Expected: Build succeeded, 0 Error(s).

- [ ] **Step 5: Run permissionset tests**

  ```
  dotnet test "D:\Projects\AL Langauge Tools\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" --filter "FullyQualifiedName~PermissionSetParserTests" -v n
  ```
  Expected: 5 tests pass.

- [ ] **Step 6: Commit**

  ```
  git add "Grammar.AL.Antlr/ALPermissionSetParser.g4" "ANTLR AL Parser Tests/PermissionSetParserTests.cs" "Grammar.AL.Antlr/ALParser.g4"
  git commit -m "feat: add ALPermissionSetParser grammar and permissionset tests"
  ```

---

### Task 12: `permissionsetextension` parser + tests

**Files:**
- Create: `Grammar.AL.Antlr/ALPermissionSetExtensionParser.g4`
- Modify: `ANTLR AL Parser Tests/PermissionSetParserTests.cs` (add permissionsetextension tests)
- Modify: `Grammar.AL.Antlr/ALParser.g4`

- [ ] **Step 1: Create `ALPermissionSetExtensionParser.g4`**

  Inherits `permissionSetProperty` (and thus `permissionsProperty` and `includedPermissionSetsProperty`) from `ALPermissionSetParser`.
  ```antlr
  parser grammar ALPermissionSetExtensionParser;

  options { tokenVocab=ALLexer; }

  import ALPermissionSetParser;

  permissionsetextension
      : namespaceDeclaration? usingDeclarations?
        PERMISSIONSETEXTENSION INTEGER_LITERAL identifier EXTENDS identifier
        LEFTCBRACE permissionSetProperty* RIGHTCBRACE
      ;
  ```

- [ ] **Step 2: Add `permissionsetextension` tests to `PermissionSetParserTests.cs`**

  Append:
  ```csharp
      [Fact]
      public void Minimal_permissionsetextension_parses_without_errors()
      {
          var source = @"
  permissionsetextension 50000 ""MyPermSetExt"" extends ""MyPermSet""
  {
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a minimal permissionsetextension should parse without errors");
      }

      [Fact]
      public void Minimal_permissionsetextension_produces_PermissionsetextensionContext()
      {
          var source = @"
  permissionsetextension 50000 ""MyPermSetExt"" extends ""MyPermSet""
  {
  }";
          var (tree, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty();
          ParseTreeSearch.FindFirst<ALParser.PermissionsetextensionContext>(tree)
              .Should().NotBeNull("a permissionsetextension object should produce a PermissionsetextensionContext node");
      }

      [Fact]
      public void Permissionsetextension_with_permissions_parses_without_errors()
      {
          var source = @"
  permissionsetextension 50000 ""MyPermSetExt"" extends ""MyPermSet""
  {
      Permissions = tabledata ""Sales Invoice Header"" = R,
                    codeunit ""My Codeunit"" = X;
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a permissionsetextension with permissions should parse without errors");
      }
  ```

- [ ] **Step 3: Update `ALParser.g4`**

  Replace the import list and `alUnit` with:
  ```antlr
  import ALCodeunitParser,
         ALTableParser, ALTableExtensionParser,
         ALPageParser, ALPageExtensionParser, ALPageCustomizationParser,
         ALQueryParser,
         ALReportParser, ALReportExtensionParser,
         ALXmlPortParser,
         ALEnumParser, ALEnumExtensionParser,
         ALInterfaceParser,
         ALProfileParser, ALProfileExtensionParser,
         ALPermissionSetParser, ALPermissionSetExtensionParser;
  ```
  ```antlr
  alUnit
     : codeunitDeclaration
     | table | tableExtension
     | page | pageextension | pagecustomization
     | query | report | reportextension | xmlPort
     | enum | enumextension
     | interface
     | profile | profileextension
     | permissionset | permissionsetextension
     ;
  ```

- [ ] **Step 4: Build + test**

  ```
  dotnet build "D:\Projects\AL Langauge Tools\Grammar.AL.Antlr\Grammar.AL.Antlr.csproj"
  dotnet test "D:\Projects\AL Langauge Tools\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" --filter "FullyQualifiedName~PermissionSetParserTests" -v n
  ```
  Expected: 8 tests pass.

- [ ] **Step 5: Commit**

  ```
  git add "Grammar.AL.Antlr/ALPermissionSetExtensionParser.g4" "ANTLR AL Parser Tests/PermissionSetParserTests.cs" "Grammar.AL.Antlr/ALParser.g4"
  git commit -m "feat: add ALPermissionSetExtensionParser grammar and permissionsetextension tests"
  ```

---

## Chunk 4: Entitlement + DotNet + ControlAddIn + Batch Tests

### Task 13: `entitlement` parser + tests

**Files:**
- Create: `Grammar.AL.Antlr/ALEntitlementParser.g4`
- Create: `ANTLR AL Parser Tests/EntitlementParserTests.cs`
- Modify: `Grammar.AL.Antlr/ALParser.g4`

- [ ] **Step 1: Create `ALEntitlementParser.g4`**

  `ObjectEntitlements` takes a comma-separated identifier list — distinct from `keyValueProperty`'s single value.
  ```antlr
  parser grammar ALEntitlementParser;

  options { tokenVocab=ALLexer; }

  import ALPropertyParser;

  objectEntitlementsProperty
      : {TokenMatches("ObjectEntitlements")}? identifier EQUAL identifierList SEMICOLON
      ;

  entitlementProperty
      : objectEntitlementsProperty
      | keyValueProperty     // Type, Id, RoleType
      ;

  entitlement
      : namespaceDeclaration? usingDeclarations?
        ENTITLEMENT identifier
        LEFTCBRACE entitlementProperty* RIGHTCBRACE
      ;
  ```

- [ ] **Step 2: Create `ANTLR AL Parser Tests/EntitlementParserTests.cs`**

  ```csharp
  using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

  namespace Org.Edgerunner.Language.AL.Grammar.Tests;

  /// <summary>
  /// Verifies structural parsing of AL entitlement objects.
  /// </summary>
  public class EntitlementParserTests
  {
      [Fact]
      public void Minimal_entitlement_parses_without_errors()
      {
          var source = @"
  entitlement MyEntitlement
  {
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a minimal entitlement with no properties should parse without errors");
      }

      [Fact]
      public void Minimal_entitlement_produces_EntitlementContext()
      {
          var source = @"
  entitlement MyEntitlement
  {
  }";
          var (tree, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty();
          ParseTreeSearch.FindFirst<ALParser.EntitlementContext>(tree)
              .Should().NotBeNull("an entitlement object should produce an EntitlementContext node");
      }

      [Fact]
      public void Entitlement_with_role_properties_parses_without_errors()
      {
          var source = @"
  entitlement MyEntitlement
  {
      Type = Role;
      RoleType = Local;
      Id = '{00000000-0000-0000-0000-000000000001}';
      ObjectEntitlements = ""My Permission Set"", ""Another Permission Set"";
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("an entitlement with Type, RoleType, Id, and ObjectEntitlements should parse without errors");
      }

      [Fact]
      public void Entitlement_with_application_service_type_parses_without_errors()
      {
          var source = @"
  entitlement MyEntitlement
  {
      Type = ApplicationService;
      Id = 'my-app-service';
      ObjectEntitlements = ""D365 BASIC"";
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("an entitlement with ApplicationService type should parse without errors");
      }
  }
  ```

- [ ] **Step 3: Update `ALParser.g4`**

  Replace the import list and `alUnit` with:
  ```antlr
  import ALCodeunitParser,
         ALTableParser, ALTableExtensionParser,
         ALPageParser, ALPageExtensionParser, ALPageCustomizationParser,
         ALQueryParser,
         ALReportParser, ALReportExtensionParser,
         ALXmlPortParser,
         ALEnumParser, ALEnumExtensionParser,
         ALInterfaceParser,
         ALProfileParser, ALProfileExtensionParser,
         ALPermissionSetParser, ALPermissionSetExtensionParser,
         ALEntitlementParser;
  ```
  ```antlr
  alUnit
     : codeunitDeclaration
     | table | tableExtension
     | page | pageextension | pagecustomization
     | query | report | reportextension | xmlPort
     | enum | enumextension
     | interface
     | profile | profileextension
     | permissionset | permissionsetextension
     | entitlement
     ;
  ```

- [ ] **Step 4: Build + test**

  ```
  dotnet build "D:\Projects\AL Langauge Tools\Grammar.AL.Antlr\Grammar.AL.Antlr.csproj"
  dotnet test "D:\Projects\AL Langauge Tools\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" --filter "FullyQualifiedName~EntitlementParserTests" -v n
  ```
  Expected: 4 tests pass.

- [ ] **Step 5: Commit**

  ```
  git add "Grammar.AL.Antlr/ALEntitlementParser.g4" "ANTLR AL Parser Tests/EntitlementParserTests.cs" "Grammar.AL.Antlr/ALParser.g4"
  git commit -m "feat: add ALEntitlementParser grammar and EntitlementParserTests"
  ```

---

### Task 14: `dotnet` parser + tests

**Files:**
- Create: `Grammar.AL.Antlr/ALDotNetParser.g4`
- Create: `ANTLR AL Parser Tests/DotNetParserTests.cs`
- Modify: `Grammar.AL.Antlr/ALParser.g4`

- [ ] **Step 1: Create `ALDotNetParser.g4`**

  The `dotnet` object has no ID or name — it is a file-level container. `DOTNET` is a dedicated token. `assembly` and `type` are soft keywords (matched by `IDENTIFIER` with predicates — there is no `ASSEMBLY` token; only `DOTNETASSEMBLY` exists which matches the keyword `dotnetassembly`). Assembly and type names can be quoted string literals or dot-separated identifier chains.
  ```antlr
  parser grammar ALDotNetParser;

  options { tokenVocab=ALLexer; }

  import ALPropertyParser;

  dotNetName
      : STRING_LITERAL
      | identifier (PERIOD identifier)*
      ;

  dotNetType
      : {TokenMatches("type")}? IDENTIFIER
        LEFTPAREN dotNetName SEMICOLON dotNetName RIGHTPAREN
        LEFTCBRACE keyValueProperty*? RIGHTCBRACE
      ;

  dotNetAssembly
      : {TokenMatches("assembly")}? IDENTIFIER
        LEFTPAREN dotNetName RIGHTPAREN
        LEFTCBRACE keyValueProperty*? dotNetType* RIGHTCBRACE
      ;

  dotnet
      : namespaceDeclaration? usingDeclarations?
        DOTNET
        LEFTCBRACE dotNetAssembly* RIGHTCBRACE
      ;
  ```

- [ ] **Step 2: Create `ANTLR AL Parser Tests/DotNetParserTests.cs`**

  ```csharp
  using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

  namespace Org.Edgerunner.Language.AL.Grammar.Tests;

  /// <summary>
  /// Verifies structural parsing of AL dotnet objects.
  /// </summary>
  public class DotNetParserTests
  {
      [Fact]
      public void Minimal_dotnet_parses_without_errors()
      {
          var source = @"
  dotnet
  {
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a minimal dotnet container with no assemblies should parse without errors");
      }

      [Fact]
      public void Minimal_dotnet_produces_DotnetContext()
      {
          var source = @"
  dotnet
  {
  }";
          var (tree, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty();
          ParseTreeSearch.FindFirst<ALParser.DotnetContext>(tree)
              .Should().NotBeNull("a dotnet object should produce a DotnetContext node");
      }

      [Fact]
      public void Dotnet_with_assembly_and_types_parses_without_errors()
      {
          var source = @"
  dotnet
  {
      assembly(""mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"")
      {
          type(System.Collections.Generic.List; List)
          {
              IsTypeDefinition = true;
          }
          type(System.Collections.Generic.Dictionary; Dictionary) { }
      }
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a dotnet with assembly and types using quoted names should parse without errors");
      }

      [Fact]
      public void Dotnet_with_dotted_identifier_names_parses_without_errors()
      {
          var source = @"
  dotnet
  {
      assembly(Microsoft.Dynamics.Nav.MX)
      {
          type(Microsoft.Dynamics.Nav.MX.QRCodeGenerator; QRCodeGenerator)
          {
              IsTypeDefinition = true;
          }
      }
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a dotnet with dotted identifier assembly and type names should parse without errors");
      }

      [Fact]
      public void Dotnet_with_multiple_assemblies_parses_without_errors()
      {
          var source = @"
  dotnet
  {
      assembly(""Assembly.One"")
      {
          AliasPrefix = 'Prefix1';
          type(""Assembly.One.TypeA""; TypeA) { }
      }
      assembly(""Assembly.Two"")
      {
          AliasPrefix = 'Prefix2';
          type(""Assembly.Two.TypeB""; TypeB) { }
      }
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a dotnet with multiple assemblies should parse without errors");
      }
  }
  ```

- [ ] **Step 3: Update `ALParser.g4`**

  Replace the import list and `alUnit` with:
  ```antlr
  import ALCodeunitParser,
         ALTableParser, ALTableExtensionParser,
         ALPageParser, ALPageExtensionParser, ALPageCustomizationParser,
         ALQueryParser,
         ALReportParser, ALReportExtensionParser,
         ALXmlPortParser,
         ALEnumParser, ALEnumExtensionParser,
         ALInterfaceParser,
         ALProfileParser, ALProfileExtensionParser,
         ALPermissionSetParser, ALPermissionSetExtensionParser,
         ALEntitlementParser,
         ALDotNetParser;
  ```
  ```antlr
  alUnit
     : codeunitDeclaration
     | table | tableExtension
     | page | pageextension | pagecustomization
     | query | report | reportextension | xmlPort
     | enum | enumextension
     | interface
     | profile | profileextension
     | permissionset | permissionsetextension
     | entitlement
     | dotnet
     ;
  ```

- [ ] **Step 4: Build + test**

  ```
  dotnet build "D:\Projects\AL Langauge Tools\Grammar.AL.Antlr\Grammar.AL.Antlr.csproj"
  dotnet test "D:\Projects\AL Langauge Tools\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" --filter "FullyQualifiedName~DotNetParserTests" -v n
  ```
  Expected: 5 tests pass.

- [ ] **Step 5: Commit**

  ```
  git add "Grammar.AL.Antlr/ALDotNetParser.g4" "ANTLR AL Parser Tests/DotNetParserTests.cs" "Grammar.AL.Antlr/ALParser.g4"
  git commit -m "feat: add ALDotNetParser grammar and DotNetParserTests"
  ```

---

### Task 15: `controladdin` parser + tests

**Files:**
- Create: `Grammar.AL.Antlr/ALControlAddInParser.g4`
- Create: `ANTLR AL Parser Tests/ControlAddInParserTests.cs`
- Modify: `Grammar.AL.Antlr/ALParser.g4`

- [ ] **Step 1: Create `ALControlAddInParser.g4`**

  Events and procedures have signatures but no body. The terminating semicolon is optional. `EVENT` and `PROCEDURE` are dedicated tokens. Imports `ALPropertyParser` (not just `ALCodeParser`) to make `keyValueProperty` available; `ALPropertyParser` transitively imports `ALCodeParser`, so `parameterList` and `returnValue` are accessible.
  ```antlr
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
  ```

- [ ] **Step 2: Create `ANTLR AL Parser Tests/ControlAddInParserTests.cs`**

  ```csharp
  using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

  namespace Org.Edgerunner.Language.AL.Grammar.Tests;

  /// <summary>
  /// Verifies structural parsing of AL controladdin objects.
  /// </summary>
  public class ControlAddInParserTests
  {
      [Fact]
      public void Minimal_controladdin_parses_without_errors()
      {
          var source = @"
  controladdin MyControlAddIn
  {
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a minimal controladdin with no members should parse without errors");
      }

      [Fact]
      public void Minimal_controladdin_produces_ControladdinContext()
      {
          var source = @"
  controladdin MyControlAddIn
  {
  }";
          var (tree, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty();
          ParseTreeSearch.FindFirst<ALParser.ControladdinContext>(tree)
              .Should().NotBeNull("a controladdin object should produce a ControladdinContext node");
      }

      [Fact]
      public void Controladdin_with_properties_and_scripts_parses_without_errors()
      {
          var source = @"
  controladdin MyControlAddIn
  {
      RequestedHeight = 300;
      RequestedWidth = 700;
      MinimumHeight = 100;
      MinimumWidth = 300;
      HorizontalStretch = true;
      VerticalStretch = true;
      Scripts = 'Scripts/MyScript.js';
      StyleSheets = 'Stylesheets/MyStyle.css';
      StartupScript = 'Scripts/startup.js';
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a controladdin with key/value properties should parse without errors");
      }

      [Fact]
      public void Controladdin_with_events_parses_without_errors()
      {
          var source = @"
  controladdin MyControlAddIn
  {
      event ControlReady()
      event ValueChanged(NewValue: Text)
      event SelectionChanged(SelectedIds: Text; Count: Integer)
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a controladdin with event declarations (no semicolons) should parse without errors");
      }

      [Fact]
      public void Controladdin_with_events_and_semicolons_parses_without_errors()
      {
          var source = @"
  controladdin MyControlAddIn
  {
      event ControlReady();
      event ValueChanged(NewValue: Text);
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a controladdin with event declarations (with semicolons) should parse without errors");
      }

      [Fact]
      public void Controladdin_with_procedures_parses_without_errors()
      {
          var source = @"
  controladdin MyControlAddIn
  {
      procedure SetValue(NewValue: Text)
      procedure GetValue(): Text
      procedure Initialize(Data: JsonObject; Width: Integer);
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a controladdin with procedure declarations should parse without errors");
      }

      [Fact]
      public void Controladdin_with_mixed_members_parses_without_errors()
      {
          var source = @"
  controladdin MyControlAddIn
  {
      RequestedHeight = 300;
      Scripts = 'js/control.js';
      event ControlReady()
      event ValueChanged(Value: Text)
      procedure SetValue(Value: Text)
      procedure GetValue(): Text
  }";
          var (_, errors) = ALParseHelper.ParseAlUnit(source);
          errors.Should().BeEmpty("a controladdin with mixed properties, events, and procedures should parse without errors");
      }
  }
  ```

- [ ] **Step 3: Update `ALParser.g4`**

  Add `ALControlAddInParser` to imports and `controladdin` to `alUnit`. This completes the full wiring. The final `ALParser.g4` import list and `alUnit` should be:
  ```antlr
  // ALPageParser is kept explicit here: it defines the `page` rule used in alUnit below.
  // It also arrives transitively through ALReportParser, ALXmlPortParser, ALPageExtensionParser,
  // ALPageCustomizationParser, and ALReportExtensionParser — ANTLR4 deduplicates imported rules.
  // ALTableParser is kept explicit here: it defines the `table` rule used in alUnit below.
  // It also arrives transitively through ALTableExtensionParser.
  import ALCodeunitParser,
         ALTableParser, ALTableExtensionParser,
         ALPageParser, ALPageExtensionParser, ALPageCustomizationParser,
         ALQueryParser,
         ALReportParser, ALReportExtensionParser,
         ALXmlPortParser,
         ALEnumParser, ALEnumExtensionParser,
         ALInterfaceParser,
         ALProfileParser, ALProfileExtensionParser,
         ALPermissionSetParser, ALPermissionSetExtensionParser,
         ALEntitlementParser,
         ALDotNetParser,
         ALControlAddInParser;

  alUnit
     : codeunitDeclaration
     | table           | tableExtension
     | page            | pageextension      | pagecustomization
     | query
     | report          | reportextension
     | xmlPort
     | enum            | enumextension
     | interface
     | profile         | profileextension
     | permissionset   | permissionsetextension
     | entitlement
     | dotnet
     | controladdin
     ;
  ```

- [ ] **Step 4: Build**

  ```
  dotnet build "D:\Projects\AL Langauge Tools\Grammar.AL.Antlr\Grammar.AL.Antlr.csproj"
  ```
  Expected: Build succeeded, 0 Error(s).

- [ ] **Step 5: Run controladdin tests and full test suite**

  ```
  dotnet test "D:\Projects\AL Langauge Tools\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" -v n
  ```
  Expected: All tests pass. `ControlAddInParserTests` (7 tests) pass alongside all prior tests.

- [ ] **Step 6: Commit**

  ```
  git add "Grammar.AL.Antlr/ALControlAddInParser.g4" "ANTLR AL Parser Tests/ControlAddInParserTests.cs" "Grammar.AL.Antlr/ALParser.g4"
  git commit -m "feat: add ALControlAddInParser grammar, ControlAddInParserTests, finalize ALParser.g4 wiring"
  ```

---

### Task 16: Real-file batch tests

Add `[Theory]`/`[MemberData]` tests to each test class to validate against up to 40 real BC26 source files per type. These tests skip gracefully if the source directory does not exist.

**Files:**
- Modify: all test files created in Tasks 2–15

For each test class, add the following static helper method and `[Theory]` test. Replace `<GlobPattern>`, `<BaseDir>`, `<TypeDescription>`, and the filter as appropriate per the table below.

**Pattern for each batch test:**
```csharp
public static IEnumerable<object[]> <TypeName>Files()
{
    var root = @"<BaseDir>";
    if (!Directory.Exists(root))
        return Enumerable.Empty<object[]>();
    return Directory.GetFiles(root, "<GlobPattern>", SearchOption.AllDirectories)
        .Take(40)
        .Select(f => new object[] { f });
}

[Theory]
[MemberData(nameof(<TypeName>Files))]
public void Real_<typename>_file_parses_without_errors(string filePath)
{
    var source = File.ReadAllText(filePath);
    var (_, errors) = ALParseHelper.ParseAlUnit(source);
    errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
}
```

**Batch test configuration per type:**

| Test class | Method name | Base dir | Pattern |
|---|---|---|---|
| `EnumParserTests` | `EnumFiles` / `EnumExtFiles` | `D:\Projects\Business Central\Applications\Microsoft\BC26` | `*.Enum.al` / `*.EnumExt.al` |
| `InterfaceParserTests` | `InterfaceFiles` | same BC26 root | `*.Interface.al` |
| `TableExtensionParserTests` | `TableExtFiles` | same BC26 root | `*.TableExt.al` |
| `PageExtensionParserTests` | `PageExtFiles` | same BC26 root | `*.PageExt.al` |
| `PageCustomizationParserTests` | `PageCustFiles` | same BC26 root | `*.PageCust.al` |
| `ReportExtensionParserTests` | `ReportExtFiles` | same BC26 root | `*.ReportExt.al` |
| `ProfileParserTests` | `ProfileFiles` / `ProfileExtFiles` | same BC26 root | `*.Profile.al` / `*.ProfileExt.al` |
| `PermissionSetParserTests` | `PermSetFiles` / `PermSetExtFiles` | same BC26 root | `*.PermissionSet.al` / `*.PermissionSetExt.al` |
| `EntitlementParserTests` | `EntitlementFiles` | same BC26 root | `*.Entitlement.al` |
| `DotNetParserTests` | `DotNetFiles` | same BC26 root | `dotnet*.al` |
| `ControlAddInParserTests` | `ControlAddInFiles` | `D:\Projects\Business Central\Applications\Lanham\BC26\Lanham Associates_Lanham ACE Warehousing_25.2.1.5_symbols\Source\Control Addins` | `*.al` |

- [ ] **Step 1: Add batch tests to `EnumParserTests.cs`**

  Add at the bottom of the class (inside the class body, after the last inline test):
  ```csharp
  public static IEnumerable<object[]> EnumFiles()
  {
      var root = @"D:\Projects\Business Central\Applications\Microsoft\BC26";
      if (!Directory.Exists(root))
          return Enumerable.Empty<object[]>();
      return Directory.GetFiles(root, "*.Enum.al", SearchOption.AllDirectories)
          .Take(40).Select(f => new object[] { f });
  }

  [Theory]
  [MemberData(nameof(EnumFiles))]
  public void Real_enum_file_parses_without_errors(string filePath)
  {
      var source = File.ReadAllText(filePath);
      var (_, errors) = ALParseHelper.ParseAlUnit(source);
      errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
  }

  public static IEnumerable<object[]> EnumExtFiles()
  {
      var root = @"D:\Projects\Business Central\Applications\Microsoft\BC26";
      if (!Directory.Exists(root))
          return Enumerable.Empty<object[]>();
      return Directory.GetFiles(root, "*.EnumExt.al", SearchOption.AllDirectories)
          .Take(40).Select(f => new object[] { f });
  }

  [Theory]
  [MemberData(nameof(EnumExtFiles))]
  public void Real_enumextension_file_parses_without_errors(string filePath)
  {
      var source = File.ReadAllText(filePath);
      var (_, errors) = ALParseHelper.ParseAlUnit(source);
      errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
  }
  ```

- [ ] **Step 2: Add batch tests to remaining test files**

  Note: The test project uses `<ImplicitUsings>enable</ImplicitUsings>` targeting `net6.0`, so `System.IO`, `System.Linq`, and `System.Collections.Generic` are already available without explicit `using` directives.

  For each file below, insert the methods before the final closing `}` of the class.

  **`InterfaceParserTests.cs`** — insert before closing `}`:
  ```csharp
  public static IEnumerable<object[]> InterfaceFiles()
  {
      var root = @"D:\Projects\Business Central\Applications\Microsoft\BC26";
      if (!Directory.Exists(root))
          return Enumerable.Empty<object[]>();
      return Directory.GetFiles(root, "*.Interface.al", SearchOption.AllDirectories)
          .Take(40).Select(f => new object[] { f });
  }

  [Theory]
  [MemberData(nameof(InterfaceFiles))]
  public void Real_interface_file_parses_without_errors(string filePath)
  {
      var source = File.ReadAllText(filePath);
      var (_, errors) = ALParseHelper.ParseAlUnit(source);
      errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
  }
  ```

  **`TableExtensionParserTests.cs`** — insert before closing `}`:
  ```csharp
  public static IEnumerable<object[]> TableExtFiles()
  {
      var root = @"D:\Projects\Business Central\Applications\Microsoft\BC26";
      if (!Directory.Exists(root))
          return Enumerable.Empty<object[]>();
      return Directory.GetFiles(root, "*.TableExt.al", SearchOption.AllDirectories)
          .Take(40).Select(f => new object[] { f });
  }

  [Theory]
  [MemberData(nameof(TableExtFiles))]
  public void Real_tableextension_file_parses_without_errors(string filePath)
  {
      var source = File.ReadAllText(filePath);
      var (_, errors) = ALParseHelper.ParseAlUnit(source);
      errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
  }
  ```

  **`PageExtensionParserTests.cs`** — insert before closing `}`:
  ```csharp
  public static IEnumerable<object[]> PageExtFiles()
  {
      var root = @"D:\Projects\Business Central\Applications\Microsoft\BC26";
      if (!Directory.Exists(root))
          return Enumerable.Empty<object[]>();
      return Directory.GetFiles(root, "*.PageExt.al", SearchOption.AllDirectories)
          .Take(40).Select(f => new object[] { f });
  }

  [Theory]
  [MemberData(nameof(PageExtFiles))]
  public void Real_pageextension_file_parses_without_errors(string filePath)
  {
      var source = File.ReadAllText(filePath);
      var (_, errors) = ALParseHelper.ParseAlUnit(source);
      errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
  }
  ```

  **`PageCustomizationParserTests.cs`** — insert before closing `}`:
  ```csharp
  public static IEnumerable<object[]> PageCustFiles()
  {
      var root = @"D:\Projects\Business Central\Applications\Microsoft\BC26";
      if (!Directory.Exists(root))
          return Enumerable.Empty<object[]>();
      return Directory.GetFiles(root, "*.PageCust.al", SearchOption.AllDirectories)
          .Take(40).Select(f => new object[] { f });
  }

  [Theory]
  [MemberData(nameof(PageCustFiles))]
  public void Real_pagecustomization_file_parses_without_errors(string filePath)
  {
      var source = File.ReadAllText(filePath);
      var (_, errors) = ALParseHelper.ParseAlUnit(source);
      errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
  }
  ```

  **`ReportExtensionParserTests.cs`** — insert before closing `}`:
  ```csharp
  public static IEnumerable<object[]> ReportExtFiles()
  {
      var root = @"D:\Projects\Business Central\Applications\Microsoft\BC26";
      if (!Directory.Exists(root))
          return Enumerable.Empty<object[]>();
      return Directory.GetFiles(root, "*.ReportExt.al", SearchOption.AllDirectories)
          .Take(40).Select(f => new object[] { f });
  }

  [Theory]
  [MemberData(nameof(ReportExtFiles))]
  public void Real_reportextension_file_parses_without_errors(string filePath)
  {
      var source = File.ReadAllText(filePath);
      var (_, errors) = ALParseHelper.ParseAlUnit(source);
      errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
  }
  ```

  **`ProfileParserTests.cs`** — insert before closing `}`:
  ```csharp
  public static IEnumerable<object[]> ProfileFiles()
  {
      var root = @"D:\Projects\Business Central\Applications\Microsoft\BC26";
      if (!Directory.Exists(root))
          return Enumerable.Empty<object[]>();
      return Directory.GetFiles(root, "*.Profile.al", SearchOption.AllDirectories)
          .Take(40).Select(f => new object[] { f });
  }

  [Theory]
  [MemberData(nameof(ProfileFiles))]
  public void Real_profile_file_parses_without_errors(string filePath)
  {
      var source = File.ReadAllText(filePath);
      var (_, errors) = ALParseHelper.ParseAlUnit(source);
      errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
  }

  public static IEnumerable<object[]> ProfileExtFiles()
  {
      var root = @"D:\Projects\Business Central\Applications\Microsoft\BC26";
      if (!Directory.Exists(root))
          return Enumerable.Empty<object[]>();
      return Directory.GetFiles(root, "*.ProfileExt.al", SearchOption.AllDirectories)
          .Take(40).Select(f => new object[] { f });
  }

  [Theory]
  [MemberData(nameof(ProfileExtFiles))]
  public void Real_profileextension_file_parses_without_errors(string filePath)
  {
      var source = File.ReadAllText(filePath);
      var (_, errors) = ALParseHelper.ParseAlUnit(source);
      errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
  }
  ```

  **`PermissionSetParserTests.cs`** — insert before closing `}`:
  ```csharp
  public static IEnumerable<object[]> PermSetFiles()
  {
      var root = @"D:\Projects\Business Central\Applications\Microsoft\BC26";
      if (!Directory.Exists(root))
          return Enumerable.Empty<object[]>();
      return Directory.GetFiles(root, "*.PermissionSet.al", SearchOption.AllDirectories)
          .Take(40).Select(f => new object[] { f });
  }

  [Theory]
  [MemberData(nameof(PermSetFiles))]
  public void Real_permissionset_file_parses_without_errors(string filePath)
  {
      var source = File.ReadAllText(filePath);
      var (_, errors) = ALParseHelper.ParseAlUnit(source);
      errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
  }

  public static IEnumerable<object[]> PermSetExtFiles()
  {
      var root = @"D:\Projects\Business Central\Applications\Microsoft\BC26";
      if (!Directory.Exists(root))
          return Enumerable.Empty<object[]>();
      return Directory.GetFiles(root, "*.PermissionSetExt.al", SearchOption.AllDirectories)
          .Take(40).Select(f => new object[] { f });
  }

  [Theory]
  [MemberData(nameof(PermSetExtFiles))]
  public void Real_permissionsetextension_file_parses_without_errors(string filePath)
  {
      var source = File.ReadAllText(filePath);
      var (_, errors) = ALParseHelper.ParseAlUnit(source);
      errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
  }
  ```

  **`EntitlementParserTests.cs`** — insert before closing `}`:
  ```csharp
  public static IEnumerable<object[]> EntitlementFiles()
  {
      var root = @"D:\Projects\Business Central\Applications\Microsoft\BC26";
      if (!Directory.Exists(root))
          return Enumerable.Empty<object[]>();
      return Directory.GetFiles(root, "*.Entitlement.al", SearchOption.AllDirectories)
          .Take(40).Select(f => new object[] { f });
  }

  [Theory]
  [MemberData(nameof(EntitlementFiles))]
  public void Real_entitlement_file_parses_without_errors(string filePath)
  {
      var source = File.ReadAllText(filePath);
      var (_, errors) = ALParseHelper.ParseAlUnit(source);
      errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
  }
  ```

  **`DotNetParserTests.cs`** — insert before closing `}`:
  ```csharp
  public static IEnumerable<object[]> DotNetFiles()
  {
      var root = @"D:\Projects\Business Central\Applications\Microsoft\BC26";
      if (!Directory.Exists(root))
          return Enumerable.Empty<object[]>();
      return Directory.GetFiles(root, "dotnet*.al", SearchOption.AllDirectories)
          .Take(40).Select(f => new object[] { f });
  }

  [Theory]
  [MemberData(nameof(DotNetFiles))]
  public void Real_dotnet_file_parses_without_errors(string filePath)
  {
      var source = File.ReadAllText(filePath);
      var (_, errors) = ALParseHelper.ParseAlUnit(source);
      errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
  }
  ```

  **`ControlAddInParserTests.cs`** — insert before closing `}`:
  ```csharp
  public static IEnumerable<object[]> ControlAddInFiles()
  {
      var root = @"D:\Projects\Business Central\Applications\Lanham\BC26\Lanham Associates_Lanham ACE Warehousing_25.2.1.5_symbols\Source\Control Addins";
      if (!Directory.Exists(root))
          return Enumerable.Empty<object[]>();
      return Directory.GetFiles(root, "*.al", SearchOption.AllDirectories)
          .Take(40).Select(f => new object[] { f });
  }

  [Theory]
  [MemberData(nameof(ControlAddInFiles))]
  public void Real_controladdin_file_parses_without_errors(string filePath)
  {
      var source = File.ReadAllText(filePath);
      var (_, errors) = ALParseHelper.ParseAlUnit(source);
      errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
  }
  ```

- [ ] **Step 3: Build**

  ```
  dotnet build "D:\Projects\AL Langauge Tools\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj"
  ```
  Expected: Build succeeded, 0 Error(s).

- [ ] **Step 4: Run all batch tests**

  ```
  dotnet test "D:\Projects\AL Langauge Tools\ANTLR AL Parser Tests\ANTLR AL Parser Tests.csproj" -v n
  ```
  Expected: All inline tests pass. Batch tests run only if the BC26/Lanham source directories exist and are populated; if the directories are absent, the `[Theory]` methods produce 0 test cases and are silently skipped. For any batch test failure, the error message will include the filename — use that to investigate the specific real-world AL construct that is not yet handled.

- [ ] **Step 5: Fix any batch test failures**

  If a batch test fails, examine the failing file to identify which construct the grammar does not handle. Apply the minimal grammar fix needed. Re-run tests to confirm the fix resolves the failure without regressing other tests. Commit the fix separately with a message describing the construct that was fixed.

- [ ] **Step 6: Commit batch tests**

  ```
  git add "ANTLR AL Parser Tests/"
  git commit -m "test: add real-file batch tests for all 14 new AL object types"
  ```
