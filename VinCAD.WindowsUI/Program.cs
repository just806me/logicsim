﻿using System;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Windows.Forms;

#if DEBUG
using VinCAD.Logger;
#endif

namespace VinCAD.WindowsUI
{
	static class Program
	{
		private const string CURRENT_VERSION = "qwerty";
		private const string PrimaryWebSite = "http://vincad.tk";

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

            //using (var cl = new WebClient())
            //{
            //    var version = cl.DownloadString("http://vincad.tk/api/version");
            //    if (version != CURRENT_VERSION && 
            //        MessageBox.Show("A new update is available! Download it?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            //            == DialogResult.Yes)
            //    {
            //        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            //        {
            //            cl.DownloadFile("http://vincad.tk/api/setup?platform=windows", "update.exe");
            //            Process.Start("update.exe");
            //        }
            //        else
            //        {
            //            // ? Может просто архив скачать и распаковать в себя.
            //        }

            //        return;
            //    }
            //}


            Application.Run(new MainForm(args.Length > 0 ? args[0] : null));

#if DEBUG
			Log.Stop();
#endif
		}
	}
}
