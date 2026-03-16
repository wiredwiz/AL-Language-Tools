#region MIT License
// <copyright company = "Edgerunner.org" file = "SourceLineMapper.cs">
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

namespace Org.Edgerunner.Language.AL.Parsing
{
   public class SourceLineMapper
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="SourceLineMapper"/> class.
      /// </summary>
      public SourceLineMapper()
      {
         NewToOldMapping = new Dictionary<int, int>();
      }

      private Dictionary<int, int> NewToOldMapping { get; }

      /// <summary>
      /// Gets the original line number from the unprocessed source file that corresponds to the specified line.
      /// </summary>
      /// <param name="lineNumber">The line number.</param>
      /// <returns>An integer that represents the original line number.</returns>
      public int GetOriginalLineNo(int lineNumber)
      {
         return NewToOldMapping[lineNumber];
      }

      /// <summary>
      /// Registers a mapping between a new source line number to the old.
      /// </summary>
      /// <param name="oldLine">The old line number.</param>
      /// <param name="newLine">The new line number.</param>
      public void RegisterMapping(int oldLine, int newLine)
      {
         NewToOldMapping[newLine] = oldLine;
      }
   }
}