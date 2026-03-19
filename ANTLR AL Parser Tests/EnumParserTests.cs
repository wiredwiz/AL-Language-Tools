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
}
