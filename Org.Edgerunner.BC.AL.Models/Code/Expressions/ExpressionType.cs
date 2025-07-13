#region MIT License
// <copyright company = "Edgerunner.org" file = "ExpressionType.cs">
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

namespace Org.Edgerunner.BC.AL.Models.Code.Expressions
{
   /// <summary>
   /// Enum representing an AL expression type.
   /// </summary>
   public enum ExpressionType
   {
      // ReSharper disable StyleCop.SA1630
      // ReSharper disable StyleCop.SA1631

      /// <summary>
      /// A string literal expression
      /// </summary>
      /// <example>"foo bar"</example>
      StringLiteral,

      /// <summary>
      /// An integer literal expression
      /// </summary>
      /// <example>5</example>
      IntegerLiteral,

      /// <summary>
      /// A float literal expression
      /// </summary>
      /// <example>2.5</example>
      FloatLiteral,

      /// <summary>
      /// A Date literal expression
      /// </summary>
      /// <example>20020526D</example>
      DateLiteral,

      /// <summary>
      /// A Time literal expression
      /// </summary>
      /// <example>120215.333T</example>
      TimeLiteral,

      /// <summary>
      /// A DateTime literal expression
      /// </summary>
      /// <example>0DT</example>
      DateTimeLiteral,

      /// <summary>
      /// A boolean literal expression
      /// </summary>
      /// <example>true</example>
      BooleanLiteral,

      /// <summary>
      /// An add expression
      /// </summary>
      /// <example>5 + 2</example>
      Add,

      /// <summary>
      /// A subtract expression
      /// </summary>
      /// <example>5 - 2</example>
      Subtract,

      /// <summary>
      /// A multiply expression
      /// </summary>
      /// <example>5 * 2</example>
      Multiply,

      /// <summary>
      /// A divide expression
      /// </summary>
      /// <example>5 / 2</example>
      Divide,

      /// <summary>
      /// A modulus expression
      /// </summary>
      /// <example>5 % 2</example>
      Modulus,

      /// <summary>
      /// A not expression
      /// </summary>
      /// <example>not foo</example>
      Not,

      /// <summary>
      /// A negative expression
      /// </summary>
      /// <example>-foo</example>
      Negative,

      /// <summary>
      /// An or expression
      /// </summary>
      /// <example>foo or bar</example>
      Or,

      /// <summary>
      /// An and expression.
      /// </summary>
      /// <example>foo and bar</example>
      And,

      /// <summary>
      /// A ternary expression
      /// </summary>
      /// <example>foo ? bar : bah</example>
      Ternary,

      /// <summary>
      /// An in expression
      /// </summary>
      /// <example>2 in [1, 5, 10, 2]</example>
      In,

      /// <summary>
      /// An is equal expression.
      /// </summary>
      /// <example>2 = 5</example>
      IsEqual,

      /// <summary>
      /// A less than expression
      /// </summary>
      /// <example>2 &lt; 3</example>
      LessThan,

      /// <summary>
      /// A greater than expression
      /// </summary>
      /// <example>2 &gt; 5</example>
      GreaterThan,

      /// <summary>
      /// A greater than or equal expression
      /// </summary>
      /// <example>2 &gt;= 5</example>
      GreaterThanOrEqual,

      /// <summary>
      /// A less than or equal expression
      /// </summary>
      /// <example>2 &lt;= 3</example>
      LessThanOrEqual,

      /// <summary>
      /// A member access expression
      /// </summary>
      /// <example>foo.bar</example>
      MemberAccess,

      /// <summary>
      /// A scope literal expression
      /// </summary>
      /// <example>foo::bar</example>
      Scope,

      /// <summary>
      /// A method call expression.
      /// </summary>
      /// <example>foo.bar() or foo()</example>
      MethodCall,

      /// <summary>
      /// An index expression.
      /// </summary>
      /// <example>Foo[0..3]</example>
      Index,

      /// <summary>
      /// A set expression.
      /// </summary>
      /// <example>[1, 2, 3]</example>
      Set

      // ReSharper restore StyleCop.SA1630
      // ReSharper restore StyleCop.SA1631
   }
}