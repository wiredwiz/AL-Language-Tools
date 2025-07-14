#region MIT License
// <copyright company = "Edgerunner.org" file = "StringLiteralExpression.cs">
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

using System;
using System.Collections.Generic;
using System.Text;

namespace Org.Edgerunner.BC.AL.Models.Code.Expressions
{
   /// <summary>
   /// Class that represents a AL string literal expression.
   /// Implements the <see cref="Org.Edgerunner.BC.AL.Models.Code.Expressions.IExpression" />
   /// </summary>
   /// <seealso cref="Org.Edgerunner.BC.AL.Models.Code.Expressions.IExpression" />
   public class StringLiteralExpression : IExpression
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="StringLiteralExpression"/> class.
      /// </summary>
      /// <param name="value">The value.</param>
      public StringLiteralExpression(string value)
      {
         Value = value;
      }

      /// <summary>
      /// Gets or the expression type.
      /// </summary>
      /// <value>The expression type.</value>
      public NodeType Type => NodeType.StringLiteralExpression;

      /// <summary>
      /// Gets or sets the children.
      /// </summary>
      /// <value>The child expressions.</value>
      public List<IExpression> Children { get; set; }

      /// <summary>
      /// Gets or sets the value.
      /// </summary>
      /// <value>The value.</value>
      public string Value { get; set; }

      /// <inheritdoc />
      public void Format(CodeFormatter formatter, StringBuilder builder)
      {
         builder.Append("\"");
         builder.Append(Value);
         builder.Append("\"");
      }
   }
}