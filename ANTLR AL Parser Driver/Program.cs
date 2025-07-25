using System.Diagnostics;
using Org.Edgerunner.Language.AL.Parsing;
using Org.Edgerunner.Language.AL.Parsing.Preprocessing;

namespace ANTLR_AL_Parser_Driver
{
   internal class Program
   {
      static void Main(string[] args)
      {
         var parser = new ALParser(["CLEAN24"]);
         var source = new StreamReader("SalesHeader.Table.al");

         //// Initial parse to warm things up
         //parser.Parse(source);

         var watch = Stopwatch.StartNew();
         var result = parser.Parse(source);
         watch.Stop();
         if (parser.Errors.Count > 0)
         {
            foreach (var message in parser.Errors)
               Console.WriteLine($"{message.Source} Error {message.Line}:{message.Position} {message.Text}");
         }
         else
            Console.WriteLine("No errors encountered.");
         Console.WriteLine($"done parsing in {watch.ElapsedMilliseconds} milliseconds");
      }
   }
}
