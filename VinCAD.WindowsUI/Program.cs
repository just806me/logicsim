using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

#if DEBUG
using VinCAD.Logger;
#endif

namespace VinCAD.WindowsUI
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
            Log.Start();
            Log.OnError += (sender, e) =>
            {
                /* 
                 * TODO:
                 * Спросить отправлять ли отчет
                 * Если да - отправить файлы Log.ErrorLogFile и Log.MethodLogFile на сервер
                 * 
                 */
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
