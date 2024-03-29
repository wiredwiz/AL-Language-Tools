﻿#region MIT License
// <copyright company = "Edgerunner.org" file = "TraceAttribute.cs">
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
using Metalama.Framework.Aspects;
using Org.Edgerunner.BC.AL.Language.Parsers;
using Org.Edgerunner.BC.AL.Language.Parsers.Rules;
using Org.Edgerunner.BC.AL.Language.Tokens;
using Org.Edgerunner.Language.Parsers;

namespace Org.Edgerunner.BC.AL.Language.Aspects
{
   public class TraceAttribute : OverrideMethodAspect
   {
      public override dynamic? OverrideMethod()
      {
         if (meta.This is not ParserRule<AlToken, AlSyntaxNodeType> rule)
            return meta.Proceed();

         if (meta.Target.Parameters.Count < 2 || meta.Target.Parameters[1].Value is not AlParser parser)
            return meta.Proceed();

         if (!parser.EnableTracing)
            return meta.Proceed();

         try
         {
            parser.GenerateTraceEvent(rule, TraceEvent.Enter);
            return meta.Proceed();
         }
         finally
         {
            parser.GenerateTraceEvent(rule, TraceEvent.Exit);
         }
      }
   }
}