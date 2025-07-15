#region MIT License
// <copyright company = "Edgerunner.org" file = "CodeFormatter.cs">
// Copyright(c)  2025
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

using Org.Edgerunner.BC.AL.Models.Code.Expressions;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

using Org.Edgerunner.BC.AL.Models.Code;

namespace Org.Edgerunner.BC.AL.Models
{
   /// <summary>
   /// Class representing a textual code formatter.
   /// </summary>
   public class CodeFormatter
   {
      /// <summary>
      /// Gets or sets text that represents a new line character sequence.
      /// </summary>
      /// <value>The new line text.</value>
      public string NewLine { get; set; } = "\r\n";

      /// <summary>
      /// Gets or sets a value indicating whether [use spaces instead of tabs].
      /// </summary>
      /// <value><c>true</c> if [use spaces instead of tabs]; otherwise, <c>false</c>.</value>
      public bool UseSpacesInsteadOfTabs { get; set; }

      /// <summary>
      /// Gets or sets the number of spaces a tab should represent.
      /// </summary>
      /// <value>The width of the tab.</value>
      public int TabWidth { get; set; }

      /// <summary>
      /// Gets or sets a value indicating whether to pad each side of an operand with a space.
      /// </summary>
      /// <value><c>true</c> if [pad operands]; otherwise, <c>false</c>.</value>
      public bool PadOperands { get; set; }

      /// <summary>
      /// Gets or sets a value indicating whether to [pad inside parenthesis].
      /// </summary>
      /// <value><c>true</c> if [pad inside parenthesis]; otherwise, <c>false</c>.</value>
      public bool PadInsideParenthesis { get; set; }

      /// <summary>
      /// Gets or sets a value indicating whether to [pad inside brackets].
      /// </summary>
      /// <value><c>true</c> if [pad inside brackets]; otherwise, <c>false</c>.</value>
      public bool PadInsideBrackets { get; set; }

      /// <summary>
      /// Gets or sets a value indicating whether to [pad inside braces].
      /// </summary>
      /// <value><c>true</c> if [pad inside braces]; otherwise, <c>false</c>.</value>
      public bool PadInsideBraces { get; set; }

      /// <summary>
      /// Gets or sets a value indicating whether to pad separator characters.
      /// </summary>
      /// <value><c>true</c> if [pad separator]; otherwise, <c>false</c>.</value>
      public bool PadSeparator { get; set; }

      /// <summary>
      /// Gets or sets the indentation level.
      /// </summary>
      /// <value>The indentation level.</value>
      public int IndentationLevel { get; set; }

      /// <summary>
      /// Gets or sets a value indicating whether to start begin keywords on new line.
      /// </summary>
      /// <value><c>true</c> if [start begin on new line]; otherwise, <c>false</c>.</value>
      public bool StartBeginOnNewLine { get; set; }

      /// <summary>
      /// Gets or sets a value indicating whether to indent begin blocks within the parent statement.
      /// </summary>
      /// <value><c>true</c> if [indent begin blocks]; otherwise, <c>false</c>.</value>
      public bool IndentBeginBlocks { get; set; } = true;

      /// <summary>
      /// Appends the new line terminator.
      /// </summary>
      /// <param name="builder">The builder.</param>
      public void AppendNewLine(StringBuilder builder)
      {
         builder.Append(NewLine);
      }

      /// <summary>
      /// Formats the math operator for code display.
      /// </summary>
      /// <param name="builder">The builder.</param>
      /// <param name="operand">The math operand to format.</param>
      public void FormatMathOperator(StringBuilder builder, string operand)
      {
         if (PadOperands) builder.Append(" ");
         builder.Append(operand);
         if (PadOperands) builder.Append(" ");
      }

      /// <summary>
      /// Formats the assignment operator for code display.
      /// </summary>
      /// <param name="builder">The builder.</param>
      /// <param name="operand">The assignment operand to format.</param>
      public void FormatAssignmentOperator(StringBuilder builder, string operand)
      {
         if (PadOperands) builder.Append(" ");
         builder.Append(operand);
         if (PadOperands) builder.Append(" ");
      }

      /// <summary>
      /// Formats the brace for code display.
      /// </summary>
      /// <param name="builder">The builder.</param>
      /// <param name="brace">The brace to format.</param>
      public void FormatBraces(StringBuilder builder, string brace)
      {
         if (brace == "{" || brace == "[" || brace == "(")
         {
            builder.Append(brace);
            if (brace == "(" && PadInsideParenthesis) builder.Append(" ");
            if (brace == "[" && PadInsideBrackets) builder.Append(" ");
            if (brace == "{" && PadInsideBraces) builder.Append(" ");
         }
         else if (brace == "}" || brace == "]" || brace == ")")
         {
            if (brace == ")" && PadInsideParenthesis) builder.Append(" ");
            if (brace == "]" && PadInsideBrackets) builder.Append(" ");
            if (brace == "}" && PadInsideBraces) builder.Append(" ");
            builder.Append(brace);
         }
      }

      /// <summary>
      /// Formats a code separator.
      /// </summary>
      /// <param name="builder">The builder.</param>
      /// <param name="separator">The separator.</param>
      public void FormatSeparator(StringBuilder builder, string separator)
      {
         builder.Append(separator);
         if (PadSeparator) builder.Append(" ");
      }

      /// <summary>
      /// Formats the set of entities with a separator.
      /// </summary>
      /// <param name="entities">The entities.</param>
      /// <param name="builder">The builder.</param>
      public void FormatSetWithSeparator(List<IFormattable> entities, StringBuilder builder)
      {
         if (entities.Count == 1)
            entities[0].Format(this, builder);
         if (entities.Count > 0)
         {
            for (int i = 0; i < entities.Count - 1; i++)
            {
               entities[i].Format(this, builder);
               FormatSeparator(builder, ",");
            }

            entities[entities.Count - 1].Format(this, builder);
         }
      }

      /// <summary>
      /// Formats the set of entities with a separator.
      /// </summary>
      /// <param name="entities">The entities.</param>
      /// <param name="builder">The builder.</param>
      public void FormatSetWithSeparator(List<string> entities, StringBuilder builder)
      {
         if (entities.Count == 1)
            builder.Append(entities[0]);
         if (entities.Count > 0)
         {
            for (int i = 0; i < entities.Count - 1; i++)
            {
               builder.Append(entities[i]);
               FormatSeparator(builder, ",");
            }

            builder.Append(entities[entities.Count - 1]);
         }
      }

      /// <summary>
      /// Gets the indentation padding for the current code position.
      /// </summary>
      /// <returns>A string containing the padding text.</returns>
      public string GetIndentationPadding()
      {
         if (UseSpacesInsteadOfTabs)
            return new string(' ', IndentationLevel * TabWidth);

         return new string('\t', IndentationLevel);
      }
   }
}