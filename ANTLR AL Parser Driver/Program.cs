using System.Diagnostics;
using Org.Edgerunner.Language.AL.Parsing;
using Org.Edgerunner.Language.AL.Parsing.Preprocessing;

namespace ANTLR_AL_Parser_Driver
{
   internal static class Program
   {
      private static Stopwatch Watch;

      static void Main(string[] args)
      {
         var parser = new ALParser(["CLEAN24"]);
         var source = new StreamReader("SalesHeader.Table.al");

         parser.PreProcessingFinished += Parser_PreProcessingFinished;
         parser.ParsingFinished += Parser_ParsingFinished;

         //// Initial parse to warm things up
         //parser.Parse(source);

         Watch = Stopwatch.StartNew();
         var result = parser.Parse(source);
         if (parser.Errors.Count > 0)
         {
            foreach (var message in parser.Errors)
               Console.WriteLine($"{message.Source} Error {message.Line}:{message.Position} {message.Text}");
         }
         else
            Console.WriteLine("No errors encountered.");
         Console.WriteLine("Press any key to close");
         Console.ReadKey();
      }

      private static void Parser_ParsingFinished(object? sender, ParseEventArgs e)
      {
         Watch.Stop();
         Console.WriteLine($"Finished parsing in {Watch.ElapsedMilliseconds} milliseconds");
      }

      private static void Parser_PreProcessingFinished(object? sender, ParseEventArgs e)
      {
         Watch.Stop();
         Console.WriteLine($"Finished pre-processing in {Watch.ElapsedMilliseconds} milliseconds");
         Watch.Restart();
      }
   }
}
