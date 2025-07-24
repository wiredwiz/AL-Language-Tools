#region MIT License
// <copyright company = "Edgerunner.org" file = "PreProcessedTokenSource.cs">
// Copyright(c) Thaddeus Ryker 2025
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

using System.Collections.Generic;
using Antlr4.Runtime;

namespace Org.Edgerunner.Language.AL.Parsing.Preprocessing
{
   public class PreProcessedTokenSource : ITokenSource
   {
      public PreProcessedTokenSource(List<IToken> tokens)
      {
         Tokens = tokens;
         TokenIndex = -1;
      }

      public List<IToken> Tokens { get; }

      private IToken CurrentToken => TokenIndex == -1 ? null : Tokens[TokenIndex];

      private int TokenIndex { get; set; }

      public IToken NextToken()
      {
         if (TokenIndex < Tokens.Count)
            return Tokens[++TokenIndex];

         return null;
      }

      public int Line => CurrentToken?.Line ?? 0;

      public int Column => CurrentToken?.Column ?? 0;

      public ICharStream InputStream { get; }

      public string SourceName { get; }

      public ITokenFactory TokenFactory { get; set; }
   }
}