using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace SharedLibraries.Common
{
    public class LoggerManager
    {
        private static ILog _logger = LogManager.GetLogger(typeof(LoggerManager));
        public LoggerManager()
        {
            try
            {
                XmlDocument log4netConfig = new XmlDocument();
                using (var fs = File.OpenRead("log4net.config"))
                {
                    log4netConfig.Load(fs);
                    var repo = LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
                    XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
                    _logger.Info("Log System Initialized");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error", ex);
            }
        }
        public static void LogInformation(string message, string module, string url)
        {
            try
            {
                var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
                _logger = LogManager.GetLogger(typeof(LoggerManager));
                _logger.Info("--------------------------------------" + DateTime.Now + "---------------------------------------------------------------");
                _logger.Info("Module: " + module);
                _logger.Info("URL: " + url);
                _logger.Info("Info: " + message.Replace('\n', '_').Replace('\r', '_').Replace('\t', '-'));
                _logger.Info("-------------------------------------------------------------------------------------------------------------------");
            }
            catch (NullReferenceException Ex)
            {
                LoggerManager.LoggingErrorTrack(Ex, MethodBase.GetCurrentMethod().Name, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception Ex)
            {
                LoggerManager.LoggingErrorTrack(Ex, MethodBase.GetCurrentMethod().Name, string.Empty, string.Empty, string.Empty, string.Empty);
            }
        }
        public static void LoggingMessage(string message)
        {
            try
            {
                var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
                _logger = LogManager.GetLogger(typeof(LoggerManager));
                _logger.Error("--------------------------------------" + DateTime.Now + "---------------------------------------------------------------");
                _logger.Error("Request and Response: " + message.Replace('\n', '_').Replace('\r', '_').Replace('\t', '-'));
                _logger.Error("-------------------------------------------------------------------------------------------------------------------");
            }
            catch (NullReferenceException Ex)
            {
                LoggerManager.LoggingErrorTrack(Ex, MethodBase.GetCurrentMethod().Name, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception Ex)
            {
                LoggerManager.LoggingErrorTrack(Ex, MethodBase.GetCurrentMethod().Name, string.Empty, string.Empty, string.Empty, string.Empty);
            }
        }
        public static void LoggingErrorTrack(Exception ex, string module, string url, string parameter = "", string message = "", string deviceType = "")
        {
            try
            {
                var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
                _logger = LogManager.GetLogger(typeof(LoggerManager));
                _logger.Info("DateTime" + "\t" + "URL" + "\t" + "Data" + "\t" + "Exception" + "\t" + "Message" + "\t" + "StackTrace");
                _logger.Info("--------------------------------------" + module + "---------------------------------------------------------------");
                _logger.Info("Date: " + DateTime.Now);
                _logger.Info("URL: " + url);
                _logger.Info("Data: " + ex.Data);
                _logger.Info("InnerException: " + ex.InnerException);
                _logger.Info("Message: " + ex.Message);
                _logger.Info("StackTrace: " + ex.StackTrace);
                _logger.Info("-------------------------------------------------------------------------------------------------------------------");
            }
            catch (NullReferenceException Ex)
            {
                LoggerManager.LoggingErrorTrack(Ex, MethodBase.GetCurrentMethod().Name, string.Empty, string.Empty, string.Empty, string.Empty);
            }
            catch (Exception Ex)
            {
                LoggerManager.LoggingErrorTrack(Ex, MethodBase.GetCurrentMethod().Name, string.Empty, string.Empty, string.Empty, string.Empty);
            }
        }
    }
}