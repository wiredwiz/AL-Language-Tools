#region MIT License
// <copyright company = "Edgerunner.org" file = "ModulusExpression.cs">
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
   /// Class that represents a modulus expression.
   /// Implements the <see cref="Org.Edgerunner.BC.AL.Models.Code.Expressions.IExpression" />
   /// </summary>
   /// <seealso cref="Org.Edgerunner.BC.AL.Models.Code.Expressions.IExpression" />
   public class ModulusExpression : IExpression
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ModulusExpression"/> class.
      /// </summary>
      /// <param name="leftExpression">The left expression.</param>
      /// <param name="rightExpression">The right expression.</param>
      public ModulusExpression(IExpression leftExpression, IExpression rightExpression)
      {
         Left = leftExpression;
         Right = rightExpression;
      }

      /// <summary>
      /// Gets or the expression type.
      /// </summary>
      /// <value>The expression type.</value>
      public ExpressionType Type => ExpressionType.Modulus;

      /// <summary>
      /// Gets or sets the left side of the expression.
      /// </summary>
      /// <value>The left side expression.</value>
      public IExpression Left { get; set; }

      /// <summary>
      /// Gets or sets the right side of the expression.
      /// </summary>
      /// <value>The right side expression.</value>
      public IExpression Right { get; set; }

      /// <summary>
      /// Formats this instance as code text.
      /// </summary>
      /// <param name="formatter">The formatting options.</param>
      /// <param name="builder">The string builder to populate.</param>
      /// <exception cref="InvalidOperationException">Left side or right side of modulus expression is empty.</exception>
      public void Format(CodeFormatter formatter, StringBuilder builder)
      {
         if (Left == null)
            throw new InvalidOperationException("Left side of modulus expression must not be empty");
         if (Right == null)
            throw new InvalidOperationException("Right side of modulus expression must not be empty");

         bool parenthesis = Left is AddExpression || Left is SubtractExpression;
         if (parenthesis)
            formatter.FormatBraces(builder, "(");
         Left.Format(formatter, builder);
         if (parenthesis)
            formatter.FormatBraces(builder, ")");
         formatter.FormatMathOperator(builder, "%");
         parenthesis = Left is AddExpression || Left is SubtractExpression;
         if (parenthesis)
            formatter.FormatBraces(builder, "(");
         Right.Format(formatter, builder);
         if (parenthesis)
            formatter.FormatBraces(builder, ")");
      }
   }
}