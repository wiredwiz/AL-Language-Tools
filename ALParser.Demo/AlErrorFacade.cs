#region MIT License
// <copyright company = "Edgerunner.org" file = "AlErrorFacade.cs">
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
using Org.Edgerunner.Language.Lexers;

namespace ALParser.Demo
{
   public class AlErrorFacade
   {
      public AlErrorFacade(LanguageError<AlToken> error)
      {
         Source = error.Source;
         Message = error.Message;
         Line = error.StartToken.StartingLine;
         Column = error.StartToken.StartingColumn;
      }

      /// <summary>
      /// Gets or sets the error message.
      /// </summary>
      /// <value>The error message.</value>
      public string Message { get; set; }

      /// <summary>
      /// Gets or sets the error source.
      /// </summary>
      /// <value>The source.</value>
      public string Source { get; set; }

      /// <summary>
      /// Gets or sets the line number where the error starts.
      /// </summary>
      /// <value>The line number.</value>
      public int Line { get; set; }

      /// <summary>
      /// Gets or sets the column number where the error starts.
      /// </summary>
      /// <value>The column number.</value>
      public int Column { get; set; }
   }
}