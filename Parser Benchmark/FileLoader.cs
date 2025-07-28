#region MIT License
// <copyright company = "Edgerunner.org" file = "FileLoader.cs">
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
using System.Threading;
using System.Threading.Tasks;

namespace Parser_Benchmark
{
   public class FileLoader : IDisposable
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="FileLoader" /> class.
      /// </summary>
      /// <param name="files">The files to load.</param>
      /// <param name="fileQueue">The file queue.</param>
      public FileLoader(string[] files, FileQueue fileQueue)
      {
         Files = files;
         FileQueue = fileQueue;
         _Cts = new CancellationTokenSource();
      }

      ~FileLoader()
      {
         Dispose(false);
      }

      private readonly object _Lock = new object();
      private CancellationTokenSource _Cts;

      private string[] Files { get; set; }

      private FileQueue FileQueue { get; set; }

      public void StartLoadFiles()
      {
         
      }

      public void StopLoadFiles()
      {
         lock (_Lock)
         {
            _Cts.Cancel();
            _Cts.Dispose();
            _Cts = null;
         }
      }

      private void ReleaseUnmanagedResources()
      {
         // TODO release unmanaged resources here
      }

      private void Dispose(bool disposing)
      {
         ReleaseUnmanagedResources();
         if (disposing)
         {
            _Cts?.Dispose();
         }
      }

      /// <summary>
      /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
      /// </summary>
      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }
   }
}