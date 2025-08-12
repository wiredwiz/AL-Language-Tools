using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

using BrightIdeasSoftware;

using Org.Edgerunner.Language.AL.Parsing;
using Org.Edgerunner.Language.AL.Parsing.Messaging;
using Timer = System.Timers.Timer;

namespace Parser_Benchmark
{
   public partial class Benchmark : Form
   {
      private IParserWorker[] _ParserWorker;
      private Task[] _ParserTasks;
      private FileQueue _FileQueue;
      private bool _IsRunning = false;
      private CancellationTokenSource _Cts;
      private List<ErrorMessage> _Errors;
      private List<BenchEvent> _BenchEvents;
      private BindingList<BenchEvent> _Observable;
      private int _TotalFiles;
      private int _ParseCount;
      private int _LoadCount;
      private DateTime _Start;
      private Timer _Timer;

      public Benchmark()
      {
         InitializeComponent();
         _Errors = new List<ErrorMessage>();
         _BenchEvents = new List<BenchEvent>();
         _Observable = new BindingList<BenchEvent>();
         viewStatus.DataSource = _Observable;
         _Timer = new Timer();
         _Timer.Interval = 1000;
         _Timer.Elapsed += TimerElapsed;
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
         folderBrowserDialog1.SelectedPath = txtDirPath.Text;
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

            _Errors.Clear();
            _Observable.Clear();
            viewStatus.DataSource = _Observable;
            progLoading.Value = 0;
            progParsing.Value = 0;
            this.Refresh();
            _IsRunning = true;
            if (_Cts != null)
               _Cts.Dispose();
            _Cts = new CancellationTokenSource();
            _Start = DateTime.UtcNow;
            _Timer.Interval = 1000;
            _Timer.Start();

            if (chkThreadSafety.CheckState == CheckState.Unchecked)
               await ParseAllFilesAsync(_Cts, txtDirPath.Text).ConfigureAwait(true);
            else
            {
               _FileQueue = new FileQueue();

               ReadAllFilesAsync(_Cts, _FileQueue, txtDirPath.Text, (int)numFileThreads.Value);

               _ParserWorker = new IParserWorker[(int)numParserThreads.Value];
               _ParserTasks = new Task[(int)numParserThreads.Value];
               for (int i = 0; i < _ParserWorker.Length; i++)
               {
                  if (radAntlrParser.Checked)
                     _ParserWorker[i] = new AntlrParseWorker(_FileQueue, _Cts);

                  _ParserWorker[i].ReportData += Benchmark_ReportData;
                  _ParserWorker[i].ReportErrors += Benchmark_ReportErrors;
                  _ParserTasks[i] = _ParserWorker[i].StartAsync();
               }

               var watch = Stopwatch.StartNew();
               await Task.WhenAll(_ParserTasks).ConfigureAwait(true);
               watch.Stop();
               AddBenchEvent(BenchAction.Parsed, "ALL FILES", watch.Elapsed.ToString());
               _Timer.Stop();
            }
         }
         catch (OperationCanceledException)
         {
            // do nothing
            _BenchEvents.Add(new BenchEvent(BenchAction.Halted, "Parsing", DateTime.Now.ToString()));
            _IsRunning = false;
            _Timer.Stop();
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message, "Error");
         }
         finally
         {
            viewErrors.SetObjects(_Errors);
         }
      }

      private void Benchmark_ReportErrors(object sender, ParseErrorsEvent e)
      {
         if (e.SourceFile.Contains("Table") || e.SourceFile.Contains("Codeunit") || e.SourceFile.Contains("Page"))
            _Errors.AddRange(e.Errors);
      }

      private void Benchmark_ReportData(object sender, Datum e)
      {
         if (chkUpdates.CheckState == CheckState.Checked)
            AddBenchEvent(BenchAction.Parsed, e.FileName, e.ParseTime.ToString());
         progParsing.Invoke((Action)(() => progParsing.Value += 1));
      }

      private void BtnStopParsing_Click(object sender, EventArgs e)
      {
         _Cts.Cancel();
         _IsRunning = false;
      }

      void AddBenchEvent(BenchAction action, string fileName, string time)
      {
         if (viewStatus.InvokeRequired)
            viewStatus.Invoke((Action)(() =>
                                    {
                                       var benchEvent = new BenchEvent(action, fileName, time);
                                       _Observable.Add(benchEvent);
                                       viewStatus.EnsureModelVisible(benchEvent);
                                    }));
         else
         {
            var benchEvent = new BenchEvent(action, fileName, time);
            _Observable.Add(benchEvent);
            viewStatus.EnsureModelVisible(benchEvent);
         }

      }

      public async void ReadAllFilesAsync(CancellationTokenSource cts, FileQueue fileQueue, string directoryPath, int workerCount)
      {
         if (cts is null) throw new ArgumentNullException(nameof(cts));
         if (string.IsNullOrWhiteSpace(directoryPath)) throw new ArgumentException("Path is required.", nameof(directoryPath));
         if (!Directory.Exists(directoryPath)) throw new DirectoryNotFoundException(directoryPath);
         if (workerCount <= 0) throw new ArgumentOutOfRangeException(nameof(workerCount), "Must be > 0");

         var token = cts.Token;

         // Producer: enumerate files
         var files = await Task.Run(() => Directory.EnumerateFiles(directoryPath, "*.al", SearchOption.AllDirectories).ToArray()).ConfigureAwait(true);
         _TotalFiles = files.Length;
         progLoading.Maximum = _TotalFiles;
         progParsing.Maximum = _TotalFiles;
         var queue = new ConcurrentQueue<string>(files);

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
                     if (chkUpdates.CheckState == CheckState.Checked)
                        AddBenchEvent(BenchAction.Loaded, Path.GetFileName(file), string.Empty);
                     progLoading.Invoke((Action)(() => progLoading.Value += 1)); 
                     //viewStatus.Invoke((Action)(() => viewStatus.Refresh()));
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
               AddBenchEvent(BenchAction.Halted, "Parsing", DateTime.Now.ToString());
            AddBenchEvent(BenchAction.Loaded, "ALL FILES", watch.Elapsed.ToString());
         }
         catch (OperationCanceledException)
         {
            // Swallow if you want graceful cancellation
            AddBenchEvent(BenchAction.Halted, "Parsing", DateTime.Now.ToString());
            _Timer.Stop();
         }

         _IsRunning = false;
         _FileQueue.LoadingComplete = true;
      }

      private async Task ParseAllFilesAsync(CancellationTokenSource cts, string dirPath)
      {
         try
         {
            var files = Directory.GetFiles(dirPath, "*.al", SearchOption.AllDirectories);
            _TotalFiles = files.Length;
            progLoading.Maximum = _TotalFiles;
            progParsing.Maximum = _TotalFiles;
            var parser = new ALParser();
            Stopwatch watch = new Stopwatch();
            Stopwatch watch2 = Stopwatch.StartNew();
            foreach (var file in files)
            {
               // break out if cancellation requested
               cts.Token.ThrowIfCancellationRequested();

               // Read next source file
               var reader = new StreamReader(file);
               //progLoading.Invoke((Action)(() => progLoading.Value += 1)); 
               progLoading.Value += 1;
               if (chkUpdates.CheckState == CheckState.Checked)
                  AddBenchEvent(BenchAction.Loaded, Path.GetFileName(file), string.Empty);

               // Parse next source file
               parser.FileName = file;
               watch.Restart();
               await parser.ParseAsync(reader, cts).ConfigureAwait(true);
               watch.Stop();
               var elapsed = watch.Elapsed;
               if (chkUpdates.CheckState == CheckState.Checked)
                  AddBenchEvent(BenchAction.Parsed, Path.GetFileName(file), elapsed.ToString());
               if (parser.Errors.Count > 0)
                  _Errors.AddRange(parser.Errors);
               progParsing.Value += 1;
            }

            watch2.Stop();
            AddBenchEvent(BenchAction.Loaded, "ALL FILES", watch2.Elapsed.ToString());
            AddBenchEvent(BenchAction.Parsed, "ALL FILES", watch2.Elapsed.ToString());
         }
         catch (OperationCanceledException)
         {
            AddBenchEvent(BenchAction.Halted, "ALL FILES", string.Empty);
         }
         finally
         {
            _Timer.Stop();
            _IsRunning = false;
            viewErrors.SetObjects(_Errors);
         }
      }

      private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
      {
         if (txtRunTime.InvokeRequired)
            txtRunTime.Invoke((Action)(() => txtRunTime.Text = (DateTime.UtcNow - _Start).ToString(@"hh\:mm\:ss")));
         else
            txtRunTime.Text = (DateTime.UtcNow - _Start).ToString(@"hh\:mm\:ss");
      }

      private void viewErrors_DoubleClick(object sender, EventArgs e)
      {
         OLVListItem selected;
         object rowObject = null;
         if ((selected = viewErrors.SelectedItem) != null && selected.RowObject != null)
            rowObject = selected.RowObject;

         if (rowObject != null)
         {
            var progFiles = Environment.ExpandEnvironmentVariables("%ProgramW6432%");
            var vsCode = $"{progFiles}\\Microsoft VS Code\\Code.exe";
            if (!File.Exists(vsCode))
            {
               var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
               vsCode = $"{appData}\\Programs\\Microsoft VS Code\\Code.exe";
            }
            if (!File.Exists(vsCode))
            {
               MessageBox.Show("Unable to locate Visual Studio Code executable");
               return;
            }
            var message = (ErrorMessage)rowObject;
            var startInfo = new ProcessStartInfo(vsCode);
            Process.Start(vsCode, $"--goto \"{message.SourceFile}:{message.Line}:{message.Position}\"");
         }
      }
   }
}
