using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

namespace Org.Edgerunner.Language.AL.Grammar.Tests;

/// <summary>
/// Verifies structural parsing of AL dotnet objects.
/// </summary>
public class DotNetParserTests
{
    [Fact]
    public void Minimal_dotnet_parses_without_errors()
    {
        var source = @"
dotnet
{
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a minimal dotnet container with no assemblies should parse without errors");
    }

    [Fact]
    public void Minimal_dotnet_produces_DotnetContext()
    {
        var source = @"
dotnet
{
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.DotnetContext>(tree)
            .Should().NotBeNull("a dotnet object should produce a DotnetContext node");
    }

    [Fact]
    public void Dotnet_with_assembly_and_types_parses_without_errors()
    {
        var source = @"
dotnet
{
    assembly(""mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"")
    {
        type(System.Collections.Generic.List; List)
        {
            IsTypeDefinition = true;
        }
        type(System.Collections.Generic.Dictionary; Dictionary) { }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a dotnet with assembly and types using quoted names should parse without errors");
    }

    [Fact]
    public void Dotnet_with_dotted_identifier_names_parses_without_errors()
    {
        var source = @"
dotnet
{
    assembly(Microsoft.Dynamics.Nav.MX)
    {
        type(Microsoft.Dynamics.Nav.MX.QRCodeGenerator; QRCodeGenerator)
        {
            IsTypeDefinition = true;
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a dotnet with dotted identifier assembly and type names should parse without errors");
    }

    [Fact]
    public void Dotnet_with_multiple_assemblies_parses_without_errors()
    {
        var source = @"
dotnet
{
    assembly(""Assembly.One"")
    {
        AliasPrefix = 'Prefix1';
        type(""Assembly.One.TypeA""; TypeA) { }
    }
    assembly(""Assembly.Two"")
    {
        AliasPrefix = 'Prefix2';
        type(""Assembly.Two.TypeB""; TypeB) { }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a dotnet with multiple assemblies should parse without errors");
    }

    public static IEnumerable<object[]> DotNetFiles()
    {
        var root = @"D:\Projects\Business Central\Applications\Microsoft\BC26";
        if (!Directory.Exists(root))
            return Enumerable.Empty<object[]>();
        return Directory.GetFiles(root, "dotnet*.al", SearchOption.AllDirectories)
            .Take(40).Select(f => new object[] { f });
    }

    [Theory]
    [MemberData(nameof(DotNetFiles))]
    public void Real_dotnet_file_parses_without_errors(string filePath)
    {
        var source = File.ReadAllText(filePath);
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
    }
}
