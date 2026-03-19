using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

namespace Org.Edgerunner.Language.AL.Grammar.Tests;

/// <summary>
/// Verifies structural parsing of AL permissionset and permissionsetextension objects.
/// </summary>
public class PermissionSetParserTests
{
    [Fact]
    public void Minimal_permissionset_parses_without_errors()
    {
        var source = @"
permissionset 50000 ""MyPermSet""
{
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a minimal permissionset with no properties should parse without errors");
    }

    [Fact]
    public void Minimal_permissionset_produces_PermissionsetContext()
    {
        var source = @"
permissionset 50000 ""MyPermSet""
{
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.PermissionsetContext>(tree)
            .Should().NotBeNull("a permissionset object should produce a PermissionsetContext node");
    }

    [Fact]
    public void Permissionset_with_tabledata_permissions_parses_without_errors()
    {
        var source = @"
permissionset 50000 ""MyPermSet""
{
    Assignable = true;
    Permissions = tabledata Customer = RIMD,
                  tabledata ""Sales Header"" = R,
                  tabledata ""Sales Line"" = R;
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a permissionset with tabledata permissions should parse without errors");
    }

    [Fact]
    public void Permissionset_with_object_type_permissions_parses_without_errors()
    {
        var source = @"
permissionset 50000 ""MyPermSet""
{
    Permissions = codeunit ""My Codeunit"" = X,
                  page ""My Page"" = X,
                  report * = X;
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a permissionset with codeunit/page/report object permissions and wildcard should parse without errors");
    }

    [Fact]
    public void Permissionset_with_included_sets_parses_without_errors()
    {
        var source = @"
permissionset 50000 ""MyPermSet""
{
    IncludedPermissionSets = ""BaseSet"", ""AnotherSet"";
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a permissionset with IncludedPermissionSets property should parse without errors");
    }

    [Fact]
    public void Minimal_permissionsetextension_parses_without_errors()
    {
        var source = @"
permissionsetextension 50000 ""MyPermSetExt"" extends ""MyPermSet""
{
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a minimal permissionsetextension should parse without errors");
    }

    [Fact]
    public void Minimal_permissionsetextension_produces_PermissionsetextensionContext()
    {
        var source = @"
permissionsetextension 50000 ""MyPermSetExt"" extends ""MyPermSet""
{
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.PermissionsetextensionContext>(tree)
            .Should().NotBeNull("a permissionsetextension object should produce a PermissionsetextensionContext node");
    }

    [Fact]
    public void Permissionsetextension_with_permissions_parses_without_errors()
    {
        var source = @"
permissionsetextension 50000 ""MyPermSetExt"" extends ""MyPermSet""
{
    Permissions = tabledata ""Sales Invoice Header"" = R,
                  codeunit ""My Codeunit"" = X;
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a permissionsetextension with permissions should parse without errors");
    }

    public static IEnumerable<object[]> PermSetFiles()
    {
        var root = @"D:\Projects\Business Central\Applications\Microsoft\BC26";
        if (!Directory.Exists(root))
            return Enumerable.Empty<object[]>();
        return Directory.GetFiles(root, "*.PermissionSet.al", SearchOption.AllDirectories)
            .Take(40).Select(f => new object[] { f });
    }

    [Theory]
    [MemberData(nameof(PermSetFiles))]
    public void Real_permissionset_file_parses_without_errors(string filePath)
    {
        var source = File.ReadAllText(filePath);
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
    }

    public static IEnumerable<object[]> PermSetExtFiles()
    {
        var root = @"D:\Projects\Business Central\Applications\Microsoft\BC26";
        if (!Directory.Exists(root))
            return Enumerable.Empty<object[]>();
        return Directory.GetFiles(root, "*.PermissionSetExt.al", SearchOption.AllDirectories)
            .Take(40).Select(f => new object[] { f });
    }

    [Theory]
    [MemberData(nameof(PermSetExtFiles))]
    public void Real_permissionsetextension_file_parses_without_errors(string filePath)
    {
        var source = File.ReadAllText(filePath);
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
    }
}
