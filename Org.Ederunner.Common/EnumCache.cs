#region MIT License
// <copyright company = "Edgerunner.org" file = "EnumCache.cs">
// Copyright(c)  2023
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


namespace Org.Edgerunner.Common
{
   /// <summary>
   /// Class that represents a fast lookup cache of Enumeration names and values.
   /// </summary>
   /// <typeparam name="TTarget">The type of the Enumeration.</typeparam>
   public static class EnumCache<TTarget> where TTarget : Enum
   {
      static EnumCache()
      {
         Dict = Enum.GetNames(typeof(TTarget)).ToDictionary(x => x, x => (TTarget)Enum.Parse(typeof(TTarget), x), StringComparer.OrdinalIgnoreCase);
      }

      private static readonly Dictionary<string, TTarget> Dict;

      /// <summary>
      /// Gets the value for the specified enumeration text value.
      /// </summary>
      /// <param name="value">The enumeration value.</param>
      /// <returns>An enumeration of type <see cref="TTarget"/>.</returns>
      public static TTarget GetValueFor(string value)
      {
         return Dict[value];
      }

      /// <summary>
      /// Determines whether the enumeration has a value with a name that matches the string.
      /// </summary>
      /// <param name="value">The enumeration value name.</param>
      /// <returns><c>true</c> if the enumeration contains a value with the specified name; otherwise, <c>false</c>.</returns>
      public static bool Contains(string value)
      {
         return Dict.ContainsKey(value);
      }
   }
}