#region MIT License
// <copyright company = "Edgerunner.org" file = "ALPreProcessingWorker.cs">
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

using System.Collections.Generic;
using Grammar.AL.Antlr;

namespace Org.Edgerunner.Language.AL.Parsing.Preprocessing
{
   // ReSharper disable once InconsistentNaming
   /// <summary>
   /// Class that represents an AL language pre-processing directive worker.
   /// Implements the <see cref="ALPreprocessorParserBaseListener" />
   /// </summary>
   /// <seealso cref="ALPreprocessorParserBaseListener" />
   public class ALPreProcessingWorker : ALPreprocessorParserBaseListener
   {
      public static List<string> Symbols { get; private set; }

      public Stack<bool> ValueStack { get; set; }

      public Stack<int> State { get; set; }

      public override void EnterPragmaWarningDirective(ALPreprocessorParser.PragmaWarningDirectiveContext context)
      {
         base.EnterPragmaWarningDirective(context);
      }

      public override void EnterIfDirective(ALPreprocessorParser.IfDirectiveContext context)
      {
         State.Push(Grammar.AL.Antlr.ALParser.IF);
      }

      public override void ExitIfDirective(ALPreprocessorParser.IfDirectiveContext context)
      {
         base.ExitIfDirective(context);
      }

      public override void EnterRegionEndDirective(ALPreprocessorParser.RegionEndDirectiveContext context)
      {
         base.EnterRegionEndDirective(context);
      }

      public override void EnterEndifDirective(ALPreprocessorParser.EndifDirectiveContext context)
      {
         base.EnterEndifDirective(context);
      }

      public override void EnterPragmaImplicitWithDirective(ALPreprocessorParser.PragmaImplicitWithDirectiveContext context)
      {
         base.EnterPragmaImplicitWithDirective(context);
      }

      public override void EnterDefineDirective(ALPreprocessorParser.DefineDirectiveContext context)
      {
         var symbol = context.GetChild(1);
         if (!Symbols.Contains(symbol.GetText()))
            Symbols.Add(symbol.GetText());
      }

      public override void EnterUndefDirective(ALPreprocessorParser.UndefDirectiveContext context)
      {
         var symbol = context.GetChild(1);
         Symbols.Remove(symbol.GetText());
      }
      public override void EnterElseDirective(ALPreprocessorParser.ElseDirectiveContext context)
      {
         base.EnterElseDirective(context);
      }

      public override void EnterRegionDirective(ALPreprocessorParser.RegionDirectiveContext context)
      {
         base.EnterRegionDirective(context);
      }

      public override void EnterElseifDirective(ALPreprocessorParser.ElseifDirectiveContext context)
      {
         base.EnterElseifDirective(context);
      }

      public override void ExitElseifDirective(ALPreprocessorParser.ElseifDirectiveContext context)
      {
         base.ExitElseifDirective(context);
      }

      public override void ExitOrExpression(ALPreprocessorParser.OrExpressionContext context)
      {
         if (ValueStack.Pop()) // the left side is true so the OR expression is true
         {
            ValueStack.Pop(); // Pop the right side and discard it
            ValueStack.Push(true);
         }

         // We don't bother with pushing or popping for the right side of the expression because
         // whatever the current value of the right side is, is the final result we would push back anyway
      }

      public override void ExitAndExpression(ALPreprocessorParser.AndExpressionContext context)
      {
         if (!ValueStack.Pop()) // the left side is false so the AND expression is false
         {
            ValueStack.Pop(); // Pop the right side and discard it
            ValueStack.Push(false);
         }

         // We don't bother with pushing or popping for the right side of the expression because
         // whatever the current value of the right side is, is the final result we would push back anyway
      }

      public override void EnterIdentifierExpression(ALPreprocessorParser.IdentifierExpressionContext context)
      {
         ValueStack.Push(Symbols.Contains(context.IDENTIFIER().GetText()));
      }
      
      public override void ExitNotExpression(ALPreprocessorParser.NotExpressionContext context)
      {
         ValueStack.Push(!ValueStack.Pop());
      }
      public override void EnterWarningList(ALPreprocessorParser.WarningListContext context)
      {
         base.EnterWarningList(context);
      }
   }
}