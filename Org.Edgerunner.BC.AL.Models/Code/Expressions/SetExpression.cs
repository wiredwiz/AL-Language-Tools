#region MIT License
// <copyright company = "Edgerunner.org" file = "SetExpression.cs">
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
using System.Linq;
using System.Text;

namespace Org.Edgerunner.BC.AL.Models.Code.Expressions
{
   /// <summary>
   /// Class that represents an AL set expression.
   /// Implements the <see cref="Org.Edgerunner.BC.AL.Models.Code.Expressions.IExpression" />
   /// </summary>
   /// <seealso cref="Org.Edgerunner.BC.AL.Models.Code.Expressions.IExpression" />
   public class SetExpression : ExpressionBase
   {
      

      /// <summary>
      /// Gets or the expression type.
      /// </summary>
      /// <value>The expression type.</value>
      public override ExpressionType Type => ExpressionType.Scope;

      /// <summary>
      /// Formats this instance as code text.
      /// </summary>
      /// <param name="formatter">The code formatter.</param>
      /// <param name="builder">The string builder to populate.</param>
      public override void Format(CodeFormatter formatter, StringBuilder builder)
      {
         formatter.FormatBraces(builder, "[");

         if (Children.Count == 1)
            Children[0].Format(formatter, builder);
         if (Children.Count > 0)
         {
            for (int i = 0; i < Children.Count - 1; i++)
            {
               Children[0].Format(formatter, builder);
               formatter.FormatSeparator(builder, ",");
            }

            Children[Children.Count - 1].Format(formatter, builder);
         }

         formatter.FormatBraces(builder, "]");
      }
   }
}