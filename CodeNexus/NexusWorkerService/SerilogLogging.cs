using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace NexusWorkerService
{
    public class SerilogLogging
    {
        public static Serilog.ILogger exceptionLog = null;
        public static long LogFileSize = 2000000;
        public static string SchedulerLogFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LogFile", "Scheduler_");
        /// <summary>
        /// Method to Log response data of all APIs
        /// </summary>
        /// <param name="response"></param>
        public static void LogSchedulerInfo(string info)
        {
            if (exceptionLog == null)
            {
                exceptionLog = new LoggerConfiguration().WriteTo.File(SchedulerLogFilePath, fileSizeLimitBytes: LogFileSize, rollOnFileSizeLimit: true, rollingInterval: RollingInterval.Day).CreateLogger();
            }
            exceptionLog.Information("DATE : " + DateTime.Now);
            exceptionLog.Information(info);
        }
        /// <summary>
        /// Method to Log travertex API response
        /// </summary>
        /// <param name="response"></param>
        public static void LogSchedulerError(Exception ex)
        {
            if (exceptionLog == null)
            {
                exceptionLog = new LoggerConfiguration().WriteTo.File(SchedulerLogFilePath, fileSizeLimitBytes: LogFileSize, rollOnFileSizeLimit: true, rollingInterval: RollingInterval.Day).CreateLogger();
            }
            exceptionLog.Error("*** ---------------- ***");
            exceptionLog.Error("*** START ***");
            exceptionLog.Error("DATE	: " + DateTime.Now);
            exceptionLog.Error("INNEREXCEPTION	: " + ex.InnerException);
            exceptionLog.Error("MESSAGE					: " + ex.Message);
            exceptionLog.Error("STACKTRACE			: " + ex.StackTrace);
            exceptionLog.Error("*** END ***");
            exceptionLog.Error("*** ---------------- ***");
        }
    }
}
