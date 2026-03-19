using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

namespace Org.Edgerunner.Language.AL.Grammar.Tests;

/// <summary>
/// Verifies structural parsing of AL profile and profileextension objects.
/// </summary>
public class ProfileParserTests
{
    [Fact]
    public void Minimal_profile_parses_without_errors()
    {
        var source = @"
profile MyProfile
{
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a minimal profile with no properties should parse without errors");
    }

    [Fact]
    public void Minimal_profile_produces_ProfileContext()
    {
        var source = @"
profile MyProfile
{
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.ProfileContext>(tree)
            .Should().NotBeNull("a profile object should produce a ProfileContext node");
    }

    [Fact]
    public void Profile_with_properties_parses_without_errors()
    {
        var source = @"
profile MyProfile
{
    Caption = 'My Profile';
    RoleCenter = ""Business Manager Role Center"";
    Enabled = true;
    Customizations = MyCustomization;
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a profile with Caption, RoleCenter, Enabled, and Customizations properties should parse without errors");
    }

    [Fact]
    public void Minimal_profileextension_parses_without_errors()
    {
        var source = @"
profileextension MyProfileExt extends MyProfile
{
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a minimal profileextension with no properties should parse without errors");
    }

    [Fact]
    public void Minimal_profileextension_produces_ProfileextensionContext()
    {
        var source = @"
profileextension MyProfileExt extends MyProfile
{
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.ProfileextensionContext>(tree)
            .Should().NotBeNull("a profileextension object should produce a ProfileextensionContext node");
    }

    [Fact]
    public void Profileextension_with_customization_parses_without_errors()
    {
        var source = @"
profileextension MyProfileExt extends MyProfile
{
    Customizations = AdditionalCustomization;
    Enabled = true;
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a profileextension with properties should parse without errors");
    }

    public static IEnumerable<object[]> ProfileFiles()
    {
        var root = @"D:\Projects\Business Central\Applications\Microsoft\BC26";
        if (!Directory.Exists(root))
            return Enumerable.Empty<object[]>();
        return Directory.GetFiles(root, "*.Profile.al", SearchOption.AllDirectories)
            .Take(40).Select(f => new object[] { f });
    }

    [Theory]
    [MemberData(nameof(ProfileFiles))]
    public void Real_profile_file_parses_without_errors(string filePath)
    {
        var source = File.ReadAllText(filePath);
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
    }

    public static IEnumerable<object[]> ProfileExtFiles()
    {
        var root = @"D:\Projects\Business Central\Applications\Microsoft\BC26";
        if (!Directory.Exists(root))
            return Enumerable.Empty<object[]>();
        return Directory.GetFiles(root, "*.ProfileExt.al", SearchOption.AllDirectories)
            .Take(40).Select(f => new object[] { f });
    }

    [Theory]
    [MemberData(nameof(ProfileExtFiles))]
    public void Real_profileextension_file_parses_without_errors(string filePath)
    {
        var source = File.ReadAllText(filePath);
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
    }
}
