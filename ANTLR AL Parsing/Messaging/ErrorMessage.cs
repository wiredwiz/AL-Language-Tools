#region MIT License
// <copyright company = "Edgerunner.org" file = "ErrorMessage.cs">
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

namespace Org.Edgerunner.Language.AL.Parsing.Messaging
{
   /// <summary>
   /// Struct that represents a source code parsing error message.
   /// </summary>
   public struct ErrorMessage
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ErrorMessage" /> struct.
      /// </summary>
      /// <param name="source">The message source.</param>
      /// <param name="fileName">Path of the source file.</param>
      /// <param name="severity">The message severity.</param>
      /// <param name="line">The source line number.</param>
      /// <param name="position">The source position within the line.</param>
      /// <param name="text">The message text.</param>
      // ReSharper disable once TooManyDependencies
      public ErrorMessage(MessageSource source, string fileName, MessageSeverity severity, int line, int position, string text)
      {
         Source = source;
         SourceFile = fileName;
         Severity = severity;
         Line = line;
         Position = position;
         Text = text;
      }

      /// <summary>
      /// Gets or sets the source of the message.
      /// </summary>
      /// <value>The source.</value>
      public MessageSource Source { get; set; }

      /// <summary>
      /// Gets or sets the source file path.
      /// </summary>
      /// <value>The source file path.</value>
      public string SourceFile { get; set; }

      /// <summary>
      /// Gets or sets the severity of the message.
      /// </summary>
      /// <value>The severity.</value>
      public MessageSeverity Severity { get; set; }

      /// <summary>
      /// Gets or sets the line number within the source code that the message relates to.
      /// </summary>
      /// <value>The line number.</value>
      public int Line { get; set; }

      /// <summary>
      /// Gets or sets the position in the line within the source code that the message relates to.
      /// </summary>
      /// <value>The position.</value>
      public int Position { get; set; }

      /// <summary>
      /// Gets or sets the text of the message.
      /// </summary>
      /// <value>The message text.</value>
      public string Text { get; set; }
   }
}