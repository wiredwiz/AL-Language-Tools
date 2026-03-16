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
