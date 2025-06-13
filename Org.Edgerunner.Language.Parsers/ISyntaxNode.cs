#region MIT License
// <copyright company = "Edgerunner.org" file = "ISyntaxNode.cs">
// Copyright(c) Thaddeus Ryker 2023
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
   /// Interface that defines a syntax node.
   /// </summary>
   /// <typeparam name="TToken">A type of <see cref="IToken" /></typeparam>
   /// <typeparam name="TType">The type of the node.</typeparam>
   public interface ISyntaxNode<out TToken, out TType>
   where TToken : IToken
   {
      /// <summary>
      /// Gets the token at the start of the syntax node.
      /// </summary>
      /// <value>The starting token.</value>
      TToken? Start { get; }

      /// <summary>
      /// Gets the token at the end of the syntax node.
      /// </summary>
      /// <value>The ending token.</value>
      TToken? End { get; }

      /// <summary>
      /// Gets the node type.
      /// </summary>
      /// <value>The node type.</value>
      TType Type { get; }

      /// <summary>
      /// Gets the node name.
      /// </summary>
      /// <value>The node name.</value>
      string Name { get; }

      /// <summary>
      /// Gets a value indicating whether this syntax node is indicating an error.
      /// </summary>
      /// <value><c>true</c> if this node indicates an error; otherwise, <c>false</c>.</value>
      public bool IsError { get; }

      /// <summary>
      /// Gets the text that represents this node.
      /// </summary>
      /// <returns>A <see cref="string"/> containing the text.</returns>
      string GetText();
   }
}