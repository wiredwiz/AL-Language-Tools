using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

namespace Org.Edgerunner.Language.AL.Grammar.Tests;

/// <summary>
/// Verifies that AL enforces declaration ordering within field blocks:
/// properties must precede triggers.
/// </summary>
public class FieldOrderingTests
{
    // -----------------------------------------------------------------------
    // Table field ordering
    // -----------------------------------------------------------------------

    [Fact]
    public void Table_field_with_property_before_trigger_parses_without_errors()
    {
        var source = @"
table 50000 ""TestTable""
{
    fields
    {
        field(1; Name; Text[100])
        {
            Caption = 'Name';
            trigger OnValidate()
            begin
            end;
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a table field with property before trigger is valid AL");
    }

    [Fact]
    public void Table_field_with_trigger_before_property_produces_parse_errors()
    {
        var source = @"
table 50000 ""TestTable""
{
    fields
    {
        field(1; Name; Text[100])
        {
            trigger OnValidate()
            begin
            end;
            Caption = 'Name';
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().NotBeEmpty("a property declared after a trigger violates AL ordering rules");
    }

    // -----------------------------------------------------------------------
    // Page field ordering
    // -----------------------------------------------------------------------

    [Fact]
    public void Page_field_with_property_before_trigger_parses_without_errors()
    {
        var source = @"
page 50000 ""TestPage""
{
    layout
    {
        area(content)
        {
            group(Group1)
            {
                field(Name; Name)
                {
                    Caption = 'Name';
                    trigger OnValidate()
                    begin
                    end;
                }
            }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a page field with property before trigger is valid AL");
    }

    [Fact]
    public void Page_field_with_trigger_before_property_produces_parse_errors()
    {
        var source = @"
page 50000 ""TestPage""
{
    layout
    {
        area(content)
        {
            group(Group1)
            {
                field(Name; Name)
                {
                    trigger OnValidate()
                    begin
                    end;
                    Caption = 'Name';
                }
            }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().NotBeEmpty("a property declared after a trigger violates AL ordering rules");
    }
}
