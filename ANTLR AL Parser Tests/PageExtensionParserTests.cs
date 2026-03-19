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
}
