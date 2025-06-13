#region MIT License
// <copyright company = "Edgerunner.org" file = "IdentifierTokenizer.cs">
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
using Org.Edgerunner.Buffers;
using Org.Edgerunner.Common;
using Org.Edgerunner.Pooling;

namespace Org.Edgerunner.BC.AL.Language.Lexers
{
   /// <summary>
   /// Class containing tokenizer logic for identifiers.
   /// </summary>
   public static class IdentifierTokenizer
   {
      /// <summary>
      /// Reads an IdentifierToken from a buffer.
      /// </summary>
      /// <param name="buffer">The buffer to read from.</param>
      /// <returns>A new <see cref="IdentifierToken"/>.</returns>
      public static AlToken? ReadIdentifierTokenFromBuffer(ITextBuffer buffer, AlLexer lexer)
      {
         // no valid token so we return a null
         if (buffer.AtEndOfBuffer()) return null;

         // begin our token reading process
         var text = StringBuilderPool.Current.Get();
         var start = buffer.GetBufferPoint();

         text.Append(buffer.Current);
         var quoted = buffer.Current == '"';
         if (!quoted)
         {
            while (buffer.GetNextChar() is '_' or >= 'a' and <= 'z' or >= 'A' and <= 'Z' or >= '0' and <= '9')
               text.Append(buffer.Current);
         }
         else
         {
            while (buffer.GetNextChar() is not '"' and not '\r' and not '\n' and not '\0')
               text.Append(buffer.Current);

            if (buffer.Current != '"')
               return LexerError.PackageError(lexer, text.ToString(), start, buffer.GetBufferPoint(),
                                              "Quoted identifier not terminated correctly");

            text.Append(buffer.Current);
            buffer.GetNextChar();
         }

         return new IdentifierToken(
                                    text.ToString(), 
                                    start, 
                                    buffer.GetBufferPoint(-1), 
                                    EnumCache<ReservedWord>.Contains(text.ToString().ToUpperInvariant()),
                                    EnumCache<Keyword>.Contains(text.ToString().ToUpperInvariant()));
      }
   }
}