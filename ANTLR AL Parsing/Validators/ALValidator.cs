#region MIT License
// <copyright company = "Edgerunner.org" file = "ALValidator.cs">
// Copyright(c) Thaddeus Ryker 2025
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

using System.Collections.Generic;
using Antlr4.Runtime;
using Grammar.AL.Antlr;
using Org.Edgerunner.Language.AL.Parsing.Messaging;

namespace Org.Edgerunner.Language.AL.Parsing.Validators
{
   // ReSharper disable once InconsistentNaming
   /// <summary>
   /// Class that represents a validator for an AL language parse tree.
   /// </summary>
   public class ALValidator : ALParserBaseListener
   {
      public List<ErrorMessage> Messages { get; set; }


      public override void ExitIndexExpression(Grammar.AL.Antlr.ALParser.IndexExpressionContext context)
      {
         var ok = true;
         Grammar.AL.Antlr.ALParser.ExpressionContext expr = context.expr;

         if (expr != null && !IsValidNumericExpression(expr))
         {
            // TODO: Create an error guide of some kind

            //ok = false;
            //var start = expr.start;
            //var stop = expr.stop ?? start;
            //if (start != null)
            //   errGuide = new ErrorGuide(start.Line, start.Column, stop.Line, stop.Column + 1);
         }

         // TODO: Add validation failure code

         //if (!ok)
         //   Errors.Add(new ParseMessage(Document,
         //                               context.lhs.Start.Line,
         //                               context.lhs.Start.Column + 1,
         //                               "Parser", "Invalid expression on left hand side of assignment operation",
         //                               errGuide));

         base.ExitIndexExpression(context);
      }

      /// <summary>
      /// Determines whether the specified expression is a valid numeric expression.
      /// </summary>
      /// <param name="expression">The expression to validate.</param>
      /// <returns><c>true</c> if the specified expression is a valid numeric expression; otherwise, <c>false</c>.</returns>
      private static bool IsValidNumericExpression(Grammar.AL.Antlr.ALParser.ExpressionContext expression)
      {
         UnWrapParenthesis(ref expression);

         // ReSharper disable once ComplexConditionExpression
         if (expression is Grammar.AL.Antlr.ALParser.AddExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.SubtractExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.MultiplyExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.DivideExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.ModulusExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.IntegerDivideExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.FloatLiteralExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.IntegerLiteralExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.IdentifierExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.NegativeExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.SystemEnumerationLiteralExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.ScopeExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.MemberAccessExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.FunctionCallExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.MethodCallExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.TernaryExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.IndexExpressionContext)
            return true;

         return false;
      }

      /// <summary>
      /// Determines whether the specified expression is a valid boolean expression.
      /// </summary>
      /// <param name="expression">The expression to validate.</param>
      /// <returns><c>true</c> if the specified expression is a valid boolean expression; otherwise, <c>false</c>.</returns>
      private static bool IsValidBooleanExpression(Grammar.AL.Antlr.ALParser.ExpressionContext expression)
      {
         UnWrapParenthesis(ref expression);

         // ReSharper disable once ComplexConditionExpression
         if (expression is Grammar.AL.Antlr.ALParser.BooleanLiteralExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.ComparisonExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.AndExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.XorExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.OrExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.NotExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.GuiAllowedFunctionExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.IdentifierExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.MemberAccessExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.FunctionCallExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.MethodCallExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.TernaryExpressionContext ||
             expression is Grammar.AL.Antlr.ALParser.IndexExpressionContext)
            return true;

         return false;
      }

      private static void UnWrapParenthesis(ref Grammar.AL.Antlr.ALParser.ExpressionContext expression)
      {
         while (expression is Grammar.AL.Antlr.ALParser.ParenthesisExpressionContext paren)
            expression = paren.expr;
      }
   }
}