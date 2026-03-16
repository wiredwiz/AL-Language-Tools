namespace Parser_Benchmark
{
   partial class Benchmark
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
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
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         this.panel1 = new System.Windows.Forms.Panel();
         this.splitContainer1 = new System.Windows.Forms.SplitContainer();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.panel2 = new System.Windows.Forms.Panel();
         this.chkUpdates = new System.Windows.Forms.CheckBox();
         this.txtRunTime = new System.Windows.Forms.TextBox();
         this.label6 = new System.Windows.Forms.Label();
         this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
         this.progParsing = new System.Windows.Forms.ProgressBar();
         this.progLoading = new System.Windows.Forms.ProgressBar();
         this.label5 = new System.Windows.Forms.Label();
         this.label4 = new System.Windows.Forms.Label();
         this.chkThreadSafety = new System.Windows.Forms.CheckBox();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.radBespokeParser = new System.Windows.Forms.RadioButton();
         this.radAntlrParser = new System.Windows.Forms.RadioButton();
         this.btnStopParsing = new System.Windows.Forms.Button();
         this.btnStartParsing = new System.Windows.Forms.Button();
         this.label2 = new System.Windows.Forms.Label();
         this.btnFolderSelect = new System.Windows.Forms.Button();
         this.label3 = new System.Windows.Forms.Label();
         this.txtDirPath = new System.Windows.Forms.TextBox();
         this.numFileThreads = new System.Windows.Forms.NumericUpDown();
         this.label1 = new System.Windows.Forms.Label();
         this.numParserThreads = new System.Windows.Forms.NumericUpDown();
         this.viewStatus = new BrightIdeasSoftware.FastDataListView();
         this.olvColAction = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
         this.olvColFile = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
         this.olvColTime = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
         this.viewErrors = new BrightIdeasSoftware.FastObjectListView();
         this.olvColSource = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
         this.olvColParseFile = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
         this.olvColSeverity = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
         this.olvColLine = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
         this.olvColColumn = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
         this.olvColMessage = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
         this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
         this.splitContainer1.Panel1.SuspendLayout();
         this.splitContainer1.Panel2.SuspendLayout();
         this.splitContainer1.SuspendLayout();
         this.tableLayoutPanel1.SuspendLayout();
         this.panel2.SuspendLayout();
         this.tableLayoutPanel2.SuspendLayout();
         this.groupBox1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.numFileThreads)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numParserThreads)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.viewStatus)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.viewErrors)).BeginInit();
         this.SuspendLayout();
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.splitContainer1);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(734, 531);
         this.panel1.TabIndex = 1;
         // 
         // splitContainer1
         // 
         this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer1.Location = new System.Drawing.Point(0, 0);
         this.splitContainer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.splitContainer1.Name = "splitContainer1";
         this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
         // 
         // splitContainer1.Panel1
         // 
         this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
         // 
         // splitContainer1.Panel2
         // 
         this.splitContainer1.Panel2.Controls.Add(this.viewErrors);
         this.splitContainer1.Size = new System.Drawing.Size(734, 531);
         this.splitContainer1.SplitterDistance = 380;
         this.splitContainer1.SplitterWidth = 3;
         this.splitContainer1.TabIndex = 0;
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 1;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.viewStatus, 0, 1);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 2;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 244F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
         this.tableLayoutPanel1.Size = new System.Drawing.Size(734, 380);
         this.tableLayoutPanel1.TabIndex = 7;
         // 
         // panel2
         // 
         this.panel2.Controls.Add(this.chkUpdates);
         this.panel2.Controls.Add(this.txtRunTime);
         this.panel2.Controls.Add(this.label6);
         this.panel2.Controls.Add(this.tableLayoutPanel2);
         this.panel2.Controls.Add(this.chkThreadSafety);
         this.panel2.Controls.Add(this.groupBox1);
         this.panel2.Controls.Add(this.btnStopParsing);
         this.panel2.Controls.Add(this.btnStartParsing);
         this.panel2.Controls.Add(this.label2);
         this.panel2.Controls.Add(this.btnFolderSelect);
         this.panel2.Controls.Add(this.label3);
         this.panel2.Controls.Add(this.txtDirPath);
         this.panel2.Controls.Add(this.numFileThreads);
         this.panel2.Controls.Add(this.label1);
         this.panel2.Controls.Add(this.numParserThreads);
         this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel2.Location = new System.Drawing.Point(2, 2);
         this.panel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(730, 240);
         this.panel2.TabIndex = 0;
         // 
         // chkUpdates
         // 
         this.chkUpdates.Appearance = System.Windows.Forms.Appearance.Button;
         this.chkUpdates.AutoSize = true;
         this.chkUpdates.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.chkUpdates.Location = new System.Drawing.Point(39, 151);
         this.chkUpdates.Name = "chkUpdates";
         this.chkUpdates.Size = new System.Drawing.Size(156, 26);
         this.chkUpdates.TabIndex = 17;
         this.chkUpdates.Text = "Display Update Details";
         this.chkUpdates.UseVisualStyleBackColor = true;
         // 
         // txtRunTime
         // 
         this.txtRunTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.txtRunTime.Location = new System.Drawing.Point(436, 80);
         this.txtRunTime.Name = "txtRunTime";
         this.txtRunTime.ReadOnly = true;
         this.txtRunTime.Size = new System.Drawing.Size(137, 22);
         this.txtRunTime.TabIndex = 16;
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label6.Location = new System.Drawing.Point(442, 50);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(77, 20);
         this.label6.TabIndex = 15;
         this.label6.Text = "Run Time";
         // 
         // tableLayoutPanel2
         // 
         this.tableLayoutPanel2.ColumnCount = 2;
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 101F));
         this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel2.Controls.Add(this.progParsing, 1, 1);
         this.tableLayoutPanel2.Controls.Add(this.progLoading, 1, 0);
         this.tableLayoutPanel2.Controls.Add(this.label5, 0, 0);
         this.tableLayoutPanel2.Controls.Add(this.label4, 0, 1);
         this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 186);
         this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.tableLayoutPanel2.Name = "tableLayoutPanel2";
         this.tableLayoutPanel2.RowCount = 2;
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
         this.tableLayoutPanel2.Size = new System.Drawing.Size(730, 54);
         this.tableLayoutPanel2.TabIndex = 14;
         // 
         // progParsing
         // 
         this.progParsing.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.progParsing.Location = new System.Drawing.Point(103, 33);
         this.progParsing.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.progParsing.Name = "progParsing";
         this.progParsing.Size = new System.Drawing.Size(625, 19);
         this.progParsing.TabIndex = 11;
         // 
         // progLoading
         // 
         this.progLoading.Dock = System.Windows.Forms.DockStyle.Bottom;
         this.progLoading.Location = new System.Drawing.Point(103, 6);
         this.progLoading.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.progLoading.Name = "progLoading";
         this.progLoading.Size = new System.Drawing.Size(625, 19);
         this.progLoading.TabIndex = 12;
         // 
         // label5
         // 
         this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.label5.AutoSize = true;
         this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label5.Location = new System.Drawing.Point(2, 7);
         this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(97, 20);
         this.label5.TabIndex = 13;
         this.label5.Text = "File Loading";
         // 
         // label4
         // 
         this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.label4.AutoSize = true;
         this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label4.Location = new System.Drawing.Point(2, 34);
         this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(97, 20);
         this.label4.TabIndex = 12;
         this.label4.Text = "File Parsing";
         // 
         // chkThreadSafety
         // 
         this.chkThreadSafety.Appearance = System.Windows.Forms.Appearance.Button;
         this.chkThreadSafety.AutoSize = true;
         this.chkThreadSafety.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.chkThreadSafety.Location = new System.Drawing.Point(43, 119);
         this.chkThreadSafety.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.chkThreadSafety.Name = "chkThreadSafety";
         this.chkThreadSafety.Size = new System.Drawing.Size(150, 27);
         this.chkThreadSafety.TabIndex = 10;
         this.chkThreadSafety.Text = "Utilize Thread Safety";
         this.chkThreadSafety.UseVisualStyleBackColor = true;
         this.chkThreadSafety.CheckedChanged += new System.EventHandler(this.chkThreadSafety_CheckedChanged);
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.radBespokeParser);
         this.groupBox1.Controls.Add(this.radAntlrParser);
         this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.groupBox1.Location = new System.Drawing.Point(220, 49);
         this.groupBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.groupBox1.Size = new System.Drawing.Size(198, 95);
         this.groupBox1.TabIndex = 9;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Parser Selection";
         // 
         // radBespokeParser
         // 
         this.radBespokeParser.AutoSize = true;
         this.radBespokeParser.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.radBespokeParser.Location = new System.Drawing.Point(12, 62);
         this.radBespokeParser.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.radBespokeParser.Name = "radBespokeParser";
         this.radBespokeParser.Size = new System.Drawing.Size(173, 24);
         this.radBespokeParser.TabIndex = 1;
         this.radBespokeParser.Text = "Use Bespoke Parser";
         this.radBespokeParser.UseVisualStyleBackColor = true;
         // 
         // radAntlrParser
         // 
         this.radAntlrParser.AutoSize = true;
         this.radAntlrParser.Checked = true;
         this.radAntlrParser.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.radAntlrParser.Location = new System.Drawing.Point(12, 33);
         this.radAntlrParser.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.radAntlrParser.Name = "radAntlrParser";
         this.radAntlrParser.Size = new System.Drawing.Size(171, 24);
         this.radAntlrParser.TabIndex = 0;
         this.radAntlrParser.TabStop = true;
         this.radAntlrParser.Text = "Use ANTLR4 Parser";
         this.radAntlrParser.UseVisualStyleBackColor = true;
         // 
         // btnStopParsing
         // 
         this.btnStopParsing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btnStopParsing.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.btnStopParsing.Location = new System.Drawing.Point(588, 84);
         this.btnStopParsing.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.btnStopParsing.Name = "btnStopParsing";
         this.btnStopParsing.Size = new System.Drawing.Size(121, 41);
         this.btnStopParsing.TabIndex = 8;
         this.btnStopParsing.Text = "Stop Parsing";
         this.btnStopParsing.UseVisualStyleBackColor = true;
         this.btnStopParsing.Click += new System.EventHandler(this.BtnStopParsing_Click);
         // 
         // btnStartParsing
         // 
         this.btnStartParsing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btnStartParsing.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.btnStartParsing.Location = new System.Drawing.Point(588, 17);
         this.btnStartParsing.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.btnStartParsing.Name = "btnStartParsing";
         this.btnStartParsing.Size = new System.Drawing.Size(121, 40);
         this.btnStartParsing.TabIndex = 7;
         this.btnStartParsing.Text = "Begin Parsing";
         this.btnStartParsing.UseVisualStyleBackColor = true;
         this.btnStartParsing.Click += new System.EventHandler(this.BtnStartParsing_Click);
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label2.Location = new System.Drawing.Point(40, 50);
         this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(115, 17);
         this.label2.TabIndex = 5;
         this.label2.Text = "# of File Threads";
         // 
         // btnFolderSelect
         // 
         this.btnFolderSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btnFolderSelect.Location = new System.Drawing.Point(530, 13);
         this.btnFolderSelect.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.btnFolderSelect.Name = "btnFolderSelect";
         this.btnFolderSelect.Size = new System.Drawing.Size(43, 27);
         this.btnFolderSelect.TabIndex = 2;
         this.btnFolderSelect.Text = "...";
         this.btnFolderSelect.UseVisualStyleBackColor = true;
         this.btnFolderSelect.Click += new System.EventHandler(this.btnFolderSelect_Click);
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label3.Location = new System.Drawing.Point(22, 80);
         this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(135, 17);
         this.label3.TabIndex = 6;
         this.label3.Text = "# of Parser Threads";
         // 
         // txtDirPath
         // 
         this.txtDirPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.txtDirPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.txtDirPath.Location = new System.Drawing.Point(161, 15);
         this.txtDirPath.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.txtDirPath.Name = "txtDirPath";
         this.txtDirPath.Size = new System.Drawing.Size(358, 23);
         this.txtDirPath.TabIndex = 1;
         // 
         // numFileThreads
         // 
         this.numFileThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.numFileThreads.Location = new System.Drawing.Point(161, 49);
         this.numFileThreads.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.numFileThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
         this.numFileThreads.Name = "numFileThreads";
         this.numFileThreads.Size = new System.Drawing.Size(43, 23);
         this.numFileThreads.TabIndex = 3;
         this.numFileThreads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
         this.numFileThreads.ValueChanged += new System.EventHandler(this.numFileThreads_ValueChanged);
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.Location = new System.Drawing.Point(15, 17);
         this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(140, 17);
         this.label1.TabIndex = 0;
         this.label1.Text = "Source File Directory";
         // 
         // numParserThreads
         // 
         this.numParserThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.numParserThreads.Location = new System.Drawing.Point(161, 80);
         this.numParserThreads.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.numParserThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
         this.numParserThreads.Name = "numParserThreads";
         this.numParserThreads.Size = new System.Drawing.Size(43, 23);
         this.numParserThreads.TabIndex = 4;
         this.numParserThreads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
         this.numParserThreads.ValueChanged += new System.EventHandler(this.numParserThreads_ValueChanged);
         // 
         // viewStatus
         // 
         this.viewStatus.AllColumns.Add(this.olvColAction);
         this.viewStatus.AllColumns.Add(this.olvColFile);
         this.viewStatus.AllColumns.Add(this.olvColTime);
         this.viewStatus.CellEditUseWholeCell = false;
         this.viewStatus.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColAction,
            this.olvColFile,
            this.olvColTime});
         this.viewStatus.Cursor = System.Windows.Forms.Cursors.Default;
         this.viewStatus.DataSource = null;
         this.viewStatus.Dock = System.Windows.Forms.DockStyle.Fill;
         this.viewStatus.HideSelection = false;
         this.viewStatus.Location = new System.Drawing.Point(3, 247);
         this.viewStatus.Name = "viewStatus";
         this.viewStatus.ShowGroups = false;
         this.viewStatus.Size = new System.Drawing.Size(728, 132);
         this.viewStatus.TabIndex = 1;
         this.viewStatus.UseCompatibleStateImageBehavior = false;
         this.viewStatus.View = System.Windows.Forms.View.Details;
         this.viewStatus.VirtualMode = true;
         // 
         // olvColAction
         // 
         this.olvColAction.AspectName = "Action";
         this.olvColAction.Text = "Action";
         this.olvColAction.Width = 80;
         // 
         // olvColFile
         // 
         this.olvColFile.AspectName = "FileName";
         this.olvColFile.Text = "File";
         this.olvColFile.Width = 300;
         // 
         // olvColTime
         // 
         this.olvColTime.AspectName = "Time";
         this.olvColTime.Text = "Time";
         this.olvColTime.Width = 400;
         // 
         // viewErrors
         // 
         this.viewErrors.AllColumns.Add(this.olvColSource);
         this.viewErrors.AllColumns.Add(this.olvColParseFile);
         this.viewErrors.AllColumns.Add(this.olvColSeverity);
         this.viewErrors.AllColumns.Add(this.olvColLine);
         this.viewErrors.AllColumns.Add(this.olvColColumn);
         this.viewErrors.AllColumns.Add(this.olvColMessage);
         this.viewErrors.CellEditUseWholeCell = false;
         this.viewErrors.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColSource,
            this.olvColParseFile,
            this.olvColSeverity,
            this.olvColLine,
            this.olvColColumn,
            this.olvColMessage});
         this.viewErrors.Cursor = System.Windows.Forms.Cursors.Default;
         this.viewErrors.Dock = System.Windows.Forms.DockStyle.Fill;
         this.viewErrors.FullRowSelect = true;
         this.viewErrors.HideSelection = false;
         this.viewErrors.Location = new System.Drawing.Point(0, 0);
         this.viewErrors.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.viewErrors.Name = "viewErrors";
         this.viewErrors.ShowGroups = false;
         this.viewErrors.Size = new System.Drawing.Size(734, 148);
         this.viewErrors.TabIndex = 0;
         this.viewErrors.UseCompatibleStateImageBehavior = false;
         this.viewErrors.View = System.Windows.Forms.View.Details;
         this.viewErrors.VirtualMode = true;
         this.viewErrors.DoubleClick += new System.EventHandler(this.viewErrors_DoubleClick);
         // 
         // olvColSource
         // 
         this.olvColSource.AspectName = "Source";
         this.olvColSource.Text = "Source";
         this.olvColSource.Width = 80;
         // 
         // olvColParseFile
         // 
         this.olvColParseFile.AspectName = "SourceFile";
         this.olvColParseFile.Text = "File";
         this.olvColParseFile.Width = 200;
         // 
         // olvColSeverity
         // 
         this.olvColSeverity.AspectName = "Severity";
         this.olvColSeverity.Text = "Severity";
         // 
         // olvColLine
         // 
         this.olvColLine.AspectName = "Line";
         this.olvColLine.Text = "Line";
         // 
         // olvColColumn
         // 
         this.olvColColumn.AspectName = "Position";
         this.olvColColumn.Text = "Column";
         // 
         // olvColMessage
         // 
         this.olvColMessage.AspectName = "Text";
         this.olvColMessage.Text = "Message";
         this.olvColMessage.Width = 620;
         // 
         // Benchmark
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(734, 531);
         this.Controls.Add(this.panel1);
         this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
         this.Name = "Benchmark";
         this.Text = "Parser Benchmark";
         this.panel1.ResumeLayout(false);
         this.splitContainer1.Panel1.ResumeLayout(false);
         this.splitContainer1.Panel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
         this.splitContainer1.ResumeLayout(false);
         this.tableLayoutPanel1.ResumeLayout(false);
         this.panel2.ResumeLayout(false);
         this.panel2.PerformLayout();
         this.tableLayoutPanel2.ResumeLayout(false);
         this.tableLayoutPanel2.PerformLayout();
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         ((System.ComponentModel.ISupportInitialize)(this.numFileThreads)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numParserThreads)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.viewStatus)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.viewErrors)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.SplitContainer splitContainer1;
      private System.Windows.Forms.NumericUpDown numFileThreads;
      private System.Windows.Forms.Button btnFolderSelect;
      private System.Windows.Forms.TextBox txtDirPath;
      private System.Windows.Forms.Label label1;
      private BrightIdeasSoftware.FastObjectListView viewErrors;
      private System.Windows.Forms.NumericUpDown numParserThreads;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.Panel panel2;
      private System.Windows.Forms.Button btnStopParsing;
      private System.Windows.Forms.Button btnStartParsing;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.RadioButton radBespokeParser;
      private System.Windows.Forms.RadioButton radAntlrParser;
      private System.Windows.Forms.CheckBox chkThreadSafety;
      private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.ProgressBar progParsing;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.ProgressBar progLoading;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
      private BrightIdeasSoftware.OLVColumn olvColParseFile;
      private BrightIdeasSoftware.OLVColumn olvColLine;
      private BrightIdeasSoftware.OLVColumn olvColColumn;
      private BrightIdeasSoftware.OLVColumn olvColMessage;
      private BrightIdeasSoftware.FastDataListView viewStatus;
      private BrightIdeasSoftware.OLVColumn olvColAction;
      private BrightIdeasSoftware.OLVColumn olvColFile;
      private BrightIdeasSoftware.OLVColumn olvColTime;
      private System.Windows.Forms.TextBox txtRunTime;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.CheckBox chkUpdates;
      private BrightIdeasSoftware.OLVColumn olvColSource;
      private BrightIdeasSoftware.OLVColumn olvColSeverity;
   }
}

