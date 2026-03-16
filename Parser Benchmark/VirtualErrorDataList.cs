#region MIT License
// <copyright company = "Edgerunner.org" file = "VirtualErrorDataList.cs">
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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BrightIdeasSoftware;
using Org.Edgerunner.Language.AL.Parsing.Messaging;

namespace Parser_Benchmark
{
   public class VirtualErrorDataList : IVirtualListDataSource
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="VirtualErrorDataList"/> class.
      /// </summary>
      /// <param name="errors">The errors.</param>
      public VirtualErrorDataList(List<ErrorMessage> errors)
      {
         Errors = errors;
      }

      private List<ErrorMessage> Errors { get; set; }

      /// <summary>
      /// Return the object that should be displayed at the n'th row.
      /// </summary>
      /// <param name="n">The index of the row whose object is to be returned.</param>
      /// <returns>The model object at the n'th row, or null if the fetching was unsuccessful.</returns>
      public object GetNthObject(int n)
      {
         return Errors[n];
      }

      /// <summary>
      /// Return the number of rows that should be visible in the virtual list
      /// </summary>
      /// <returns>The number of rows the list view should have.</returns>
      public int GetObjectCount()
      {
         return Errors.Count;
      }

      /// <summary>
      /// Get the index of the row that is showing the given model object
      /// </summary>
      /// <param name="model">The model object sought</param>
      /// <returns>The index of the row showing the model, or -1 if the object could not be found.</returns>
      public int GetObjectIndex(object model)
      {
         return Errors.IndexOf((ErrorMessage)model);
      }

      /// <summary>
      /// The ListView is about to request the given range of items. Do
      /// whatever caching seems appropriate.
      /// </summary>
      /// <param name="first">The first.</param>
      /// <param name="last">The last.</param>
      public void PrepareCache(int first, int last)
      { }

      /// <summary>
      /// Find the first row that "matches" the given text in the given range.
      /// </summary>
      /// <param name="value">The text typed by the user</param>
      /// <param name="first">Start searching from this index. This may be greater than the 'to' parameter,
      /// in which case the search should descend</param>
      /// <param name="last">Do not search beyond this index. This may be less than the 'from' parameter.</param>
      /// <param name="column">The column that should be considered when looking for a match.</param>
      /// <returns>Return the index of row that was matched, or -1 if no match was found</returns>
      public int SearchText(string value, int first, int last, OLVColumn column)
      {
         return -1;
      }

      public void Sort(OLVColumn column, SortOrder order)
      {
         throw new System.NotImplementedException();
      }
      public void AddObjects(ICollection modelObjects)
      {
         if (modelObjects is List<ErrorMessage> messages)
            Errors.AddRange(messages);
         else
            Errors.AddRange(modelObjects.Cast<ErrorMessage>().ToList());
      }

      public void InsertObjects(int index, ICollection modelObjects)
      {
         if (modelObjects is List<ErrorMessage> messages)
            Errors.InsertRange(index, messages);
         else
            Errors.InsertRange(index, modelObjects.Cast<ErrorMessage>().ToList());
      }

      /// <summary>
      /// Remove all of the given objects from the control
      /// </summary>
      /// <param name="modelObjects">Collection of objects to be removed</param>
      public void RemoveObjects(ICollection modelObjects)
      {
         if (modelObjects is List<ErrorMessage> messages)
            foreach (var message in messages)
               Errors.Remove(message);
      }

      /// <summary>
      /// Set the collection of objects that this control will show.
      /// </summary>
      /// <param name="collection">The collection.</param>
      public void SetObjects(IEnumerable collection)
      {
         if (collection is List<ErrorMessage> messages)
            Errors = messages;
         else
            Errors = collection.Cast<ErrorMessage>().ToList();
      }

      /// <summary>
      /// Update/replace the nth object with the given object
      /// </summary>
      /// <param name="index">The index.</param>
      /// <param name="modelObject">The model object.</param>
      public void UpdateObject(int index, object modelObject)
      {
         Errors[index] = (ErrorMessage)modelObject;
      }
   }
}