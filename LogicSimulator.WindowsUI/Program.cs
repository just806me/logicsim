using System;
using System.Globalization;
using System.Threading;
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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

			//ThreadLocalization
			var ci = new CultureInfo("uk");
			Thread.CurrentThread.CurrentUICulture = ci;
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);

			Application.Run(new MainForm(args.Length > 0 ? args[0] : null));
        }
    }
}
