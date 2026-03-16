#region MIT License
// <copyright company = "Edgerunner.org" file = "Datum.cs">
// Copyright(c)  2025
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

namespace Parser_Benchmark
{
   /// <summary>
   /// Struct that represents a piece of benchmark data.
   /// </summary>
   public struct Datum
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="Datum"/> struct.
      /// </summary>
      /// <param name="fileName">Name of the file.</param>
      /// <param name="parseTime">The parse time.</param>
      public Datum(string fileName, TimeSpan parseTime)
      {
         FileName = fileName;
         ParseTime = parseTime;
      }

      /// <summary>
      /// Gets the name of the file that was parsed.
      /// </summary>
      /// <value>The name of the file.</value>
      public string FileName { get; }

      /// <summary>
      /// Gets the time used to parse the file.
      /// </summary>
      /// <value>The parsing time.</value>
      public TimeSpan ParseTime { get; }
   }
}