#if DEBUG

using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.ExceptionServices;
using System.Text;

namespace VinCAD.Logger
{

    public struct MethodInfo
    {
        public string Name { get; set; }
        public Type Type { get; set; }
    }

    public struct ArgumentInfo
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public dynamic Data { get; set; }
    }

    public static class Log
    {
        private static bool started;
        private static StreamWriter ErrorFile;
        private static StreamWriter MethodFile;

        public static string ErrorLogFile => Path.Combine(Path.GetTempPath(), $"{Environment.MachineName}.{Environment.UserName}.errors.log");
        public static string MethodLogFile => Path.Combine(Path.GetTempPath(), $"{Environment.MachineName}.{Environment.UserName}.methods.log");
		public static string PrimeryServerUrl { get; private set; }
		public static string UploadServerAPI => "/api/logger/upload";


		public static event UnhandledExceptionEventHandler OnError
        {
            add
            {
                AppDomain.CurrentDomain.UnhandledException += value;
            }

            remove
            {
                AppDomain.CurrentDomain.UnhandledException -= value;
            }
        }

        public static void Start(string primeryProjectServer, bool appendLog = true)
        {
			PrimeryServerUrl = primeryProjectServer;

			AppDomain.CurrentDomain.FirstChanceException += Error;
            ErrorFile = new StreamWriter(ErrorLogFile, appendLog) { AutoFlush = false };
            MethodFile = new StreamWriter(MethodLogFile, appendLog) { AutoFlush = false };
            started = true;
        }

        public static void Stop()
        {
            started = false;
            Flush();
            ErrorFile.Dispose();
            MethodFile.Dispose();
            AppDomain.CurrentDomain.FirstChanceException -= Error;
        }

        public static void Flush()
        {
            ErrorFile.Flush();
            MethodFile.Flush();
        }

		public static string Upload()
		{
			Ping ping = new Ping();
			var reply = ping.Send(PrimeryServerUrl.Replace("http://", ""));
			if (reply.Status != IPStatus.Success)
				return reply.Status.ToString();

			Stop();

			string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(PrimeryServerUrl + UploadServerAPI);
			request.ContentType = "multipart/form-data; boundary=" + boundary;
			request.Method = "POST";
			request.KeepAlive = true;

			Stream memStream = new MemoryStream();

			var boundarybytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
			var endBoundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--");

			string headerTemplate =
				"Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" +
				"Content-Type: application/octet-stream\r\n\r\n";

			//ErrorFileName
			{
				memStream.Write(boundarybytes, 0, boundarybytes.Length);
				var header = string.Format(headerTemplate, "ErrorFile", ErrorLogFile);
				var headerbytes = Encoding.UTF8.GetBytes(header);

				memStream.Write(headerbytes, 0, headerbytes.Length);

				using (var fileStream = new FileStream(ErrorLogFile, FileMode.Open, FileAccess.Read))
				{
					var buffer = new byte[1024];
					var bytesRead = 0;
					while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
						memStream.Write(buffer, 0, bytesRead);
				}
			}

			//MethodFileName
			{
				memStream.Write(boundarybytes, 0, boundarybytes.Length);
				var header = string.Format(headerTemplate, "MethodFile", MethodLogFile);
				var headerbytes = System.Text.Encoding.UTF8.GetBytes(header);

				memStream.Write(headerbytes, 0, headerbytes.Length);

				using (var fileStream = new FileStream(MethodLogFile, FileMode.Open, FileAccess.Read))
				{
					var buffer = new byte[1024];
					var bytesRead = 0;
					while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
						memStream.Write(buffer, 0, bytesRead);
				}
			}

			memStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
			request.ContentLength = memStream.Length;

			using (Stream requestStream = request.GetRequestStream())
			{
				memStream.Position = 0;
				byte[] tempBuffer = new byte[memStream.Length];
				memStream.Read(tempBuffer, 0, tempBuffer.Length);
				memStream.Close();
				requestStream.Write(tempBuffer, 0, tempBuffer.Length);
			}

			string result = null;
			using (var response = request.GetResponse())
			{
				Stream stream2 = response.GetResponseStream();
				StreamReader reader2 = new StreamReader(stream2);
				result = reader2.ReadToEnd();
			}

			Start(PrimeryServerUrl, false);

			return result;
		}

		private static void Error(object sender, FirstChanceExceptionEventArgs e)
        {
            if (started)
            {
                ErrorFile.WriteLine($"UTC time: {DateTime.UtcNow.ToString()}");
                ErrorFile.WriteLine($"Error: {e.Exception.Message}");
                ErrorFile.WriteLine($"Short stack: {e.Exception.StackTrace}");
                ErrorFile.WriteLine("Full stack:");
                foreach (var frame in new StackTrace(true).GetFrames())
                    ErrorFile.Write(frame.ToString());
                ErrorFile.WriteLine("//-------------------------------------------//");
            }
        }

        public static void Method(MethodInfo method, params ArgumentInfo[] Arguments)
        {
            if (started)
            {
                MethodFile.WriteLine(DateTime.UtcNow.ToString());
                MethodFile.WriteLine($"{method.Type.Name} {method.Name}({string.Join(", ", Arguments.Select(a => $"{a.Type.Name} {a.Name}"))})");
                MethodFile.WriteLine("----------");
                foreach (var agrument in Arguments)
                    MethodFile.WriteLine($"{agrument.Name}: {JsonConvert.SerializeObject(agrument.Data)}");
                MethodFile.WriteLine("//-------------------------------------------//");
            }
        }
    }
}

#endif