using System.ComponentModel;
using FastColoredTextBoxNS.Types;
using Org.Edgerunner.BC.AL.Language.Lexers;
using Org.Edgerunner.BC.AL.Language.Parsers;
using Org.Edgerunner.BC.AL.Language.Parsers.Rules;
using Org.Edgerunner.BC.AL.Language.Tokens;
using Org.Edgerunner.Buffers.Input;
using Org.Edgerunner.Language.Lexers;
using Org.Edgerunner.Language.Parsers;

namespace ALParser.Demo
{
   public partial class Demo : Form
   {
      public Demo()
      {
         InitializeComponent();
         _Lexer.AddErrorListener(_ErrorListener);
         _Parser.AddErrorListener(_ErrorListener);
         cmbRules.DataSource = AlParser.Rules.Values.ToList();
      }

      private AlParser _Parser = new AlParser();
      private AlLexer _Lexer = new AlLexer();
      private TextBuffer _Buffer;
      private ITextEditorColorStyle _Styles = new AlDefaultColorStyle();
      private AlErrorListener _ErrorListener = new AlErrorListener();

      private void demoEditor_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
      {
         ParseSource();
      }

      private void ParseSource()
      {
         _Buffer = new TextBuffer(demoEditor.Text);
         _ErrorListener.Clear();
         _Parser.Reset();
         var tokens = _Lexer.ReadTokensFromBuffer(_Buffer);
         var stream = new TokenStream<AlToken>(tokens);
         var ruleName = cmbRules.SelectedItem?.ToString();
         if (string.IsNullOrEmpty(ruleName) || tokens.Count == 0)
            return;

         var parser = new Org.Edgerunner.BC.AL.Language.Parsers.AlParser();
         parser.AddErrorListener(_ErrorListener);
         parser.ParseSource(stream);
         //var ruleInstance = AlParser. ParserRules.GetInstance(ruleName);
         //ruleInstance.Parse(stream, _Parser);
         MessageList.DataSource = new BindingList<AlErrorFacade>(_ErrorListener.Errors.Select(x => new AlErrorFacade(x)).ToList());
         demoEditor.ClearAllStyles();
         var last = tokens.Last();
         foreach (var token in tokens)
         {
            var span = demoEditor.GetRange(token.GetStart(), token.GetEnd());
            if (token.IsAssignmentOperator)
               span.SetStyle(_Styles.AssignmentOperators);
            else if (token.IsComparisonOperator)
               span.SetStyle(_Styles.ComparisonOperators);
            else if (token.IsBinaryOperator)
               span.SetStyle(_Styles.BinaryOperators);
            else if (token.IsOperator)
               span.SetStyle(_Styles.Operators);
            else if (token is ErrorToken)
               span.SetStyle(_Styles.Error);
            else if (token is SymbolToken)
               span.SetStyle(_Styles.Symbols);
            else if (token is IdentifierToken { IsKeyword: true })
               span.SetStyle(_Styles.Keywords);
            else if (token is IdentifierToken identToken && identToken.Value.StartsWith('"'))
               span.SetStyle(_Styles.QuotedIdentifiers);
            //else if (token is IdentifierToken)
            //   span.SetStyle(_Styles.Identifiers);
            else if (token is LiteralToken { LiteralType: LiteralType.String })
               span.SetStyle(_Styles.StringLiterals);
            else if (token is LiteralToken { LiteralType: LiteralType.Decimal or LiteralType.Integer })
               span.SetStyle(_Styles.NumberLiterals);
            else if (token is LiteralToken)
               span.SetStyle(_Styles.OtherLiterals);
            else
               span.SetStyle(_Styles.Default);
         }

         PaintErrors(parser.Result);

         listTokens.DataSource = new BindingList<AlToken>(tokens);
         var graph = TreeGrapher.CreateGraph(parser.Result);
         gViewer1.Graph = graph;
      }

      private void PaintErrors(ParserRule<AlToken, AlSyntaxNodeType>? node)
      {
         if (node == null)
            return;

         if (node.IsError)
         {
            var start = node.Start.GetStart();
            var end = node.End.GetEnd();
            var span = demoEditor.GetRange(start, end);
            span.SetStyle(_Styles.Error);
         }

         foreach (var child in node.Children) PaintErrors(child as AlParserRule);
      }

      private void MessageList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
      {
         var selectedRow = MessageList.Rows[e.RowIndex];
         if (selectedRow.DataBoundItem is AlErrorFacade error)
         {
            demoEditor.Focus();
            demoEditor.Selection = new TextSelectionRange(demoEditor, error.Column - 1, error.Line - 1, error.Column - 1, error.Line - 1);
            demoEditor.DoCaretVisible();
         }
      }

      private void demoEditor_SelectionChanged(object sender, EventArgs e)
      {
         lblLineNo.Text = (demoEditor.Selection.Start.iLine + 1).ToString();
         lblColumnNo.Text = (demoEditor.Selection.Start.iChar + 1).ToString();
      }

      private void cmbRules_SelectedValueChanged(object sender, EventArgs e)
      {
         ParseSource();
      }
   }
}