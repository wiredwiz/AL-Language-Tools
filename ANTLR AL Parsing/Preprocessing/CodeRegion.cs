#region MIT License
// <copyright company = "Edgerunner.org" file = "CodeRegion.cs">
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

namespace Org.Edgerunner.Language.AL.Parsing.Preprocessing
{
   /// <summary>
   /// Struct that represents a code region block.
   /// </summary>
   public struct CodeRegion
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="CodeRegion"/> struct.
      /// </summary>
      /// <param name="startLine">The start line.</param>
      /// <param name="endLine">The end line.</param>
      /// <param name="comment">The comment.</param>
      public CodeRegion(int startLine, int endLine, string comment)
      {
         Comment = comment;
         StartLine = startLine;
         EndLine = endLine;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="CodeRegion"/> struct.
      /// </summary>
      /// <param name="startLine">The start line.</param>
      /// <param name="endLine">The end line.</param>
      public CodeRegion(int startLine, int endLine)
      {
         StartLine = startLine;
         EndLine = endLine;
         Comment = String.Empty;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="CodeRegion"/> struct.
      /// </summary>
      /// <param name="startLine">The start line.</param>
      /// <param name="comment">The comment.</param>
      public CodeRegion(int startLine, string comment)
      {
         Comment = comment;
         StartLine = startLine;
         EndLine = startLine;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="CodeRegion"/> struct.
      /// </summary>
      /// <param name="startLine">The start line.</param>
      public CodeRegion(int startLine)
      {
         StartLine = startLine;
         EndLine = startLine;
         Comment = String.Empty;
      }

      /// <summary>
      /// Gets or sets the region comment.
      /// </summary>
      /// <value>The comment.</value>
      public string Comment { get; set; }

      /// <summary>
      /// Gets or sets the region starting line number.
      /// </summary>
      /// <value>The starting line number.</value>
      public int StartLine { get; set; }

      /// <summary>
      /// Gets or sets the region ending line number.
      /// </summary>
      /// <value>The ending line number.</value>
      public int EndLine { get; set; }
   }
}