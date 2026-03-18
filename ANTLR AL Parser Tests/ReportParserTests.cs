using Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

namespace Org.Edgerunner.Language.AL.Grammar.Tests;

/// <summary>
/// Verifies structural parsing of AL report objects.
/// </summary>
public class ReportParserTests
{
    [Fact]
    public void Minimal_report_parses_without_errors()
    {
        var source = @"
report 50000 ""MinimalReport""
{
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a minimal report with no sections should parse without errors");
    }

    [Fact]
    public void Minimal_report_produces_ReportContext()
    {
        var source = @"
report 50000 ""MinimalReport""
{
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.ReportContext>(tree)
            .Should().NotBeNull("a report object should produce a ReportContext node");
    }

    [Fact]
    public void Report_with_dataset_and_columns_parses_without_errors()
    {
        var source = @"
report 50000 ""CustomerReport""
{
    dataset
    {
        dataitem(Customer; Customer)
        {
            column(No; ""No."") { }
            column(Name; Name) { }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a report with a dataset section should parse without errors");
    }

    [Fact]
    public void Report_with_nested_dataitems_parses_without_errors()
    {
        var source = @"
report 50000 ""NestedReport""
{
    dataset
    {
        dataitem(Customer; Customer)
        {
            column(No; Customer.""No."") { }
            dataitem(SalesLine; ""Sales Line"")
            {
                column(Amount; ""Sales Line"".Amount) { }
            }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a report with nested dataitems should parse without errors");
    }

    [Fact]
    public void Report_with_requestpage_parses_without_errors()
    {
        var source = @"
report 50000 ""ReportWithRequestPage""
{
    dataset
    {
        dataitem(Customer; Customer)
        {
            column(No; ""No."") { }
        }
    }
    requestpage
    {
        layout
        {
            area(content)
            {
                group(Options)
                {
                    field(ShowAll; ShowAll) { }
                }
            }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a report with a requestpage section should parse without errors");
    }

    [Fact]
    public void Report_with_rendering_section_parses_without_errors()
    {
        var source = @"
report 50000 ""ReportWithRendering""
{
    rendering
    {
        layout(RdlcLayout)
        {
            Type = RDLC;
            LayoutFile = 'MyReport.rdlc';
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a report with a rendering section should parse without errors");
    }

    [Fact]
    public void Report_with_labels_parses_without_errors()
    {
        var source = @"
report 50000 ""ReportWithLabels""
{
    labels
    {
        MyLabel = 'Hello', Comment = 'A greeting';
        AmountLabel = 'Amount';
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a report with a labels section should parse without errors");
    }

    [Fact]
    public void Report_dataitem_produces_ReportDataItemContext()
    {
        var source = @"
report 50000 ""CustomerReport""
{
    dataset
    {
        dataitem(Customer; Customer)
        {
            column(No; ""No."") { }
        }
    }
}";
        var (tree, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty();
        ParseTreeSearch.FindFirst<ALParser.ReportDataItemContext>(tree)
            .Should().NotBeNull("a report dataitem should produce a ReportDataItemContext node");
    }

    [Fact]
    public void Report_dataitem_with_RequestFilterFields_parses_without_errors()
    {
        var source = @"
report 50000 ""FilterFieldsReport""
{
    dataset
    {
        dataitem(Customer; Customer)
        {
            RequestFilterFields = ""No."",""Sell-to Customer No."",""No. Printed"";
            column(No; ""No."") { }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("RequestFilterFields with a comma-separated field list should parse without errors");
    }

    [Fact]
    public void Report_dataitem_with_DataItemTableView_parses_without_errors()
    {
        var source = @"
report 50000 ""TableViewReport""
{
    dataset
    {
        dataitem(SalesHeader; ""Sales Header"")
        {
            DataItemTableView = sorting(""Document Type"",""No."") where(""Document Type""=CONST(Order));
            column(No; ""No."") { }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("DataItemTableView with sorting and where clause should parse without errors");
    }

    [Fact]
    public void Report_dataitem_with_DataItemLink_parses_without_errors()
    {
        var source = @"
report 50000 ""LinkedReport""
{
    dataset
    {
        dataitem(Customer; Customer)
        {
            column(No; ""No."") { }
            dataitem(SalesLine; ""Sales Line"")
            {
                DataItemLink = ""Sell-to Customer No.""=FIELD(""No."");
                column(Amount; Amount) { }
            }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("DataItemLink with field reference should parse without errors");
    }

    [Fact]
    public void Report_dataitem_with_DataItemTableFilter_parses_without_errors()
    {
        var source = @"
report 50000 ""FilteredTableReport""
{
    dataset
    {
        dataitem(SalesHeader; ""Sales Header"")
        {
            DataItemTableFilter = ""Document Type""=CONST(Order);
            column(No; ""No."") { }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("DataItemTableFilter with CONST filter should parse without errors");
    }

    [Fact]
    public void Report_column_with_integer_literal_source_parses_without_errors()
    {
        var source = @"
report 50000 ""LiteralSourceReport""
{
    dataset
    {
        dataitem(Customer; Customer)
        {
            column(ReportForNavId_2; 2) { }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a report column with an integer literal source should parse without errors");
    }

    [Fact]
    public void Report_column_with_method_call_source_parses_without_errors()
    {
        var source = @"
report 50000 ""MethodSourceReport""
{
    dataset
    {
        dataitem(Header; ""Sales Header"")
        {
            column(ReportForNav_Header; ReportForNavWriteDataItem('Header',Header)) { }
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a report column with a method call source should parse without errors");
    }

    [Fact]
    public void Report_dataitem_trigger_with_semicolon_separator_parses_without_errors()
    {
        var source = @"
report 50000 ""TriggerSemicolonReport""
{
    dataset
    {
        dataitem(Customer; Customer)
        {
            column(No; ""No."") { }
            trigger OnPreDataItem();
            begin
            end;
        }
    }
}";
        var (_, errors) = ALParseHelper.ParseAlUnit(source);
        errors.Should().BeEmpty("a trigger with an optional semicolon before the body should parse without errors");
    }
}
