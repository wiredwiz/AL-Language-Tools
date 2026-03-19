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
