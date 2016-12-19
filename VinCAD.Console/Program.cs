using CommandLine;
using CommandLine.Text;
using VinCAD.Main;
using System.IO;

namespace VinCAD.Console
{
    class Options
    {
        [Option('e', "equation", Required = false, HelpText = "Logical equation. Example: (x1+!x2)*x3")]
        public string InputEquation { get; set; }

        [Option('f', "file", Required = false, HelpText = "Path to the jsch file with the logical scheme.")]
        public string InputJsch { get; set; }

        [Option('t', "table", Required = true, HelpText = "Path to the html file where to write the table of values for the specified scheme.")]
        public string OutputTable { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage() => HelpText.AutoBuild(this, curr => HelpText.DefaultParsingErrorsHandler(this, curr));
    }

    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
                if (!string.IsNullOrEmpty(options.InputEquation))
                {
                    var scheme = Scheme.FromEquation(options.InputEquation);

                    using (var tableFile = File.Open(options.OutputTable, FileMode.Create))
                        scheme.CalculateAndWriteTable(tableFile);
                }

                if (!string.IsNullOrEmpty(options.InputJsch))
                {
                    Scheme scheme;

                    using (var jschFile = File.Open(options.InputJsch, FileMode.Open))
                        scheme = SchemeStorage.Load(jschFile);

                    using (var tableFile = File.Open(options.OutputTable, FileMode.Create))
                        scheme.CalculateAndWriteTable(tableFile);
                }
            }

        }
    }
}
