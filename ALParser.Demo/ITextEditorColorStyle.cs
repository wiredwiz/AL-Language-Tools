#region MIT License
// <copyright company = "Edgerunner.org" file = "TextEditorColorPalette.cs">
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

using FastColoredTextBoxNS.Types;

namespace ALParser.Demo
{
   /// <summary>
   /// Interface that defines a text editor color mapping palette
   /// </summary>
   public interface ITextEditorColorStyle
   {
      Style Default { get; }
      Style QuotedIdentifiers { get; }
      Style NumberLiterals { get; }
      Style StringLiterals { get; }
      Style OtherLiterals { get; }
      Style Names { get; }
      Style Identifiers { get; }
      Style Keywords { get; }
      Style StatementKeywords { get; }
      Style Symbols { get; }
      Style Operators { get; }
      Style ComparisonOperators { get; }
      Style AssignmentOperators { get; }
      Style BinaryOperators { get; }
      Style Error { get; }
   }
}