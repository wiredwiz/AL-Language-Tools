#region MIT License
// <copyright company = "Edgerunner.org" file = "DateLiteralExpression.cs">
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
using System.Globalization;
using System.Text;

namespace Org.Edgerunner.BC.AL.Models.Code.Expressions
{
   /// <summary>
   /// Class representing an AL date literal expression.
   /// Implements the <see cref="Org.Edgerunner.BC.AL.Models.Code.Expressions.IExpression" />
   /// </summary>
   /// <seealso cref="Org.Edgerunner.BC.AL.Models.Code.Expressions.IExpression" />
   public class DateLiteralExpression : IExpression
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="DateLiteralExpression"/> class.
      /// </summary>
      /// <param name="value">The value.</param>
      public DateLiteralExpression(DateTime value)
      {
         Value = value;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="DateLiteralExpression"/> class.
      /// </summary>
      /// <param name="text">The Date value text.</param>
      public DateLiteralExpression(string text)
      {
         Value = Parse(text);
      }

      /// <summary>
      /// Parses the specified date text.
      /// </summary>
      /// <param name="dateText">The date text.</param>
      /// <returns>A new <see cref="DateTime" />.</returns>
      /// <exception cref="ArgumentException">Value does not appear to be a valid Date literal</exception>
      /// <exception cref="ArgumentNullException">An empty value is not a valid Date literal value <paramref name="dateText"/></exception>
      public static DateTime Parse(string dateText)
      {
         if (string.IsNullOrEmpty(dateText))
            throw new ArgumentNullException(nameof(dateText), "An empty value is not a valid Date literal value");

         var terminator = dateText[dateText.Length - 1];
         if (terminator != 'D' && terminator != 'd')
            throw new ArgumentException("Date literals must end with a D");

         if (dateText.Length == 2 && dateText[0] == '0')
            return new DateTime(1753, 1, 1, 0, 0, 0);

         if (dateText.Length < 7)
            throw new ArgumentException("Value does not appear to be a valid Date literal");
         var text = dateText.Substring(0, dateText.Length - 2);
         var pattern = "yyyyMMdd";
         if (DateTime.TryParseExact(text, pattern, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, DateTimeKind.Utc);

         throw new ArgumentException("Value does not appear to be a valid Date literal");
      }

      /// <summary>
      /// Gets or the expression type.
      /// </summary>
      /// <value>The expression type.</value>
      public ExpressionType Type => ExpressionType.DateLiteral;

      /// <summary>
      /// Gets or sets the children.
      /// </summary>
      /// <value>The child expressions.</value>
      public List<IExpression> Children { get; set; }

      /// <summary>
      /// Gets or sets the value.
      /// </summary>
      /// <value>The value.</value>
      public DateTime Value { get; set; }

      /// <inheritdoc />
      public void Format(CodeFormatter formatter, StringBuilder builder)
      {
         builder.Append(Value.ToString("yyyyMMdd"));
         builder.Append("D");
      }
   }
}