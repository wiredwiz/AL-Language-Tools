#region MIT License
// <copyright company = "Edgerunner.org" file = "ForStatement.cs">
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

using System.Text;

using Org.Edgerunner.BC.AL.Models.Code.Expressions;

namespace Org.Edgerunner.BC.AL.Models.Code.Statements
{
   /// <summary>
   /// Class that represents an AL For expression.
   /// Implements the <see cref="Org.Edgerunner.BC.AL.Models.Code.Statements.IStatement" />
   /// </summary>
   /// <seealso cref="Org.Edgerunner.BC.AL.Models.Code.Statements.IStatement" />
   public class ForStatement : IStatement
   {
      /// <summary>
      /// Gets the type.
      /// </summary>
      /// <value>The type.</value>
      public NodeType Type => NodeType.ForStatement;

      /// <summary>
      /// Gets or sets the iterator.
      /// </summary>
      /// <value>The iterator.</value>
      public VariableExpression Iterator { get; set; }

      /// <summary>
      /// Gets or sets the start value of the loop.
      /// </summary>
      /// <value>The start value.</value>
      public IExpression Start { get; set; }

      /// <summary>
      /// Gets or sets the end value of the loop.
      /// </summary>
      /// <value>The end value.</value>
      public IExpression End { get; set; }

      /// <summary>
      /// Gets or sets the statement.
      /// </summary>
      /// <value>The statement.</value>
      public IStatement Statement { get; set; }

      /// <summary>
      /// Gets or sets a value indicating whether this <see cref="ForStatement"/> is ascending.
      /// </summary>
      /// <value><c>true</c> if ascending; otherwise, <c>false</c>.</value>
      public bool Ascending { get; set; }

      /// <summary>
      /// Formats the specified formatter.
      /// </summary>
      /// <param name="formatter">The formatter.</param>
      /// <param name="builder">The builder.</param>
      public void Format(CodeFormatter formatter, StringBuilder builder)
      {
         builder.Append("for ");
         Iterator.Format(formatter, builder);
         formatter.FormatAssignmentOperator(builder, ":=");
         Start.Format(formatter, builder);
         builder.Append(Ascending ? " to " : " downto ");
         End.Format(formatter, builder);
         builder.Append(" do");
         if (formatter.StartBeginOnNewLine)
            formatter.AppendNewLine(builder);
         else
            builder.Append(" ");
         Statement.Format(formatter, builder);
      }
   }
}