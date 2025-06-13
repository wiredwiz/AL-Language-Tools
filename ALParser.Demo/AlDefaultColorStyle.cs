#region MIT License
// <copyright company = "Edgerunner.org" file = "AlDefaultColorStyle.cs">
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
   public class AlDefaultColorStyle : ITextEditorColorStyle
   {
      private Style _Error;

      public AlDefaultColorStyle()
      {
         var back = new SolidBrush(Color.Transparent);
         Default = new TextStyle(new SolidBrush(Color.LightSkyBlue), back, FontStyle.Regular);
         QuotedIdentifiers = new TextStyle(new SolidBrush(Color.MediumSpringGreen), back, FontStyle.Regular);
         NumberLiterals = new TextStyle(new SolidBrush(Color.MediumPurple), back, FontStyle.Regular);
         StringLiterals = new TextStyle(new SolidBrush(Color.OrangeRed), back, FontStyle.Regular);
         OtherLiterals = new TextStyle(new SolidBrush(Color.YellowGreen), back, FontStyle.Regular);
         Names = new TextStyle(new SolidBrush(Color.PaleGoldenrod), back, FontStyle.Regular);
         Identifiers = new TextStyle(new SolidBrush(Color.DarkGoldenrod), back, FontStyle.Regular);
         Keywords = new TextStyle(new SolidBrush(Color.DodgerBlue), back, FontStyle.Regular);
         StatementKeywords = new TextStyle(new SolidBrush(Color.DeepPink), back, FontStyle.Regular);
         Symbols = new TextStyle(new SolidBrush(Color.Fuchsia), back, FontStyle.Regular);
         Operators = new TextStyle(new SolidBrush(Color.DarkOrange), back, FontStyle.Regular);
         ComparisonOperators = new TextStyle(new SolidBrush(Color.DarkOrange), back, FontStyle.Regular);
         AssignmentOperators = new TextStyle(new SolidBrush(Color.MediumOrchid), back, FontStyle.Regular);
         BinaryOperators = new TextStyle(new SolidBrush(Color.DarkOrange), back, FontStyle.Regular);
         Error = new WavyLineStyle(255, Color.Red);
      }

      public Style Default { get; }

      public Style QuotedIdentifiers { get; }

      public Style NumberLiterals { get; }

      public Style StringLiterals { get; }

      public Style OtherLiterals { get; }

      public Style Names { get; }

      public Style Identifiers { get; }

      public Style Keywords { get; }

      public Style StatementKeywords { get; }

      public Style Symbols { get; }

      public Style Operators { get; }

      public Style ComparisonOperators { get; }

      public Style AssignmentOperators { get; }

      public Style BinaryOperators { get; }

      public Style Error { get; }
   }
}