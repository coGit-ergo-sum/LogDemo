using log4net;
using log4net.Repository.Hierarchy;
using log4net.Core;
using log4net.Appender;
using log4net.Layout;
using log4net.Filter;
using System.Runtime.CompilerServices;
using System;
using System.Reflection;
using System.Collections.Generic;


// http://www.codeproject.com/Articles/140911/log-net-Tutorial

namespace Vi.Log4Vi
{
    /// <summary>
    /// Wraps a Log4Net object to make it compatible with the Vi.Logger. 
    /// </summary>
    /// <include file='XMLs/Logger.xml' path='Docs/type[@name="Logger"]/*' />
    public class Logger: Vi.ILog
    {
        /// <summary>
        /// The pointer to the Log4Net logger.
        /// </summary>
        private log4net.ILog Log = null;

        /// <summary>
        /// the list of full file names (if any) of the Log4Net object
        /// </summary>
        public List<string> Files
        {
            get;
            private set;
        }


        /// <summary>
        /// Use this CTor with if a log4Net objecct is already available. After this CTor the 'Files' field will be 'populated with the list of full file names (if any)
        /// </summary>
        /// <param name="log">The log4Net objecct</param>
        public Logger(log4net.ILog log) {
            this.Log = log;
            this.Files = Vi.Log4Vi.Tools.GetFiles(); 
        }

        /// <summary>
        ///  Creates 2 loggers, using 'Log4Net'. One with range between 'Debug' to 'Warn' and the other for the range between 'Error' and 'Fatal. The name of the logger is 'DefaultName'.
        /// </summary>
        /// <param name="logRoot">The path where the log will be saved.</param>
        /// <param name="useTraceAppender">True to write also in the EventLog.</param>
        public Logger(string logRoot, bool useTraceAppender) : this(logRoot, "DefaultName", useTraceAppender) { }

        /// <summary>
        ///  Creates 2 loggers, using 'Log4Net'. One with range between 'Debug' to 'Warn' and the other for the range between 'Error' and 'Fatal.
        /// </summary>
        /// <param name="logRoot">The path where the log will be saved.</param>
        /// <param name="name">The name of the logger.</param>
        /// <param name="useTraceAppender">True to write also in the EventLog.</param>
        public Logger(string logRoot, string name, bool useTraceAppender)
            : this(Vi.Log4Vi.Tools.GetLogger(logRoot, name, useTraceAppender))
        {
        }

        #region Infrastructure

        /// <summary>
        /// This is the most verbose logging level (maximum volume setting). Debug should be out-of-bounds for a production system and used only for development and testing.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="line">The line from where this method was called.</param>
        /// <param name="member">The member from where this method was called.</param>
        /// <param name="file">The file from where this method was called.</param>
        public virtual void Debug(string text, [CallerLineNumber] int line = 0, [CallerMemberName] string member = "?", [CallerFilePath] string file = "?")
        {
            this.Log.Debug(GetMessage(text, file, member, line));
        }



        /// <summary>
        /// The 'Info' level is typically used to output information that is useful to the running and management of your system (production). 'Info' would also be the level used to log Entry and Exit points in key areas of your application. However, you may choose to add more entry and exit points at Debug level for more granularity during development and testing.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="line">The line from where this method was called.</param>
        /// <param name="member">The member from where this method was called.</param>
        /// <param name="file">The file from where this method was called.</param>
        public virtual void Info(string text, [CallerLineNumber] int line = 0, [CallerMemberName] string member = "?", [CallerFilePath] string file = "?")
        {
            this.Log.Info(GetMessage(text, file, member, line));
        }


        /// <summary>
        /// Warning is often used for handled 'exceptions' or other important log events. For example, if your application requires a configuration setting but has a default in case the setting is missing, then the Warning level should be used to log the missing configuration setting.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="line">The line from where this method was called.</param>
        /// <param name="member">The member from where this method was called.</param>
        /// <param name="file">The file from where this method was called.</param>
        public virtual void Warn(string text, [CallerLineNumber] int line = 0, [CallerMemberName] string member = "?", [CallerFilePath] string file = "?")
        {
            this.Log.Warn(GetMessage(text, file, member, line));
        }

        /// <summary>
        /// Error is used to log all unhandled exceptions. This is typically logged inside a catch block at the boundary of your application.
        /// </summary>
        /// <param name="se">The exception to log.</param>
        /// <param name="line">The line from where this method was called.</param>
        /// <param name="member">The member from where this method was called.</param>
        /// <param name="file">The file from where this method was called.</param>
        public virtual void Error(Exception se, [CallerLineNumber] int line = 0, [CallerMemberName] string member = "?", [CallerFilePath] string file = "?")
        {
            this.Log.Error(se);

            // The logger in this class writes logs in two different files. 
            // One file is for Error and Fatal. The other file is for the other methods.
            // This line writes in the second file.
            this.Log.Warn(GetMessage("System.Exception: " + se.Message, file, member, line));
        }

        /// <summary>
        /// Fatal is reserved for special exceptions/conditions where it is imperative that you can quickly pick out these events. Fatal should to be used early in an application's development. It's usually only with experience it is possible identify situations worthy of the FATAL moniker experience do specific events become worth of promotion to Fatal. After all, an error's an error.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="line">The line from where this method was called.</param>
        /// <param name="member">The member from where this method was called.</param>
        /// <param name="file">The file from where this method was called.</param>
        public virtual void Fatal(string text, [CallerLineNumber] int line = 0, [CallerMemberName] string member = "?", [CallerFilePath] string file = "?")
        {
            this.Log.Fatal(GetMessage(text, file, member, line));

            // The logger in this class writes logs in two different files. 
            // One file is for Error and Fatal. The other file is for the other methods.
            // This line writes in the second file.
            this.Log.Warn(GetMessage("System.Exception: " + text, file, member, line));
        }

        /// <summary>
        /// Used by the other methods to create the same output structure: "text = {0}; line = {1}; member = {2}; file = {3};"
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="line">The line from where this method was called.</param>
        /// <param name="member">The member from where this method was called.</param>
        /// <param name="file">The file from where this method was called.</param>
        /// <returns>the string "text = {0}; line = {1}; member = {2}; file = {3};" filled with the proper values.</returns>
        private string GetMessage(string text, string file, string member, int line)
        {
            return System.String.Format("text = {0}; line = {1}; member = {2}; file = {3};", text, line, member, file);
        }


        #endregion

    }

}



//// ===================================================== //
//// Do not BIN: enables debug
//log4net.Util.LogLog.InternalDebugging = false;
//// ===================================================== //

//this.LogRoot = logRoot;

//LevelRangeFilter levelRangeFilterE = new LevelRangeFilter { LevelMin = Level.Error, LevelMax = Level.Fatal, AcceptOnMatch = true };
//string conversionPatternE = "%date [%thread] %-5level %logger - %message%newline%newline";
//var rollerE = GetFileAppender("ExceptionAppender", logRoot, "E", levelRangeFilterE, conversionPatternE);

//LevelRangeFilter levelRangeFilterN = new LevelRangeFilter { LevelMin = Level.Debug, LevelMax = Level.Warn, AcceptOnMatch = true };
//string conversionPatternN = "%date [%thread] %-5level %logger - %message%newline";
//var rollerN = GetFileAppender("NormalAppender", logRoot, "N", levelRangeFilterN, conversionPatternN); //, "%date [%thread] %level %message %exception %newline", "yyyy-MM-dd'.N.log'");
//rollerE.Threshold = log4net.Core.Level.Info;

//Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
//hierarchy.Root.AddAppender(rollerE);
//hierarchy.Root.AddAppender(rollerN);

//if (useTraceAppender)
//{
//    PatternLayout patternLayout = new PatternLayout();
//    patternLayout.ConversionPattern = "%date [%thread] %level %message %exception %newline";
//    patternLayout.ActivateOptions();

//    // logs the output in the Debug Output window.
//    TraceAppender tracer = new TraceAppender();
//    tracer.Layout = patternLayout;
//    tracer.ActivateOptions();

//    hierarchy.Root.AddAppender(tracer);
//}

//hierarchy.Root.Level = Level.Debug;
//hierarchy.Configured = true;

//// ---------------------------------------------------------------------- //
//this.Log = LogManager.GetLogger(name);
//this.Files = GetFiles(hierarchy);

///// <summary>
/////  Creates 2 loggers, using 'Log4Net'. One with range between 'Debug' to 'Warn' and the other for the range between 'Error' and 'Fatal.
///// </summary>
///// <param name="logRoot">The path where the log will be saved.</param>
///// <param name="name">The name of the logger.</param>
///// <param name="useTraceAppender">True to write also in the EventLog.</param>
//public log4net.ILog GetLogger(string logRoot, string name, bool useTraceAppender)
//{

//    // ===================================================== //
//    // Do not BIN: enables debug
//    log4net.Util.LogLog.InternalDebugging = false;
//    // ===================================================== //

//    //this.LogRoot = logRoot;

//    LevelRangeFilter levelRangeFilterE = new LevelRangeFilter { LevelMin = Level.Error, LevelMax = Level.Fatal, AcceptOnMatch = true };
//    string conversionPatternE = "%date [%thread] %-5level %logger - %message%newline%newline";
//    var rollerE = GetFileAppender("ExceptionAppender", logRoot, "E", levelRangeFilterE, conversionPatternE);

//    LevelRangeFilter levelRangeFilterN = new LevelRangeFilter { LevelMin = Level.Debug, LevelMax = Level.Warn, AcceptOnMatch = true };
//    string conversionPatternN = "%date [%thread] %-5level %logger - %message%newline";
//    var rollerN = GetFileAppender("NormalAppender", logRoot, "N", levelRangeFilterN, conversionPatternN); //, "%date [%thread] %level %message %exception %newline", "yyyy-MM-dd'.N.log'");
//    rollerE.Threshold = log4net.Core.Level.Info;

//    Hierarchy Hierarchy = (Hierarchy)LogManager.GetRepository();
//    Hierarchy.Root.AddAppender(rollerE);
//    Hierarchy.Root.AddAppender(rollerN);

//    if (useTraceAppender)
//    {
//        PatternLayout patternLayout = new PatternLayout();
//        patternLayout.ConversionPattern = "%date [%thread] %level %message %exception %newline";
//        patternLayout.ActivateOptions();

//        // logs the output in the Debug Output window.
//        TraceAppender tracer = new TraceAppender();
//        tracer.Layout = patternLayout;
//        tracer.ActivateOptions();

//        Hierarchy.Root.AddAppender(tracer);
//    }

//    Hierarchy.Root.Level = Level.Debug;
//    Hierarchy.Configured = true;

//    // ---------------------------------------------------------------------- //
//    return LogManager.GetLogger(name);
//}

//private RollingFileAppender GetFileAppender(string name, string file, string type, LevelRangeFilter levelRangeFilter, string conversionPattern)
//{
//    /// Set the pattern layout for this appender
//    PatternLayout patternLayout = new PatternLayout();
//    patternLayout.ConversionPattern = conversionPattern;
//    patternLayout.ActivateOptions();

//    /// Instantiate a new file appender
//    var fileAppender = new RollingFileAppender();
//    fileAppender.Name = name + "FileAppender";
//    fileAppender.AppendToFile = true;
//    fileAppender.Layout = patternLayout;
//    fileAppender.File = file;

//    // if you name your RollingFiles logs with log4net with Dates – be sure to include the staticLogFileName = false setting.
//    fileAppender.StaticLogFileName = false;

//    fileAppender.PreserveLogFileNameExtension = true;

//    fileAppender.DatePattern = type + ".yyyy-MM-dd'.log'";
//    fileAppender.RollingStyle = RollingFileAppender.RollingMode.Composite;
//    fileAppender.MaximumFileSize = "10MB";
//    fileAppender.MaxSizeRollBackups = 10;


//    /// Define filter for this appender
//    //LevelRangeFilter levelRangeFilter = new LevelRangeFilter { LevelMin = Level.Error, LevelMax = Level.Fatal, AcceptOnMatch = true };
//    DenyAllFilter denyAllFilter = new log4net.Filter.DenyAllFilter();
//    fileAppender.AddFilter(levelRangeFilter);
//    fileAppender.AddFilter(denyAllFilter);
//    fileAppender.ActivateOptions();
//    //this.Hierarchy.Root.AddAppender(fileAppender);


//    return fileAppender;
//}

//private List<string> GetFiles(Hierarchy hierarchy)
//{
//    var files = new List<string>();
//    foreach (var appender in hierarchy.Root.Appenders)
//    {
//        if (appender is RollingFileAppender)
//        {
//            var rollingFileAppender = (RollingFileAppender)appender;
//            files.Add(rollingFileAppender.File);
//        }
//    }                
//    return files;
//}
