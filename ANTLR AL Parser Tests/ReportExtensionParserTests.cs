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
}
