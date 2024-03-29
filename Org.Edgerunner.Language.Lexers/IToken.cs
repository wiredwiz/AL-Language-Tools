﻿#region MIT License
// <copyright company = "Edgerunner.org" file = "IToken.cs">
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

namespace Org.Edgerunner.Language.Lexers
{
   /// <summary>
   /// Interface representing a language token.
   /// </summary>
   public interface IToken
   {
      /// <summary>
      /// Gets the type of the token.
      /// </summary>
      /// <value>The type of the token.</value>
      int TokenType { get; }

      /// <summary>
      /// Gets the value of the token.
      /// </summary>
      /// <value>The token value.</value>
      string Value { get; }

      /// <summary>
      /// Gets the starting line of the token.
      /// </summary>
      /// <value>The starting line.</value>
      int StartingLine { get; }

      /// <summary>
      /// Gets the ending line number of the token.
      /// </summary>
      /// <value>The ending line number.</value>
      int EndingLine { get; }

      /// <summary>
      /// Gets the starting column position of the token.
      /// </summary>
      /// <value>The starting column position.</value>
      int StartingColumn { get; }

      /// <summary>
      /// Gets the ending column position of the token.
      /// </summary>
      /// <value>The ending column position.</value>
      int EndingColumn { get; }
   }
}