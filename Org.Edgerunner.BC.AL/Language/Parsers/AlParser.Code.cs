#region MIT License
// <copyright company = "Edgerunner.org" file = "AlParser.Code.cs">
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

using Org.Edgerunner.BC.AL.Language.Aspects;
using Org.Edgerunner.BC.AL.Objects.Code;
using Org.Edgerunner.Language.Lexers;
using Org.Edgerunner.BC.AL.Language.Parsers.Rules;
using Org.Edgerunner.BC.AL.Language.Parsers.Rules.Code.Variables;
using Org.Edgerunner.BC.AL.Language.Parsers.Rules.Terminals;
using Org.Edgerunner.BC.AL.Language.Tokens;
using Org.Edgerunner.BC.AL.Language.Parsers.Rules.Code.Source.Expressions;
using Org.Edgerunner.Language.Parsers;

namespace Org.Edgerunner.BC.AL.Language.Parsers
{
   public partial class AlParser
   {
      /// <summary>
      /// Parses an AL length declaration.
      /// </summary>
      /// <param name="tokens">The tokens to read.</param>
      /// <returns>A new <see cref="AlParserRule"/> instance.</returns>
      [ParserRule(AlSyntaxNodeType.LengthDeclaration)]
      public AlParserRule ParseLengthDeclaration(TokenStream<AlToken> tokens)
      {
         var newRule = new LengthDeclarationRule();

         newRule.AddChildNode(ParseSymbol(tokens, "["));
         newRule.AddChildNode(ParseIntegerLiteral(tokens));
         newRule.AddChildNode(ParseSymbol(tokens, "]"));

         return newRule;
      }

      /// <summary>
      /// Parses an AL array dimensions declaration.
      /// </summary>
      /// <param name="tokens">The tokens to read.</param>
      /// <returns>A new <see cref="AlParserRule"/> instance.</returns>
      [ParserRule(AlSyntaxNodeType.DimensionsDeclaration)]
      public AlParserRule ParseArrayDimensionsDeclaration(TokenStream<AlToken> tokens)
      {
         var newRule = new DimensionsDeclarationRule();

         newRule.AddChildNode(ParseSymbol(tokens, "["));
         // ReSharper disable once ExceptionNotDocumented
         ParseRepeatingDelimitedExpression(tokens, newRule, ",", "]", ParseIntegerLiteral);
         newRule.AddChildNode(ParseSymbol(tokens, "]"));

         return newRule;
      }

      /// <summary>
      /// Parses an AL object declaration.
      /// </summary>
      /// <param name="tokens">The tokens to read.</param>
      /// <returns>A new <see cref="AlParserRule"/> instance.</returns>
      [ParserRule(AlSyntaxNodeType.ObjectReferenceDeclaration)]
      public AlParserRule ParseVariableObjectDeclaration(TokenStream<AlToken> tokens)
      {
         var token = tokens.Current;
         var newRule = new ObjectReferenceDeclarationRule();
         var errorMessage = $"Expected an object number or name identifier, instead encountered ${token.Value}";

         // Look for an object integer number
         var isInteger = token.TokenType == (int)TokenType.Literal && token is LiteralToken { LiteralType: LiteralType.Integer };
         if (isInteger)
            newRule.AddChildNode(ParseIntegerLiteral(tokens));
         else if (token.TokenType == (int)TokenType.Identifier)
            // if we didn't have a number, but instead an identifier, then we are still good
            newRule.AddChildNode(ParseIdentifier(tokens));
         else
         {
            GenerateParserError(token, token, errorMessage);
            newRule.AddChildNode(new ErrorNode(errorMessage, token));
         }

         return newRule;
      }

      /// <summary>
      /// Parses an AL array variable declaration.
      /// </summary>
      /// <param name="tokens">The tokens to read.</param>
      /// <returns>A new <see cref="AlParserRule"/> instance.</returns>
      [ParserRule(AlSyntaxNodeType.ArrayDeclaration)]
      public AlParserRule ParseArrayVariableDeclaration(TokenStream<AlToken> tokens)
      {
         var newRule = new ArrayDeclarationRule();

         newRule.AddChildNode(ParseIdentifier(tokens));
         
         // look for length declaration
         newRule.AddChildNode(ParseArrayDimensionsDeclaration(tokens));

         // Look for identifier
         newRule.AddChildNode(ParseIdentifier(tokens, "of"));

         // Now parse our array sub type declaration
         newRule.AddChildNode(ParseVariableType(tokens));

         return newRule;
      }

      /// <summary>
      /// Parses an AL option values declaration.
      /// </summary>
      /// <param name="tokens">The tokens to read.</param>
      /// <returns>A new <see cref="AlParserRule"/> instance.</returns>
      [ParserRule(AlSyntaxNodeType.OptionValuesDeclaration)]
      public AlParserRule ParseOptionValuesDeclaration(TokenStream<AlToken> tokens)
      {
         var token = tokens.Current;
         var newRule = new OptionValuesDeclarationRule();

         var allowed = new[] { ",", ";" };
         var message = FormatSetError(Resources.ExpectedSymbolFromSet, allowed, token.Value);
         while (token.TokenType != (int)TokenType.Symbol || token.Value != ";")
         {
            // read values
            bool noMatch = true;

            // check if we have a semi-colon
            var tokenValidates = ValidateToken(token, TokenType.Symbol, ",", message);
            if (tokenValidates)
            {
               noMatch = false;
               newRule.AddChildNode(new SymbolRule(token));
            }

            // check if we have an identifier
            tokenValidates = ValidateToken(token, TokenType.Identifier, string.Format(Resources.ExpectedOptionValue, token.Value));
            if (tokenValidates)
            {
               newRule.AddChildNode(new IdentifierRule(token));
               noMatch = false;
            }

            // If we matched neither, move the token stream position forward and keep trying to match
            if (noMatch)
            {
               // We have reached the end of the token stream, so we bail out
               if (tokens.Position == tokens.Size - 1)
                  break;

               // The next token is a semi-colon, so we are done parsing option values
               if (tokens.Next()?.TokenType == (int)TokenType.Symbol && tokens.Current.Value == ";")
                  break;
            }
            token = tokens.Current;
         }

         return newRule;
      }


      /// <summary>
      /// Parses an AL list variable declaration.
      /// </summary>
      /// <param name="tokens">The tokens to read.</param>
      /// <returns>A new <see cref="AlParserRule"/> instance.</returns>
      [ParserRule(AlSyntaxNodeType.ListDeclaration)]
      public AlParserRule ParseListDeclaration(TokenStream<AlToken> tokens)
      {
            var newRule = new ListDeclarationRule();

            newRule.AddChildNode(ParseIdentifier(tokens, "list"));
            newRule.AddChildNode(ParseIdentifier(tokens, "of"));
            newRule.AddChildNode(ParseSymbol(tokens, "["));
            newRule.AddChildNode(ParseVariableType(tokens));
            newRule.AddChildNode(ParseSymbol(tokens, "]"));
            
            return newRule;
      }

      /// <summary>
      /// Parses an AL dictionary variable declaration.
      /// </summary>
      /// <param name="tokens">The tokens to read.</param>
      /// <returns>A new <see cref="AlParserRule"/> instance.</returns>
      [ParserRule(AlSyntaxNodeType.DictionaryDeclaration)]
      public AlParserRule ParseDictionaryDeclaration(TokenStream<AlToken> tokens)
      {
         var newRule = new ListDeclarationRule();

         newRule.AddChildNode(ParseIdentifier(tokens, "dictionary"));
         newRule.AddChildNode(ParseIdentifier(tokens, "of"));
         newRule.AddChildNode(ParseSymbol(tokens, "["));
         newRule.AddChildNode(ParseVariableType(tokens));
         newRule.AddChildNode(ParseSymbol(tokens, ","));
         newRule.AddChildNode(ParseVariableType(tokens));
         newRule.AddChildNode(ParseSymbol(tokens, "]"));
            
         return newRule;
      }

      /// <summary>
      /// Parses the variable type declaration.
      /// </summary>
      /// <param name="tokens">The token stream.</param>
      /// <returns>A new <see cref="AlParserRule"/> instance.</returns>
      [ParserRule(AlSyntaxNodeType.VariableTypeDeclaration)]
      public AlParserRule ParseVariableType(TokenStream<AlToken> tokens)
      {
         var token = tokens.Current;
         var newRule = new VariableTypeDeclarationRule();
         var isArray = false;
         object? varType = null;

         if (token.TokenType == (int)TokenType.Identifier && token.Value.ToLowerInvariant() == "array")
            isArray = true;
         else if (token.TokenType != (int)TokenType.Identifier ||
                  !Enum.TryParse(typeof(VariableType), token.Value, true, out varType))
         {
            var errorMessage = $"Expected a valid AL data type, but instead encountered: {token.Value}";
            GenerateParserError(token, token, errorMessage);
            newRule.AddChildNode(new ErrorNode(errorMessage, token));

            return newRule;
         }

         if (isArray)
         {
            // Parse an array declaration e.g. array[5] of text[20]
            newRule.AddChildNode(ParseArrayVariableDeclaration(tokens));
            return newRule;
         }

         switch (varType)
         {
            case VariableType.BigInteger:
            case VariableType.BigText:
            case VariableType.Blob:
            case VariableType.Boolean:
            case VariableType.Byte:
            case VariableType.Char:
            case VariableType.CompanyProperty:
            case VariableType.Database:
            case VariableType.DataTransfer:
            case VariableType.Date:
            case VariableType.DateFormula:
            case VariableType.DateTime:
            case VariableType.Debugger:
            case VariableType.Decimal:
            case VariableType.Dialog:
            case VariableType.Duration:
            case VariableType.ErrorInfo:
            case VariableType.FieldRef:
            case VariableType.File:
            case VariableType.FilterPageBuilder:
            case VariableType.Guid:
            case VariableType.HttpClient:
            case VariableType.HttpHeaders:
            case VariableType.HttpRequestMessage:
            case VariableType.HttpResponseMessage:
            case VariableType.InStream:
            case VariableType.Integer:
            case VariableType.IsolatedStorage:
            case VariableType.JsonArray:
            case VariableType.JsonObject:
            case VariableType.JsonToken:
            case VariableType.JsonValue:
            case VariableType.KeyRef:
            case VariableType.Media:
            case VariableType.MediaSet:
            case VariableType.ModuleDependencyInfo:
            case VariableType.ModuleInfo:
            case VariableType.NavApp:
            case VariableType.None:
            case VariableType.Notification:
            case VariableType.NumberSequence:
            case VariableType.OutStream:
            case VariableType.ProductName:
            case VariableType.RecordId:
            case VariableType.RecordRef:
            case VariableType.RequestPage:
            case VariableType.SecretText:
            case VariableType.Session:
            case VariableType.SessionInformation:
            case VariableType.SessionSettings:
            case VariableType.System:
            case VariableType.TaskScheduler:
            case VariableType.TestAction:
            case VariableType.TestField:
            case VariableType.TestFilterField:
            case VariableType.TestPage:
            case VariableType.TestPart:
            case VariableType.TestPageRequest:
            case VariableType.TextBuilder:
            case VariableType.TextConst:
            case VariableType.Time:
            case VariableType.Variant:
            case VariableType.Version:
            case VariableType.WebServiceActionContext:
            case VariableType.XmlAttribute:
            case VariableType.XmlAttributeCollection:
            case VariableType.XmlCData:
            case VariableType.XmlDeclaration:
            case VariableType.XmlDocument:
            case VariableType.XmlDocumentType:
            case VariableType.XmlElement:
            case VariableType.XmlNamespaceManager:
            case VariableType.XmlNameTable:
            case VariableType.XmlNode:
            case VariableType.XmlNodeList:
            case VariableType.Xmlport:
            case VariableType.XmlProcessingInstruction:
            case VariableType.XmlReadOptions:
            case VariableType.XmlText:
            case VariableType.XmlWriteOptions:
            case VariableType.Action:
            case VariableType.AuditCategory:
            case VariableType.ClientType:
            case VariableType.CommitBehavior:
            case VariableType.DataClassification:
            case VariableType.DataScope:
            case VariableType.DefaultLayout:
            case VariableType.ErrorBehavior:
            case VariableType.ErrorType:
            case VariableType.ExecutionContext:
            case VariableType.ExecutionMode:
            case VariableType.FieldClass:
            case VariableType.FieldType:
            case VariableType.InherentPermissionsScope:
            case VariableType.IsolationLevel:
            case VariableType.NotificationScope:
            case VariableType.ObjectType:
            case VariableType.PageBackgroundTaskErrorLevel:
            case VariableType.PermissionObjectType:
            case VariableType.ReportFormat:
            case VariableType.ReportLayoutType:
            case VariableType.SecurityFilter:
            case VariableType.SecurityOperationResult:
            case VariableType.TableConnectionType:
            case VariableType.TelemetryScope:
            case VariableType.TestPermissions:
            case VariableType.TextEncoding:
            case VariableType.TransactionModel:
            case VariableType.TransactionType:
            case VariableType.Verbosity:
            case VariableType.WebServiceActionResultCode:
               {
                  // we have nothing extra to do, these variable types have no extra declaration
                  newRule.AddChildNode(ParseIdentifier(tokens));
                  break;
               }
            case VariableType.Code:
            case VariableType.Text:
               {
                  // Parse a length declaration e.g. [20]
                  newRule.AddChildNode(ParseIdentifier(tokens));
                  newRule.AddChildNode(ParseLengthDeclaration(tokens));
                  break;
               }
            case VariableType.Record:
            case VariableType.Codeunit:
            case VariableType.Enum:
            case VariableType.Page:
            case VariableType.Query:
            case VariableType.Report:
               {
                  newRule.AddChildNode(ParseIdentifier(tokens));

                  // Parse an object declaration e.g. 20 or "Customer"
                  newRule.AddChildNode(ParseVariableObjectDeclaration(tokens));
                  break;
               }
            case VariableType.Option:
               {
                  newRule.AddChildNode(ParseIdentifier(tokens));
                  
                  // parse an option value declaration e.g. foo,bar,bah
                  newRule.AddChildNode(ParseOptionValuesDeclaration(tokens));
                  break;
               }
            case VariableType.Dictionary:
               {
                  break;
               }
            case VariableType.List:
               {
                  // parse a list declaration
                  newRule.AddChildNode(ParseListDeclaration(tokens));
                  break;
               }
            case VariableType.DotNet:
               {
                  break;
               }
            case VariableType.Label:
               {
                  break;
               }
            default:
               return newRule;
         }

         return newRule;
      }

      /// <summary>
      /// Parses the variable declaration.
      /// </summary>
      /// <param name="tokens">The token stream.</param>
      /// <returns>A new <see cref="AlParserRule"/> instance.</returns>
      [ParserRule(AlSyntaxNodeType.VariableDeclaration)]
      public AlParserRule ParseVariableDeclaration(TokenStream<AlToken> tokens)
      {
         var newRule = new VariableDeclarationRule();


         newRule.AddChildNode(ParseIdentifier(tokens));
         newRule.AddChildNode(ParseSymbol(tokens, ":"));
         newRule.AddChildNode(ParseVariableType(tokens));

         newRule.AddChildNode(ParseSymbol(tokens, ";"));
         return newRule;
      }

      /// <summary>
      /// Parses a simple expression.
      /// </summary>
      /// <param name="tokens">The token stream.</param>
      /// <returns>A new <see cref="AlParserRule"/> instance.</returns>
      public AlParserRule ParseSimpleExpression(TokenStream<AlToken> tokens)
      {
         var token = tokens.Current;
         AlParserRule expression;
         if (token is IdentifierToken { IsReservedWord: true })
         {
            var errorMessage = $"Encountered unexpected reserved word: \"{token.Value}\"";
            GenerateParserError(token, token, errorMessage);
            return new ErrorNode(errorMessage, token);
         }

         if (token is SymbolToken { Value: "[" })
         {
            // we have a set expression
            
         }

         if (token is SymbolToken { Value: "(" })
         {
            // we have a parentheses expression
            
         }

         return ParseValueExpression(tokens);
      }

      /// <summary>
      /// Parses a value expression.
      /// </summary>
      /// <param name="tokens">The token stream.</param>
      /// <returns>A new <see cref="AlParserRule"/> instance.</returns>
      public AlParserRule ParseValueExpression(TokenStream<AlToken> tokens)
      {
         var token = tokens.Current;

         switch ((TokenType)token.TokenType)
         {
            case TokenType.Identifier:
               return ParseIdentifier(tokens);
            case TokenType.Literal:
            {
               var literal = token as LiteralToken;
               switch (literal!.LiteralType)
               {
                  case LiteralType.Boolean:
                     return ParseBooleanLiteral(tokens);
                  case LiteralType.DateTime:
                     return ParseDatetimeLiteral(tokens);
                  case LiteralType.Date:
                     return ParseDateLiteral(tokens);
                  case LiteralType.Time:
                     return ParseTimeLiteral(tokens);
                  case LiteralType.Decimal:
                     return ParseDecimalLiteral(tokens);
                  case LiteralType.Integer:
                     return ParseIntegerLiteral(tokens);
                  case LiteralType.String:
                     return ParseStringLiteral(tokens);
               }

               break;
            }
         }

         var errorMessage = $"Invalid expression, encountered unexpected symbol: \"{token.Value}\"";
         GenerateParserError(token, token, errorMessage);
         return new ErrorNode(errorMessage, token);
      }
   }
}