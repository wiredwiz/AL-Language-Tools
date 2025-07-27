using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parser_Benchmark
{
   public partial class Benchmark : Form
   {
      public Benchmark()
      {
         InitializeComponent();
      }

      private void numParserThreads_ValueChanged(object sender, EventArgs e)
      {
         if (numFileThreads.Value > numParserThreads.Value)
            numFileThreads.Value = numParserThreads.Value;
      }

      private void numFileThreads_ValueChanged(object sender, EventArgs e)
      {
         if (numFileThreads.Value > numParserThreads.Value)
            numParserThreads.Value = numFileThreads.Value;
      }

      private void chkThreadSafety_CheckedChanged(object sender, EventArgs e)
      {
         if (chkThreadSafety.CheckState == CheckState.Unchecked)
         {
            numFileThreads.Value = 1;
            numParserThreads.Value = 1;
         }
      }

      private void btnFolderSelect_Click(object sender, EventArgs e)
      {
         folderBrowserDialog1.ShowNewFolderButton = false;
         if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
         {
            txtDirPath.Text = folderBrowserDialog1.SelectedPath;
         }
      }
   }
}
