#region MIT License
// <copyright company = "Edgerunner.org" file = "WhileStatement.cs">
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
   /// Class that represents an AL While statement.
   /// Implements the <see cref="Org.Edgerunner.BC.AL.Models.Code.Statements.IStatement" />
   /// </summary>
   /// <seealso cref="Org.Edgerunner.BC.AL.Models.Code.Statements.IStatement" />
   public class WhileStatement : IStatement
   {
      /// <summary>
      /// Gets the type.
      /// </summary>
      /// <value>The type.</value>
      public NodeType Type => NodeType.WhileStatement;

      /// <summary>
      /// Gets or sets the expression.
      /// </summary>
      /// <value>The expression.</value>
      public IExpression Expression { get; set; }

      /// <summary>
      /// Gets or sets the statement.
      /// </summary>
      /// <value>The statement.</value>
      public IStatement Statement { get; set; }

      /// <summary>
      /// Formats the specified formatter.
      /// </summary>
      /// <param name="formatter">The formatter.</param>
      /// <param name="builder">The builder.</param>
      public void Format(CodeFormatter formatter, StringBuilder builder)
      {
         builder.Append("while ");
         Expression.Format(formatter, builder);
         builder.Append(" do");
         if (formatter.StartBeginOnNewLine)
            formatter.AppendNewLine(builder);
         else
            builder.Append(" ");
         formatter.IndentationLevel++;
         Statement.Format(formatter, builder);
         formatter.IndentationLevel--;
      }
   }
}