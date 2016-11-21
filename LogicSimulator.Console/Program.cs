using CommandLine;
using CommandLine.Text;
using LogicSimulator.Main;
using System.IO;

namespace LogicSimulator.Console
{
    class Options
    {
        [Option('i', "input", Required = true, HelpText = "Path to the jsch file with the logical scheme.")]
        public string InputJson { get; set; }

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
                Scheme elements;

                using (var schemeFile = File.Open(options.InputJson, FileMode.Open))
                    elements = SchemeStorage.Load(schemeFile);

                using (var tableFile = File.Open(options.OutputTable, FileMode.Create))
                    elements.CalculateAndWriteTable(tableFile);
            }

        }
    }
}
