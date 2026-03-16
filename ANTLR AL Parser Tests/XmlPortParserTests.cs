using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

namespace Org.Edgerunner.Language.AL.Grammar.Tests;

/// <summary>
/// Verifies structural parsing of AL xmlport objects.
/// </summary>
public class XmlPortParserTests
{
    [Fact]
    public void Minimal_xmlport_parses_without_errors()
    {
        var source = @"
xmlport 50000 ""MinimalXmlPort""
{
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a minimal xmlport with no sections should parse without errors");
    }

    [Fact]
    public void Minimal_xmlport_produces_XmlPortContext()
    {
        var source = @"
xmlport 50000 ""MinimalXmlPort""
{
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.XmlPortContext>(tree)
            .Should().NotBeNull("an xmlport object should produce a XmlPortContext node");
    }

    [Fact]
    public void XmlPort_with_textelement_schema_parses_without_errors()
    {
        var source = @"
xmlport 50000 ""ExportPort""
{
    Direction = Export;
    schema
    {
        textelement(Root)
        {
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("an xmlport with a textelement schema should parse without errors");
    }

    [Fact]
    public void XmlPort_with_tableelement_and_fieldelements_parses_without_errors()
    {
        var source = @"
xmlport 50000 ""CustomerExport""
{
    Direction = Export;
    schema
    {
        textelement(Root)
        {
            tableelement(Customer; Customer)
            {
                fieldelement(No; Customer.""No."") { }
                fieldelement(Name; Customer.Name) { }
            }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("an xmlport with tableelement and fieldelements should parse without errors");
    }

    [Fact]
    public void XmlPort_with_nested_elements_and_attributes_parses_without_errors()
    {
        var source = @"
xmlport 50000 ""NestedXmlPort""
{
    schema
    {
        textelement(Root)
        {
            textelement(Header)
            {
                textattribute(version) { }
            }
            tableelement(Customer; Customer)
            {
                fieldelement(No; Customer.""No."") { }
                fieldattribute(Status; Customer.Status) { }
            }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("an xmlport with nested elements and attributes should parse without errors");
    }

    [Fact]
    public void XmlPort_with_requestpage_parses_without_errors()
    {
        var source = @"
xmlport 50000 ""XmlPortWithRequestPage""
{
    schema
    {
        textelement(Root)
        {
            tableelement(Customer; Customer)
            {
                fieldelement(No; Customer.""No."") { }
            }
        }
    }
    requestpage
    {
        layout
        {
            area(content)
            {
            }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("an xmlport with a requestpage section should parse without errors");
    }

    [Fact]
    public void XmlPort_tableelement_produces_XmlPortTableElementContext()
    {
        var source = @"
xmlport 50000 ""CustomerExport""
{
    schema
    {
        textelement(Root)
        {
            tableelement(Customer; Customer)
            {
                fieldelement(No; Customer.""No."") { }
            }
        }
    }
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.XmlPortTableElementContext>(tree)
            .Should().NotBeNull("a tableelement should produce a XmlPortTableElementContext node");
    }
}
