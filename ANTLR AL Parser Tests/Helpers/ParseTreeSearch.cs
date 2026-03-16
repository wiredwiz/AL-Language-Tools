using Antlr4.Runtime.Tree;

namespace Org.Edgerunner.Language.AL.Parsing.Tests.Helpers;

/// <summary>
/// Utility for finding specific context types in an ANTLR4 parse tree.
/// </summary>
public static class ParseTreeSearch
{
    /// <summary>
    /// Returns the first node of type T found via depth-first search, or null.
    /// </summary>
    public static T? FindFirst<T>(ITree tree) where T : class
    {
        if (tree is T match)
            return match;

        for (var i = 0; i < tree.ChildCount; i++)
        {
            var found = FindFirst<T>(tree.GetChild(i));
            if (found != null)
                return found;
        }

        return null;
    }

    /// <summary>
    /// Returns all nodes of type T found via depth-first search.
    /// </summary>
    public static IReadOnlyList<T> FindAll<T>(ITree tree) where T : class
    {
        var results = new List<T>();
        CollectAll(tree, results);
        return results;
    }

    private static void CollectAll<T>(ITree tree, List<T> results) where T : class
    {
        if (tree is T match)
            results.Add(match);

        for (var i = 0; i < tree.ChildCount; i++)
            CollectAll(tree.GetChild(i), results);
    }
}
