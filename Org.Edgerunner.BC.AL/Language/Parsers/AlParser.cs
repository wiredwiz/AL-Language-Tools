#region MIT License
// <copyright company = "Edgerunner.org" file = "AlParser.cs">
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

using Org.Edgerunner.Language.Lexers;
using Org.Edgerunner.Language.Parsers;
using Org.Edgerunner.BC.AL.Language.Parsers.Rules;
using Org.Edgerunner.BC.AL.Language.Tokens;
using Org.Edgerunner.BC.AL.Language.Parsers.Rules.Terminals;
using System.Xml;

namespace Org.Edgerunner.BC.AL.Language.Parsers
{
   public partial class AlParser : IParser<AlToken, AlSyntaxNodeType>
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="AlParser"/> class.
      /// </summary>
      public AlParser()
      {
         HasErrors = false;
         _State = 0;
      }

      public static readonly Dictionary<AlSyntaxNodeType, string> Rules = new Dictionary<AlSyntaxNodeType, string>
                                                                    {
                                                                       { AlSyntaxNodeType.ArrayDeclaration, "Array Declaration"},
                                                                       { AlSyntaxNodeType.BinaryExpression, "Binary Expression"},
                                                                       { AlSyntaxNodeType.Boolean, "Boolean"},
                                                                       { AlSyntaxNodeType.CodeBlockStatement, "Code Block Statement"},
                                                                       { AlSyntaxNodeType.Date, "Date"},
                                                                       { AlSyntaxNodeType.DateTime, "DateTime"},
                                                                       { AlSyntaxNodeType.Decimal, "Decimal"},
                                                                       { AlSyntaxNodeType.DictionaryDeclaration, "Dictionary Declaration"},
                                                                       { AlSyntaxNodeType.DimensionsDeclaration, "Dimensions Declaration"},
                                                                       { AlSyntaxNodeType.DotNetDeclaration, "DotNet Declaration"},
                                                                       { AlSyntaxNodeType.Error, "Error"},
                                                                       { AlSyntaxNodeType.Expression, "Expression"},
                                                                       { AlSyntaxNodeType.Identifier, "Identifier"},
                                                                       { AlSyntaxNodeType.IndexedExpression, "Indexed Expression"},
                                                                       { AlSyntaxNodeType.Integer, "Integer"},
                                                                       { AlSyntaxNodeType.LabelDeclaration, "Label Declaration"},
                                                                       { AlSyntaxNodeType.LengthDeclaration, "Length Declaration"},
                                                                       { AlSyntaxNodeType.ListDeclaration, "List Declaration"},
                                                                       { AlSyntaxNodeType.MemberAccessExpression, "Member Access Expression"},
                                                                       { AlSyntaxNodeType.ObjectReferenceDeclaration , "Object Reference Declaration"},
                                                                       { AlSyntaxNodeType.OptionValuesDeclaration , "Option Values Declaration"},
                                                                       { AlSyntaxNodeType.ParenthesesExpression , "Parentheses Expression"},
                                                                       { AlSyntaxNodeType.RangeExpression , "Range Expression"},
                                                                       { AlSyntaxNodeType.SetExpression , "Set Expression"},
                                                                       { AlSyntaxNodeType.SimpleExpression , "Simple Expression"},
                                                                       { AlSyntaxNodeType.Statement , "Statement"},
                                                                       { AlSyntaxNodeType.Symbol , "Symbol"},
                                                                       { AlSyntaxNodeType.String , "String"},
                                                                       { AlSyntaxNodeType.Time , "Time"},
                                                                       { AlSyntaxNodeType.VariableDeclaration , "Variable Declaration"},
                                                                       { AlSyntaxNodeType.VariableTypeDeclaration , "Variable Type Declaration"}
                                                                    };

      protected internal readonly List<IErrorListener<AlToken>> ErrorListeners = new List<IErrorListener<AlToken>>();

      protected internal readonly List<ITraceListener> TraceListeners = new List<ITraceListener>();

      private int _State;

      private readonly HashSet<AlToken> _Registry = new HashSet<AlToken>();

      public ParserRule<AlToken, AlSyntaxNodeType>? Result { get; private set; }

      /// <inheritdoc />
      public bool HasErrors { get; private set; }

      /// <inheritdoc />
      public virtual int State
      {
         get => _State;

         set
         {
            if (value == 1)
               HasErrors = true;

            _State = value;
         }
      }

      /// <inheritdoc />
      public bool EnableTracing { get; set; } = false;

      /// <inheritdoc />
      public void AddErrorListener(IErrorListener<AlToken> listener)
      {
         ErrorListeners.Add(listener);
      }

      /// <inheritdoc />
      public void ClearErrorListeners()
      {
         ErrorListeners.Clear();
      }

      /// <inheritdoc />
      public void RemoveErrorListener(IErrorListener<AlToken> listener)
      {
         ErrorListeners.Remove(listener);
      }

      /// <inheritdoc />
      public void AddTraceListener(ITraceListener listener)
      {
         TraceListeners.Add(listener);
      }

      /// <inheritdoc />
      public void ClearTraceListeners()
      {
         TraceListeners.Clear();
      }

      /// <inheritdoc />
      public void RemoveTraceListener(ITraceListener listener)
      {
         TraceListeners.Remove(listener);
      }

      /// <summary>
      /// Parses AL code from tokens in the stream and generates a parser rule tree.
      /// </summary>
      /// <param name="tokens">The tokens.</param>
      /// <returns><c>true</c> if parsing succeeded, <c>false</c> otherwise.</returns>
      public bool ParseSource(TokenStream<AlToken> tokens)
      {
         Result = null;
         HasErrors = false;
         return false;
      }

      /// <inheritdoc />
      public void Reset()
      {
         HasErrors = false;
         State = 0;
      }

      /// <summary>
      /// Gets the name of the rule.
      /// </summary>
      /// <param name="type">The type.</param>
      /// <returns>A string containing the name of the rule or null if unable to find the rule.</returns>
      public string GetRuleName(AlSyntaxNodeType type)
      {
         return Rules[type];
      }

      /// <summary>
      /// Validates that the <see cref="AlToken" /> matches the expected type and value.
      /// </summary>
      /// <param name="token">The token.</param>
      /// <param name="type">The type to match.</param>
      /// <param name="value">The value to match.</param>
      /// <param name="errorMessage">The error message to generate if validation fails.</param>
      /// <returns><c>true</c> if the token passes validation, <c>false</c> otherwise.</returns>
      /// <seealso cref="AlToken" />
      // ReSharper disable once FlagArgument
      // ReSharper disable once TooManyArguments
      protected virtual bool ValidateToken(AlToken? token, TokenType type, string value, string errorMessage)
      {
         if (token == null)
            return false;

         if (token.TokenType != (int)type || token.Value != value)
         {
            if (State == 0) GenerateParserError(token, token, errorMessage);
            State = 1;
            return false;
         }

         State = 0;
         return true;
      }

      /// <summary>
      /// Validates that the <see cref="AlToken" /> matches the expected type and one of the allowed values.
      /// </summary>
      /// <param name="token">The token.</param>
      /// <param name="type">The type to match.</param>
      /// <param name="allowedValues">The allowable values.</param>
      /// <param name="errorMessage">The error message to generate if validation fails.</param>
      /// <returns><c>true</c> if the token passes validation, <c>false</c> otherwise.</returns>
      /// <seealso cref="AlToken" />
      // ReSharper disable once FlagArgument
      // ReSharper disable once TooManyArguments
      protected virtual bool ValidateToken(AlToken? token, TokenType type, IEnumerable<string> allowedValues, string errorMessage)
      {
         if (token == null)
            return false;

         if (token.TokenType != (int)type || allowedValues.Contains(token.Value))
         {
            if (State == 0) GenerateParserError(token, token, errorMessage);
            State = 1;
            return false;
         }

         State = 0;
         return true;
      }

      /// <summary>
      /// Validates that the <see cref="AlToken"/> matches the expected type.
      /// </summary>
      /// <param name="token">The token.</param>
      /// <param name="type">The type to match.</param>
      /// <param name="errorMessage">The error message to use if validation fails.</param>
      /// <returns><c>true</c> if the token passes validation, <c>false</c> otherwise.</returns>
      /// <seealso cref="AlToken"/>
      // ReSharper disable once FlagArgument
      protected virtual bool ValidateToken(AlToken? token, TokenType type, string errorMessage)
      {
         if (token == null)
            return false;

         if (token.TokenType != (int)type)
         {
            if (State == 0) GenerateParserError(token, token, errorMessage);
            State = 1;
            return false;
         }

         State = 0;
         return true;
      }

      /// <summary>
      /// Validates that the <see cref="AlToken"/> is a literal token of the expected literal type.
      /// </summary>
      /// <param name="token">The token.</param>
      /// <param name="type">The token literal type to match.</param>
      /// <param name="errorMessage">The error message to use if validation fails.</param>
      /// <returns><c>true</c> if the token passes validation, <c>false</c> otherwise.</returns>
      /// <remarks>Assumes a token type of Literal in this case</remarks>
      /// <seealso cref="AlToken"/>
      // ReSharper disable once FlagArgument
      protected virtual bool ValidateToken(AlToken? token, LiteralType type, string errorMessage)
      {
         if (token == null)
            return false;

         if (token is not LiteralToken literal)
         {
            if (State == 0) GenerateParserError(token, token, errorMessage);
            State = 1;
            return false;
         }

         // ReSharper disable once ComplexConditionExpression
         if (literal.TokenType != (int)TokenType.Literal || literal.LiteralType != type)
         {
            if (State == 0) GenerateParserError(token, token, errorMessage);
            State = 1;
            return false;
         }

         State = 0;
         return true;
      }

      /// <inheritdoc />
      public virtual void GenerateParserError(AlToken startToken, AlToken endToken, string message)
      {
         HasErrors = true;
         if (_Registry.Contains(startToken))
            return;

         var error = new ParserError(startToken, endToken, message);
         _Registry.Add(startToken);
         foreach (var listener in ErrorListeners) listener.AnnounceError(error);
      }

      /// <inheritdoc />
      public void GenerateTraceEvent(AlToken token, string ruleName, TraceEvent traceEvent)
      {
         foreach (var listener in TraceListeners) listener.AnnounceTraceMessage(token, ruleName, traceEvent);
      }

      protected virtual void AppendErrorNode(AlParserRule parent, string message, AlToken symbol)
      {
         var node = new ErrorNode(message, symbol)
                    {
                       Parent = parent
                    };
         parent.Children.Add(node);
      }

      protected virtual string FormatSetError(string message, IEnumerable<string> allowed, string encountered)
      {
         var enumerable = allowed as string[] ?? allowed.ToArray();
         if (enumerable.Length == 0)
            return message;

         var set = $"\"{enumerable[0]}\"";

         if (enumerable.Length > 1)
         {
            if (enumerable.Length == 2)
               set += $" or \"{enumerable[1]}\"";
            else
            {
               for (int i = 1; i < enumerable.Length - 1; i++) set += $", \"{enumerable[1]}\"";
               set += $" or \"{enumerable[^1]}\"";
            }
         }

         return string.Format(message, set, encountered);
      }

      protected internal void Enter(AlToken token, AlSyntaxNodeType type)
      {
         GenerateTraceEvent(token, GetRuleName(type), TraceEvent.Enter);
      }

      protected internal void Exit(AlToken token, AlSyntaxNodeType type)
      {
         GenerateTraceEvent(token, GetRuleName(type), TraceEvent.Exit);
      }
      protected internal void Consume(AlToken token, AlSyntaxNodeType type)
      {
         GenerateTraceEvent(token, GetRuleName(type), TraceEvent.Consume);
      }

      protected virtual void ScanTillSymbolReached(TokenStream<AlToken> tokens, IEnumerable<string> terminators)
      {
         var enumerable = terminators as string[] ?? terminators.ToArray();
         while (tokens.Current.TokenType != (int)TokenType.Symbol && !enumerable.Contains(tokens.Current.Value))
         {
            if (tokens.EndOfStream())
               break;

            tokens.MoveNext();
         }
      }
   }
}