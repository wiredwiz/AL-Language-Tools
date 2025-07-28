#region MIT License
// <copyright company = "Edgerunner.org" file = "ALParser.cs">
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
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using Grammar.AL.Antlr;
using Org.Edgerunner.Language.AL.Parsing.Messaging;
using Org.Edgerunner.Language.AL.Parsing.Preprocessing;
using System;

namespace Org.Edgerunner.Language.AL.Parsing
{
   /// <summary>
   /// Class that represents a generalized AL language parser.
   /// </summary>
   // ReSharper disable once InconsistentNaming
   public class ALParser
   {
      public ALParser()
      {
         Symbols = new List<string>();
         Errors = new List<ErrorMessage>();
      }

      public ALParser(List<string> symbols)
      : this()
      {
         Symbols = symbols;
      }

      public event EventHandler<ParseEventArgs> PreProcessingFinished;

      public event EventHandler<ParseEventArgs> ParsingFinished;

      /// <summary>
      /// Gets the lexer tokens.
      /// </summary>
      /// <value>The lexer tokens.</value>
      public List<IToken> Tokens { get; private set; }

      public ITokenStream TokenStream { get; private set; }

      public List<string> Symbols { get; private set; }

      public List<ErrorMessage> Errors { get; private set; }

      public string FileName { get; set; }

      protected ISyntaxTree ParseSource(TextReader reader)
      {
         var preProcessor = new ALPreProcessor(Symbols);
         preProcessor.FileName = FileName;
         var source = preProcessor.ProcessSource(reader);
         PreProcessingFinished?.Invoke(this, new ParseEventArgs(null, null, preProcessor.Errors));
         Errors.AddRange(preProcessor.Errors);
         var tokenStream = new CommonTokenStream(source, 0);

         Grammar.AL.Antlr.ALParser parser = new Grammar.AL.Antlr.ALParser(tokenStream);
         parser.RemoveErrorListeners();
         var listener = new ParserErrorListener(MessageSource.Parser);
         parser.AddErrorListener(listener);
         var result = parser.alUnit();
         Errors.AddRange(listener.Messages);
         ParsingFinished?.Invoke(this, new ParseEventArgs(tokenStream.GetTokens(), result, Errors));
         return result;
      }

      private void ExecutePreProcessing(Grammar.AL.Antlr.ALLexer lexer)
      {
         var tokenStream = new CommonTokenStream(lexer, 3);
      }

      public ISyntaxTree Parse(TextReader reader)
      {
         return ParseSource(reader);
      }

      public ISyntaxTree Parse(Stream stream)
      {
         return ParseSource(new StreamReader(stream));
      }

      public ISyntaxTree Parse(string sourceCode)
      {
         var stream = new MemoryStream();
         var writer = new StreamWriter(stream);
         writer.Write(sourceCode);
         writer.Flush();
         stream.Position = 0;
         return ParseSource(new StreamReader(stream));
      }
   }
}