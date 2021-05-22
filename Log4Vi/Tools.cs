using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Filter;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vi.Log4Vi
{
    /// <summary>
    /// A collection of methods used along this project.
    /// </summary>
    /// <include file='XMLs/Tools.xml' path='Docs/type[@name="Tools"]/*' />
    public static class Tools
    {
        /// <summary>
        /// Loops over all the Appenders and when a 'RollingFileAppender' is found gets from it the full file name (full path and file name)
        /// </summary>
        /// <returns>A list of all the available full file names.</returns>
        public static List<string> GetFiles()
        {
            var files = new List<string>();
            var appenders = LogManager.GetRepository().GetAppenders();

            foreach (var appender in appenders)
            {
                if (appender is RollingFileAppender)
                {
                    var rollingFileAppender = (RollingFileAppender)appender;
                    files.Add(rollingFileAppender.File);
                }
            }
            return files;
        }

        /// <summary>
        ///  Creates 2 VERY BASIC loggers (using 'Log4Net'). One with range between 'Debug' to 'Warn' and the other for the range between 'Error' and 'Fatal. The name of the logger is 'DefaultName'.
        /// </summary>
        /// <param name="logRoot">The path where the log will be saved.</param>
        /// <param name="useTraceAppender">True to write also in the EventLog.</param>
        /// <returns>Log4Net object ready to log info on 2 different files.</returns>
        /// <include file='XMLs/Tools.xml' path='Docs/method[@name="GetLogger"]/*' />
        public static log4net.ILog GetLogger(string logRoot, bool useTraceAppender) { return Tools.GetLogger(logRoot, "DefaultName", useTraceAppender); }

        /// <summary>
        /// Creates 2 VERY BASIC loggers, using 'Log4Net'. One with range between 'Debug' to 'Warn' and the other for the range between 'Error' and 'Fatal. The name of the logger is 'DefaultName'.
        /// </summary>
        /// <param name="logRoot">The path where the log will be saved.</param>
        /// <param name="name">The name of the logger.</param>
        /// <param name="useTraceAppender">True to write also in the EventLog.</param>
        /// <returns>Log4Net object ready to log info on 2 different files.</returns>
        public static log4net.ILog GetLogger(string logRoot, string name, bool useTraceAppender)
        {

            // ===================================================== //
            // Do not BIN: enables debug
            log4net.Util.LogLog.InternalDebugging = false;
            // ===================================================== //

            //this.LogRoot = logRoot;
            
            LevelRangeFilter levelRangeFilterE = new LevelRangeFilter { LevelMin = Level.Error, LevelMax = Level.Fatal, AcceptOnMatch = true };
            string conversionPatternE = "%date [%thread] %-5level %logger - %message%newline%newline";
            var rollerE = GetFileAppender("ExceptionAppender", logRoot, "E", levelRangeFilterE, conversionPatternE);

            LevelRangeFilter levelRangeFilterN = new LevelRangeFilter { LevelMin = Level.Debug, LevelMax = Level.Warn, AcceptOnMatch = true };
            string conversionPatternN = "%date [%thread] %-5level %logger - %message%newline";
            var rollerN = GetFileAppender("NormalAppender", logRoot, "N", levelRangeFilterN, conversionPatternN); //, "%date [%thread] %level %message %exception %newline", "yyyy-MM-dd'.N.log'");
            rollerE.Threshold = log4net.Core.Level.Info;

            Hierarchy Hierarchy = (Hierarchy)LogManager.GetRepository();
            Hierarchy.Root.AddAppender(rollerE);
            Hierarchy.Root.AddAppender(rollerN);

            if (useTraceAppender)
            {
                PatternLayout patternLayout = new PatternLayout();
                patternLayout.ConversionPattern = "%date [%thread] %level %message %exception %newline";
                patternLayout.ActivateOptions();

                // logs the output in the Debug Output window.
                TraceAppender tracer = new TraceAppender();
                tracer.Layout = patternLayout;
                tracer.ActivateOptions();

                Hierarchy.Root.AddAppender(tracer);
            }

            Hierarchy.Root.Level = Level.Debug;
            Hierarchy.Configured = true;

            // ---------------------------------------------------------------------- //
            return LogManager.GetLogger(name);
        }

        /// <summary>
        /// Produces a VERY BASIC RollingFileAppender
        /// </summary>
        /// <param name="name">The name of the rolling file appender the string "FileAppender" will be appended to the name.</param>
        /// <param name="file">Assigned to 'fileAppender.File'</param>
        /// <param name="type">type could be N for 'Normal' log; E for exception log.</param>
        /// <param name="levelRangeFilter">The range for the file appender. will be Debug to Warn for files type N; Error Fatal for files type E.</param>
        /// <param name="conversionPattern">The pattern used to log.</param>
        /// <returns>A new 'RollingFileAppender' with the provided settings.</returns>
        private static RollingFileAppender GetFileAppender(string name, string file, string type, LevelRangeFilter levelRangeFilter, string conversionPattern)
        {            
            // Set the pattern layout for this appender
            PatternLayout patternLayout = new PatternLayout();
            patternLayout.ConversionPattern = conversionPattern;
            patternLayout.ActivateOptions();

            // Instantiate a new file appender
            var fileAppender = new RollingFileAppender();
            fileAppender.Name = name + "FileAppender";
            fileAppender.AppendToFile = true;
            fileAppender.Layout = patternLayout;
            fileAppender.File = file;

            // if you name your RollingFiles logs with log4net with Dates – be sure to include the staticLogFileName = false setting.
            fileAppender.StaticLogFileName = false;

            fileAppender.PreserveLogFileNameExtension = true;

            fileAppender.DatePattern = type + ".yyyy-MM-dd'.log'";
            fileAppender.RollingStyle = RollingFileAppender.RollingMode.Composite;
            fileAppender.MaximumFileSize = "10MB";
            fileAppender.MaxSizeRollBackups = 10;


            // Define filter for this appender
            //LevelRangeFilter levelRangeFilter = new LevelRangeFilter { LevelMin = Level.Error, LevelMax = Level.Fatal, AcceptOnMatch = true };
            DenyAllFilter denyAllFilter = new log4net.Filter.DenyAllFilter();
            fileAppender.AddFilter(levelRangeFilter);
            fileAppender.AddFilter(denyAllFilter);
            fileAppender.ActivateOptions();
            ///////////////Hierarchy.Root.AddAppender(fileAppender);


            return fileAppender;
        }
    }
}
