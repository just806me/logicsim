using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogicSimulator.WindowsUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
#if DEBUG
            var dom = AppDomain.CurrentDomain;
            dom.FirstChanceException += (sender, e) =>
            {
                var builder = new StringBuilder();

                builder.AppendLine($"UTC time: {DateTime.UtcNow.ToString()}");
                builder.AppendLine($"Error: {e.Exception.Message}");
                builder.AppendLine($"Short stack: {e.Exception.StackTrace}");
                builder.AppendLine("Full stack:");
                foreach (var frame in new StackTrace(true).GetFrames())
                    builder.Append(frame.ToString());
                builder.AppendLine("//-------------------------------------------//");

                File.AppendAllText("log.txt", builder.ToString());
            };
#endif

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //ThreadLocalization
            var ci = new CultureInfo("uk");
            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);

            Trace.Listeners.Add(new TextWriterTraceListener("trace.log"));
            Trace.AutoFlush = true;

            Application.Run(new MainForm(args.Length > 0 ? args[0] : null));
        }
    }
}
