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

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;

namespace Parser_Benchmark
{
   /// <summary>
   /// Class that represents a thread safe queue of file entries.
   /// Implements the <see cref="System.Collections.Concurrent.ConcurrentQueue{SourceFileEntry}" />
   /// </summary>
   /// <seealso cref="System.Collections.Concurrent.ConcurrentQueue{SourceFileEntry}" />
   public class FileQueue : ConcurrentQueue<SourceFileEntry>
   {
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
      /// Enqueues a file at the specified path.
      /// </summary>
      /// <param name="filePath">The file path.</param>
      public void Enqueue(string filePath)
      {
         this.Enqueue(new SourceFileEntry(filePath, ReadFileIntoMemoryStream(filePath)));
      }

      /// <summary>
      /// Reads the file into memory stream.
      /// </summary>
      /// <param name="filePath">The file path.</param>
      /// <returns>MemoryStream.</returns>
      /// <exception cref="System.IO.FileNotFoundException">The file was not found: {filePath}</exception>
      public static MemoryStream ReadFileIntoMemoryStream(string filePath)
      {
         // Check if the file exists
         if (!File.Exists(filePath))
         {
            throw new FileNotFoundException($"The file was not found: {filePath}");
         }

         // Create a new MemoryStream
         MemoryStream memoryStream = new MemoryStream();

         // Open the file as a FileStream
         using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
         {
            // Copy the contents of the FileStream to the MemoryStream
            fileStream.CopyTo(memoryStream);
         }

         // Reset the position of the MemoryStream to the beginning
         // so it can be read from
         memoryStream.Position = 0;

         return memoryStream;
      }
   }
}