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

using System;
using System.Collections.Generic;
using Antlr4.Runtime;

namespace Org.Edgerunner.Language.AL.Parsing.Preprocessing
{
   public class PreProcessedTokenSource : ITokenSource
   {
      public PreProcessedTokenSource(ITokenSource source, List<Tuple<int, int>> skipRanges)
      {
         InternalSource = source;
         SkipRanges = skipRanges;
      }

      private ITokenSource InternalSource { get; }

      public List<Tuple<int, int>> SkipRanges { get; set; }

      /// <summary>
      /// Return a
      /// <see cref="T:Antlr4.Runtime.IToken" />
      /// object from your input stream (usually a
      /// <see cref="T:Antlr4.Runtime.ICharStream" />
      /// ). Do not fail/return upon lexing error; keep chewing
      /// on the characters until you get a good one; errors are not passed through
      /// to the parser.
      /// </summary>
      /// <returns>Antlr4.Runtime.IToken.</returns>
      public IToken NextToken()
      {
         if (SkipRanges.Count == 0)
            return InternalSource.NextToken();

         var token = InternalSource.NextToken();

         while (SkipRanges.Count > 0)
         {
            if (token.Line < SkipRanges[0].Item1)
               return token;

            while (token.Line >= SkipRanges[0].Item1 && token.Line <= SkipRanges[0].Item2)
            {
               token = InternalSource.NextToken();
               if (token == null)
                  return null;
            }

            if (token.Line > SkipRanges[0].Item1)
               SkipRanges.RemoveAt(0);
         }

         return token;
      }

      /// <summary>
      /// Get the line number for the current position in the input stream.
      /// </summary>
      /// <value>The line.</value>
      /// <remarks>Get the line number for the current position in the input stream. The
      /// first line in the input is line 1.</remarks>
      public int Line => InternalSource.Line;

      /// <summary>
      /// Get the index into the current line for the current position in the input
      /// stream.
      /// </summary>
      /// <value>The column.</value>
      /// <remarks>Get the index into the current line for the current position in the input
      /// stream. The first character on a line has position 0.</remarks>
      public int Column => InternalSource.Column;

      /// <summary>
      /// Get the
      /// <see cref="T:Antlr4.Runtime.ICharStream" />
      /// from which this token source is currently
      /// providing tokens.
      /// </summary>
      /// <value>The input stream.</value>
      public ICharStream InputStream => InternalSource.InputStream;

      /// <summary>
      /// Gets the name of the underlying input source.
      /// </summary>
      /// <value>The name of the source.</value>
      /// <remarks>Gets the name of the underlying input source. This method returns a
      /// non-null, non-empty string. If such a name is not known, this method
      /// returns
      /// <see cref="F:Antlr4.Runtime.IntStreamConstants.UnknownSourceName" />
      /// .</remarks>
      public string SourceName => InternalSource.SourceName;

      /// <summary>
      /// Set the
      /// <see cref="T:Antlr4.Runtime.ITokenFactory" />
      /// this token source should use for creating
      /// <see cref="T:Antlr4.Runtime.IToken" />
      /// objects from the input.
      /// </summary>
      /// <value>The
      /// <see cref="T:Antlr4.Runtime.ITokenFactory" />
      /// to use for creating tokens.</value>
      public ITokenFactory TokenFactory
      {
         get => InternalSource.TokenFactory;
         set => InternalSource.TokenFactory = value;
      }
   }
}