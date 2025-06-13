#region MIT License
// <copyright company = "Edgerunner.org" file = "DictionaryDeclarationRule.cs">
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

using Org.Edgerunner.Pooling;

namespace Org.Edgerunner.BC.AL.Language.Parsers.Rules.Code.Variables
{
   public class DictionaryDeclarationRule : AlParserRule
   {
      public DictionaryDeclarationRule() : base(AlSyntaxNodeType.DictionaryDeclaration, "Dictionary Declaration Rule") {}

      public override string GetText()
      {
         var builder = StringBuilderPool.Current.Get();
         var prevText = ((AlParserRule)Children[0]).GetText();
         builder.Append(prevText);
         if (Children.Count > 1)
            for (var index = 1; index < Children.Count; index++)
            {
               var child = Children[index];
               var childText = ((AlParserRule)child).GetText();
               if (childText is not ";" and not "," and not "]" && prevText is not "[")
                  builder.Append(" ");
               builder.Append(childText);
               prevText = childText;
            }

         return builder.ToString();
      }
   }
}