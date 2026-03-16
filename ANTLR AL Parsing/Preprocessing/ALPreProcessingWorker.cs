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

using System;
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
      public List<string> Symbols { get; } = new List<string>();

      public List<Tuple<int, int>> SkipRanges { get; } = new List<Tuple<int, int>>();

      public List<CodeRegion> Regions { get; } = new List<CodeRegion>();

      public Dictionary<string, List<PragmaInstruction>> Pragmas { get; } =
         new Dictionary<string, List<PragmaInstruction>>();

      public string FileName { get; set; }

      protected Stack<bool> ValueStack { get; set; } = new Stack<bool>();

      protected Stack<int> State { get; set; } = new Stack<int>();

      protected Stack<CodeRegion> WorkingRegions { get; set; } = new Stack<CodeRegion>();

      public List<string> WarningCodes { get; set; } = new List<string>();

      private int SkipIndex { get; set; }

      private const int IF_SKIP = 1;
      private const int PARENT_SKIP = 2;
      private const int IF_MATCHING = 3;
      private const int IF_MATCHED = 4;

      public override void EnterIfDirective(ALPreprocessorParser.IfDirectiveContext context)
      {
         if (State.Count != 0)
         {
            var state = State.Peek();
            if (state == IF_SKIP || state == PARENT_SKIP || state == IF_MATCHED)
               State.Push(PARENT_SKIP);
            else
               State.Push(IF_MATCHING);
         }
         else
            State.Push(IF_MATCHING);
      }

      public override void ExitIfDirective(ALPreprocessorParser.IfDirectiveContext context)
      {
         if (ValueStack.Count == 0)
            throw new InvalidOperationException("No value found on the preprocessor value stack when expected");

         var value = ValueStack.Pop();
         var state = State.Pop();

         if (state == IF_SKIP || state == PARENT_SKIP)
         {
            State.Push(state);
            return;
         }

         // Check our value stack return for true or false
         if (!value)
         {
            State.Push(IF_SKIP);
            SkipIndex = context.stop.Line + 1;
            return;
         }

         State.Push(IF_MATCHING);
      }

      public override void EnterElseifDirective(ALPreprocessorParser.ElseifDirectiveContext context)
      {
         var state = State.Pop();
         State.Push(state == IF_MATCHING ? IF_MATCHED : state);
         if (state == IF_MATCHED || state == IF_SKIP)
            SkipRanges.Add(new Tuple<int, int>(SkipIndex, context.start.Line - 1));
      }

      public override void ExitElseifDirective(ALPreprocessorParser.ElseifDirectiveContext context)
      {
         if (ValueStack.Count == 0)
            throw new InvalidOperationException("No value found on the preprocessor value stack when expected");

         var value = ValueStack.Pop();
         var state = State.Pop();

         if (state == PARENT_SKIP)
         {
            State.Push(state);
            return;
         }

         if (state == IF_MATCHED || state == IF_SKIP)
         {
            State.Push(state);
            SkipIndex = context.stop.Line + 1;
            return;
         }

         // Check our value stack return for true or false
         if (!value)
         {
            State.Push(IF_SKIP);
            SkipIndex = context.stop.Line + 1;
            return;
         }

         State.Push(IF_MATCHING);
      }

      public override void EnterElseDirective(ALPreprocessorParser.ElseDirectiveContext context)
      {
         var state = State.Pop();
         State.Push(state == IF_MATCHING ? IF_MATCHED : state);
         if (state == IF_MATCHED || state == IF_SKIP)
            SkipRanges.Add(new Tuple<int, int>(SkipIndex, context.start.Line - 1));
      }

      public override void ExitElseDirective(ALPreprocessorParser.ElseDirectiveContext context)
      {
         var state = State.Peek();
         if (state == IF_MATCHED || state == IF_SKIP)
            SkipIndex = context.stop.Line + 1;
      }

      public override void EnterEndifDirective(ALPreprocessorParser.EndifDirectiveContext context)
      {
         var state = State.Pop();

         if (state == IF_SKIP || state == IF_MATCHED)
            SkipRanges.Add(new Tuple<int, int>(SkipIndex, context.start.Line - 1));
      }

      public override void EnterRegionDirective(ALPreprocessorParser.RegionDirectiveContext context)
      {
         string comment = String.Empty;
         if (context.ChildCount > 1)
            comment = context.GetChild(1).GetText().Trim();

         WorkingRegions.Push(new CodeRegion(context.start.Line, comment));
      }

      public override void EnterRegionEndDirective(ALPreprocessorParser.RegionEndDirectiveContext context)
      {
         var inProgress = WorkingRegions.Pop();
         inProgress.EndLine = context.start.Line;
         Regions.Add(inProgress);
      }

      public override void EnterPragmaWarningDirective(ALPreprocessorParser.PragmaWarningDirectiveContext context)
      {
         WarningCodes.Clear();
      }

      public override void ExitPragmaWarningDirective(ALPreprocessorParser.PragmaWarningDirectiveContext context)
      {
         var actionText = context.GetChild(2).GetText().Trim().ToLowerInvariant();
         PragmaAction action;
         if (actionText == "disable")
            action = PragmaAction.disable;
         else if (actionText == "restore")
            action = PragmaAction.restore;
         else
            throw new ArgumentException($"Invalid pragma warning action '{actionText}' specified");

         foreach (var code in WarningCodes)
         {
            if (!Pragmas.TryGetValue(code, out var pragmaInstructions))
            {
               pragmaInstructions = new List<PragmaInstruction>();
               Pragmas[code] = pragmaInstructions;
            }
            pragmaInstructions.Add(new PragmaInstruction(action, code, context.start.Line));
         }
      }

      public override void ExitPragmaImplicitWithDirective(ALPreprocessorParser.PragmaImplicitWithDirectiveContext context)
      {
         var actionText = context.GetChild(2).GetText().Trim().ToLowerInvariant();
         PragmaAction action;
         if (actionText == "disable")
            action = PragmaAction.disable;
         else if (actionText == "restore")
            action = PragmaAction.restore;
         else
            throw new ArgumentException($"Invalid pragma implicitwith action '{actionText}' specified");

         const string code = "IMPLICITWITH";
         if (!Pragmas.TryGetValue(code, out var pragmaInstructions))
         {
            pragmaInstructions = new List<PragmaInstruction>();
            Pragmas[code] = pragmaInstructions;
         }
         pragmaInstructions.Add(new PragmaInstruction(action, code, context.start.Line));
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
         ValueStack.Push(Symbols.Contains(context.GetText()));
      }

      public override void ExitNotExpression(ALPreprocessorParser.NotExpressionContext context)
      {
         ValueStack.Push(!ValueStack.Pop());
      }
      public override void EnterWarningList(ALPreprocessorParser.WarningListContext context)
      {
         for (int i = 0; i < context.ChildCount; i++)
         {
            if (i % 2 == 0)
            {
               var child = context.GetChild(i);
               WarningCodes.Add(child.GetText().ToUpperInvariant().Trim());
            }
         }
      }
   }
}