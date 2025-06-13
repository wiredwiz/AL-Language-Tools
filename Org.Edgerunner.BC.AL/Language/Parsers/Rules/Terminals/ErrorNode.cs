#region MIT License
// <copyright company = "Edgerunner.org" file = "ErrorExpression.cs">
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

using Org.Edgerunner.BC.AL.Language.Tokens;

namespace Org.Edgerunner.BC.AL.Language.Parsers.Rules.Terminals
{
   /// <summary>
   /// Class that represents an error expression.
   /// Implements the <see cref="AlParserRule" />
   /// </summary>
   /// <seealso cref="AlParserRule" />
   public class ErrorNode : AlTerminalNode
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ErrorNode"/> class.
      /// </summary>
      /// <param name="text">The error text.</param>
      /// <param name="token">The start.</param>
      public ErrorNode(string text, AlToken token) : base(AlSyntaxNodeType.Error, token, "Error")
      {
         ErrorText = text;
      }

      /// <summary>
      /// The error text.
      /// </summary>
      protected string ErrorText;

      /// <inheritdoc />
      public override bool IsError => true;

      /// <inheritdoc />
      public override string ToString()
      {
         return ErrorText;
      }
   }
}