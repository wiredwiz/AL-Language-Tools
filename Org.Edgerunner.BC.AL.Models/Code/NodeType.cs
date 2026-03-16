#region MIT License
// <copyright company = "Edgerunner.org" file = "NodeType.cs">
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

namespace Org.Edgerunner.BC.AL.Models.Code
{
   /// <summary>
   /// Enum representing an AL syntax node type.
   /// </summary>
   public enum NodeType
   {
      // ReSharper disable StyleCop.SA1630
      // ReSharper disable StyleCop.SA1631

      /// <summary>
      /// A string literal expression
      /// </summary>
      /// <example>"foo bar"</example>
      StringLiteralExpression,

      /// <summary>
      /// An integer literal expression
      /// </summary>
      /// <example>5</example>
      IntegerLiteralExpression,

      /// <summary>
      /// A float literal expression
      /// </summary>
      /// <example>2.5</example>
      FloatLiteralExpression,

      /// <summary>
      /// A Date literal expression
      /// </summary>
      /// <example>20020526D</example>
      DateLiteralExpression,

      /// <summary>
      /// A Time literal expression
      /// </summary>
      /// <example>120215.333T</example>
      TimeLiteralExpression,

      /// <summary>
      /// A DateTime literal expression
      /// </summary>
      /// <example>0DT</example>
      DateTimeLiteralExpression,

      /// <summary>
      /// A boolean literal expression
      /// </summary>
      /// <example>true</example>
      BooleanLiteralExpression,

      /// <summary>
      /// A variable expression
      /// </summary>
      VariableExpression,

      /// <summary>
      /// An add expression
      /// </summary>
      /// <example>5 + 2</example>
      AddExpression,

      /// <summary>
      /// A subtract expression
      /// </summary>
      /// <example>5 - 2</example>
      SubtractExpression,

      /// <summary>
      /// A multiply expression
      /// </summary>
      /// <example>5 * 2</example>
      MultiplyExpression,

      /// <summary>
      /// A divide expression
      /// </summary>
      /// <example>5 / 2</example>
      DivideExpression,

      /// <summary>
      /// A modulus expression
      /// </summary>
      /// <example>5 % 2</example>
      ModulusExpression,

      /// <summary>
      /// A not expression
      /// </summary>
      /// <example>not foo</example>
      NotExpression,

      /// <summary>
      /// A negative expression
      /// </summary>
      /// <example>-foo</example>
      NegativeExpression,

      /// <summary>
      /// An or expression
      /// </summary>
      /// <example>foo or bar</example>
      OrExpression,

      /// <summary>
      /// An and expression.
      /// </summary>
      /// <example>foo and bar</example>
      AndExpression,

      /// <summary>
      /// A ternary expression
      /// </summary>
      /// <example>foo ? bar : bah</example>
      TernaryExpression,

      /// <summary>
      /// An in expression
      /// </summary>
      /// <example>2 in [1, 5, 10, 2]</example>
      InRangeExpression,

      /// <summary>
      /// An is equal expression.
      /// </summary>
      /// <example>2 = 5</example>
      IsEqualExpression,

      /// <summary>
      /// A less than expression
      /// </summary>
      /// <example>2 &lt; 3</example>
      LessThanExpression,

      /// <summary>
      /// A greater than expression
      /// </summary>
      /// <example>2 &gt; 5</example>
      GreaterThanExpression,

      /// <summary>
      /// A greater than or equal expression
      /// </summary>
      /// <example>2 &gt;= 5</example>
      GreaterThanOrEqualExpression,

      /// <summary>
      /// A less than or equal expression
      /// </summary>
      /// <example>2 &lt;= 3</example>
      LessThanOrEqualExpression,

      /// <summary>
      /// A member access expression
      /// </summary>
      /// <example>foo.bar</example>
      MemberAccessExpression,

      /// <summary>
      /// A scope literal expression
      /// </summary>
      /// <example>foo::bar</example>
      ScopeExpression,

      /// <summary>
      /// A method call expression.
      /// </summary>
      /// <example>foo.bar() or foo()</example>
      MethodCallExpression,

      /// <summary>
      /// An index expression.
      /// </summary>
      /// <example>Foo[0..3]</example>
      IndexExpression,

      /// <summary>
      /// A set expression.
      /// </summary>
      /// <example>[1, 2, 3]</example>
      SetExpression,

      /// <summary>
      /// An add assignment expression
      /// </summary>
      /// <example>a += 4</example>
      AddAssignExpression,

      /// <summary>
      /// A subtract assignment expression
      /// </summary>
      /// <example>a -= 4</example>
      SubtractAssignExpression,

      /// <summary>
      /// An multiply assignment expression
      /// </summary>
      /// <example>a *= 4</example>
      MultiplyAssignExpression,

      /// <summary>
      /// A divide assignment expression
      /// </summary>
      /// <example>a /= 4</example>
      DivideAssignExpression,

      /// <summary>
      /// An assignment expression
      /// </summary>
      /// <example>a := 4</example>
      AssignExpression,

      /// <summary>
      /// A statement
      /// </summary>
      Statement,

      /// <summary>
      /// A statement block
      /// </summary>
      StatementBlock,
      
      /// <summary>
      /// A With statement
      /// </summary>
      WithStatement,

      /// <summary>
      /// An If statement
      /// </summary>
      IfStatement,

      /// <summary>
      /// A Case statement
      /// </summary>
      CaseStatement,

      /// <summary>
      /// A For statement
      /// </summary>
      ForStatement,

      /// <summary>
      /// A ForEach statement
      /// </summary>
      ForEachStatement,

      /// <summary>
      /// A While statement
      /// </summary>
      WhileStatement,
      
      /// <summary>
      /// A RepeatUntil statement
      /// </summary>
      RepeatUntilStatement,

      /// <summary>
      /// A Break statement
      /// </summary>
      BreakStatement,

      /// <summary>
      /// A Continue statement
      /// </summary>
      ContinueStatement

      // ReSharper restore StyleCop.SA1630
      // ReSharper restore StyleCop.SA1631
   }
}