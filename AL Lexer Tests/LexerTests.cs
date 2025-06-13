using System.Text;
using FluentAssertions;
using Org.Edgerunner.BC.AL.Language.Tokens;
using Org.Edgerunner.Buffers;
using Org.Edgerunner.Buffers.Input;

// ReSharper disable ExceptionNotDocumented

namespace Org.Edgerunner.BC.AL.Language.Lexers.Tests
{
   public class LexerTests
   {
      [Theory]
      [InlineData(1, 7, "14")]
      [InlineData(15, 15, "2")]
      [InlineData(15, 29, "100")]
      public void Fetching_an_integer_literal_token_results_in_the_correct_result(int lineNumber, int position, string number)
      {
         // Given a new AL lexer
         var lexer = new AlLexer();

         // loading a demo al file
         var buffer = new TextBuffer("demo.al", Encoding.Default.CodePage);

         // setting the buffer position
         buffer.SetBufferPoint(new BufferPoint(lineNumber, position));

         // and then reading a token
         var token = lexer.ReadTokenFromBuffer(buffer) as LiteralToken;

         // The token is a valid LiteralToken of type integer
         token!.LiteralType.Should().Be(LiteralType.Integer);
         token.TokenType.Should().Be((int)TokenType.Literal);
         token.Value.Should().Be(number);
      }

      [Theory]
      [InlineData(258, 44, "false")]
      [InlineData(314, 50, "true")]
      public void Fetching_a_boolean_literal_token_results_in_the_correct_result(int lineNumber, int position, string boolean)
      {
         // Given a new AL lexer
         var lexer = new AlLexer();

         // loading a demo al file
         var buffer = new TextBuffer("demo.al", Encoding.Default.CodePage);

         // setting the buffer position
         buffer.SetBufferPoint(new BufferPoint(lineNumber, position));

         // and then reading a token
         var token = lexer.ReadTokenFromBuffer(buffer) as LiteralToken;

         // The token is a valid LiteralToken of type boolean
         token!.LiteralType.Should().Be(LiteralType.Boolean);
         token.TokenType.Should().Be((int)TokenType.Literal);
         token.Value.Should().Be(boolean);
      }

      [Theory]
      [InlineData(249, 40, "\"Warehouse Receipt Header\"", false)]
      [InlineData(252, 17, "if", true)]
      [InlineData(251, 13, "begin", true)]
      public void Fetching_an_identifier_token_results_in_the_correct_result(int lineNumber, int position, string name, bool reserved)
      {
         // Given a new AL lexer
         var lexer = new AlLexer();

         // loading a demo al file
         var buffer = new TextBuffer("demo.al", Encoding.Default.CodePage);

         // setting the buffer position
         buffer.SetBufferPoint(new BufferPoint(lineNumber, position));

         // and then reading a token
         var token = lexer.ReadTokenFromBuffer(buffer) as IdentifierToken;

         // The token is a valid IdentifierToken
         token!.IsReservedWord.Should().Be(reserved);
         token.TokenType.Should().Be((int)TokenType.Identifier);
         token.Value.Should().Be(name);
      }

      [Theory]
      [InlineData(39, 63, "''")]
      [InlineData(34, 23, "'Address 2'")]
      [InlineData(136, 23, "'Country/Region Code ''test'''")]
      public void Fetching_a_string_literal_token_results_in_the_correct_result(int lineNumber, int position, string name)
      {
         // Given a new AL lexer
         var lexer = new AlLexer();

         // loading a demo al file
         var buffer = new TextBuffer("demo.al", Encoding.Default.CodePage);

         // setting the buffer position
         buffer.SetBufferPoint(new BufferPoint(lineNumber, position));

         // and then reading a token
         var token = lexer.ReadTokenFromBuffer(buffer) as LiteralToken;

         // The token is a valid LiteralToken of type string
         token!.LiteralType.Should().Be(LiteralType.String);
         token.TokenType.Should().Be((int)TokenType.Literal);
         token.StartingLine.Should().Be(lineNumber);
         token.EndingLine.Should().Be(lineNumber);
         token.StartingColumn.Should().Be(position);
         token.EndingColumn.Should().Be(name.Length + position - 1);
         token.Value.Should().Be(name);
      }

      [Theory]
      [InlineData(932, 24, "0D")]
      [InlineData(933, 23, "01012008D")]
      public void Fetching_a_date_literal_token_results_in_the_correct_result(int lineNumber, int position, string name)
      {
         // Given a new AL lexer
         var lexer = new AlLexer();

         // loading a demo al file
         var buffer = new TextBuffer("demo.al", Encoding.Default.CodePage);

         // setting the buffer position
         buffer.SetBufferPoint(new BufferPoint(lineNumber, position));

         // and then reading a token
         var token = lexer.ReadTokenFromBuffer(buffer) as LiteralToken;

         // The token is a valid LiteralToken of type date
         token!.LiteralType.Should().Be(LiteralType.Date);
         token.TokenType.Should().Be((int)TokenType.Literal);
         token.Value.Should().Be(name);
      }

      [Theory]
      [InlineData(934, 24, "0T")]
      [InlineData(935, 23, "112032300T")]
      public void Fetching_a_time_literal_token_results_in_the_correct_result(int lineNumber, int position, string name)
      {
         // Given a new AL lexer
         var lexer = new AlLexer();

         // loading a demo al file
         var buffer = new TextBuffer("demo.al", Encoding.Default.CodePage);

         // setting the buffer position
         buffer.SetBufferPoint(new BufferPoint(lineNumber, position));

         // and then reading a token
         var token = lexer.ReadTokenFromBuffer(buffer) as LiteralToken;

         // The token is a valid LiteralToken of type time
         token!.LiteralType.Should().Be(LiteralType.Time);
         token.TokenType.Should().Be((int)TokenType.Literal);
         token.Value.Should().Be(name);
      }

      [Theory]
      [InlineData(936, 25, "0DT")]
      public void Fetching_a_datetime_literal_token_results_in_the_correct_result(int lineNumber, int position, string name)
      {
         // Given a new AL lexer
         var lexer = new AlLexer();

         // loading a demo al file
         var buffer = new TextBuffer("demo.al", Encoding.Default.CodePage);

         // setting the buffer position
         buffer.SetBufferPoint(new BufferPoint(lineNumber, position));

         // and then reading a token
         var token = lexer.ReadTokenFromBuffer(buffer) as LiteralToken;

         // The token is a valid LiteralToken of type datetime
         token!.LiteralType.Should().Be(LiteralType.DateTime);
         token.TokenType.Should().Be((int)TokenType.Literal);
         token.Value.Should().Be(name);
      }

      [Theory]
      [InlineData(937, 24, "2.53")]
      public void Fetching_a_decimal_literal_token_results_in_the_correct_result(int lineNumber, int position, string name)
      {
         // Given a new AL lexer
         var lexer = new AlLexer();

         // loading a demo al file
         var buffer = new TextBuffer("demo.al", Encoding.Default.CodePage);

         // setting the buffer position
         buffer.SetBufferPoint(new BufferPoint(lineNumber, position));

         // and then reading a token
         var token = lexer.ReadTokenFromBuffer(buffer) as LiteralToken;

         // The token is a valid LiteralToken of type decimal
         token!.LiteralType.Should().Be(LiteralType.Decimal);
         token.TokenType.Should().Be((int)TokenType.Literal);
         token.Value.Should().Be(name);
      }

      [Theory]
      [InlineData(42, 13, "//This property is currently not supported")]
      [InlineData(43, 13, "//TestTableRelation = false;")]
      public void Fetching_a_comment_literal_token_results_in_the_correct_result(int lineNumber, int position, string name)
      {
         // Given a new AL lexer
         var lexer = new AlLexer();

         // loading a demo al file
         var buffer = new TextBuffer("demo.al", Encoding.Default.CodePage);

         // setting the buffer position
         buffer.SetBufferPoint(new BufferPoint(lineNumber, position));

         // and then reading a token
         var token = lexer.ReadTokenFromBuffer(buffer) as CommentToken;

         // The token is a valid CommentToken
         token!.TokenType.Should().Be((int)TokenType.Comment);
         token.StartingLine.Should().Be(lineNumber);
         token.EndingLine.Should().Be(lineNumber);
         token.StartingColumn.Should().Be(position);
         token.EndingColumn.Should().Be(name.Length + position - 1);
         token.Value.Should().Be(name);
      }

      [Theory]
      [InlineData(984, 9, 987, 29, "/*******************\r\n         * demo Multi-line *\r\n         * comment         *\r\n         *******************/")]
      public void Fetching_a_multiline_comment_literal_token_results_in_the_correct_result(int lineNumber, int position, int endLine, int endPos, string name)
      {
         // Given a new AL lexer
         var lexer = new AlLexer();

         // loading a demo al file
         var buffer = new TextBuffer("demo.al", Encoding.Default.CodePage);

         // setting the buffer position
         buffer.SetBufferPoint(new BufferPoint(lineNumber, position));

         // and then reading a token
         var token = lexer.ReadTokenFromBuffer(buffer) as CommentToken;

         // The token is a valid CommentToken
         token!.TokenType.Should().Be((int)TokenType.Comment);
         token.StartingLine.Should().Be(lineNumber);
         token.EndingLine.Should().Be(endLine);
         token.StartingColumn.Should().Be(position);
         token.EndingColumn.Should().Be(endPos);
         token.Value.Should().Be(name);
      }

      [Theory]
      [InlineData(1005, 5, "/// <summary>")]
      [InlineData(1006, 5, "///  Indicates whether the location is configured to pick using the first expire first out rule.")]
      [InlineData(1007, 5, "/// </summary>")]
      [InlineData(1008, 5, "/// <returns>True if picking is done according to first expire first out, otherwise false.</returns>")]
      public void Fetching_a_xml_comment_literal_token_results_in_the_correct_result(int lineNumber, int position, string name)
      {
         // Given a new AL lexer
         var lexer = new AlLexer();

         // loading a demo al file
         var buffer = new TextBuffer("demo.al", Encoding.Default.CodePage);

         // setting the buffer position
         buffer.SetBufferPoint(new BufferPoint(lineNumber, position));

         // and then reading a token
         var token = lexer.ReadTokenFromBuffer(buffer) as XmlCommentToken;

         // The token is a valid XmlCommentToken
         token!.TokenType.Should().Be((int)TokenType.XmlComment);
         token.Value.Should().Be(name);
      }

      [Theory]
      [InlineData(2, 1, "{")]
      [InlineData(3, 13, "=")]
      [InlineData(56, 27, ":=")]
      [InlineData(341, 48, "<>")]
      [InlineData(397, 71, "::")]
      [InlineData(417, 52, ".")]
      public void Fetching_a_symbol_token_results_in_the_correct_result(int lineNumber, int position, string name)
      {
         // Given a new AL lexer
         var lexer = new AlLexer();

         // loading a demo al file
         var buffer = new TextBuffer("demo.al", Encoding.Default.CodePage);

         // setting the buffer position
         buffer.SetBufferPoint(new BufferPoint(lineNumber, position));

         // and then reading a token
         var token = lexer.ReadTokenFromBuffer(buffer) as SymbolToken;

         // The token is a valid SymbolToken
         token!.TokenType.Should().Be((int)TokenType.Symbol);
         token.Value.Should().Be(name);
      }

      [Fact]
      public void Reading_a_series_of_tokens_succeeds()
      {
         // Given a new AL lexer
         var lexer = new AlLexer();

         // loading a demo al file
         var buffer = new TextBuffer("demo.al", Encoding.Default.CodePage);

         // setting the buffer position
         buffer.SetBufferPoint(new BufferPoint(1075, 5));

         // and then reading a series of tokens
         var tokens = lexer.ReadTokensFromBuffer(buffer);

         // Results in the expected tokens
         tokens.Count.Should().Be(38);
         var last = tokens[37];
         last.StartingLine.Should().Be(1079);
         last.EndingLine.Should().Be(1079);
         last.StartingColumn.Should().Be(1);
         last.EndingColumn.Should().Be(1);
         last.Value.Should().Be("}");
         var other = tokens[10];
         other.StartingLine.Should().Be(1076);
         other.EndingLine.Should().Be(1076);
         other.StartingColumn.Should().Be(21);
         other.EndingColumn.Should().Be(48);
         other.Value.Should().Be("OnBeforeIsBinBWReceiveOrShip");
      }

      [Fact]
      public void End_tokens_are_correct()
      {
         // Given a new AL lexer
         var lexer = new AlLexer();

         // loading a invoice posting codeunit al file
         var buffer = new TextBuffer("SalesPostInvoice.Codeunit.al", Encoding.Default.CodePage);

         // and then reading a series of tokens
         var tokens = lexer.ReadTokensFromBuffer(buffer);

         // Results in the expected tokens
         tokens.Count.Should().Be(4792);
         var last = tokens.Last();
         last.StartingLine.Should().Be(796);
         last.EndingLine.Should().Be(796);
         last.StartingColumn.Should().Be(1);
         last.EndingColumn.Should().Be(1);
         last.Value.Should().Be("}");
         var indexOfLast = tokens.IndexOf(last);
         var other = tokens[indexOfLast - 5];
         other.StartingLine.Should().Be(794);
         other.EndingLine.Should().Be(794);
         other.StartingColumn.Should().Be(82);
         other.EndingColumn.Should().Be(101);
         other.Value.Should().Be("PostedDeferralHeader");
      }
   }
}