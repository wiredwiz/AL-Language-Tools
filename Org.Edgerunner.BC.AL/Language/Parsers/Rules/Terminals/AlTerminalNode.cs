#region MIT License

// <copyright company = "Edgerunner.org" file = "AlTerminalNode.cs">
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

using Org.Edgerunner.BC.AL.Language.Tokens;
using Org.Edgerunner.Language.Parsers;

namespace Org.Edgerunner.BC.AL.Language.Parsers.Rules.Terminals
{
   /// <summary>
   /// Class that represents an AL terminal syntax node.
   /// Implements the <see cref="TerminalNode{AlToken, AlSyntaxNodeType}" />
   /// </summary>
   /// <seealso cref="TerminalNode{AlToken, AlSyntaxNodeType}" />
   public abstract class AlTerminalNode : AlParserRule, ITerminalNode<AlToken, AlSyntaxNodeType>
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="AlTerminalNode" /> class.
      /// </summary>
      /// <param name="type">The node type.</param>
      /// <param name="symbol">The start/end symbol token.</param>
      /// <param name="name">The node name.</param>
      /// <remarks>This overload assumes that the start and end positions are both the same symbol token.</remarks>
      protected AlTerminalNode(AlSyntaxNodeType type, AlToken symbol, string name) : base(type, name)
      {
         Token = symbol;
         Text = symbol.Value;
      }

      /// <summary>
      /// Gets the terminal token.
      /// </summary>
      /// <value>The terminal token.</value>
      public virtual AlToken Token { get; }

      /// <inheritdoc />
      public override AlToken Start => Token;

      /// <inheritdoc />
      public override AlToken End => Token;

      /// <summary>
      /// Gets or sets the text.
      /// </summary>
      /// <value>The text.</value>
      public string Text { get; set; }

      /// <inheritdoc />
      public override string GetText()
      {
         return Text;
      }
   }
}