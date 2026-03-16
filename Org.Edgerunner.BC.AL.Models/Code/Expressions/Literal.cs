#region MIT License
// <copyright company = "Edgerunner.org" file = "Literal.cs">
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
using System.Globalization;

namespace Org.Edgerunner.BC.AL.Models.Code.Expressions
{
   /// <summary>
   /// Class that represents an AL Literal.
   /// </summary>
   public class Literal
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="Literal"/> class.
      /// </summary>
      /// <param name="type">The literal type.</param>
      /// <param name="value">The literal value.</param>
      public Literal(LiteralType type, object value)
      {
         Type = type;
         Value = GetNewLiteralValue(type, value);
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="Literal" /> class.
      /// </summary>
      /// <param name="type">The literal type.</param>
      /// <param name="value">The literal value.</param>
      /// <exception cref="OverflowException"><paramref name="value">literalValue</paramref> represents a number less than int or decimal min value or greater than int or decimal max value.</exception>
      public Literal(LiteralType type, string value)
      {
         Type = type;
         object actual = Parse(type, value);
         Value = GetNewLiteralValue(type, actual);
      }

      /// <summary>
      /// Gets or sets the type.
      /// </summary>
      /// <value>The type.</value>
      public LiteralType Type { get; set; }

      /// <summary>
      /// Gets or sets the value.
      /// </summary>
      /// <value>The value.</value>
      public LiteralValue Value { get; set; }

      /// <summary>
      /// Parses the specified literal value.
      /// </summary>
      /// <param name="type">The literal type.</param>
      /// <param name="literalValue">The literal value.</param>
      /// <returns>A System.Object that contains the corresponding parsed value.</returns>
      /// <exception cref="InvalidOperationException">Cannot parse unknown literal type</exception>
      /// <exception cref="OverflowException"><paramref name="literalValue">literalValue</paramref> represents a number less than int or decimal min value or greater than int or decimal max value.</exception>
      public static object Parse(LiteralType type, string literalValue)
      {
         switch (type)
         {
            case LiteralType.String:
               if (literalValue.Length > 1 && literalValue[0] == '\'')
                  return literalValue.Substring(1, literalValue.Length - 2);
               
               return literalValue;
            case LiteralType.Float:
               return decimal.Parse(literalValue);
            case LiteralType.Integer:
               return int.Parse(literalValue);
            case LiteralType.Boolean:
               return bool.Parse(literalValue);
            case LiteralType.Date:
               return DateTime.Parse(literalValue);
            case LiteralType.Time:
               return DateTime.Parse(literalValue);
            case LiteralType.DateTime:
               return DateTime.Parse(literalValue);
            default: throw new InvalidOperationException("Cannot parse unknown literal type");
         }
      }

      /// <summary>
      /// Gets the underlying value of the literal.
      /// </summary>
      /// <returns>A decimal, int, string, DateTime or bool value depending on the underlying type.</returns>
      /// <exception cref="InvalidOperationException">Unknown literal type defined</exception>
      public object GetValue()
      {
         // ReSharper disable StyleCop.SA1025
         switch (Type)
         {
            // ReSharper disable StringLiteralTypo
            case LiteralType.Float:    return Value.Decimal;
            case LiteralType.Integer:  return Value.Integer;
            case LiteralType.String:   return Value.String;
            case LiteralType.Date:     return Value.DateTime;
            case LiteralType.Time:     return Value.DateTime;
            case LiteralType.DateTime: return Value.DateTime;
            case LiteralType.Boolean:  return Value.Boolean;
            default:                   throw new InvalidOperationException("Unknown literal type defined");
            // ReSharper restore StringLiteralTypo
         }
         // ReSharper restore StyleCop.SA1025
      }

      /// <summary>
      /// Gets the underlying value of the literal formatted as a string.
      /// </summary>
      /// <returns>A string that represents the underlying type.</returns>
      /// <exception cref="InvalidOperationException">Unknown literal type defined</exception>
      public string GetFormattedValue()
      {
         // ReSharper disable StyleCop.SA1025
         switch (Type)
         {
            // ReSharper disable StringLiteralTypo
            case LiteralType.Float:    return Value.Decimal.ToString(CultureInfo.InvariantCulture);
            case LiteralType.Integer:  return Value.Integer.ToString();
            case LiteralType.String:   return Value.String;
            case LiteralType.Date:     return Value.DateTime.ToString("yyyyMMdd'D'");
            case LiteralType.Time:     return Value.DateTime.ToString("HH:mm:ss.fff'T'");
            case LiteralType.DateTime: return "0DT"; // if this parsed properly then it should only be a 0 datetime value
            case LiteralType.Boolean:  return Value.Boolean.ToString();
            // ReSharper restore StringLiteralTypo
            default:                   throw new InvalidOperationException("Unknown literal type defined");
         }
         // ReSharper restore StyleCop.SA1025
      }

      /// <summary>
      /// Gets a new <see cref="LiteralValue"/>.
      /// </summary>
      /// <param name="type">The type.</param>
      /// <param name="value">The value.</param>
      /// <returns>A new <see cref="LiteralValue"/> instance.</returns>
      protected LiteralValue GetNewLiteralValue(LiteralType type, object value)
      {
         LiteralValue newValue = new LiteralValue();
         switch (type)
         {
            case LiteralType.Float: 
               newValue.Decimal = (decimal)value;
               break;
            case LiteralType.Integer:
               newValue.Integer = (int)value;
               break;
            case LiteralType.Boolean:
               newValue.Boolean = (bool)value;
               break;
            case LiteralType.String:
               newValue.String = (string)value;
               break;
            case LiteralType.DateTime:
               newValue.DateTime = (DateTime)value;
               break;
            case LiteralType.Date:
               newValue.DateTime = (DateTime)value;
               break;
            case LiteralType.Time:
               newValue.DateTime = (DateTime)value;
               break;
         }

         return newValue;
      }
   }
}