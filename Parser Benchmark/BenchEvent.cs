#region MIT License
// <copyright company = "Edgerunner.org" file = "BenchEvent.cs">
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

namespace Parser_Benchmark
{
   public struct BenchEvent
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="BenchEvent"/> struct.
      /// </summary>
      /// <param name="action">The action.</param>
      /// <param name="fileName">Name of the file.</param>
      /// <param name="time">The time.</param>
      public BenchEvent(BenchAction action, string fileName, string time)
      {
         Action = action;
         FileName = fileName;
         Time = time;
      }

      public BenchAction Action { get; set; }
      public string FileName { get; set; }

      public string Time { get; set; }
   }
}