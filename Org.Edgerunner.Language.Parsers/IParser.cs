#region MIT License
// <copyright company = "Edgerunner.org" file = "IParser.cs">
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

namespace Org.Edgerunner.Language.Parsers
{
   public interface IParser<TToken, TType>
   where TToken : IToken
   {
      /// <summary>
      /// Gets the result of parsing.
      /// </summary>
      /// <value>The resulting parser rule tree.</value>
      ParserRule<TToken, TType>? Result { get; }

      /// <summary>
      /// Gets a value indicating whether this instance has errors after the last parse attempt.
      /// </summary>
      /// <value><c>true</c> if this instance has errors; otherwise, <c>false</c>.</value>
      bool HasErrors { get; }

      /// <summary>
      /// Gets or sets the parser state.
      /// </summary>
      /// <value>The state.</value>
      int State { get; set; }

      /// <summary>
      /// Gets or sets a value indicating whether enable parser tracing.
      /// </summary>
      /// <value><c>true</c> if tracing is enabled; otherwise, <c>false</c>.</value>
      bool EnableTracing { get; set; }

      /// <summary>
      /// Adds a listener to be notified of parser errors.
      /// </summary>
      /// <param name="listener">The listener.</param>
      void AddErrorListener(IErrorListener<TToken> listener);

      /// <summary>
      /// Removes all error listeners.
      /// </summary>
      void ClearErrorListeners();

      /// <summary>
      /// Removes the error listener.
      /// </summary>
      /// <param name="listener">The listener.</param>
      void RemoveErrorListener(IErrorListener<TToken> listener);

      /// <summary>
      /// Adds a listener to be notified of parser tracing.
      /// </summary>
      /// <param name="listener">The listener.</param>
      void AddTraceListener(ITraceListener listener);

      /// <summary>
      /// Removes all trace listeners.
      /// </summary>
      void ClearTraceListeners();

      /// <summary>
      /// Removes the trace listener.
      /// </summary>
      /// <param name="listener">The listener.</param>
      void RemoveTraceListener(ITraceListener listener);

      /// <summary>
      /// Generates a parser error and announces it.
      /// </summary>
      /// <param name="startToken">The start token.</param>
      /// <param name="endToken">The end token.</param>
      /// <param name="message">The message to display in the error.</param>
      void GenerateParserError(TToken startToken, TToken endToken, string message);

      /// <summary>
      /// Generates a parser trace event and announces it.
      /// </summary>
      /// <param name="token">The token.</param>
      /// <param name="ruleName">The parser rule name.</param>
      /// <param name="traceEvent">The trace event type.</param>
      void GenerateTraceEvent(TToken token, string ruleName, TraceEvent traceEvent);

      /// <summary>
      /// Parses tokens from the stream into an object of some kind.
      /// </summary>
      /// <param name="tokens">The tokens.</param>
      /// <returns><c>true</c> if parsing succeeded, <c>false</c> otherwise.</returns>
      bool ParseSource(TokenStream<TToken> tokens);

      /// <summary>
      /// Resets this instance.
      /// </summary>
      void Reset();
   }
}