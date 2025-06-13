#region MIT License
// <copyright company = "Edgerunner.org" file = "ExpressionRule.cs">
// Copyright(c)  2023
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

using Org.Edgerunner.BC.AL.Language.Parsers.Rules.Terminals;
using Org.Edgerunner.BC.AL.Language.Tokens;
using Org.Edgerunner.Language.Lexers;

namespace Org.Edgerunner.BC.AL.Language.Parsers.Rules.Code.Source.Expressions
{
   public class ExpressionRule : AlParserRule
   {
      protected ExpressionRule() : base(AlSyntaxNodeType.Expression, "Expression Rule") {}

      /// <summary>
      /// Initializes a new instance of the <see cref="ValueExpressionRule"/> class.
      /// </summary>
      public ExpressionRule(AlSyntaxNodeType type, string name) : base(type, name) {}

      //public override bool Parse(TokenStream<AlToken> tokens, AlParser context)
      //{
      //   var token = tokens.Current;
      //   AlParserRule constructedExpression;

      //   if (tokens.EndOfStream())
      //   {
      //      var last = tokens.Last();
      //      context.GenerateParserError(last, last, Resources.UnexpectedEof);
      //      AddChildNode(new ErrorNode(Resources.UnexpectedEof, last));
      //      return false;
      //   }

      //   if (!ParseBaseExpression(tokens, context, out constructedExpression))
      //   {
      //      var last = tokens.Last();
      //      context.GenerateParserError(last, last, Resources.UnexpectedEof);
      //      AddChildNode(new ErrorNode(Resources.UnexpectedEof, last));
      //      return false;
      //   }
      //   
      //   if (!tokens.TryMoveNext(ref token))
      //   {
      //      AddChildNode(constructedExpression);
      //      return true;
      //   }

      //   while (!tokens.EndOfStream())
      //   {
      //      var result = BuildOuterExpression(tokens, context, ref constructedExpression);
      //      if (!result)
      //         break;
      //   }

      //   AddChildNode(constructedExpression);
      //   return true;
      //}

      //protected virtual bool ParseBaseExpression(TokenStream<AlToken> tokens, AlParser context, out AlParserRule alParserRule)
      //{
      //   var token = tokens.Current;
      //   AlParserRule expression;
      //   bool parsed;
      //   if (token is IdentifierToken { IsReservedWord: true })
      //   {
      //      var errorMessage = $"Encountered unexpected reserved word: \"{token.Value}\"";
      //      context.GenerateParserError(token, token, errorMessage);
      //      alParserRule = new ErrorNode(errorMessage, token);
      //      return false;
      //   }

      //   if (token is SymbolToken { Value: "[" })
      //   {
      //      // we have a set expression
      //      parsed = ParseSetExpression(tokens, context, out expression);
      //      alParserRule = expression;
      //      return parsed;
      //   }

      //   if (token is SymbolToken { Value: "(" })
      //   {
      //      // we have a parentheses expression
      //      parsed = ParseParenthesesExpression(tokens, context, out expression);
      //      alParserRule = expression;
      //      return parsed;
      //   }

      //   expression = new ValueExpressionRule();
      //   parsed = ((ValueExpressionRule)expression).Parse(tokens, context);
      //   alParserRule = expression;
      //   return parsed;
      //}

      //protected virtual bool BuildOuterExpression(TokenStream<AlToken> tokens, AlParser context, ref AlParserRule alParserRule)
      //{
      //   var token = tokens.Current;
      //   var errorMessage = string.Empty;
      //   var parsed = true;
      //   switch ((TokenType)token.TokenType)
      //   {
      //      case TokenType.Identifier:
      //         if (token.IsBinaryOperator)
      //         {
      //            var binExp = new BinaryExpressionRule();
      //            parsed = binExp.AssembleFrom(tokens, context, alParserRule);
      //            alParserRule = binExp;
      //            return parsed;
      //         }

      //         errorMessage = string.Format(Resources.UnexpectedIdentifier, token.Value);
      //         context.GenerateParserError(token, token, errorMessage);
      //         alParserRule = new ErrorNode(errorMessage, token);
      //         return false;
      //      case TokenType.Literal:
      //         // unexpected but not our problem. kick this can up the parser chain
      //         return false;
      //      case TokenType.Symbol:
      //      {

      //         if (token.Value == "[")
      //         {
      //            // we seem to have a indexed expression
      //         }
      //         else if (token.Value == ".")
      //         {
      //            // we seem to have a member access expression
      //         }
      //         else if (token.Value == "(")
      //         {
      //            // we seem to have a method invocation expression
      //         }
      //         else if (token.Value == "::")
      //         {
      //            // we seem to have a scope expression
      //         }
      //         else if (token.Value == "..")
      //         {
      //            // we seem to have a range expression
      //         }
      //         else if (token.IsAssignmentOperator)
      //         {
      //            // we seem to be in an assignment statement, bail out with the expression we have built
      //         }
      //         else if (token.IsComparisonOperator)
      //         {
      //            // we seem to have a comparison expression
      //         }
      //         else if (token.IsBinaryOperator)
      //         {
      //            // we seem to have a non-comparative binary expression
      //         }
      //         else
      //            return false;

      //         break;
      //      }
      //   }

      //   return false;
      //}
   }
}