# Design: Expression Rule Operator Precedence Fix

**Date:** 2026-03-16
**File:** `Grammar.AL.Antlr/ALCodeParser.g4`
**Branch:** AntlrGrammar

---

## Problem

The `expression` rule in `ALCodeParser.g4` lists all binary operators as flat alternatives with no enforced precedence. In ANTLR4's left-recursive rule handling, the order of alternatives determines which operators bind more tightly. With all operators at the same level, expressions like `2 + 3 * 4` may produce incorrect parse trees depending on input order and ANTLR4's tie-breaking behavior — not mathematical precedence.

Additionally, assignment operators (`:=`, `+=`, `-=`, `*=`, `/=`) are currently modelled as expression alternatives. Assignment is not an expression in AL — it does not produce a value and cannot appear nested inside another expression. Treating it as an expression creates false ambiguity in the grammar.

---

## Goals

1. Establish correct, deterministic operator precedence in the `expression` rule.
2. Extract assignment out of `expression` into a dedicated statement rule.
3. Split the combined `#LogicalComparisonExpression` label into separate labels per operator (`#AndExpression`, `#XorExpression`, `#OrExpression`) to give downstream listeners distinct hooks.
4. Make no changes outside `ALCodeParser.g4` except adding `assignmentStatement` to `statementLine`.

---

## Design

### Change 1: Extract Assignment to Statement Level

Remove the `#AssignmentExpression` alternative from `expression`. Add a new `assignmentStatement` rule and insert it at the top of `statementLine` (before the `expression` fallback).

```antlr
assignmentStatement
    : lhs=expression (ASSGN | DIV_ASSGN | MULTIPLY_ASSGN | ADD_ASSGN | MINUS_ASSGN) rhs=expression
    ;
```

`statementLine` becomes:

```antlr
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
    | expression
    ;
```

**Rationale:** Placing `assignmentStatement` before `expression` in `statementLine` ensures ANTLR4's LL(*) lookahead will correctly route `x := expr` to `assignmentStatement` rather than treating the whole thing as an expression. The right-hand side `rhs=expression` can itself be any valid expression (including method calls, arithmetic, etc.).

---

### Change 2: Reorder `expression` Alternatives by Precedence

In ANTLR4's left-recursive rule transformation, **alternatives listed earlier have higher precedence**. Non-left-recursive alternatives (literals, prefix operators) are treated as primary — they always match before any left-recursive (binary) alternative is attempted. This means `NOT` and unary `-` naturally bind tighter than all binary operators, which matches AL's operator precedence specification.

**Precedence table (highest to lowest):**

| Level | Operators | Notes |
|-------|-----------|-------|
| Primary | literals, identifier, `GUIALLOWED`, `[]` set, `()`, bare `f(args)` | non-left-recursive |
| Prefix unary | `NOT expr`, `-expr` | non-left-recursive, treated as primary |
| Scope | `expr :: identifier` | left-recursive |
| Method call | `expr . method ( args )` | left-recursive |
| Member access | `expr . member` | left-recursive |
| Index | `expr [ index ]` | left-recursive |
| Multiplicative | `* / DIV MOD` | left-recursive |
| Additive | `+ -` | left-recursive |
| Membership | `expr IN [ set ]` | left-recursive, relational level |
| Comparison | `= <> < > <= >=` | left-recursive |
| Logical AND | `AND` | left-recursive |
| Logical XOR | `XOR` | left-recursive |
| Logical OR | `OR` | left-recursive |
| Ternary | `expr ? expr : expr` | right-associative, use `<assoc=right>` |

**`expression` rule after the change:**

```antlr
expression
    // --- Primary (non-left-recursive) ---
    : LEFTPAREN expr=expression RIGHTPAREN                                                           #ParenthesisExpression
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
    // --- Prefix unary (non-left-recursive, binds tightest among unary) ---
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

---

### Change 3: Split `#LogicalComparisonExpression`

The existing label `#LogicalComparisonExpression` covers `AND`, `OR`, and `XOR` in a single alternative. Now that they are separated into their own alternatives for precedence purposes, they get distinct labels: `#AndExpression`, `#XorExpression`, `#OrExpression`.

This is a breaking change to any existing listener/visitor code that handles `ExitLogicalComparisonExpression`. The `ALValidator.cs` does not currently handle this case (the relevant validation logic is commented out), so no downstream code is affected.

---

### Change 4: Rename `#MethodCallExpression` for Bare Function Calls

The existing grammar has two alternatives both labeled `#MethodCallExpression`:
1. `expression PERIOD name=methodName LEFTPAREN arguments RIGHTPAREN` — method on a receiver
2. `name=methodName LEFTPAREN arguments RIGHTPAREN` — bare function call

After the reorder, the bare function call alternative is non-left-recursive (primary) and is now labeled `#FunctionCallExpression` to distinguish it. This gives listeners a distinct node for standalone calls vs. method dispatch.

---

## Files Changed

| File | Change |
|------|--------|
| `ALCodeParser.g4` | Reorder `expression` alternatives; add `assignmentStatement`; update `statementLine`; split logical comparison labels; rename bare call label |

No other files require changes. The grammar import chain (`ALParser.g4` → `ALCodeunitParser.g4` → `ALCodeParser.g4` → `ALCoreParser.g4`) is unaffected.

---

## Downstream Impact

| Consumer | Impact |
|----------|--------|
| `ALValidator.cs` | `ExitLogicalComparisonExpression` no longer exists; replace with `ExitAndExpression`, `ExitXorExpression`, `ExitOrExpression` if needed. Currently no active code uses this — commented out. |
| `ALParser.g4.cs` / `ALLexer.g4.cs` | No change — these are lexer/parser partial class stubs. |
| Parser Benchmark | Parses whole files; no AST node type dependencies. Unaffected. |
| ANTLR-generated C# | Must regenerate after grammar change. |

---

## Out of Scope

- Adding new AL object types (Query, Report, XmlPort — separate effort)
- Fixing the `identifier` rule performance (780+ alternatives — deferred)
- Fixing commented-out `propExpression` rules in `ALPropertyParser.g4` (deferred)
- Duplicate `OBJECTTYPE` in `systemEnumerationLiteral` (deferred)
