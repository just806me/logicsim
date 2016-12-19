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

        public static string ErrorLogFile => $"{Environment.MachineName}.{Environment.UserName}.errors.log";
        public static string MethodLogFile => $"{Environment.MachineName}.{Environment.UserName}.methods.log";

        public static event EventHandler<FirstChanceExceptionEventArgs> OnError;

        public static void Start()
        {
            AppDomain.CurrentDomain.FirstChanceException += Error;
            started = true;
        }

        public static void Stop()
        {
            started = false;
            AppDomain.CurrentDomain.FirstChanceException -= Error;
        }
        
        private static void Error(object sender, FirstChanceExceptionEventArgs e)
        {
            if (started)
            {
                var builder = new StringBuilder();

                builder.AppendLine($"UTC time: {DateTime.UtcNow.ToString()}");
                builder.AppendLine($"Error: {e.Exception.Message}");
                builder.AppendLine($"Short stack: {e.Exception.StackTrace}");
                builder.AppendLine("Full stack:");
                foreach (var frame in new StackTrace(true).GetFrames())
                    builder.Append(frame.ToString());
                builder.AppendLine("//-------------------------------------------//");

                File.AppendAllText(ErrorLogFile, builder.ToString());

                OnError?.Invoke(sender, e);
            }
        }

        public static void Method(MethodInfo method, params ArgumentInfo[] Arguments)
        {
            if (started)
            {
                var builder = new StringBuilder();

                builder.AppendLine(DateTime.UtcNow.ToString());
                builder.AppendLine($"{method.Type.Name} {method.Name}({string.Join(", ", Arguments.Select(a => $"{a.Type.Name} {a.Name}"))})");
                builder.AppendLine("----------");
                foreach (var agrument in Arguments)
                    builder.AppendLine($"{agrument.Name}: {JsonConvert.SerializeObject(agrument.Data)}");
                builder.AppendLine("//-------------------------------------------//");

                File.AppendAllText(MethodLogFile, builder.ToString());
            }
        }
    }
}

#endif
