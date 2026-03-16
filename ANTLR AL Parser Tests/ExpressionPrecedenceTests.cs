using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

namespace Org.Edgerunner.Language.AL.Grammar.Tests;

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
        var (tree, errors) = ALParseHelper.ParseProcedureBodyWithVars(
            "x: Integer;",
            "x := 2 + 3 * 4;");

        errors.Should().BeEmpty();

        var assignment = ParseTreeSearch.FindFirst<ALParser.AssignmentStatementContext>(tree);
        assignment.Should().NotBeNull();

        var addExpr = assignment!.rhs as ALParser.AddExpressionContext;
        addExpr.Should().NotBeNull("top-level operator should be + (lower precedence), not *");

        var mulExpr = addExpr!.rhs as ALParser.MultiplyExpressionContext;
        mulExpr.Should().NotBeNull("right-hand operand of + should be a multiply expression (3 * 4)");
    }

    [Fact]
    public void Divide_has_higher_precedence_than_subtract()
    {
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
