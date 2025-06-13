#region MIT License
// <copyright company = "Edgerunner.org" file = "IParserContext.cs">
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
using Org.Edgerunner.Pooling;

namespace Org.Edgerunner.Language.Parsers
{
   /// <summary>
   /// Class that represents a parser rule.
   /// Implements the <see cref="Org.Edgerunner.Language.Parsers.ITree" />.
   /// Implements the <see cref="Org.Edgerunner.Language.Parsers.ISyntaxNode{TToken, TType}" />.
   /// </summary>
   /// <typeparam name="TToken">A type of <see cref="IToken" /></typeparam>
   /// <typeparam name="TType">The type of the parser rule.</typeparam>
   /// <seealso cref="Org.Edgerunner.Language.Parsers.ITree" />
   /// <seealso cref="Org.Edgerunner.Language.Parsers.ISyntaxNode{TToken, TType}" />
   /// <seealso cref="IToken" />
   public abstract class ParserRule<TToken, TType> : ITree, ISyntaxNode<TToken, TType>
   where TToken : IToken
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ParserRule{TToken,TType}" /> class.
      /// </summary>
      /// <param name="type">The parser rule type.</param>
      /// <param name="name">The rule name.</param>
      protected ParserRule(TType type, string name)
      {
         Type = type;
         Name = name;
      }
      
      /// <inheritdoc />
      public virtual ITree? Parent { get; set; }

      /// <inheritdoc />
      public virtual IList<ITree> Children { get; } = new List<ITree>();

      /// <inheritdoc />
      public string Name { get; }

      /// <inheritdoc />
      public ITree AddChildNode(ITree node)
      {
         node.Parent = this;
         Children.Add(node);
         return node;
      }

      /// <inheritdoc />
      public ITree RemoveChildNode(ITree node)
      {
         node.Parent = null;
         Children.Remove(node);
         return node;
      }

      /// <inheritdoc />
      public virtual TToken? Start
      {
         get
         {
            if (Children.FirstOrDefault() is ISyntaxNode<TToken, TType> node)
               return node.Start;

            return default;
         }
      }

      /// <inheritdoc />
      public virtual TToken? End
      {
         get
         {
            if (Children.LastOrDefault() is ISyntaxNode<TToken, TType> node)
               return node.End;

            return default;
         }
      }

      /// <inheritdoc />
      public TType Type { get; }

      /// <inheritdoc />
      public virtual bool IsError => false;

      /// <summary>
      /// Gets a value indicating whether this syntax node or any of its children is an error.
      /// </summary>
      /// <value><c>true</c> if this node contains errors; otherwise, <c>false</c>.</value>
      public virtual bool HasErrors { get; } = false;

      /// <inheritdoc />
      public virtual string GetText()
      {
         switch (Children.Count)
         {
            case 0:
               return string.Empty;
            case 1:
               return ((ISyntaxNode<TToken, TType>)Children[0]).GetText();
            default:
            {
               var text = StringBuilderPool.Current.Get();
               foreach (var child in Children) text.Append(((ISyntaxNode<TToken, TType>)child).GetText());
               return text.ToString();
            }
         }
      }

      /// <inheritdoc />
      public override string ToString()
      {
         return GetText();
      }
   }
}