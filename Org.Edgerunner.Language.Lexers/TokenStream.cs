#region MIT License
// <copyright company = "Edgerunner.org" file = "TokenStream.cs">
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

using System.Collections;

namespace Org.Edgerunner.Language.Lexers
{
   public class TokenStream<T> : IEnumerable<T>
      where T : IToken
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="TokenStream{T}"/> class.
      /// </summary>
      /// <param name="list">The list.</param>
      public TokenStream(List<T> list)
      {
         Stream = list;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="TokenStream{T}"/> class.
      /// </summary>
      public TokenStream()
      {
         Stream = new List<T>();
      }

      protected List<T> Stream;
      protected int _Position;

      /// <summary>
      /// Gets or sets the position in the stream.
      /// </summary>
      /// <value>The position.</value>
      /// <exception cref="System.ArgumentOutOfRangeException">Position - Position must be a non-negative number less than the size</exception>
      public virtual int Position
      {
         get => _Position;
         set
         {
            if (Position < 0 || Position > Stream.Count - 1)
               throw new ArgumentOutOfRangeException(nameof(Position), "Position must be a non-negative number less than or equal to the size minus 1");

            _Position = value;
         }
      }

      /// <summary>
      /// Gets the number of tokens in the stream.
      /// </summary>
      /// <value>The size.</value>
      public virtual int Size => Stream.Count;

      /// <summary>
      /// Gets the current token.
      /// </summary>
      /// <value>The current token.</value>
      public virtual T Current => Stream[_Position];

      /// <summary>
      /// Gets the token at the specified position within the stream.
      /// </summary>
      /// <param name="position">The position.</param>
      /// <returns>A token of type T.</returns>
      /// <exception cref="System.ArgumentOutOfRangeException">position - Invalid token position within the stream</exception>
      public T GetTokenAt(int position)
      {
         if (position < 0 || position >= Size)
            throw new ArgumentOutOfRangeException(nameof(position), "Invalid token position within the stream");

         return Stream[position];
      }

      /// <summary>
      /// Moves the stream position forward by one and returns the token at that position.
      /// </summary>
      /// <returns>A <see cref="T"/> instance.</returns>
      /// <exception cref="InvalidOperationException">Attempted to advance past the end of the stream.</exception>
      public virtual T MoveNext()
      {
         if (EndOfStream())
            throw new InvalidOperationException("Cannot advance past the end of the stream");

         return Stream[++_Position];
      }

      /// <summary>
      /// Tries to move the stream position forward by one, fetch the token and returns a boolean value.
      /// </summary>
      /// <param name="token">The next token or null if at end of the stream.</param>
      /// <returns>False if at end of stream; otherwise true.</returns>
      public virtual bool TryMoveNext(ref T? token)
      {
         if (EndOfStream())
            return false;

         token = Stream[++_Position];
         return true;
      }

      /// <summary>
      /// Moves the stream position backward by one and returns the token at that position.
      /// </summary>
      /// <returns>A <see cref="T"/> instance.</returns>
      /// <exception cref="InvalidOperationException">Attempted to rewind past the start of the stream.</exception>
      public virtual T MovePrevious()
      {
         if (BeginningOfStream())
            throw new InvalidOperationException("Cannot rewind past the start of the stream");

         return Stream[--_Position];
      }

      /// <summary>
      /// Tries to move the stream position forward by one, fetch the token and returns a boolean value.
      /// </summary>
      /// <param name="token">The next token or null if at end of the stream.</param>
      /// <returns>False if at end of stream; otherwise true.</returns>
      public virtual bool TryMovePrevious(ref T? token)
      {
         if (BeginningOfStream())
            return false;

         token = Stream[--_Position];
         return true;
      }

      /// <summary>
      /// Returns the next token in the stream.
      /// </summary>
      /// <returns>A instance of <see cref="T"/> or null if at the end of the stream.</returns>
      public virtual T? Next()
      {
         if (Position >= Stream.Count)
            return default;

         return Stream[Position + 1];
      }

      /// <summary>
      /// Returns the previous token in the stream.
      /// </summary>
      /// <returns>A instance of <see cref="T"/> or null if at the beginning of the stream.</returns>
      public virtual T? Previous()
      {
         if (Position <= 0)
            return default;

         return Stream[Position - 1];
      }

      /// <summary>
      /// Gets the position of of the specified token in the stream.
      /// </summary>
      /// <param name="token">The token to find.</param>
      /// <returns>An integer that represents the position.</returns>
      public virtual int GetPositionOf(T token)
      {
         return Stream.IndexOf(token);
      }

      /// <summary>
      /// Determines if the stream position is at the beginning.
      /// </summary>
      /// <returns><c>true</c> if position is at the beginning, <c>false</c> otherwise.</returns>
      public virtual bool BeginningOfStream()
      {
         return _Position == 0;
      }

      /// <summary>
      /// Determines if the stream position is at the end.
      /// </summary>
      /// <returns><c>true</c> if position is at the end, <c>false</c> otherwise.</returns>
      public virtual bool EndOfStream()
      {
         return _Position >= Stream.Count - 1;
      }

      /// <summary>
      /// Determines whether this token stream is empty.
      /// </summary>
      /// <returns><c>true</c> if the token stream is empty; otherwise, <c>false</c>.</returns>
      public virtual bool IsEmpty()
      {
         return EndOfStream() && BeginningOfStream();
      }

      /// <summary>
      /// Returns an enumerator that iterates through the collection.
      /// </summary>
      /// <returns>An enumerator that can be used to iterate through the collection.</returns>
      public IEnumerator<T> GetEnumerator()
      {
         return Stream.GetEnumerator();
      }

      /// <summary>
      /// Returns an enumerator that iterates through a collection.
      /// </summary>
      /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
      IEnumerator IEnumerable.GetEnumerator()
      {
         return ((IEnumerable)Stream).GetEnumerator();
      }
   }
}