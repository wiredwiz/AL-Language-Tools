# Expression Rule Operator Precedence Fix — Implementation Plan

> **For agentic workers:** REQUIRED: Use superpowers:subagent-driven-development (if subagents available) or superpowers:executing-plans to implement this plan. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Fix operator precedence in `ALCodeParser.g4` so that expressions like `2 + 3 * 4` parse correctly, extract assignment into a dedicated statement rule, and update `ALValidator.cs` to keep the build green.

**Architecture:** The ANTLR4 `Antlr4` NuGet package (v4.6.6) auto-generates C# parser code from `.g4` files during `dotnet build` — no manual ANTLR tool invocation is needed. Grammar changes take effect on the next build. A new xUnit test project tests the ANTLR-based parser end-to-end by parsing AL source snippets and inspecting the resulting parse tree.

**Tech Stack:** C# / .NET Standard 2.0, ANTLR4 (Antlr4 + Antlr4.Runtime v4.6.6), xUnit 2.4.2, FluentAssertions 6.12.0

**Spec:** `Grammar.AL.Antlr/docs/superpowers/specs/2026-03-16-expression-rule-precedence-fix-design.md`

---

## File Map

| File | Action | Responsibility |
|------|--------|----------------|
| `Grammar.AL.Antlr/ALCodeParser.g4` | Modify | Grammar rule changes (expression precedence, assignmentStatement) |
| `ANTLR AL Parsing/Validators/ALValidator.cs` | Modify | Fix compile break + silent regression from label renames |
| `ANTLR AL Parser Tests/ANTLR AL Parser Tests.csproj` | Create | New xUnit test project for ANTLR-based parser |
| `ANTLR AL Parser Tests/GlobalUsings.cs` | Create | Global usings for test project |
| `ANTLR AL Parser Tests/Helpers/ALParseHelper.cs` | Create | Wraps AL source in a codeunit shell for parsing; returns errors + tree |
| `ANTLR AL Parser Tests/Helpers/ParseTreeSearch.cs` | Create | Finds first/all nodes of a given context type in an ANTLR parse tree |
| `ANTLR AL Parser Tests/ExpressionPrecedenceTests.cs` | Create | Tests that verify correct operator precedence and assignment-as-statement |

---

## Chunk 1: Test Project + Failing Tests

### Task 1: Create the ANTLR Parser test project

**Files:**
- Create: `ANTLR AL Parser Tests/ANTLR AL Parser Tests.csproj`
- Create: `ANTLR AL Parser Tests/GlobalUsings.cs`

- [ ] **Step 1: Create the project file**

Create `D:/Projects/AL Langauge Tools/ANTLR AL Parser Tests/ANTLR AL Parser Tests.csproj`:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <RootNamespace>Org.Edgerunner.Language.AL.Parsing.Tests</RootNamespace>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
    <IsTestProject>true</IsTestProject>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="FluentAssertions" Version="6.12.0" />
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.5.0" />
    <PackageReference Include="xunit" Version="2.4.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.4.5">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
    <PackageReference Include="coverlet.collector" Version="3.2.0">
      <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      <PrivateAssets>all</PrivateAssets>
    </PackageReference>
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\ANTLR AL Parsing\ANTLR AL Parsing.csproj" />
    <ProjectReference Include="..\Grammar.AL.Antlr\Grammar.AL.Antlr.csproj" />
  </ItemGroup>

</Project>
```

- [ ] **Step 2: Create GlobalUsings.cs**

Create `D:/Projects/AL Langauge Tools/ANTLR AL Parser Tests/GlobalUsings.cs`:

```csharp
global using Xunit;
global using FluentAssertions;
global using Antlr4.Runtime.Tree;
global using Org.Edgerunner.Language.AL.Parsing;
global using Org.Edgerunner.Language.AL.Parsing.Messaging;
global using Grammar.AL.Antlr;
```

- [ ] **Step 3: Add project to solution**

```bash
cd "D:/Projects/AL Langauge Tools"
dotnet sln "AL Language Tools.sln" add "ANTLR AL Parser Tests/ANTLR AL Parser Tests.csproj"
```

Expected output: `Project 'ANTLR AL Parser Tests/ANTLR AL Parser Tests.csproj' added to the solution.`

- [ ] **Step 4: Verify project builds (empty)**

```bash
cd "D:/Projects/AL Langauge Tools"
dotnet build "ANTLR AL Parser Tests/ANTLR AL Parser Tests.csproj"
```

Expected: `Build succeeded.`

- [ ] **Step 5: Commit**

```bash
cd "D:/Projects/AL Langauge Tools/Grammar.AL.Antlr"
git add "../ANTLR AL Parser Tests/ANTLR AL Parser Tests.csproj" "../ANTLR AL Parser Tests/GlobalUsings.cs"
git commit -m "Add ANTLR parser test project scaffold"
```

---

### Task 2: Create parse test helpers

**Files:**
- Create: `ANTLR AL Parser Tests/Helpers/ALParseHelper.cs`
- Create: `ANTLR AL Parser Tests/Helpers/ParseTreeSearch.cs`

- [ ] **Step 1: Create ALParseHelper.cs**

Create `D:/Projects/AL Langauge Tools/ANTLR AL Parser Tests/Helpers/ALParseHelper.cs`:

```csharp
using Antlr4.Runtime.Tree;
using Org.Edgerunner.Language.AL.Parsing;
using Org.Edgerunner.Language.AL.Parsing.Messaging;

namespace Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

/// <summary>
/// Wraps AL source snippets in a codeunit shell and parses them via ALParser.
/// </summary>
public static class ALParseHelper
{
    /// <summary>
    /// Wraps the given procedure body statements in a minimal codeunit and parses them.
    /// Returns the parse result and any errors.
    /// </summary>
    public static (ISyntaxTree Tree, IReadOnlyList<ErrorMessage> Errors) ParseProcedureBody(string statements)
    {
        var source = $@"codeunit 50000 ""TestCodeunit""
{{
    procedure TestProc()
    begin
        {statements}
    end;
}}";
        var parser = new ALParser();
        var tree = parser.Parse(source);
        return (tree, parser.Errors);
    }

    /// <summary>
    /// Wraps the given procedure body (with optional var block) and parses.
    /// </summary>
    public static (ISyntaxTree Tree, IReadOnlyList<ErrorMessage> Errors) ParseProcedureBodyWithVars(
        string varDeclarations,
        string statements)
    {
        var source = $@"codeunit 50000 ""TestCodeunit""
{{
    procedure TestProc()
    var
        {varDeclarations}
    begin
        {statements}
    end;
}}";
        var parser = new ALParser();
        var tree = parser.Parse(source);
        return (tree, parser.Errors);
    }
}
```

- [ ] **Step 2: Create ParseTreeSearch.cs**

Create `D:/Projects/AL Langauge Tools/ANTLR AL Parser Tests/Helpers/ParseTreeSearch.cs`:

```csharp
using Antlr4.Runtime.Tree;

namespace Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

/// <summary>
/// Utility for finding specific context types in an ANTLR4 parse tree.
/// </summary>
public static class ParseTreeSearch
{
    /// <summary>
    /// Returns the first node of type T found via depth-first search, or null.
    /// </summary>
    public static T? FindFirst<T>(ITree tree) where T : class
    {
        if (tree is T match)
            return match;

        for (var i = 0; i < tree.ChildCount; i++)
        {
            var found = FindFirst<T>(tree.GetChild(i));
            if (found != null)
                return found;
        }

        return null;
    }

    /// <summary>
    /// Returns all nodes of type T found via depth-first search.
    /// </summary>
    public static IReadOnlyList<T> FindAll<T>(ITree tree) where T : class
    {
        var results = new List<T>();
        CollectAll(tree, results);
        return results;
    }

    private static void CollectAll<T>(ITree tree, List<T> results) where T : class
    {
        if (tree is T match)
            results.Add(match);

        for (var i = 0; i < tree.ChildCount; i++)
            CollectAll(tree.GetChild(i), results);
    }
}
```

- [ ] **Step 3: Verify helpers compile**

```bash
cd "D:/Projects/AL Langauge Tools"
dotnet build "ANTLR AL Parser Tests/ANTLR AL Parser Tests.csproj"
```

Expected: `Build succeeded.`

- [ ] **Step 4: Commit**

```bash
cd "D:/Projects/AL Langauge Tools/Grammar.AL.Antlr"
git add "../ANTLR AL Parser Tests/Helpers/ALParseHelper.cs" "../ANTLR AL Parser Tests/Helpers/ParseTreeSearch.cs"
git commit -m "Add test helpers: ALParseHelper and ParseTreeSearch"
```

---

### Task 3: Write failing expression precedence tests

**Files:**
- Create: `ANTLR AL Parser Tests/ExpressionPrecedenceTests.cs`

These tests assert the CORRECT behavior. They should FAIL against the current grammar. Run them first to confirm they fail, then proceed to the grammar fix.

- [ ] **Step 1: Create ExpressionPrecedenceTests.cs**

Create `D:/Projects/AL Langauge Tools/ANTLR AL Parser Tests/ExpressionPrecedenceTests.cs`:

```csharp
using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

namespace Org.Edgerunner.Language.AL.Parsing.Tests;

/// <summary>
/// Verifies correct operator precedence and assignment handling in AL expressions.
/// </summary>
public class ExpressionPrecedenceTests
{
    // -----------------------------------------------------------------------
    // Assignment extracted to statement level
    // -----------------------------------------------------------------------

    [Fact]
    public void Assignment_is_parsed_as_assignmentStatement_not_expression()
    {
        var (tree, errors) = ALParseHelper.ParseProcedureBodyWithVars("x: Integer;", "x := 5;");

        errors.Should().BeEmpty("assignment is valid AL and should produce no parse errors");

        // After the fix: the statement line should contain an assignmentStatement context,
        // not an AssignmentExpressionContext nested inside an expression.
        var assignmentNode = ParseTreeSearch.FindFirst<ALParser.AssignmentStatementContext>(tree);
        assignmentNode.Should().NotBeNull("x := 5 should parse as an assignmentStatement, not an expression");
    }

    [Fact]
    public void Compound_assignment_is_parsed_as_assignmentStatement()
    {
        var (tree, errors) = ALParseHelper.ParseProcedureBodyWithVars("x: Integer;", "x += 1;");

        errors.Should().BeEmpty();

        var assignmentNode = ParseTreeSearch.FindFirst<ALParser.AssignmentStatementContext>(tree);
        assignmentNode.Should().NotBeNull("x += 1 should parse as an assignmentStatement");
    }

    // -----------------------------------------------------------------------
    // Multiplicative binds tighter than additive
    // -----------------------------------------------------------------------

    [Fact]
    public void Multiply_has_higher_precedence_than_add()
    {
        // 2 + 3 * 4 should parse as 2 + (3 * 4)
        // Top-level expression: AddExpression
        //   lhs: IntegerLiteralExpression (2)
        //   rhs: MultiplyExpression
        //          lhs: IntegerLiteralExpression (3)
        //          rhs: IntegerLiteralExpression (4)
        var (tree, errors) = ALParseHelper.ParseProcedureBodyWithVars(
            "x: Integer;",
            "x := 2 + 3 * 4;");

        errors.Should().BeEmpty();

        var assignment = ParseTreeSearch.FindFirst<ALParser.AssignmentStatementContext>(tree);
        assignment.Should().NotBeNull();

        // The rhs of the assignment should be an AddExpression
        var addExpr = assignment!.rhs as ALParser.AddExpressionContext;
        addExpr.Should().NotBeNull("top-level operator should be + (lower precedence), not *");

        // The rhs of the add should be a MultiplyExpression
        var mulExpr = addExpr!.rhs as ALParser.MultiplyExpressionContext;
        mulExpr.Should().NotBeNull("right-hand operand of + should be a multiply expression (3 * 4)");
    }

    [Fact]
    public void Divide_has_higher_precedence_than_subtract()
    {
        // 10 - 6 / 2 should parse as 10 - (6 / 2)
        var (tree, errors) = ALParseHelper.ParseProcedureBodyWithVars(
            "x: Integer;",
            "x := 10 - 6 / 2;");

        errors.Should().BeEmpty();

        var assignment = ParseTreeSearch.FindFirst<ALParser.AssignmentStatementContext>(tree);
        var subtractExpr = assignment!.rhs as ALParser.SubtractExpressionContext;
        subtractExpr.Should().NotBeNull("top-level operator should be - (lower precedence)");

        var divExpr = subtractExpr!.rhs as ALParser.DivideExpressionContext;
        divExpr.Should().NotBeNull("right-hand operand of - should be a divide expression (6 / 2)");
    }

    // -----------------------------------------------------------------------
    // Additive binds tighter than comparison
    // -----------------------------------------------------------------------

    [Fact]
    public void Add_has_higher_precedence_than_comparison()
    {
        // a + 1 > b + 2 should parse as (a + 1) > (b + 2)
        // Top-level: ComparisonExpression
        //   lhs: AddExpression
        //   rhs: AddExpression
        var (tree, errors) = ALParseHelper.ParseProcedureBodyWithVars(
            "a: Integer;\nb: Integer;\nx: Boolean;",
            "x := a + 1 > b + 2;");

        errors.Should().BeEmpty();

        var assignment = ParseTreeSearch.FindFirst<ALParser.AssignmentStatementContext>(tree);
        var compExpr = assignment!.rhs as ALParser.ComparisonExpressionContext;
        compExpr.Should().NotBeNull("top-level operator should be > (comparison)");

        compExpr!.lhs.Should().BeOfType<ALParser.AddExpressionContext>("lhs of comparison should be a + 1");
        compExpr.rhs.Should().BeOfType<ALParser.AddExpressionContext>("rhs of comparison should be b + 2");
    }

    // -----------------------------------------------------------------------
    // Logical AND binds tighter than OR
    // -----------------------------------------------------------------------

    [Fact]
    public void And_has_higher_precedence_than_or()
    {
        // a OR b AND c should parse as a OR (b AND c)
        // Top-level: OrExpression
        //   lhs: IdentifierExpression (a)
        //   rhs: AndExpression
        var (tree, errors) = ALParseHelper.ParseProcedureBodyWithVars(
            "a: Boolean;\nb: Boolean;\nc: Boolean;\nx: Boolean;",
            "x := a OR b AND c;");

        errors.Should().BeEmpty();

        var assignment = ParseTreeSearch.FindFirst<ALParser.AssignmentStatementContext>(tree);
        var orExpr = assignment!.rhs as ALParser.OrExpressionContext;
        orExpr.Should().NotBeNull("top-level operator should be OR (lower precedence than AND)");

        orExpr!.rhs.Should().BeOfType<ALParser.AndExpressionContext>("rhs of OR should be (b AND c)");
    }

    [Fact]
    public void And_and_or_produce_separate_context_types()
    {
        // Verify that AND and OR now produce distinct context types (not LogicalComparison)
        var (tree, errors) = ALParseHelper.ParseProcedureBodyWithVars(
            "a: Boolean;\nb: Boolean;\nx: Boolean;",
            "x := a AND b;");

        errors.Should().BeEmpty();

        var assignment = ParseTreeSearch.FindFirst<ALParser.AssignmentStatementContext>(tree);
        assignment!.rhs.Should().BeOfType<ALParser.AndExpressionContext>(
            "AND should produce AndExpressionContext, not LogicalComparisonExpressionContext");
    }

    [Fact]
    public void Or_produces_separate_context_type()
    {
        var (tree, errors) = ALParseHelper.ParseProcedureBodyWithVars(
            "a: Boolean;\nb: Boolean;\nx: Boolean;",
            "x := a OR b;");

        errors.Should().BeEmpty();

        var assignment = ParseTreeSearch.FindFirst<ALParser.AssignmentStatementContext>(tree);
        assignment!.rhs.Should().BeOfType<ALParser.OrExpressionContext>(
            "OR should produce OrExpressionContext, not LogicalComparisonExpressionContext");
    }

    // -----------------------------------------------------------------------
    // NOT (prefix unary) binds tighter than AND
    // -----------------------------------------------------------------------

    [Fact]
    public void Not_has_higher_precedence_than_and()
    {
        // NOT a AND b should parse as (NOT a) AND b
        // Top-level: AndExpression
        //   lhs: NotExpression
        //   rhs: IdentifierExpression (b)
        var (tree, errors) = ALParseHelper.ParseProcedureBodyWithVars(
            "a: Boolean;\nb: Boolean;\nx: Boolean;",
            "x := NOT a AND b;");

        errors.Should().BeEmpty();

        var assignment = ParseTreeSearch.FindFirst<ALParser.AssignmentStatementContext>(tree);
        var andExpr = assignment!.rhs as ALParser.AndExpressionContext;
        andExpr.Should().NotBeNull("top-level operator should be AND");

        andExpr!.lhs.Should().BeOfType<ALParser.NotExpressionContext>(
            "lhs of AND should be (NOT a) — NOT binds tighter than AND");
    }

    // -----------------------------------------------------------------------
    // Bare function call produces FunctionCallExpression (not MethodCall)
    // -----------------------------------------------------------------------

    [Fact]
    public void Bare_function_call_produces_FunctionCallExpression_context()
    {
        var (tree, errors) = ALParseHelper.ParseProcedureBodyWithVars(
            "x: Integer;",
            "x := StrLen('hello');");

        errors.Should().BeEmpty();

        var assignment = ParseTreeSearch.FindFirst<ALParser.AssignmentStatementContext>(tree);
        assignment!.rhs.Should().BeOfType<ALParser.FunctionCallExpressionContext>(
            "a bare function call like StrLen() should produce FunctionCallExpressionContext");
    }

    [Fact]
    public void Method_call_on_receiver_produces_MethodCallExpression_context()
    {
        var (tree, errors) = ALParseHelper.ParseProcedureBodyWithVars(
            "rec: Record Customer;",
            "rec.Init();");

        errors.Should().BeEmpty();

        var methodCall = ParseTreeSearch.FindFirst<ALParser.MethodCallExpressionContext>(tree);
        methodCall.Should().NotBeNull(
            "a method call on a receiver like rec.Init() should produce MethodCallExpressionContext");
    }

    // -----------------------------------------------------------------------
    // Ternary is right-associative
    // -----------------------------------------------------------------------

    [Fact]
    public void Ternary_is_right_associative()
    {
        // a ? b : c ? d : e should parse as a ? b : (c ? d : e)
        // Top-level: TernaryExpression
        //   condition: a
        //   trueExpr:  b
        //   falseExpr: TernaryExpression (c ? d : e)
        var (tree, errors) = ALParseHelper.ParseProcedureBodyWithVars(
            "a: Boolean;\nb: Integer;\nc: Boolean;\nd: Integer;\ne: Integer;\nx: Integer;",
            "x := a ? b : c ? d : e;");

        errors.Should().BeEmpty();

        var assignment = ParseTreeSearch.FindFirst<ALParser.AssignmentStatementContext>(tree);
        var outerTernary = assignment!.rhs as ALParser.TernaryExpressionContext;
        outerTernary.Should().NotBeNull("top-level should be a ternary expression");

        outerTernary!.falseExpr.Should().BeOfType<ALParser.TernaryExpressionContext>(
            "false branch should be the inner ternary (c ? d : e) — right-associative");
    }

    // -----------------------------------------------------------------------
    // Parse valid complex expressions without errors
    // -----------------------------------------------------------------------

    [Theory]
    [InlineData("x := (a + b) * c;", "parenthesised sub-expression")]
    [InlineData("x := a * b + c * d;", "mixed multiplicative and additive")]
    [InlineData("x := a DIV b MOD c;", "integer divide and modulus")]
    [InlineData("flag := NOT (a > 0) AND (c < 10);", "negation with comparison and AND")]
    [InlineData("x := a IN [1, 2, 3];", "IN range check")]
    [InlineData("x := rec.Field;", "member access")]
    [InlineData("x := rec.Items[1];", "index access")]
    [InlineData("x := MyEnum::Value;", "scope operator")]
    public void Valid_expressions_produce_no_parse_errors(string statement, string description)
    {
        var (_, errors) = ALParseHelper.ParseProcedureBodyWithVars(
            "a: Integer;\nb: Integer;\nc: Integer;\nd: Integer;\nx: Integer;\nflag: Boolean;\nrec: Record Customer;",
            statement);

        errors.Should().BeEmpty($"{description} should parse without errors");
    }
}
```

- [ ] **Step 2: Attempt to build the test project to confirm it fails against current grammar**

```bash
cd "D:/Projects/AL Langauge Tools"
dotnet build "ANTLR AL Parser Tests/ANTLR AL Parser Tests.csproj"
```

Expected: **Build failure** — `ALParser.AssignmentStatementContext`, `ALParser.AndExpressionContext`, `ALParser.OrExpressionContext`, and `ALParser.FunctionCallExpressionContext` do not exist in the pre-fix grammar. This is the correct TDD "red" state. The test project will only compile successfully after the Chunk 2 grammar changes are applied.

- [ ] **Step 3: Commit failing tests**

```bash
cd "D:/Projects/AL Langauge Tools/Grammar.AL.Antlr"
git add "../ANTLR AL Parser Tests/ExpressionPrecedenceTests.cs"
git commit -m "Add failing expression precedence tests (TDD red phase)"
```

---

## Chunk 2: Grammar Changes + Validator Fix

### Task 4: Fix `ALCodeParser.g4` — extract assignment to statement level

**Files:**
- Modify: `Grammar.AL.Antlr/ALCodeParser.g4` (lines 347–368 and 389–420)

- [ ] **Step 1: Add `assignmentStatement` rule and update `statementLine`**

In `ALCodeParser.g4`, replace the `statementLine` rule (currently lines 347–358) with the version that includes `assignmentStatement`:

Old `statementLine`:
```antlr
statementLine
   : ifStatement
   | forStatement
   | forEachStatement
   | caseStatement
   | withStatement
   | whileStatement
   | repeatUntilStatement
   | exitStatement
   | continueStatement
   | breakStatement
   | expression;
```

New — add `assignmentStatement` rule above `statementLine`, and add it as the first alternative:
```antlr
assignmentStatement
   : lhs=expression (ASSGN | DIV_ASSGN | MULTIPLY_ASSGN | ADD_ASSGN | MINUS_ASSGN) rhs=expression
   ;

statementLine
   : assignmentStatement
   | ifStatement
   | forStatement
   | forEachStatement
   | caseStatement
   | withStatement
   | whileStatement
   | repeatUntilStatement
   | exitStatement
   | continueStatement
   | breakStatement
   | expression;
```

- [ ] **Step 2: Note — `#AssignmentExpression` removal**

No separate edit is needed here. Task 5 replaces the entire `expression` rule, which automatically omits the `#AssignmentExpression` alternative. This step is a checkpoint only: confirm the old rule at line 417 reads:
```antlr
   | lhs=expression (ASSGN | DIV_ASSGN | MULTIPLY_ASSGN | ADD_ASSGN | MINUS_ASSGN) rhs=expression #AssignmentExpression
```
If it does, proceed — Task 5 will remove it as part of the full rule replacement.

- [ ] **Step 3: Build to verify grammar change compiles**

```bash
cd "D:/Projects/AL Langauge Tools"
dotnet build "Grammar.AL.Antlr/Grammar.AL.Antlr.csproj"
```

Expected: `Build succeeded.` — if ANTLR reports grammar errors, fix them before proceeding.

- [ ] **Step 4: Commit**

```bash
cd "D:/Projects/AL Langauge Tools/Grammar.AL.Antlr"
git add "ALCodeParser.g4"
git commit -m "Extract assignment to assignmentStatement rule; insert into statementLine"
```

---

### Task 5: Fix `ALCodeParser.g4` — reorder expression alternatives for correct precedence

**Files:**
- Modify: `Grammar.AL.Antlr/ALCodeParser.g4` (the `expression` rule, now ~lines 389–419 minus the removed assignment line)

- [ ] **Step 1: Replace the `expression` rule body with the correctly ordered version**

Replace the entire `expression` rule with the following. Ordering is highest-precedence first for left-recursive alternatives. Non-left-recursive alternatives (literals, prefix operators) are primary and naturally bind tightest.

> **Note on `methodName`:** The existing grammar references `methodName` (a rule defined as `methodName : identifier`) in two expression alternatives (lines 400–401). The new rule inlines `identifier` directly in place of `methodName` — this is semantically equivalent since `methodName` is defined as a single `identifier` alias. The `methodName` rule itself is still used in `triggerDeclaration` and is not removed.

```antlr
expression
   // --- Primary (non-left-recursive) ---
   : LEFTPAREN expr=expression RIGHTPAREN                                                            #ParenthesisExpression
   | booleanLiteral                                                                                  #BooleanLiteralExpression
   | DATE_LITERAL                                                                                    #DateLiteralExpression
   | TIME_LITERAL                                                                                    #TimeLiteralExpression
   | DATETIME_LITERAL                                                                                #DatetimeLiteralExpression
   | STRING_LITERAL                                                                                  #StringLiteralExpression
   | FLOAT_LITERAL                                                                                   #FloatLiteralExpression
   | INTEGER_LITERAL                                                                                 #IntegerLiteralExpression
   | systemEnumerationLiteral                                                                        #SystemEnumerationLiteralExpression
   | GUIALLOWED                                                                                      #GuiAllowedFunctionExpression
   | name=identifier LEFTPAREN arguments=methodCallArguments? RIGHTPAREN                            #FunctionCallExpression
   | identifier                                                                                      #IdentifierExpression
   | LEFTBRACKET setExpr=valueSet? RIGHTBRACKET                                                     #SetExpression
   // --- Prefix unary (non-left-recursive; bind tighter than all binary operators) ---
   | NOT expr=expression                                                                             #NotExpression
   | MINUS expr=expression                                                                           #NegativeExpression
   // --- Left-recursive binary operators (highest precedence first) ---
   | expr=expression SCOPE scope=identifier                                                         #ScopeExpression
   | expr=expression PERIOD name=identifier LEFTPAREN arguments=methodCallArguments? RIGHTPAREN    #MethodCallExpression
   | expr=expression PERIOD member=identifier                                                       #MemberAccessExpression
   | expr=expression LEFTBRACKET indexValue=expression RIGHTBRACKET                                #IndexExpression
   | lhs=expression ASTERISK rhs=expression                                                         #MultiplyExpression
   | lhs=expression BACKSLASH rhs=expression                                                        #DivideExpression
   | lhs=expression DIV rhs=expression                                                              #IntegerDivideExpression
   | lhs=expression MOD rhs=expression                                                              #ModulusExpression
   | lhs=expression PLUS rhs=expression                                                             #AddExpression
   | lhs=expression MINUS rhs=expression                                                            #SubtractExpression
   | lhs=expression IN LEFTBRACKET setExpr=valueSet? RIGHTBRACKET                                  #InRangeExpression
   | lhs=expression (LESSTHAN | GREATERTHAN | LESSTHANEQUAL | GREATERTHANEQUAL | NOTEQUAL | EQUAL) rhs=expression #ComparisonExpression
   | lhs=expression AND rhs=expression                                                              #AndExpression
   | lhs=expression XOR rhs=expression                                                              #XorExpression
   | lhs=expression OR rhs=expression                                                               #OrExpression
   | <assoc=right> condition=expression CONDITION trueExpr=expression COLON falseExpr=expression   #TernaryExpression
   ;
```

- [ ] **Step 2: Build to verify grammar change compiles cleanly**

```bash
cd "D:/Projects/AL Langauge Tools"
dotnet build "Grammar.AL.Antlr/Grammar.AL.Antlr.csproj"
```

Expected: `Build succeeded.` with no ANTLR warnings about ambiguity on the expression rule.

- [ ] **Step 3: Commit**

```bash
cd "D:/Projects/AL Langauge Tools/Grammar.AL.Antlr"
git add "ALCodeParser.g4"
git commit -m "Reorder expression rule alternatives for correct operator precedence"
```

---

### Task 6: Fix `ALValidator.cs` to match renamed/split labels

**Files:**
- Modify: `ANTLR AL Parsing/Validators/ALValidator.cs`

These changes are required to keep the project compiling after the grammar change. Two issues:
1. `LogicalComparisonExpressionContext` no longer exists → compile break
2. `MethodCallExpressionContext` no longer covers bare function calls → silent regression

- [ ] **Step 1: Fix `IsValidBooleanExpression` — replace `LogicalComparisonExpressionContext`**

In `ALValidator.cs`, locate `IsValidBooleanExpression` (method signature at line 106, target edit at line 113). Replace:

```csharp
expression is Grammar.AL.Antlr.ALParser.LogicalComparisonExpressionContext ||
```

With:

```csharp
expression is Grammar.AL.Antlr.ALParser.AndExpressionContext ||
expression is Grammar.AL.Antlr.ALParser.XorExpressionContext ||
expression is Grammar.AL.Antlr.ALParser.OrExpressionContext ||
```

- [ ] **Step 2: Add `FunctionCallExpressionContext` to `IsValidNumericExpression`**

In `IsValidNumericExpression` (target edit at line 93), add `FunctionCallExpressionContext` alongside `MethodCallExpressionContext`. Insert the new line immediately before the existing `MethodCallExpressionContext` line:

```csharp
expression is Grammar.AL.Antlr.ALParser.FunctionCallExpressionContext ||
expression is Grammar.AL.Antlr.ALParser.MethodCallExpressionContext ||
```

- [ ] **Step 3: Add `FunctionCallExpressionContext` to `IsValidBooleanExpression`**

In `IsValidBooleanExpression` (target edit at line 118), add `FunctionCallExpressionContext` alongside `MethodCallExpressionContext`. Insert the new line immediately before the existing `MethodCallExpressionContext` line:

```csharp
expression is Grammar.AL.Antlr.ALParser.FunctionCallExpressionContext ||
expression is Grammar.AL.Antlr.ALParser.MethodCallExpressionContext ||
```

- [ ] **Step 4: Build `ANTLR AL Parsing` to verify no compile errors**

```bash
cd "D:/Projects/AL Langauge Tools"
dotnet build "ANTLR AL Parsing/ANTLR AL Parsing.csproj"
```

Expected: `Build succeeded.` with 0 errors.

- [ ] **Step 5: Commit**

```bash
cd "D:/Projects/AL Langauge Tools/Grammar.AL.Antlr"
git add "../ANTLR AL Parsing/Validators/ALValidator.cs"
git commit -m "Fix ALValidator for renamed expression labels (split logical, add FunctionCall)"
```

---

### Task 7: Verify all tests pass

- [ ] **Step 1: Build entire solution**

```bash
cd "D:/Projects/AL Langauge Tools"
dotnet build "AL Language Tools.sln"
```

Expected: `Build succeeded.` — 0 errors, 0 warnings about missing types.

- [ ] **Step 2: Run expression precedence tests**

```bash
cd "D:/Projects/AL Langauge Tools"
dotnet test "ANTLR AL Parser Tests/ANTLR AL Parser Tests.csproj" -v normal
```

Expected: All tests pass. If any fail, see the debugging notes below.

- [ ] **Step 3: Run full test suite to check for regressions**

```bash
cd "D:/Projects/AL Langauge Tools"
dotnet test "AL Language Tools.sln" -v normal
```

Expected: All previously passing tests still pass.

- [ ] **Step 4: Final commit**

```bash
cd "D:/Projects/AL Langauge Tools/Grammar.AL.Antlr"
git add -A
git commit -m "Expression rule precedence fix — all tests passing"
```

---

## Debugging Notes

**If `AssignmentStatementContext` is not found by tests:**
The `assignmentStatement` rule was not added to `statementLine` or the grammar did not regenerate. Verify `ALCodeParser.g4` contains both the rule definition and the reference in `statementLine`, then rebuild.

**If `FunctionCallExpressionContext` is not found:**
ANTLR4 generates context class names from the label after `#`. Verify the label in `ALCodeParser.g4` is exactly `#FunctionCallExpression` (case-sensitive). The generated class will be `ALParser.FunctionCallExpressionContext`.

**If ANTLR reports a left-recursion warning:**
The `<assoc=right>` annotation on the ternary rule must appear inside the alternative, before the first token: `| <assoc=right> condition=expression ...`. Check there are no extra spaces or misplacement.

**If precedence tests still show wrong tree structure:**
Verify the alternative ORDER in the `expression` rule matches the plan exactly — `ASTERISK`/`BACKSLASH`/`DIV`/`MOD` must appear before `PLUS`/`MINUS`, and `AND` must appear before `OR`.

**If `dotnet test` cannot find the test project:**
Run `dotnet sln list` to confirm the project was added to the solution. If missing, re-run the `dotnet sln add` command from Task 1 Step 3.
