using Antlr4.Runtime.Tree;
using Org.Edgerunner.Language.AL.Parsing;
using Org.Edgerunner.Language.AL.Parsing.Messaging;

namespace Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

/// <summary>
/// Wraps AL source snippets in a codeunit shell and parses them via ALParser.
/// </summary>
public static class ALParseHelper
{
    /// <summary>
    /// Wraps the given procedure body statements in a minimal codeunit and parses them.
    /// Returns the parse result and any errors.
    /// </summary>
    public static (ISyntaxTree Tree, IReadOnlyList<ErrorMessage> Errors) ParseProcedureBody(string statements)
    {
        var source = $@"codeunit 50000 ""TestCodeunit""
{{
    procedure TestProc()
    begin
        {statements}
    end;
}}";
        var parser = new ALParser();
        var tree = parser.Parse(source);
        return (tree, parser.Errors);
    }

    /// <summary>
    /// Wraps the given procedure body (with optional var block) and parses.
    /// </summary>
    public static (ISyntaxTree Tree, IReadOnlyList<ErrorMessage> Errors) ParseProcedureBodyWithVars(
        string varDeclarations,
        string statements)
    {
        var source = $@"codeunit 50000 ""TestCodeunit""
{{
    procedure TestProc()
    var
        {varDeclarations}
    begin
        {statements}
    end;
}}";
        var parser = new ALParser();
        var tree = parser.Parse(source);
        return (tree, parser.Errors);
    }
}
