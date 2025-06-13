#region MIT License
// <copyright company = "Edgerunner.org" file = "TreeGrapher.cs">
// Copyright(c)  2023
// </copyright>
// The MIT License (MIT)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#endregion

using Microsoft.Msagl.Drawing;
using Org.Edgerunner.BC.AL.Language.Parsers.Rules;
using Org.Edgerunner.BC.AL.Language.Parsers.Rules.Terminals;
using Org.Edgerunner.Language.Parsers;
using Color = Microsoft.Msagl.Drawing.Color;

namespace ALParser.Demo
{
   public static class TreeGrapher
   {
      public static Graph CreateGraph(ITree? tree)
      {
         var graph = new Graph();
         if (tree != null)
         {
            if (tree.Children.Count == 0)
               graph.AddNode(tree.GetHashCode().ToString());
            else
               GraphEdges(graph, tree);
            FormatNodes(graph, tree);
         }

         return graph;
      }

      private static void GraphEdges(Graph graph, ITree tree)
      {
         for (var i = tree.Children.Count - 1; i > -1; i--)
         {
            var child = tree.Children[i];
            graph.AddEdge(tree.GetHashCode().ToString(), child.GetHashCode().ToString());

            GraphEdges(graph, child);
         }
      }

      private static void FormatNodes(Graph graph, ITree tree)
      {
         var node = graph.FindNode(tree.GetHashCode().ToString());
         if (node != null)
         {
            node.LabelText = GetNodeText(tree);
            var rule = tree as AlParserRule;
            var isError = rule?.IsError ?? false;
            node.Label.FontColor = isError ? Color.Red : Color.Black;
            node.Attr.Color = Color.Black;
            node.Attr.Color = Color.Black;
            node.UserData = tree;
         }

         foreach (var child in tree.Children)
            FormatNodes(graph, child);
      }

      private static string GetNodeText(ITree tree)
      {
         if (tree is AlParserRule rule)
            if (rule.Children.Count != 0 && !string.IsNullOrEmpty(rule.Name))
            {
               return rule.Name;
            }

         return tree.ToString() ?? "Unknown";
      }
   }
}