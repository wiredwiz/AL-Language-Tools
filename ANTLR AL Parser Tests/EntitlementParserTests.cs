using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

namespace Org.Edgerunner.Language.AL.Grammar.Tests;

/// <summary>
/// Verifies structural parsing of AL entitlement objects.
/// </summary>
public class EntitlementParserTests
{
    [Fact]
    public void Minimal_entitlement_parses_without_errors()
    {
        var source = @"
entitlement MyEntitlement
{
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a minimal entitlement with no properties should parse without errors");
    }

    [Fact]
    public void Minimal_entitlement_produces_EntitlementContext()
    {
        var source = @"
entitlement MyEntitlement
{
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.EntitlementContext>(tree)
            .Should().NotBeNull("an entitlement object should produce an EntitlementContext node");
    }

    [Fact]
    public void Entitlement_with_role_properties_parses_without_errors()
    {
        var source = @"
entitlement MyEntitlement
{
    Type = Role;
    RoleType = Local;
    Id = '{00000000-0000-0000-0000-000000000001}';
    ObjectEntitlements = ""My Permission Set"", ""Another Permission Set"";
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("an entitlement with Type, RoleType, Id, and ObjectEntitlements should parse without errors");
    }

    [Fact]
    public void Entitlement_with_application_service_type_parses_without_errors()
    {
        var source = @"
entitlement MyEntitlement
{
    Type = ApplicationService;
    Id = 'my-app-service';
    ObjectEntitlements = ""D365 BASIC"";
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("an entitlement with ApplicationService type should parse without errors");
    }

    public static IEnumerable<object[]> EntitlementFiles()
    {
        var root = @"D:\Projects\Business Central\Applications\Microsoft\BC26";
        if (!Directory.Exists(root))
            return Enumerable.Empty<object[]>();
        return Directory.GetFiles(root, "*.Entitlement.al", SearchOption.AllDirectories)
            .Take(40).Select(f => new object[] { f });
    }

    [Theory]
    [MemberData(nameof(EntitlementFiles))]
    public void Real_entitlement_file_parses_without_errors(string filePath)
    {
        var source = File.ReadAllText(filePath);
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty($"real-world file {Path.GetFileName(filePath)} should parse without errors");
    }
}
