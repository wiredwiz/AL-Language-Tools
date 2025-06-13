#region MIT License
// <copyright company = "Edgerunner.org" file = "TerminalNode.cs">
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
   /// <summary>
   /// Class that represents a terminal syntax node.
   /// Implements the <see cref="Org.Edgerunner.Language.Parsers.ParserRule{TToken, TType}" />
   /// </summary>
   /// <typeparam name="TToken">The type of the t token.</typeparam>
   /// <typeparam name="TType">The type of the t type.</typeparam>
   /// <seealso cref="Org.Edgerunner.Language.Parsers.ParserRule{TToken, TType}" />
   public abstract class TerminalNode<TToken, TType> : ParserRule<TToken, TType>, ITerminalNode<TToken, TType>
   where TToken : IToken
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="TerminalNode{TToken, TType}" /> class.
      /// </summary>
      /// <param name="type">The node type.</param>
      /// <param name="token">The token that this terminal node corresponds to.</param>
      /// <param name="name">The node name.</param>
      /// <remarks>This overload assumes that the start and end positions are both the same symbol token.</remarks>
      protected TerminalNode(TType type, TToken token, string name) : base(type, name)
      {
         Token = token;
      }

      /// <summary>
      /// Gets the terminal token.
      /// </summary>
      /// <value>The terminal token.</value>
      public virtual TToken Token { get; }

      /// <inheritdoc />
      public override TToken Start => Token;

      /// <inheritdoc />
      public override TToken End => Token;
      
      /// <inheritdoc />
      public override string GetText()
      {
         return Token.Value;
      }

      /// <inheritdoc />
      public bool IsError { get; protected set; }
   }
}