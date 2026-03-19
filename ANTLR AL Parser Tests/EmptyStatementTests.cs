using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

namespace Org.Edgerunner.Language.AL.Grammar.Tests;

/// <summary>
/// Verifies that lone semicolons are accepted as valid empty statements
/// anywhere inside a begin...end block.
/// </summary>
public class EmptyStatementTests
{
    [Fact]
    public void Leading_semicolon_in_trigger_body_parses_without_errors()
    {
        var source = @"
codeunit 50000 LeadingSemicolon
{
    trigger OnRun()
    begin
        ;
        Init();
    end;
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a leading ; before the first statement should be valid");
    }

    [Fact]
    public void Consecutive_semicolons_between_statements_parse_without_errors()
    {
        var source = @"
codeunit 50000 ConsecutiveSemicolons
{
    trigger OnRun()
    begin
        ;
        Foo();
        ;
        Bar();
    end;
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("consecutive semicolons between statements should be valid empty statements");
    }

    [Fact]
    public void Multiple_leading_semicolons_parse_without_errors()
    {
        var source = @"
codeunit 50000 MultipleLeadingSemicolons
{
    trigger OnRun()
    begin
        ;
        ;
        ;
        Foo();
    end;
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("multiple leading semicolons (3 empty statements) should be valid");
    }

    [Fact]
    public void Trailing_semicolons_after_statements_parse_without_errors()
    {
        var source = @"
codeunit 50000 TrailingSemicolons
{
    trigger OnRun()
    begin
        Foo();
        ;
        ;
    end;
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("trailing semicolons after the last statement should be valid");
    }

    [Fact]
    public void Report_trigger_with_empty_statements_parses_without_errors()
    {
        var source = @"
report 50000 ""NavReport""
{
    trigger OnInitReport()
    begin
        ;
        ReportsForNavInit();
    end;

    trigger OnPreReport()
    begin
        ;
        ReportForNav.SetCopies('Header', NoOfCopies);
        ;
        ReportsForNavPre();
    end;
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("report triggers with empty statements should parse without errors");
    }
}
