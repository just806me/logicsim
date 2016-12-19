#if DEBUG

using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

        public static event EventHandler<FirstChanceExceptionEventArgs> OnError;

        public static void Start()
        {
            AppDomain.CurrentDomain.FirstChanceException += Error;
            ErrorFile = new StreamWriter(ErrorLogFile, true) { AutoFlush = false };
            MethodFile = new StreamWriter(MethodLogFile, true) { AutoFlush = false };
            started = true;
        }

        public static void Stop()
        {
            started = false;
            ErrorFile.Dispose();
            MethodFile.Dispose();
            AppDomain.CurrentDomain.FirstChanceException -= Error;
        }

        public static void Flush()
        {
            ErrorFile.Flush();
            MethodFile.Flush();
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

                OnError?.Invoke(sender, e);
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
