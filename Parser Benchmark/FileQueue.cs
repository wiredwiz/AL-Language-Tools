#region MIT License
// <copyright company = "Edgerunner.org" file = "FileQueue.cs">
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Antlr4.Runtime.Misc;

namespace Parser_Benchmark
{
   /// <summary>
   /// Class that represents a thread safe queue of file entries.
   /// Implements the <see cref="System.Collections.Concurrent.ConcurrentQueue{SourceFileEntry}" />
   /// </summary>
   /// <seealso cref="System.Collections.Concurrent.ConcurrentQueue{SourceFileEntry}" />
   public class FileQueue : ConcurrentQueue<SourceFileEntry>
   {
      private int _LoadingComplete;

      /// <summary>
      /// Initializes a new instance of the <see cref="FileQueue"/> class.
      /// </summary>
      public FileQueue() {}

      /// <summary>
      /// Initializes a new instance of the <see cref="FileQueue"/> class.
      /// </summary>
      /// <param name="collection">The collection whose elements are copied to the new <see cref="T:System.Collections.Concurrent.ConcurrentQueue`1" />.</param>
      public FileQueue(IEnumerable<SourceFileEntry> collection) : base(collection) {}

      /// <summary>
      /// Gets or sets a value indicating whether loading of files is complete.
      /// </summary>
      /// <value><c>true</c> if [loading complete]; otherwise, <c>false</c>.</value>
      public bool LoadingComplete
      {
         get => _LoadingComplete == 1;
         set => Interlocked.Exchange(ref _LoadingComplete, value ? 1 : 0);
      }

      /// <summary>
      /// Enqueues a file at the specified path asynchronously.
      /// </summary>
      /// <param name="filePath">The file path.</param>
      /// <param name="cts">The cancellation token source.</param>
      public async void EnqueueAsync(string filePath, [NotNull] CancellationTokenSource cts)
      {
         try
         {
            var stream = await ReadFileIntoMemoryStream(filePath, cts).ConfigureAwait(false);
            this.Enqueue(new SourceFileEntry(filePath, stream));
         }
         catch (OperationCanceledException)
         {
            return;
         }
      }

      /// <summary>
      /// Reads the file into memory stream.
      /// </summary>
      /// <param name="filePath">The file path.</param>
      /// <returns>MemoryStream.</returns>
      /// <exception cref="System.IO.FileNotFoundException">The file was not found: {filePath}</exception>
      public static async Task<MemoryStream> ReadFileIntoMemoryStream(string filePath, [NotNull] CancellationTokenSource cts)
      {
         // Check if the file exists
         if (!File.Exists(filePath)) 
            throw new FileNotFoundException($"The file was not found: {filePath}");

         // Create a new MemoryStream
         MemoryStream memoryStream = new MemoryStream();

         // Open the file as a FileStream
         using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
         {
            var info = new FileInfo(filePath);
            await fileStream.CopyToAsync(memoryStream, (int)info.Length, cts.Token).ConfigureAwait(false);
         }

         // Reset the position of the MemoryStream to the beginning
         // so it can be read from
         memoryStream.Position = 0;

         return memoryStream;
      }
   }
}