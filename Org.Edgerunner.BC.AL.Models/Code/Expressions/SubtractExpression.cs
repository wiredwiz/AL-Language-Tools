#region MIT License
// <copyright company = "Edgerunner.org" file = "SubtractExpression.cs">
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
   /// Class that represents an AL subtraction expression.
   /// Implements the <see cref="Org.Edgerunner.BC.AL.Models.Code.Expressions.IExpression" />
   /// </summary>
   /// <seealso cref="Org.Edgerunner.BC.AL.Models.Code.Expressions.IExpression" />
   public class SubtractExpression : BinaryExpressionBase
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="SubtractExpression"/> class.
      /// </summary>
      /// <param name="leftExpression">The left expression.</param>
      /// <param name="rightExpression">The right expression.</param>
      public SubtractExpression(IExpression leftExpression, IExpression rightExpression)
      {
         Left = leftExpression;
         Right = rightExpression;
      }

      /// <summary>
      /// Gets or the expression type.
      /// </summary>
      /// <value>The expression type.</value>
      public override ExpressionType Type => ExpressionType.Subtract;

      /// <summary>
      /// Formats this instance as code text.
      /// </summary>
      /// <param name="formatter">The code formatter.</param>
      /// <param name="builder">The string builder to populate.</param>
      /// <exception cref="InvalidOperationException">Left side or right side of subtract expression is empty.</exception>
      public override void Format(CodeFormatter formatter, StringBuilder builder)
      {
         if (Left == null)
            throw new InvalidOperationException("Left side of subtract expression must not be empty");
         if (Right == null)
            throw new InvalidOperationException("Right side of subtract expression must not be empty");

         Left.Format(formatter, builder);
         formatter.FormatMathOperator(builder, "-");
         Right.Format(formatter, builder);
      }
   }
}