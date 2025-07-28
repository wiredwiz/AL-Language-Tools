#region MIT License
// <copyright company = "Edgerunner.org" file = "AntlrParseWorker.cs">
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
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Org.Edgerunner.Language.AL.Parsing;
using Org.Edgerunner.Language.AL.Parsing.Messaging;

namespace Parser_Benchmark
{
   /// <summary>
   /// Class that represents an ANTLR4 AL language parser worker.
   /// Implements the <see cref="Parser_Benchmark.IParserWorker" />
   /// </summary>
   /// <seealso cref="Parser_Benchmark.IParserWorker" />
   public class AntlrParseWorker : IParserWorker
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="AntlrParseWorker" /> class.
      /// </summary>
      /// <param name="fileQueue">The file queue.</param>
      /// <param name="cts">The cancellation token source.</param>
      public AntlrParseWorker(FileQueue fileQueue, CancellationTokenSource cts)
      {
         _FileQueue = fileQueue;
         Parser = new ALParser();
         _Cts2 = cts;
      }

      private readonly FileQueue _FileQueue;
      private readonly object _Lock = new object();

      private Task _ProcessingTask;
      private CancellationTokenSource _Cts;
      private CancellationTokenSource _Cts2;

      private CancellationTokenSource _CombinedCts;
      private bool _IsRunning;
      private int _Busy;

      /// <summary>
      /// Starts this instance.
      /// </summary>
      /// <returns>A new <see cref="Task" />.</returns>
      /// <exception cref="System.InvalidOperationException">Processing is already running.</exception>
      public async Task StartAsync()
      {
         lock (_Lock)
         {
            if (_IsRunning)
               throw new InvalidOperationException("Processing is already running.");

            _Cts = new CancellationTokenSource();
            _CombinedCts = CancellationTokenSource.CreateLinkedTokenSource(_Cts.Token, _Cts2.Token);

            var token = _Cts.Token;
            _IsRunning = true;

            _ProcessingTask = Task.Run(() => ParseLoop(token), token);
         }

         // ReSharper disable once AsyncApostle.AsyncAwaitMayBeElidedHighlighting
         await _ProcessingTask.ConfigureAwait(true);
      }

      /// <summary>
      /// Stops this instance.
      /// </summary>
      /// <returns>The <see cref="Task" /> being stopped.</returns>
      public async Task StopAsync()
      {
         Task taskToWait;
         lock (_Lock)
         {
            if (!_IsRunning)
               return;

            _Cts.Cancel();
            taskToWait = _ProcessingTask;
         }

         try
         {
            await taskToWait.ConfigureAwait(true);
         }
         catch (OperationCanceledException)
         {
            // Expected when cancelled
         }
         finally
         {
            lock (_Lock)
            {
               _IsRunning = false;
               _ProcessingTask = null;
               _Cts.Dispose();
               _Cts = null;
            }
         }
      }

      private void ParseLoop(CancellationToken token)
      {
         var watch = new Stopwatch();
         while (!token.IsCancellationRequested)
         {
            if (_FileQueue.TryDequeue(out var fileEntry))
            {
               Interlocked.Exchange(ref _Busy, 1);

               watch.Restart();
               Parser.FileName = fileEntry.FileName;
               Parser.Parse(fileEntry.Stream);
               watch.Stop();
               var elapsed = watch.Elapsed;
               var datum = new Datum(fileEntry.FileName, elapsed);
               ReportData?.Invoke(this, datum);
               if (Parser.Errors.Count > 0)
                  ReportErrors?.Invoke(this, Parser.Errors);

               Interlocked.Exchange(ref _Busy, 0);
            }
            else
               Task.Delay(100, _Cts.Token);

            if (_FileQueue.LoadingComplete && _FileQueue.Count == 0)
               lock (_Lock)
               {
                  _IsRunning = false;
                  _ProcessingTask = null;
                  break;
               }
         }
      }

      /// <summary>
      /// Gets a value indicating whether this <see cref="IParserWorker" /> is busy.
      /// </summary>
      /// <value><c>true</c> if busy; otherwise, <c>false</c>.</value>
      public bool Busy => _Busy == 1;

      /// <summary>
      /// Occurs when parsing errors are reported.
      /// </summary>
      public event EventHandler<List<ErrorMessage>> ReportErrors;

      /// <summary>
      /// Occurs when parsing data is reported.
      /// </summary>
      public event EventHandler<Datum> ReportData;

      private ALParser Parser { get; }
   }
}