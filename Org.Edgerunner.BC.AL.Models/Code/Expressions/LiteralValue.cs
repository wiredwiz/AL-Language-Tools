#region MIT License
// <copyright company = "Edgerunner.org" file = "LiteralValue.cs">
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
using System.Runtime.InteropServices;

namespace Org.Edgerunner.BC.AL.Models.Code.Expressions
{
   /// <summary>
   /// Struct that defines a LiteralValue value.
   /// </summary>
   [StructLayout(LayoutKind.Explicit)]
   public struct LiteralValue
   {
      /// <summary>
      /// Gets or sets the Decimal.
      /// </summary>
      /// <value>The Decimal.</value>
      [FieldOffset(0)]
      public decimal Decimal;

      /// <summary>
      /// Gets or sets the Integer.
      /// </summary>
      /// <value>The Integer.</value>
      [FieldOffset(0)]
      public int Integer;

      /// <summary>
      /// Gets or sets the String.
      /// </summary>
      /// <value>The String.</value>
      [FieldOffset(0)]
      public string String;

      /// <summary>
      /// Gets or sets the DateTime.
      /// </summary>
      /// <value>The DateTime.</value>
      [FieldOffset(0)]
      public DateTime DateTime;

      /// <summary>
      /// Gets or sets the Boolean.
      /// </summary>
      /// <value>The Boolean.</value>
      [FieldOffset(0)]
      public bool Boolean;
   }
}