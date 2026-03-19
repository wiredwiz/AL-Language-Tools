using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

namespace Org.Edgerunner.Language.AL.Grammar.Tests;

/// <summary>
/// Verifies structural parsing of AL controladdin objects.
/// </summary>
public class ControlAddInParserTests
{
    [Fact]
    public void Minimal_controladdin_parses_without_errors()
    {
        var source = @"
controladdin MyControlAddIn
{
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a minimal controladdin with no members should parse without errors");
    }

    [Fact]
    public void Minimal_controladdin_produces_ControladdinContext()
    {
        var source = @"
controladdin MyControlAddIn
{
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.ControladdinContext>(tree)
            .Should().NotBeNull("a controladdin object should produce a ControladdinContext node");
    }

    [Fact]
    public void Controladdin_with_properties_and_scripts_parses_without_errors()
    {
        var source = @"
controladdin MyControlAddIn
{
    RequestedHeight = 300;
    RequestedWidth = 700;
    MinimumHeight = 100;
    MinimumWidth = 300;
    HorizontalStretch = true;
    VerticalStretch = true;
    Scripts = 'Scripts/MyScript.js';
    StyleSheets = 'Stylesheets/MyStyle.css';
    StartupScript = 'Scripts/startup.js';
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a controladdin with key/value properties should parse without errors");
    }

    [Fact]
    public void Controladdin_with_events_parses_without_errors()
    {
        var source = @"
controladdin MyControlAddIn
{
    event ControlReady()
    event ValueChanged(NewValue: Text)
    event SelectionChanged(SelectedIds: Text; Count: Integer)
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a controladdin with event declarations (no semicolons) should parse without errors");
    }

    [Fact]
    public void Controladdin_with_events_and_semicolons_parses_without_errors()
    {
        var source = @"
controladdin MyControlAddIn
{
    event ControlReady();
    event ValueChanged(NewValue: Text);
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a controladdin with event declarations (with semicolons) should parse without errors");
    }

    [Fact]
    public void Controladdin_with_procedures_parses_without_errors()
    {
        var source = @"
controladdin MyControlAddIn
{
    procedure SetValue(NewValue: Text)
    procedure GetValue(): Text
    procedure Initialize(Data: JsonObject; Width: Integer);
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a controladdin with procedure declarations should parse without errors");
    }

    [Fact]
    public void Controladdin_with_mixed_members_parses_without_errors()
    {
        var source = @"
controladdin MyControlAddIn
{
    RequestedHeight = 300;
    Scripts = 'js/control.js';
    event ControlReady()
    event ValueChanged(Value: Text)
    procedure SetValue(Value: Text)
    procedure GetValue(): Text
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a controladdin with mixed properties, events, and procedures should parse without errors");
    }
}
