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
         this.objectListView1 = new BrightIdeasSoftware.ObjectListView();
         this.label1 = new System.Windows.Forms.Label();
         this.txtDirPath = new System.Windows.Forms.TextBox();
         this.btnFolderSelect = new System.Windows.Forms.Button();
         this.numFileThreads = new System.Windows.Forms.NumericUpDown();
         this.numParserThreads = new System.Windows.Forms.NumericUpDown();
         this.label2 = new System.Windows.Forms.Label();
         this.label3 = new System.Windows.Forms.Label();
         this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
         this.panel2 = new System.Windows.Forms.Panel();
         this.btnStartParsing = new System.Windows.Forms.Button();
         this.btnStopParsing = new System.Windows.Forms.Button();
         this.richTextBox1 = new System.Windows.Forms.RichTextBox();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.radioButton1 = new System.Windows.Forms.RadioButton();
         this.radioButton2 = new System.Windows.Forms.RadioButton();
         this.chkThreadSafety = new System.Windows.Forms.CheckBox();
         this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
         this.panel1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
         this.splitContainer1.Panel1.SuspendLayout();
         this.splitContainer1.Panel2.SuspendLayout();
         this.splitContainer1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numFileThreads)).BeginInit();
         ((System.ComponentModel.ISupportInitialize)(this.numParserThreads)).BeginInit();
         this.tableLayoutPanel1.SuspendLayout();
         this.panel2.SuspendLayout();
         this.groupBox1.SuspendLayout();
         this.SuspendLayout();
         // 
         // panel1
         // 
         this.panel1.Controls.Add(this.splitContainer1);
         this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.panel1.Location = new System.Drawing.Point(0, 0);
         this.panel1.Name = "panel1";
         this.panel1.Size = new System.Drawing.Size(979, 597);
         this.panel1.TabIndex = 1;
         // 
         // splitContainer1
         // 
         this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.splitContainer1.Location = new System.Drawing.Point(0, 0);
         this.splitContainer1.Name = "splitContainer1";
         this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
         // 
         // splitContainer1.Panel1
         // 
         this.splitContainer1.Panel1.Controls.Add(this.tableLayoutPanel1);
         // 
         // splitContainer1.Panel2
         // 
         this.splitContainer1.Panel2.Controls.Add(this.objectListView1);
         this.splitContainer1.Size = new System.Drawing.Size(979, 597);
         this.splitContainer1.SplitterDistance = 362;
         this.splitContainer1.TabIndex = 0;
         // 
         // objectListView1
         // 
         this.objectListView1.CellEditUseWholeCell = false;
         this.objectListView1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.objectListView1.HideSelection = false;
         this.objectListView1.Location = new System.Drawing.Point(0, 0);
         this.objectListView1.Name = "objectListView1";
         this.objectListView1.Size = new System.Drawing.Size(979, 231);
         this.objectListView1.TabIndex = 0;
         this.objectListView1.UseCompatibleStateImageBehavior = false;
         this.objectListView1.View = System.Windows.Forms.View.Details;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.Location = new System.Drawing.Point(20, 21);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(168, 20);
         this.label1.TabIndex = 0;
         this.label1.Text = "Source File Directory";
         // 
         // txtDirPath
         // 
         this.txtDirPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.txtDirPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.txtDirPath.Location = new System.Drawing.Point(215, 18);
         this.txtDirPath.Name = "txtDirPath";
         this.txtDirPath.Size = new System.Drawing.Size(476, 27);
         this.txtDirPath.TabIndex = 1;
         // 
         // btnFolderSelect
         // 
         this.btnFolderSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btnFolderSelect.Location = new System.Drawing.Point(707, 16);
         this.btnFolderSelect.Name = "btnFolderSelect";
         this.btnFolderSelect.Size = new System.Drawing.Size(57, 33);
         this.btnFolderSelect.TabIndex = 2;
         this.btnFolderSelect.Text = "...";
         this.btnFolderSelect.UseVisualStyleBackColor = true;
         this.btnFolderSelect.Click += new System.EventHandler(this.btnFolderSelect_Click);
         // 
         // numFileThreads
         // 
         this.numFileThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.numFileThreads.Location = new System.Drawing.Point(215, 60);
         this.numFileThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
         this.numFileThreads.Name = "numFileThreads";
         this.numFileThreads.Size = new System.Drawing.Size(57, 27);
         this.numFileThreads.TabIndex = 3;
         this.numFileThreads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
         this.numFileThreads.ValueChanged += new System.EventHandler(this.numFileThreads_ValueChanged);
         // 
         // numParserThreads
         // 
         this.numParserThreads.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.numParserThreads.Location = new System.Drawing.Point(215, 98);
         this.numParserThreads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
         this.numParserThreads.Name = "numParserThreads";
         this.numParserThreads.Size = new System.Drawing.Size(57, 27);
         this.numParserThreads.TabIndex = 4;
         this.numParserThreads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
         this.numParserThreads.ValueChanged += new System.EventHandler(this.numParserThreads_ValueChanged);
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label2.Location = new System.Drawing.Point(53, 62);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(135, 20);
         this.label2.TabIndex = 5;
         this.label2.Text = "# of File Threads";
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label3.Location = new System.Drawing.Point(30, 99);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(158, 20);
         this.label3.TabIndex = 6;
         this.label3.Text = "# of Parser Threads";
         // 
         // tableLayoutPanel1
         // 
         this.tableLayoutPanel1.ColumnCount = 1;
         this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
         this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 0);
         this.tableLayoutPanel1.Controls.Add(this.richTextBox1, 0, 1);
         this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.tableLayoutPanel1.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
         this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
         this.tableLayoutPanel1.Name = "tableLayoutPanel1";
         this.tableLayoutPanel1.RowCount = 2;
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
         this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
         this.tableLayoutPanel1.Size = new System.Drawing.Size(979, 362);
         this.tableLayoutPanel1.TabIndex = 7;
         // 
         // panel2
         // 
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
         this.panel2.Location = new System.Drawing.Point(3, 3);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(973, 194);
         this.panel2.TabIndex = 0;
         // 
         // btnStartParsing
         // 
         this.btnStartParsing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btnStartParsing.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.btnStartParsing.Location = new System.Drawing.Point(784, 21);
         this.btnStartParsing.Name = "btnStartParsing";
         this.btnStartParsing.Size = new System.Drawing.Size(161, 49);
         this.btnStartParsing.TabIndex = 7;
         this.btnStartParsing.Text = "Begin Parsing";
         this.btnStartParsing.UseVisualStyleBackColor = true;
         // 
         // btnStopParsing
         // 
         this.btnStopParsing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
         this.btnStopParsing.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.btnStopParsing.Location = new System.Drawing.Point(784, 103);
         this.btnStopParsing.Name = "btnStopParsing";
         this.btnStopParsing.Size = new System.Drawing.Size(161, 50);
         this.btnStopParsing.TabIndex = 8;
         this.btnStopParsing.Text = "Stop Parsing";
         this.btnStopParsing.UseVisualStyleBackColor = true;
         // 
         // richTextBox1
         // 
         this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
         this.richTextBox1.Location = new System.Drawing.Point(3, 203);
         this.richTextBox1.Name = "richTextBox1";
         this.richTextBox1.Size = new System.Drawing.Size(973, 156);
         this.richTextBox1.TabIndex = 1;
         this.richTextBox1.Text = "";
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.radioButton2);
         this.groupBox1.Controls.Add(this.radioButton1);
         this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.groupBox1.Location = new System.Drawing.Point(319, 62);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(264, 117);
         this.groupBox1.TabIndex = 9;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Parser Selection";
         // 
         // radioButton1
         // 
         this.radioButton1.AutoSize = true;
         this.radioButton1.Checked = true;
         this.radioButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.radioButton1.Location = new System.Drawing.Point(16, 41);
         this.radioButton1.Name = "radioButton1";
         this.radioButton1.Size = new System.Drawing.Size(211, 29);
         this.radioButton1.TabIndex = 0;
         this.radioButton1.TabStop = true;
         this.radioButton1.Text = "Use ANTLR4 Parser";
         this.radioButton1.UseVisualStyleBackColor = true;
         // 
         // radioButton2
         // 
         this.radioButton2.AutoSize = true;
         this.radioButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.radioButton2.Location = new System.Drawing.Point(16, 76);
         this.radioButton2.Name = "radioButton2";
         this.radioButton2.Size = new System.Drawing.Size(212, 29);
         this.radioButton2.TabIndex = 1;
         this.radioButton2.Text = "Use Bespoke Parser";
         this.radioButton2.UseVisualStyleBackColor = true;
         // 
         // chkThreadSafety
         // 
         this.chkThreadSafety.Appearance = System.Windows.Forms.Appearance.Button;
         this.chkThreadSafety.AutoSize = true;
         this.chkThreadSafety.Checked = true;
         this.chkThreadSafety.CheckState = System.Windows.Forms.CheckState.Checked;
         this.chkThreadSafety.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.chkThreadSafety.Location = new System.Drawing.Point(57, 146);
         this.chkThreadSafety.Name = "chkThreadSafety";
         this.chkThreadSafety.Size = new System.Drawing.Size(175, 30);
         this.chkThreadSafety.TabIndex = 10;
         this.chkThreadSafety.Text = "Utilize Thread Safety";
         this.chkThreadSafety.UseVisualStyleBackColor = true;
         this.chkThreadSafety.CheckedChanged += new System.EventHandler(this.chkThreadSafety_CheckedChanged);
         // 
         // Form1
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(979, 597);
         this.Controls.Add(this.panel1);
         this.Name = "Form1";
         this.Text = "Parser Benchmark";
         this.panel1.ResumeLayout(false);
         this.splitContainer1.Panel1.ResumeLayout(false);
         this.splitContainer1.Panel2.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
         this.splitContainer1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.objectListView1)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numFileThreads)).EndInit();
         ((System.ComponentModel.ISupportInitialize)(this.numParserThreads)).EndInit();
         this.tableLayoutPanel1.ResumeLayout(false);
         this.panel2.ResumeLayout(false);
         this.panel2.PerformLayout();
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion
      private System.Windows.Forms.Panel panel1;
      private System.Windows.Forms.SplitContainer splitContainer1;
      private System.Windows.Forms.NumericUpDown numFileThreads;
      private System.Windows.Forms.Button btnFolderSelect;
      private System.Windows.Forms.TextBox txtDirPath;
      private System.Windows.Forms.Label label1;
      private BrightIdeasSoftware.ObjectListView objectListView1;
      private System.Windows.Forms.NumericUpDown numParserThreads;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
      private System.Windows.Forms.Panel panel2;
      private System.Windows.Forms.Button btnStopParsing;
      private System.Windows.Forms.Button btnStartParsing;
      private System.Windows.Forms.RichTextBox richTextBox1;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.RadioButton radioButton2;
      private System.Windows.Forms.RadioButton radioButton1;
      private System.Windows.Forms.CheckBox chkThreadSafety;
      private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
   }
}

