﻿#region MIT License
// <copyright company = "Edgerunner.org" file = "BasicVariableTypeDeclaration.cs">
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


namespace Org.Edgerunner.BC.AL.Objects.Code.TypeDeclarations
{
   public class BasicVariableTypeDeclaration
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="BasicVariableTypeDeclaration"/> class.
      /// </summary>
      /// <param name="type">The type.</param>
      protected BasicVariableTypeDeclaration(VariableType type)
      {
         Type = type;
      }

      /// <summary>
      /// Gets or sets the type.
      /// </summary>
      /// <value>The type.</value>
      public VariableType Type { get; set; }

      /// <summary>
      /// Returns a <see cref="System.String" /> that represents this instance.
      /// </summary>
      /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
      public override string ToString()
      {
         return Type.ToString();
      }
   }
}