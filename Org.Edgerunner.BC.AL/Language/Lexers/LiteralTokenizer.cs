#region MIT License
// <copyright company = "Edgerunner.org" file = "LiteralTokenizer.cs">
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
using Org.Edgerunner.Common.Extensions;
using Org.Edgerunner.Pooling;

namespace Org.Edgerunner.BC.AL.Language.Lexers
{
   public static class LiteralTokenizer
   {
      /// <summary>
      /// Reads a StringToken from a buffer.
      /// </summary>
      /// <param name="buffer">The buffer to read from.</param>
      /// <param name="lexer">The AL lexer.</param>
      /// <returns>A new <see cref="LiteralToken" /> or <see cref="ErrorToken" />.</returns>
      public static AlToken? ReadStringLiteralTokenFromBuffer(ITextBuffer buffer, AlLexer lexer)
      {
         // no valid token so we return a null
         if (buffer.AtEndOfBuffer()) return null;

         // begin our token reading process
         var text = StringBuilderPool.Current.Get();
         var start = buffer.GetBufferPoint();
         BufferPoint end;

         text.Append(buffer.Current);
         while (true)
         {
            // fetch the next character and then make sure we aren't at the end of buffer.
            var tk = buffer.GetNextChar();
            if (buffer.AtEndOfBuffer())
            {
               // we have reached end of buffer, thus our string is invalid, we return an error token instead
               return LexerError.PackageError(lexer, text.ToString(), start, buffer.GetBufferPoint(-1),
                                              "String reached end of file without termination");
            }

            // check for invalid line breaks in the string
            if (tk == '\n' || tk == '\r')
               return LexerError.PackageError(lexer, text.ToString(), start, buffer.GetBufferPoint(-1),
                                              "String reached end of line without termination");

            // append the currently read token and check for string termination character
            text.Append(tk);
            if (tk == '\'')
            {
               // check to make sure if the termination character is actually being escaped and if not, break out of our loop since we have reached the end of the string
               if (buffer.PeekChar() != '\'')
               {
                  end = buffer.GetBufferPoint();
                  break;
               }

               text.Append(buffer.GetNextChar());
            }
         }

         buffer.GetNextChar();
         return new LiteralToken(text.ToString(), start, end, LiteralType.String);
      }

      /// <summary>
      /// Reads a numeric based LiteralToken from a buffer.
      /// </summary>
      /// <param name="buffer">The buffer to read from.</param>
      /// <param name="lexer">The AL lexer.</param>
      /// <returns>A new <see cref="LiteralToken" /> or <see cref="ErrorToken" />.</returns>
      // ReSharper disable once MethodTooLong
      public static AlToken? ReadNumericBasedLiteralFromBuffer(ITextBuffer buffer, AlLexer lexer)
      {
         // no valid token so we return a null
         if (buffer.AtEndOfBuffer()) return null;

         // begin our token reading process
         var text = string.Empty;
         var hasDecimal = false;
         var start = buffer.GetBufferPoint();

         if (buffer.Current == '-')
         {
            text += buffer.Current;
            buffer.GetNextChar();
         }

         // read the integral portion
         text += ReadIntegerFromBuffer(buffer);

         // check for decimal
         if (buffer.Current == '.')
         {
            text += buffer.Current;
            hasDecimal = true;
            // read the fractional number if it exists
            if (buffer.GetNextChar().IsNumber())
               text += ReadIntegerFromBuffer(buffer);
            else
               return LexerError.PackageError(lexer, text, start, buffer.GetBufferPoint(-1),
                                              "Decimal fraction is missing");
         }
         // check for time
         else if (buffer.Current is 'T' or 't')
         {
            text += buffer.Current;
            buffer.GetNextChar();
            return new LiteralToken(text, start, buffer.GetBufferPoint(-1), LiteralType.Time);
         }
         // check for date or datetime
         else if (buffer.Current is 'D' or 'd')
         {
            text += buffer.Current;

            // check for datetime
            if (buffer.GetNextChar() is 'T' or 't')
            {
               text += buffer.Current;
               buffer.GetNextChar();
               return new LiteralToken(text, start, buffer.GetBufferPoint(-1), LiteralType.DateTime);
            }

            return new LiteralToken(text, start, buffer.GetBufferPoint(-1), LiteralType.Date);
         }

         // return our boring normal number since it isn't a super cool decimal, date, time or datetime
         return new LiteralToken(text, start, buffer.GetBufferPoint(-1), hasDecimal ? LiteralType.Decimal : LiteralType.Integer);
      }

      /// <summary>
      /// Reads an integer from buffer.
      /// </summary>
      /// <param name="buffer">The buffer.</param>
      /// <returns>a <see cref="string"/> containing an integer number.</returns>
      private static string ReadIntegerFromBuffer(ITextBuffer buffer)
      {
         var text = string.Empty;
         text += buffer.Current;
         while (buffer.GetNextChar().IsNumber()) text += buffer.Current;

         return text;
      }

      /// <summary>
      /// Reads the boolean literal from buffer.
      /// </summary>
      /// <param name="buffer">The buffer to read from.</param>
      /// <returns>A new <see cref="LiteralToken"/> or <see cref="ErrorToken"/>.</returns>
      public static AlToken? ReadBooleanLiteralFromBuffer(ITextBuffer buffer)
      {
         // no valid token so we return a null
         if (buffer.AtEndOfBuffer()) return null;

         // begin our token reading process
         var text = string.Empty;
         var start = buffer.GetBufferPoint();
         string booleanLiteral;

         if (buffer.Current.ToUpper() is 'T')
            booleanLiteral = "TRUE";
         else if (buffer.Current.ToUpper() is 'F')
            booleanLiteral = "FALSE";
         else
            return null;

         foreach (var letter in booleanLiteral)
         {
            if (letter != buffer.Current.ToUpper())
               return null;

            text += buffer.Current;
            buffer.GetNextChar();
         }

         return new LiteralToken(text, start, buffer.GetBufferPoint(-1), LiteralType.Boolean);
      }
   }
}