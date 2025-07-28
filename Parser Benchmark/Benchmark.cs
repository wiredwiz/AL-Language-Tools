using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Parser_Benchmark
{
   public partial class Benchmark : Form
   {
      private IParserWorker[] _ParserWorker;
      private Task[] _ParserTasks;
      private FileQueue _FileQueue;
      private bool _IsRunning = false;
      private CancellationTokenSource _Cts;

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

      private async void BtnStartParsing_Click(object sender, EventArgs e)
      {
         try
         {
            if (_IsRunning)
               return;

            txtOutput.Clear();
            _IsRunning = true;
            _FileQueue = new FileQueue();
            if (_Cts != null)
               _Cts.Dispose();
            _Cts = new CancellationTokenSource();

            ReadAllFilesAsync(_Cts, _FileQueue, txtDirPath.Text, (int)numFileThreads.Value);

            _ParserWorker = new IParserWorker[(int)numParserThreads.Value];
            _ParserTasks = new Task[(int)numParserThreads.Value];
            for (int i = 0; i < _ParserWorker.Length; i++)
            {
               if (radAntlrParser.Checked)
                  _ParserWorker[i] = new AntlrParseWorker(_FileQueue, _Cts);

               _ParserWorker[i].ReportData += Benchmark_ReportData;
               _ParserTasks[i] = _ParserWorker[i].StartAsync();
            }

            var watch = Stopwatch.StartNew();
            await Task.WhenAll(_ParserTasks).ConfigureAwait(true);
            watch.Stop();
            WriteOutput($"All parsing finished in {watch.Elapsed}");
         }
         catch (OperationCanceledException)
         {
            // do nothing
            WriteOutput($"Parsing benchmark halted at {DateTime.Now.ToString()}");
            _IsRunning = false;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message, "Error");
         }
         finally
         {

         }
      }

      private void Benchmark_ReportData(object sender, Datum e)
      {
         WriteOutput($"Finished parsing \"{Path.GetFileName(e.FileName)}\" in {e.ParseTime.ToString()}");
      }

      private void BtnStopParsing_Click(object sender, EventArgs e)
      {
         _Cts.Cancel();
         _IsRunning = false;
      }

      public async void ReadAllFilesAsync(CancellationTokenSource cts, FileQueue fileQueue, string directoryPath, int workerCount)
      {
         if (cts is null) throw new ArgumentNullException(nameof(cts));
         if (string.IsNullOrWhiteSpace(directoryPath)) throw new ArgumentException("Path is required.", nameof(directoryPath));
         if (!Directory.Exists(directoryPath)) throw new DirectoryNotFoundException(directoryPath);
         if (workerCount <= 0) throw new ArgumentOutOfRangeException(nameof(workerCount), "Must be > 0");

         var token = cts.Token;

         // Producer: enumerate files
         var files = Directory.EnumerateFiles(directoryPath, "*.al", SearchOption.AllDirectories);
         var queue = new ConcurrentQueue<string>(files);

         var results = new ConcurrentDictionary<string, byte[]>();
         var workers = new Task[workerCount];

         for (int i = 0; i < workerCount; i++)
         {
            workers[i] = Task.Run(async () =>
            {
               while (!token.IsCancellationRequested && queue.TryDequeue(out var file))
               {
                  try
                  {
                     fileQueue.EnqueueAsync(file, cts);
                     WriteOutput($"Loaded file \"{Path.GetFileName(file)}\"");
                  }
                  catch (OperationCanceledException)
                  {
                     throw;
                  }
                  catch (Exception ex)
                  {
                     // Decide what you want to do on per-file failures:
                     // - log and continue
                     // - rethrow to fail the whole operation
                     // Here we just store an empty array and continue; customize as needed.
                     Debug.WriteLine($"Failed to read {file}: {ex}");
                  }
               }
            }, token);
         }

         try
         {
            var watch = Stopwatch.StartNew();
            await Task.WhenAll(workers).ConfigureAwait(false);
            watch.Stop();
            if (token.IsCancellationRequested)
               WriteOutput($"Parsing benchmark halted at {DateTime.Now.ToString()}");
            WriteOutput($"All files loaded in {watch.Elapsed.ToString()}");
         }
         catch (OperationCanceledException)
         {
            // Swallow if you want graceful cancellation
            WriteOutput($"Parsing benchmark halted at {DateTime.Now.ToString()}");
         }

         _IsRunning = false;
         _FileQueue.LoadingComplete = true;
      }

      private void WriteOutput(string outputText)
      {
         if (txtOutput.InvokeRequired)
         {
            txtOutput.Invoke((MethodInvoker)delegate
                                               {
                                                  txtOutput.AppendText(string.Concat(outputText, "\r\n"));
                                               });
         }
         else
            txtOutput.AppendText(string.Concat(outputText, "\r\n"));
      }
   }
}
