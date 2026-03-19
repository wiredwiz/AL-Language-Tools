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
}
