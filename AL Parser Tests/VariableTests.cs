using System.Diagnostics;
using Org.Edgerunner.BC.AL.Language.Lexers;
using Org.Edgerunner.Buffers.Input;
using System.Text;
using FluentAssertions;
using Org.Edgerunner.BC.AL.Language.Parsers.Rules;
using Org.Edgerunner.BC.AL.Language.Parsers.Rules.Code.Variables;
using Org.Edgerunner.BC.AL.Language.Tokens;
using Org.Edgerunner.Language.Lexers;
// ReSharper disable TooManyArguments
// ReSharper disable UseObjectOrCollectionInitializer

// ReSharper disable ExceptionNotDocumented
// ReSharper disable TooManyDeclarations

namespace Org.Edgerunner.BC.AL.Language.Parsers.Tests
{
   public class VariableTests
   {
      [Fact]
      public void Text_variable_parses_correctly()
      {
         // loading a demo al file
         var buffer = new TextBuffer("Variables.al", Encoding.Default.CodePage);
         var lexer = new AlLexer();

         // Then tokenizing the file into a token stream
         var stream = new TokenStream<AlToken>(lexer.ReadTokensFromBuffer(buffer));

         // And moving the token stream to the text variable declaration
         stream.Position = 8;

         // And finally parsing the text declaration tokens
         var parser = new AlParser();
         var root = parser.ParseVariableDeclaration(stream);

         // results in the expected parse tree
         parser.State.Should().Be(0);
         var rule = root;
         rule.Should().NotBeNull();
         rule.Parent.Should().BeNull();
         rule.Start!.StartingLine.Should().Be(3);
         rule.Start.StartingColumn.Should().Be(4);
         rule.End!.EndingLine.Should().Be(3);
         rule.End.EndingColumn.Should().Be(25);
      }

      [Fact]
      public void Option_variable_parses_correctly()
      {
         // loading a demo al file
         var buffer = new TextBuffer("Variables.al", Encoding.Default.CodePage);
         var lexer = new AlLexer();

         // Then tokenizing the file into a token stream
         var stream = new TokenStream<AlToken>(lexer.ReadTokensFromBuffer(buffer));

         // And moving the token stream to the option variable declaration
         stream.Position = 15;

         // And finally parsing the option declaration tokens
         var parser = new AlParser();
         var root = parser.ParseVariableDeclaration(stream);

         // results in the expected parse tree
         parser.State.Should().Be(0);
         var rule = root ;
         rule.Should().NotBeNull();
         rule.Parent.Should().BeNull();
         rule.Start!.StartingLine.Should().Be(4);
         rule.Start.StartingColumn.Should().Be(4);
         rule.End!.EndingLine.Should().Be(4);
         rule.End.EndingColumn.Should().Be(39);
      }

      [Theory]
      [InlineData(24, 5, 4, 5, 31, "customer: record \"customer\";")]
      [InlineData(29, 6, 4, 6, 24, "customer2: record 20;")]
      public void Record_variable_parses_correctly(int streamPosition, int startLine, int startColumn, int endLine, int endColumn, string value)
      {
         // loading a demo al file
         var buffer = new TextBuffer("Variables.al", Encoding.Default.CodePage);
         var lexer = new AlLexer();

         // Then tokenizing the file into a token stream
         var stream = new TokenStream<AlToken>(lexer.ReadTokensFromBuffer(buffer));

         // And moving the token stream to the option variable declaration
         stream.Position = streamPosition;

         // And finally parsing the record declaration tokens
         var parser = new AlParser();
         var root = parser.ParseVariableDeclaration(stream);

         // results in the expected parse tree
         parser.State.Should().Be(0);
         var rule = root;
         rule.Should().NotBeNull();
         rule.Parent.Should().BeNull();
         rule.Start!.StartingLine.Should().Be(startLine);
         rule.Start.StartingColumn.Should().Be(startColumn);
         rule.End!.EndingLine.Should().Be(endLine);
         rule.End.EndingColumn.Should().Be(endColumn);
         rule.GetText().Should().Be(value);
      }

      [Theory]
      [InlineData(34, 7, 4, 7, 16, "myDate: Date;")]
      [InlineData(38, 8, 4, 8, 16, "myTime: Time;")]
      [InlineData(42, 9, 4, 9, 25, "myDatetime: Datetime;")]
      [InlineData(46, 10, 4, 10, 22, "doesParse: Boolean;")]
      public void Simple_variable_type_parses_correctly(int streamPosition, int startLine, int startColumn, int endLine, int endColumn, string value)
      {
         // loading a demo al file
         var buffer = new TextBuffer("Variables.al", Encoding.Default.CodePage);
         var lexer = new AlLexer();

         // Then tokenizing the file into a token stream
         var stream = new TokenStream<AlToken>(lexer.ReadTokensFromBuffer(buffer));

         // And moving the token stream to the option variable declaration
         stream.Position = streamPosition;

         // And finally parsing the record declaration tokens
         var parser = new AlParser();
         var root = parser.ParseVariableDeclaration(stream);

         // results in the expected parse tree
         parser.State.Should().Be(0);
         var rule = root;
         rule.Should().NotBeNull();
         rule.Parent.Should().BeNull();
         rule.Start!.StartingLine.Should().Be(startLine);
         rule.Start!.StartingColumn.Should().Be(startColumn);
         rule.End!.EndingLine.Should().Be(endLine);
         rule.End.EndingColumn.Should().Be(endColumn);
         rule.GetText().Should().Be(value);
      }

      [Fact]
      public void Array_variable_parses_correctly()
      {
         // loading a demo al file
         var buffer = new TextBuffer("Variables.al", Encoding.Default.CodePage);
         var lexer = new AlLexer();

         // Then tokenizing the file into a token stream
         var stream = new TokenStream<AlToken>(lexer.ReadTokensFromBuffer(buffer));

         // And moving the token stream to the option variable declaration
         stream.Position = 50;

         // And finally parsing the option declaration tokens
         var parser = new AlParser();
         var root = parser.ParseVariableDeclaration(stream);

         // results in the expected parse tree
         parser.State.Should().Be(0);
         var rule = root;
         rule.Should().NotBeNull();
         rule.Parent.Should().BeNull();
         rule.Start!.StartingLine.Should().Be(11);
         rule.Start.StartingColumn.Should().Be(4);
         rule.End!.EndingLine.Should().Be(11);
         rule.End.EndingColumn.Should().Be(37);
         rule.GetText().Should().Be("myArray: array[10, 10] of text[20];");
      }

      [Fact]
      public void List_variable_parses_correctly()
      {
         // loading a demo al file
         var buffer = new TextBuffer("Variables.al", Encoding.Default.CodePage);
         var lexer = new AlLexer();

         // Then tokenizing the file into a token stream
         var stream = new TokenStream<AlToken>(lexer.ReadTokensFromBuffer(buffer));

         // And moving the token stream to the option variable declaration
         stream.Position = 64;

         // And finally parsing the option declaration tokens
         var parser = new AlParser();
         var root = parser.ParseVariableDeclaration(stream);

         // results in the expected parse tree
         parser.State.Should().Be(0);
         var rule = root;
         rule.Should().NotBeNull();
         rule.Parent.Should().BeNull();
         rule.Start!.StartingLine.Should().Be(12);
         rule.Start.StartingColumn.Should().Be(4);
         rule.End!.EndingLine.Should().Be(12);
         rule.End.EndingColumn.Should().Be(30);
         rule.GetText().Should().Be("myList: List of [text[10]];");
      }

      [Fact]
      public void Dictionary_variable_parses_correctly()
      {
         // loading a demo al file
         var buffer = new TextBuffer("Variables.al", Encoding.Default.CodePage);
         var lexer = new AlLexer();

         // Then tokenizing the file into a token stream
         var stream = new TokenStream<AlToken>(lexer.ReadTokensFromBuffer(buffer));

         // And moving the token stream to the option variable declaration
         stream.Position = 75;

         // And finally parsing the option declaration tokens
         var parser = new AlParser();
         var root = parser.ParseVariableDeclaration(stream);

         // results in the expected parse tree
         parser.State.Should().Be(0);
         var rule = root;
         rule.Should().NotBeNull();
         rule.Parent.Should().BeNull();
         rule.Start!.StartingLine.Should().Be(13);
         rule.Start.StartingColumn.Should().Be(4);
         rule.End!.EndingLine.Should().Be(13);
         rule.End.EndingColumn.Should().Be(58);
         rule.GetText().Should().Be("myDictionary: dictionary of [code[20], List of [text]];");
      }
   }
}