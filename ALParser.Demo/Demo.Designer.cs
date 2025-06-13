namespace ALParser.Demo
{
   partial class Demo
   {
      /// <summary>
      ///  Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      ///  Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      ///  Required method for Designer support - do not modify
      ///  the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Demo));
         splitParser = new SplitContainer();
         tabConGrammar = new TabControl();
         tabPageEdit = new TabPage();
         demoEditor = new FastColoredTextBoxNS.FastColoredTextBox();
         statusStrip1 = new StatusStrip();
         lblColumnNo = new ToolStripStatusLabel();
         toolStripStatusLabel2 = new ToolStripStatusLabel();
         lblLineNo = new ToolStripStatusLabel();
         toolStripStatusLabel4 = new ToolStripStatusLabel();
         tabPageGraph = new TabPage();
         gViewer1 = new Microsoft.Msagl.GraphViewerGdi.GViewer();
         tabPageTokens = new TabPage();
         listTokens = new DataGridView();
         flowLayoutPanel1 = new FlowLayoutPanel();
         label1 = new Label();
         cmbRules = new ComboBox();
         MessageList = new DataGridView();
         Source = new DataGridViewTextBoxColumn();
         Message = new DataGridViewTextBoxColumn();
         menuStrip1 = new MenuStrip();
         ((System.ComponentModel.ISupportInitialize)splitParser).BeginInit();
         splitParser.Panel1.SuspendLayout();
         splitParser.Panel2.SuspendLayout();
         splitParser.SuspendLayout();
         tabConGrammar.SuspendLayout();
         tabPageEdit.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)demoEditor).BeginInit();
         statusStrip1.SuspendLayout();
         tabPageGraph.SuspendLayout();
         tabPageTokens.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)listTokens).BeginInit();
         flowLayoutPanel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)MessageList).BeginInit();
         SuspendLayout();
         // 
         // splitParser
         // 
         splitParser.Dock = DockStyle.Fill;
         splitParser.Location = new Point(0, 24);
         splitParser.Name = "splitParser";
         splitParser.Orientation = Orientation.Horizontal;
         // 
         // splitParser.Panel1
         // 
         splitParser.Panel1.Controls.Add(tabConGrammar);
         splitParser.Panel1.Controls.Add(flowLayoutPanel1);
         // 
         // splitParser.Panel2
         // 
         splitParser.Panel2.Controls.Add(MessageList);
         splitParser.Size = new Size(939, 726);
         splitParser.SplitterDistance = 580;
         splitParser.TabIndex = 0;
         // 
         // tabConGrammar
         // 
         tabConGrammar.Controls.Add(tabPageEdit);
         tabConGrammar.Controls.Add(tabPageGraph);
         tabConGrammar.Controls.Add(tabPageTokens);
         tabConGrammar.Dock = DockStyle.Fill;
         tabConGrammar.Location = new Point(0, 44);
         tabConGrammar.Name = "tabConGrammar";
         tabConGrammar.SelectedIndex = 0;
         tabConGrammar.Size = new Size(939, 536);
         tabConGrammar.TabIndex = 1;
         // 
         // tabPageEdit
         // 
         tabPageEdit.Controls.Add(demoEditor);
         tabPageEdit.Controls.Add(statusStrip1);
         tabPageEdit.Location = new Point(4, 29);
         tabPageEdit.Name = "tabPageEdit";
         tabPageEdit.Padding = new Padding(3);
         tabPageEdit.Size = new Size(931, 503);
         tabPageEdit.TabIndex = 0;
         tabPageEdit.Text = "Editor";
         tabPageEdit.UseVisualStyleBackColor = true;
         // 
         // demoEditor
         // 
         demoEditor.AccessibleDescription = "Textbox control";
         demoEditor.AccessibleName = "Fast Colored Text Box";
         demoEditor.AccessibleRole = AccessibleRole.Text;
         demoEditor.AutoCompleteBracketsList = new char[] { '(', ')', '{', '}', '[', ']', '"', '"', '\'', '\'' };
         demoEditor.AutoIndentCharsPatterns = "^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\r\n^\\s*(case|default)\\s*[^:]*(?<range>:)\\s*(?<range>[^;]+);";
         demoEditor.AutoScrollMinSize = new Size(0, 18126);
         demoEditor.BackBrush = null;
         demoEditor.BackColor = Color.Black;
         demoEditor.CaretColor = Color.LightGray;
         demoEditor.CharHeight = 18;
         demoEditor.CharWidth = 10;
         demoEditor.DefaultMarkerSize = 8;
         demoEditor.DisabledColor = Color.FromArgb(100, 180, 180, 180);
         demoEditor.Dock = DockStyle.Fill;
         demoEditor.EnableBlinkingStyle = false;
         demoEditor.FindForm = null;
         demoEditor.FoldingHighlightColor = Color.LightGray;
         demoEditor.FoldingHighlightEnabled = false;
         demoEditor.ForeColor = Color.White;
         demoEditor.GoToForm = null;
         demoEditor.Hotkeys = resources.GetString("demoEditor.Hotkeys");
         demoEditor.IsReplaceMode = false;
         demoEditor.LeftBracket = '{';
         demoEditor.LeftBracket2 = '(';
         demoEditor.LeftBracket3 = '[';
         demoEditor.Location = new Point(3, 3);
         demoEditor.Name = "demoEditor";
         demoEditor.Paddings = new Padding(0);
         demoEditor.ReplaceForm = null;
         demoEditor.RightBracket = '}';
         demoEditor.RightBracket2 = ')';
         demoEditor.RightBracket3 = ']';
         demoEditor.SelectionColor = Color.FromArgb(100, 0, 120, 215);
         demoEditor.ServiceColors = null;
         demoEditor.ShowFoldingLines = true;
         demoEditor.Size = new Size(925, 471);
         demoEditor.TabIndex = 0;
         demoEditor.Text = resources.GetString("demoEditor.Text");
         demoEditor.TextAreaBorderColor = Color.White;
         demoEditor.ToolTipDelay = 100;
         demoEditor.WordWrap = true;
         demoEditor.Zoom = 100;
         demoEditor.TextChanged += demoEditor_TextChanged;
         demoEditor.SelectionChanged += demoEditor_SelectionChanged;
         // 
         // statusStrip1
         // 
         statusStrip1.ImageScalingSize = new Size(20, 20);
         statusStrip1.Items.AddRange(new ToolStripItem[] { lblColumnNo, toolStripStatusLabel2, lblLineNo, toolStripStatusLabel4 });
         statusStrip1.Location = new Point(3, 474);
         statusStrip1.Name = "statusStrip1";
         statusStrip1.RightToLeft = RightToLeft.Yes;
         statusStrip1.Size = new Size(925, 26);
         statusStrip1.TabIndex = 3;
         statusStrip1.Text = "statusStrip1";
         // 
         // lblColumnNo
         // 
         lblColumnNo.Name = "lblColumnNo";
         lblColumnNo.Size = new Size(17, 20);
         lblColumnNo.Text = "1";
         // 
         // toolStripStatusLabel2
         // 
         toolStripStatusLabel2.Name = "toolStripStatusLabel2";
         toolStripStatusLabel2.Size = new Size(31, 20);
         toolStripStatusLabel2.Text = "Col";
         // 
         // lblLineNo
         // 
         lblLineNo.Name = "lblLineNo";
         lblLineNo.Size = new Size(17, 20);
         lblLineNo.Text = "1";
         // 
         // toolStripStatusLabel4
         // 
         toolStripStatusLabel4.Name = "toolStripStatusLabel4";
         toolStripStatusLabel4.Size = new Size(36, 20);
         toolStripStatusLabel4.Text = "Line";
         // 
         // tabPageGraph
         // 
         tabPageGraph.Controls.Add(gViewer1);
         tabPageGraph.Location = new Point(4, 29);
         tabPageGraph.Name = "tabPageGraph";
         tabPageGraph.Padding = new Padding(3);
         tabPageGraph.Size = new Size(931, 503);
         tabPageGraph.TabIndex = 1;
         tabPageGraph.Text = "Graph";
         tabPageGraph.UseVisualStyleBackColor = true;
         // 
         // gViewer1
         // 
         gViewer1.ArrowheadLength = 10D;
         gViewer1.AsyncLayout = false;
         gViewer1.AutoScroll = true;
         gViewer1.BackwardEnabled = false;
         gViewer1.BuildHitTree = true;
         gViewer1.CurrentLayoutMethod = Microsoft.Msagl.GraphViewerGdi.LayoutMethod.UseSettingsOfTheGraph;
         gViewer1.Dock = DockStyle.Fill;
         gViewer1.EdgeInsertButtonVisible = false;
         gViewer1.FileName = "";
         gViewer1.ForwardEnabled = false;
         gViewer1.Graph = null;
         gViewer1.IncrementalDraggingModeAlways = false;
         gViewer1.InsertingEdge = false;
         gViewer1.LayoutAlgorithmSettingsButtonVisible = false;
         gViewer1.LayoutEditingEnabled = true;
         gViewer1.Location = new Point(3, 3);
         gViewer1.LooseOffsetForRouting = 0.25D;
         gViewer1.MouseHitDistance = 0.05D;
         gViewer1.Name = "gViewer1";
         gViewer1.NavigationVisible = false;
         gViewer1.NeedToCalculateLayout = true;
         gViewer1.OffsetForRelaxingInRouting = 0.6D;
         gViewer1.PaddingForEdgeRouting = 8D;
         gViewer1.PanButtonPressed = false;
         gViewer1.SaveAsImageEnabled = true;
         gViewer1.SaveAsMsaglEnabled = true;
         gViewer1.SaveButtonVisible = false;
         gViewer1.SaveGraphButtonVisible = false;
         gViewer1.SaveInVectorFormatEnabled = true;
         gViewer1.Size = new Size(925, 497);
         gViewer1.TabIndex = 0;
         gViewer1.TightOffsetForRouting = 0.125D;
         gViewer1.ToolBarIsVisible = true;
         gViewer1.Transform = (Microsoft.Msagl.Core.Geometry.Curves.PlaneTransformation)resources.GetObject("gViewer1.Transform");
         gViewer1.UndoRedoButtonsVisible = false;
         gViewer1.WindowZoomButtonPressed = false;
         gViewer1.ZoomF = 1D;
         gViewer1.ZoomWindowThreshold = 0.05D;
         // 
         // tabPageTokens
         // 
         tabPageTokens.Controls.Add(listTokens);
         tabPageTokens.Location = new Point(4, 29);
         tabPageTokens.Name = "tabPageTokens";
         tabPageTokens.Size = new Size(931, 503);
         tabPageTokens.TabIndex = 2;
         tabPageTokens.Text = "Tokens";
         tabPageTokens.UseVisualStyleBackColor = true;
         // 
         // listTokens
         // 
         listTokens.AllowUserToAddRows = false;
         listTokens.AllowUserToDeleteRows = false;
         listTokens.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         listTokens.Dock = DockStyle.Fill;
         listTokens.Location = new Point(0, 0);
         listTokens.Name = "listTokens";
         listTokens.ReadOnly = true;
         listTokens.RowHeadersWidth = 51;
         listTokens.RowTemplate.Height = 29;
         listTokens.Size = new Size(931, 503);
         listTokens.TabIndex = 0;
         // 
         // flowLayoutPanel1
         // 
         flowLayoutPanel1.Controls.Add(label1);
         flowLayoutPanel1.Controls.Add(cmbRules);
         flowLayoutPanel1.Dock = DockStyle.Top;
         flowLayoutPanel1.Location = new Point(0, 0);
         flowLayoutPanel1.Name = "flowLayoutPanel1";
         flowLayoutPanel1.Padding = new Padding(4);
         flowLayoutPanel1.Size = new Size(939, 44);
         flowLayoutPanel1.TabIndex = 2;
         // 
         // label1
         // 
         label1.Anchor = AnchorStyles.Left;
         label1.AutoSize = true;
         label1.Location = new Point(7, 11);
         label1.Name = "label1";
         label1.Size = new Size(81, 20);
         label1.TabIndex = 1;
         label1.Text = "Parser Rule";
         // 
         // cmbRules
         // 
         cmbRules.FormattingEnabled = true;
         cmbRules.Location = new Point(94, 7);
         cmbRules.Name = "cmbRules";
         cmbRules.Size = new Size(289, 28);
         cmbRules.TabIndex = 0;
         cmbRules.SelectedValueChanged += cmbRules_SelectedValueChanged;
         // 
         // MessageList
         // 
         MessageList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
         MessageList.Columns.AddRange(new DataGridViewColumn[] { Source, Message });
         MessageList.Dock = DockStyle.Fill;
         MessageList.Location = new Point(0, 0);
         MessageList.MultiSelect = false;
         MessageList.Name = "MessageList";
         MessageList.RowHeadersWidth = 51;
         MessageList.RowTemplate.Height = 29;
         MessageList.Size = new Size(939, 142);
         MessageList.TabIndex = 0;
         MessageList.CellDoubleClick += MessageList_CellDoubleClick;
         // 
         // Source
         // 
         Source.DataPropertyName = "Source";
         Source.HeaderText = "Source";
         Source.MinimumWidth = 6;
         Source.Name = "Source";
         Source.ReadOnly = true;
         Source.Width = 125;
         // 
         // Message
         // 
         Message.DataPropertyName = "Message";
         Message.HeaderText = "Message";
         Message.MinimumWidth = 6;
         Message.Name = "Message";
         Message.ReadOnly = true;
         Message.Width = 500;
         // 
         // menuStrip1
         // 
         menuStrip1.ImageScalingSize = new Size(20, 20);
         menuStrip1.Location = new Point(0, 0);
         menuStrip1.Name = "menuStrip1";
         menuStrip1.Size = new Size(939, 24);
         menuStrip1.TabIndex = 1;
         menuStrip1.Text = "menuStrip1";
         // 
         // Demo
         // 
         AutoScaleDimensions = new SizeF(8F, 20F);
         AutoScaleMode = AutoScaleMode.Font;
         ClientSize = new Size(939, 750);
         Controls.Add(splitParser);
         Controls.Add(menuStrip1);
         Name = "Demo";
         Text = "Parser Demo";
         splitParser.Panel1.ResumeLayout(false);
         splitParser.Panel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)splitParser).EndInit();
         splitParser.ResumeLayout(false);
         tabConGrammar.ResumeLayout(false);
         tabPageEdit.ResumeLayout(false);
         tabPageEdit.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)demoEditor).EndInit();
         statusStrip1.ResumeLayout(false);
         statusStrip1.PerformLayout();
         tabPageGraph.ResumeLayout(false);
         tabPageTokens.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)listTokens).EndInit();
         flowLayoutPanel1.ResumeLayout(false);
         flowLayoutPanel1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)MessageList).EndInit();
         ResumeLayout(false);
         PerformLayout();
      }

      #endregion

      private SplitContainer splitParser;
      private TabControl tabConGrammar;
      private TabPage tabPageEdit;
      private FastColoredTextBoxNS.FastColoredTextBox demoEditor;
      private TabPage tabPageGraph;
      private Microsoft.Msagl.GraphViewerGdi.GViewer gViewer1;
      private DataGridView dataGridView1;
      private DataGridView MessageList;
      private DataGridViewTextBoxColumn Source;
      private DataGridViewTextBoxColumn Message;
      private TabPage tabPageTokens;
      private DataGridView listTokens;
      private FlowLayoutPanel flowLayoutPanel1;
      private Label label1;
      private ComboBox cmbRules;
      private MenuStrip menuStrip1;
      private StatusStrip statusStrip1;
      private ToolStripStatusLabel lblColumnNo;
      private ToolStripStatusLabel toolStripStatusLabel2;
      private ToolStripStatusLabel lblLineNo;
      private ToolStripStatusLabel toolStripStatusLabel4;
   }
}