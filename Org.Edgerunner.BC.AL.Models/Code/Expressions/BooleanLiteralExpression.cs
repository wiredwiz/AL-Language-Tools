#region MIT License
// <copyright company = "Edgerunner.org" file = "BooleanLiteralExpression.cs">
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
using System.Text;

namespace Org.Edgerunner.BC.AL.Models.Code.Expressions
{
   /// <summary>
   /// Class that represents an AL boolean literal expression.
   /// Implements the <see cref="Org.Edgerunner.BC.AL.Models.Code.Expressions.IExpression" />
   /// </summary>
   /// <seealso cref="Org.Edgerunner.BC.AL.Models.Code.Expressions.IExpression" />
   public class BooleanLiteralExpression : IExpression
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="BooleanLiteralExpression"/> class.
      /// </summary>
      /// <param name="value">boolean value of literal.</param>
      public BooleanLiteralExpression(bool value)
      {
         Value = value;
      }

      /// <summary>
      /// Gets or the expression type.
      /// </summary>
      /// <value>The expression type.</value>
      public ExpressionType Type => ExpressionType.BooleanLiteral;

      /// <summary>
      /// Gets or sets the value.
      /// </summary>
      /// <value>The value.</value>
      // ReSharper disable once StyleCop.SA1623
      public bool Value { get; set; }

      /// <summary>
      /// Gets or sets the left side of the expression.
      /// </summary>
      /// <value>The left side expression.</value>
      public IExpression Left
      {
         get => null;
         set => throw new InvalidOperationException("You cannot assign a left side expression to a literal expression");
      }

      /// <summary>
      /// Gets or sets the right side of the expression.
      /// </summary>
      /// <value>The right side expression.</value>
      public IExpression Right
      {
         get => null;
         set => throw new InvalidOperationException("You cannot assign a right side expression to a literal expression");
      }

      /// <inheritdoc />
      public void Format(CodeFormatter formatter, StringBuilder builder)
      {
         builder.Append(Value ? "true" : "false");
      }
   }
}