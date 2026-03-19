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
