#region MIT License
// <copyright company = "Edgerunner.org" file = "IParserWorker.cs">
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
using System.Collections.Generic;
using System.Threading.Tasks;
using Org.Edgerunner.Language.AL.Parsing;
using Org.Edgerunner.Language.AL.Parsing.Messaging;

namespace Parser_Benchmark
{
   /// <summary>
   /// Interface that defines a language parsing worker.
   /// </summary>
   public interface IParserWorker
   {
      /// <summary>
      /// Starts this instance.
      /// </summary>
      /// <returns>A new <see cref="Task"/>.</returns>
      Task StartAsync();

      /// <summary>
      /// Stops this instance.
      /// </summary>
      /// <returns>The <see cref="Task"/> being stopped.</returns>
      Task StopAsync();

      /// <summary>
      /// Gets a value indicating whether this <see cref="IParserWorker"/> is busy.
      /// </summary>
      /// <value><c>true</c> if busy; otherwise, <c>false</c>.</value>
      bool Busy { get; }

      /// <summary>
      /// Occurs when parsing errors are reported.
      /// </summary>
      event EventHandler<List<ErrorMessage>> ReportErrors;

      /// <summary>
      /// Occurs when parsing data is reported.
      /// </summary>
      event EventHandler<Datum> ReportData;
   }
}