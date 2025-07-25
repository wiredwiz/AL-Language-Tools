#region MIT License
// <copyright company = "Edgerunner.org" file = "ALPreProcessor.cs">
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
using System.IO;
using System.Linq;
using System.Net.Mime;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Grammar.AL.Antlr;
using Org.Edgerunner.Language.AL.Parsing.Exceptions;
using Org.Edgerunner.Language.AL.Parsing.Messaging;

namespace Org.Edgerunner.Language.AL.Parsing.Preprocessing
{
   /// <summary>
   /// Class that represents an AL source code pre-processor.
   /// </summary>
   public class ALPreProcessor
   {
      public ALPreProcessor(List<string> symbols)
      {
         Symbols = symbols.ToList();
         Errors = new List<ErrorMessage>();
      }

      public static List<string> Symbols { get; private set; }

      public List<CodeRegion> Regions { get; private set; }

      public Dictionary<string, List<PragmaInstruction>> Pragmas { get; private set; }

      public List<ErrorMessage> Errors { get; private set; }

      public ITokenSource ProcessSource(TextReader reader)
      {
         Errors.Clear();
         var inputStream = new AntlrInputStream(reader);
         ALLexer lexer = new ALLexer(inputStream);
         var lexerErrorListener = new LexerErrorListener(MessageSource.Lexer);
         lexer.RemoveErrorListeners();
         lexer.AddErrorListener(lexerErrorListener);
         var tokenStream = new CommonTokenStream(lexer, 3);
         tokenStream.Fill();
         var allTokens = tokenStream.GetTokens();
         Errors.AddRange(lexerErrorListener.Messages);

         var parserErrorListener = new ParserErrorListener(MessageSource.PreProcessor);
         ALPreprocessorParser parser = new ALPreprocessorParser(tokenStream);
         parser.RemoveErrorListeners();
         parser.AddErrorListener(parserErrorListener);
         var parseTree = parser.compileDirectives();
         Errors.AddRange(parserErrorListener.Messages);

         // Interpret pre-processor directives
         var processWorker = new ALPreProcessingWorker();
         processWorker.Symbols.AddRange(Symbols);
         ParseTreeWalker.Default.Walk(processWorker, parseTree);

         // Assign parsed regions and pragma instructions
         Regions = processWorker.Regions;
         Pragmas = processWorker.Pragmas;

         // Now we get our pre-processed token source
         lexer.Reset();
         return new PreProcessedTokenSource(lexer, processWorker.SkipRanges);
      }
   }
}