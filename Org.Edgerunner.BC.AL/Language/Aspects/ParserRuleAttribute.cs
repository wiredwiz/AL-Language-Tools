#region MIT License
// <copyright company = "Edgerunner.org" file = "ParserRuleAttribute.cs">
// Copyright(c) Thaddeus Ryker 2023
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

using System.Diagnostics;
using System.Xml.Linq;
using Metalama.Framework.Aspects;
using Org.Edgerunner.BC.AL.Language.Parsers;
using Org.Edgerunner.BC.AL.Language.Parsers.Rules;
using Org.Edgerunner.BC.AL.Language.Tokens;
using Org.Edgerunner.Language.Lexers;
using Org.Edgerunner.Language.Parsers;

namespace Org.Edgerunner.BC.AL.Language.Aspects
{
   public class ParserRuleAttribute : OverrideMethodAspect
   {
      /// <summary>
      /// Initializes a new instance of the <see cref="ParserRuleAttribute"/> class.
      /// </summary>
      /// <param name="ruleType">Type of the parser rule.</param>
      public ParserRuleAttribute(AlSyntaxNodeType ruleType)
      {
         RuleType = ruleType;
      }

      /// <summary>
      /// Gets or sets the type of the parser rule.
      /// </summary>
      /// <value>The type of the parser rule.</value>
      public AlSyntaxNodeType RuleType { get; }

      public override dynamic? OverrideMethod()
      {
         if (meta.This is not AlParser { EnableTracing: true } parser)
            return meta.Proceed();

         if (meta.Target.Method.Parameters.Count < 1)
            return meta.Proceed();

         var stream = meta.Target.Parameters[0].Value as TokenStream<AlToken>;
         if (stream == null)
            return meta.Proceed();

         var token = stream.Current;
         var name = parser.GetRuleName((AlSyntaxNodeType)RuleType);

         try
         {
            parser.GenerateTraceEvent(token, name, TraceEvent.Enter);
            return meta.Proceed();
         }
         finally
         {
            parser.GenerateTraceEvent(token, name, TraceEvent.Exit);
         }
      }
   }
}