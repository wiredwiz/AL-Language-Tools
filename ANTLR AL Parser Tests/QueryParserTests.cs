using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

namespace Org.Edgerunner.Language.AL.Grammar.Tests;

/// <summary>
/// Verifies structural parsing of AL query objects.
/// </summary>
public class QueryParserTests
{
    [Fact]
    public void Minimal_query_parses_without_errors()
    {
        var source = @"
query 50000 ""MinimalQuery""
{
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a minimal query with no sections should parse without errors");
    }

    [Fact]
    public void Minimal_query_produces_QueryContext()
    {
        var source = @"
query 50000 ""MinimalQuery""
{
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.QueryContext>(tree)
            .Should().NotBeNull("a query object should produce a QueryContext node");
    }

    [Fact]
    public void Query_with_dataitem_and_columns_parses_without_errors()
    {
        var source = @"
query 50000 ""CustomerQuery""
{
    elements
    {
        dataitem(Customer; Customer)
        {
            column(No; ""No."") { }
            column(Name; Name) { }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a query with a dataitem and columns should parse without errors");
    }

    [Fact]
    public void Query_with_filter_parses_without_errors()
    {
        var source = @"
query 50000 ""FilteredQuery""
{
    elements
    {
        dataitem(Customer; Customer)
        {
            column(No; ""No."") { }
            filter(PostingGroup; ""Customer Posting Group"") { }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a query with a filter element should parse without errors");
    }

    [Fact]
    public void Query_with_nested_dataitems_parses_without_errors()
    {
        var source = @"
query 50000 ""NestedQuery""
{
    elements
    {
        dataitem(Customer; Customer)
        {
            column(No; ""No."") { }
            dataitem(SalesLine; ""Sales Line"")
            {
                column(Amount; Amount) { }
            }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a query with nested dataitems should parse without errors");
    }

    [Fact]
    public void API_query_with_entity_properties_parses_without_errors()
    {
        var source = @"
query 50000 ""CustomerAPI""
{
    QueryType = API;
    APIPublisher = 'microsoft';
    APIGroup = 'financials';
    APIVersion = 'v2.0';
    EntityName = 'customer';
    EntitySetName = 'customers';
    elements
    {
        dataitem(Customer; Customer)
        {
            column(id; SystemId) { }
            column(number; ""No."") { }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("an API query with entity properties should parse without errors");
    }

    [Fact]
    public void Query_with_permissions_property_parses_without_errors()
    {
        var source = @"
query 50000 ""PermissionsQuery""
{
    Permissions = tabledata ""Warehouse Activity Line"" = r,
                  tabledata Bin = r,
                  tabledata ""LAX Package"" = r;
    elements
    {
        dataitem(Customer; Customer)
        {
            column(No; ""No."") { }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a query with a Permissions property should parse without errors");
    }

    [Fact]
    public void Query_dataitem_with_DataItemTableFilter_parses_without_errors()
    {
        var source = @"
query 50000 ""FilteredQuery""
{
    elements
    {
        dataitem(WhseActivityLine; ""Warehouse Activity Line"")
        {
            DataItemTableFilter = ""Action Type"" = const(""Warehouse Action Type""::Take),
                                  ""Activity Type"" = const(""Warehouse Activity Type""::Pick),
                                  ""Qty. (Base)"" = const(1.0);
            column(No; ""No."") { }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a query dataitem with DataItemTableFilter using enum and float CONST values should parse without errors");
    }

    [Fact]
    public void Query_dataitem_with_DataItemLink_qualified_reference_parses_without_errors()
    {
        var source = @"
query 50000 ""LinkedQuery""
{
    elements
    {
        dataitem(Salesperson; ""Salesperson/Purchaser"")
        {
            column(Code; Code) { }
            dataitem(SalesHeader; ""Sales Header"")
            {
                DataItemLink = ""Salesperson Code"" = Salesperson.Code;
                SqlJoinType = InnerJoin;
                column(No; ""No."") { }
            }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a query DataItemLink with DataItem.Field qualified reference syntax should parse without errors");
    }

    [Fact]
    public void Query_dataitem_produces_QueryDataItemContext()
    {
        var source = @"
query 50000 ""CustomerQuery""
{
    elements
    {
        dataitem(Customer; Customer)
        {
            column(No; ""No."") { }
        }
    }
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.QueryDataItemContext>(tree)
            .Should().NotBeNull("a query dataitem should produce a QueryDataItemContext node");
    }
}
