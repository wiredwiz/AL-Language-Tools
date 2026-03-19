using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

namespace Org.Edgerunner.Language.AL.Grammar.Tests;

/// <summary>
/// Verifies structural parsing of AL pagecustomization objects.
/// </summary>
public class PageCustomizationParserTests
{
    [Fact]
    public void Minimal_pagecustomization_parses_without_errors()
    {
        var source = @"
pagecustomization MyCustomization customizes ""Customer Card""
{
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a minimal pagecustomization should parse without errors");
    }

    [Fact]
    public void Minimal_pagecustomization_produces_PagecustomizationContext()
    {
        var source = @"
pagecustomization MyCustomization customizes ""Customer Card""
{
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.PagecustomizationContext>(tree)
            .Should().NotBeNull("a pagecustomization object should produce a PagecustomizationContext node");
    }

    [Fact]
    public void Pagecustomization_with_modify_layout_parses_without_errors()
    {
        var source = @"
pagecustomization MyCustomization customizes ""Customer Card""
{
    layout
    {
        modify(Name)
        {
            Visible = false;
        }
        modify(""No."")
        {
            Importance = Promoted;
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a pagecustomization with modify layout blocks should parse without errors");
    }

    [Fact]
    public void Pagecustomization_with_modify_actions_parses_without_errors()
    {
        var source = @"
pagecustomization MyCustomization customizes ""Customer Card""
{
    actions
    {
        modify(MyAction)
        {
            Visible = false;
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a pagecustomization with modify actions should parse without errors");
    }
}
