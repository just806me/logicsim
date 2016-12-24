using System;
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

		static string PrimaryWebSite = "http://vincad.tk";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
#if DEBUG
			Log.Start(PrimaryWebSite);
			Log.OnError += (sender, e) =>
			{
				if (MessageBox.Show(
					Resource.Localization.Error, Resource.Localization.MainForm_Name,
					MessageBoxButtons.YesNo) == DialogResult.Yes)
				{
					Log.Upload();
				}
			};
#endif
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			//ThreadLocalization
			var ci = new CultureInfo("uk");
			Thread.CurrentThread.CurrentUICulture = ci;
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);

			Application.Run(new MainForm(args.Length > 0 ? args[0] : null));

#if DEBUG
			Log.Stop();
#endif
		}
	}
}
