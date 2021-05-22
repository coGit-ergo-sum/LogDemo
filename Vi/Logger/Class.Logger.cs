using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Vi
{
    /// <summary>
    /// Exposes  methods  to log and format messages: Debug; Info; Warn; Error; Fatal; Format.    
    /// </summary>    
    /// <include file='Logger/XMLs/Logger.xml' path='Docs/type[@name="Logger"]/*' />
    public static class Logger  // Interface inheritance is not allowed in static class and in this case is not needed: Vi.Shared.ILog
    {

        // This page is only Infrastructural. Is made to present the Log object in the most structured way.
        #region Infrastructure

        /// <summary>
        /// Enumeration of the possible types of log (method)
        /// </summary>
        public enum Levels : byte
        {
            /// <summary>
            /// This is the most verbose logging level (maximum volume setting). Debug should be out-of-bounds for a production system and used only for development and testing.
            /// </summary>
            DEBUG,

            /// <summary>
            /// The 'Info' level is typically used to output information that is useful to the running and management of your system (production). 'Info' would also be the level used to log Entry and Exit points in key areas of your application. However, you may choose to add more entry and exit points at Debug level for more granularity during development and testing.
            /// </summary>
            INFO,

            /// <summary>
            /// Warning is often used for handled 'exceptions' or other important log events. For example, if your application requires a configuration setting but has a default in case the setting is missing, then the Warning level should be used to log the missing configuration setting.
            /// </summary>
            WARN,

            /// <summary>
            /// Error is used to log all unhandled exceptions. This is typically logged inside a catch block at the boundary of your application.
            /// </summary>
            ERROR,

            /// <summary>
            /// Fatal is reserved for special exceptions/conditions where it is imperative that you can quickly pick out these events. Fatal should to be used early in an application's development. It's usually only with experience it is possible identify situations worthy of the FATAL moniker experience do specific events become worth of promotion to Fatal. After all, an error's an error.
            /// </summary>
            FATAL,

        }

        #region FakeLog
        /// <summary>
        /// This is an "empty" class: all the methods are without implementation. 
        /// </summary>
        /// <include file='Logger/XMLs/FakeLog.xml' path='Docs/type[@name="FakeLog"]/*' />
        public class FakeLog : Vi.ILog
        {
            /// <summary>
            /// This Method is Without implementation
            /// </summary>
            /// <param name="text">The text to log.</param>
            /// <param name="line">The Line number in the file where this method is called.</param>
            /// <param name="member">The name of the member from which the log comes.</param>
            /// <param name="file">The name of the file from where this method is called.</param>
            public void Debug(string text, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?") { }

            /// <summary>
            /// This Method is Without implementation
            /// </summary>
            /// <param name="text">The text to log.</param>
            /// <param name="line">The Line number in the file where this method is called.</param>
            /// <param name="member">The name of the member from which the log comes.</param>
            /// <param name="file">The name of the file from where this method is called.</param>
            public void Info(string text, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?") { }

            /// <summary>
            /// This Method is Without implementation
            /// </summary>
            /// <param name="text">The text to log.</param>
            /// <param name="line">The Line number in the file where this method is called.</param>
            /// <param name="member">The name of the member from which the log comes.</param>
            /// <param name="file">The name of the file from where this method is called.</param>
            public void Warn(string text, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?") { }

            /// <summary>
            /// This Method is Without implementation
            /// </summary>
            /// <param name="se">The exception to log.</param>
            /// <param name="line">The Line number in the file where this method is called.</param>
            /// <param name="member">The name of the member from which the log comes.</param>
            /// <param name="file">The name of the file from where this method is called.</param>
            public void Error(Exception se, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?") { }

            /// <summary>
            /// This Method is Without implementation
            /// </summary>
            /// <param name="text">The text to log.</param>
            /// <param name="line">The Line number in the file where this method is called.</param>
            /// <param name="member">The name of the member from which the log comes.</param>
            /// <param name="file">The name of the file from where this method is called.</param>
            public void Fatal(string text, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?") { }
        } 
        #endregion

        #region FormatClass
        /// <summary>
        /// Defined only for the class 'FormatClass'
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="line">The Line number in the file where this method is called.</param>
        /// <param name="member">The name of the member from which the log comes.</param>
        /// <param name="file">The name of the file from where this method is called.</param>
        public delegate void FormatDelegate(string text, int line, string member, string file);


        /// <summary>
        /// This class is a 'trick' necessary because is not possible have optional parameter (file, member and line) and a param array. this are necessary for the 'Format' method.
        /// </summary>
        /// <include file='Logger/XMLs/FormatClass.xml' path='Docs/type[@name="FormatClass"]/*' />
        public class FormatClass
        {
            /// <summary>
            /// The callback used to log the message.
            /// </summary>
            FormatDelegate CallBack = null;

            /// <summary>
            /// The name of the file from where this method is called.
            /// </summary>
            string File { get; set; }

            /// <summary>
            /// The name of the member where this method is called.
            /// </summary>
            string Member { get; set; }

            /// <summary>
            /// The Line of the file where this method is called.
            /// </summary>
            int Line { get; set; }

            #region CTors


            /// <summary>
            /// CTor. Assign the parameter to the inner fields.
            /// </summary>
            /// <param name="callBack">The callback function used to log one of {Debug; Info; Warn; Fatal}</param>
            /// <param name="file">The name of the file from where this method is called.</param>
            /// <param name="member">The name of the member where this method is called.</param>
            /// <param name="line">The Line of the file where this method is called.</param>
            public FormatClass(FormatDelegate callBack, int line, string member, string file)
            {
                this.CallBack = callBack;
                this.File = file;
                this.Member = member;
                this.Line = line;
            }

            ///////////////////// <summary>
            ///////////////////// CTor. Assign the parameter to the inner fields.
            ///////////////////// </summary>
            ///////////////////// <param name="callBack">The callback function used to log one of {Debug; Info; Warn; Fatal}</param>
            ///////////////////// <param name="token">A number related with an incoming 'Signal'.  Used to group together all the logs for the same 'Signal'.</param>
            ///////////////////// <param name="level">Specifies which kind of log {Debug; Warn; ...}</param>
            ///////////////////// <param name="file">The name of the file from where this method is called.</param>
            ///////////////////// <param name="member">The name of the member where this method is called.</param>
            ///////////////////// <param name="line">The Line of the file where this method is called.</param>
            //////////////////public FormatClass(FormatDelegate callBack, Levels level, int line, string member, string file)
            //////////////////    : this(callBack, line, member, file)
            //////////////////{
            //////////////////    this.Token = token;
            //////////////////}
            #endregion

            /// <summary>
            /// Logs the message formatting the text exactly as 'String.Format'.
            /// </summary>
            /// <param name="format">A composite format string.</param>
            /// <param name="args">An object array that contains zero or more objects to format.</param>
            public void Format(string format, params object[] args)
            {
                this.CallBack(System.String.Format(format, args), this.Line, this.Member, this.File);
            }

        } 
        #endregion

        /// <summary>
        /// If true 'Debug' will not be traced.
        /// </summary>
        public static bool SkipDebug = false;


        /// <summary>
        /// The object effectively used to execute the log. By default is a 'Fake' log (an empty class that does not writes anything). To assigne a true Log object use the method 'SetLogger'. 
        /// It is not public to avoid misuse like: Logger._Logger.Debug(...).
        /// </summary>
        private static Vi.ILog _Logger = new FakeLog();

        /// <summary>
        /// Assign the provided logger to this class (Vi.Logger). By default this class uses a 'fake' logger that does logs nothing.
        /// After this assignment, every log will be managed by the provided class. This class in made with Log4Net in mind, but any other way to log can used.
        /// </summary>
        /// <param name="logger">Any kind of logger that inherits from 'Vi.Shared.ILog'.</param>
        #region SetLogger
        public static void SetLogger(Vi.ILog logger)
        {
            Vi.Logger._Logger = logger ?? new FakeLog();

        } 
        #endregion

        /// <summary>
        /// Gives back the 'Logger' currently used. This method is defined to avoid direct access to this 'logger'
        /// </summary>
        /// <returns>The Logger object currently used.</returns>
        public static Vi.ILog GetLogger()
        {
            return Logger._Logger;
        }

        #region Log Methods

        /// <summary>
        /// Writes one of the log method based on level.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="level">Specifies which kind of log {Debug; Warn; ...}</param>
        /// <param name="line">The Line number in the file where this method is called.</param>
        /// <param name="member">The name of the member from which the log comes.</param>
        /// <param name="file">The name of the file from where this method is called.</param>
        [DebuggerStepThrough]
        public static void Write(string text, Levels level, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?")
        {
            switch (level)
            {
                case Levels.DEBUG:
                    Logger.Debug(text, line, member, file);
                    break;

                case Levels.FATAL:
                    Logger.Fatal(text, line, member, file);
                    break;

                case Levels.INFO:
                    Logger.Info(text, line, member, file);
                    break;

                case Levels.WARN:
                    Logger.Warn(text, line, member, file);
                    break;
            }
        }


        /// <summary>
        /// Writes one of the log method based on level.
        /// </summary>
        /// <param name="level">Specifies which kind of log {Debug; Warn; ...}</param>
        /// <param name="line">The Line number in the file where this method is called.</param>
        /// <param name="member">The name of the member from which the log comes.</param>
        /// <param name="file">The name of the file from where this method is called.</param>
        /// <returns>An instance of 'FormatClass' with the method 'Format' used to compose the text to log like the 'String.Format'</returns>
        [DebuggerStepThrough]
        public static FormatClass Write(Levels level, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?")
        {
            Action<string, int, string, string> write = (_text, _line, _member, _file) =>
            {
                Logger.Write(level, line, member, file);
            };
            return new FormatClass(new FormatDelegate(write), line, member, file);
        }

        /// <summary>
        /// The same as Error(). (defined only for consinstency.)
        /// </summary>
        /// <param name="se">The exception to log.</param>
        /// <param name="line">The Line number in the file where this method is called.</param>
        /// <param name="member">The name of the member from which the log comes.</param>
        /// <param name="file">The name of the file from where this method is called.</param>
        [DebuggerStepThrough]
        public static void Write(System.Exception se, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?")
        {
            Vi.Logger.Error(se, line, member, file);
        }


        #region Debug
        /// <summary>
        /// This is the most verbose logging level (maximum volume setting). Debug should be out-of-bounds for a production system and used only for development and testing.
        /// Logs a 'Debug in the log file if skepDebug (in config file) is false.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="file">The name of the file from where this method is called.</param>
        /// <param name="member">The name of the member where this method is called.</param>
        /// <param name="line">The Line of the file where this method is called.</param>
        [DebuggerStepThrough]
        public static void Debug(string text, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?")
        {
            if (!Logger.SkipDebug)
            {
                if (Vi.Logger._Logger != null)
                {
                    Vi.Logger._Logger.Debug(text, line, member, file);
                }
            }
        }


        /// <summary>
        /// Call this method to reach the Format method 'Debug().Format(...)';
        /// </summary>
        /// <param name="file">The name of the file from where this method is called.</param>
        /// <param name="member">The name of the member where this method is called.</param>
        /// <param name="line">The Line of the file where this method is called.</param>
        /// <returns>An instance of 'FormatClass' with the method 'Format' used to compose the text to log like the 'String.Format'</returns>
        [DebuggerStepThrough]
        public static FormatClass Debug([CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?")
        {
            return new FormatClass(Logger.Debug, line, member, file);
        }
        #endregion

        #region Info
        /// <summary>
        /// The 'Info' level is typically used to output information that is useful to the running and management of your system (production). 'Info' would also be the level used to log Entry and Exit points in key areas of your application. However, you may choose to add more entry and exit points at Debug level for more granularity during development and testing.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="file">The name of the file from where this method is called.</param>
        /// <param name="member">The name of the member where this method is called.</param>
        /// <param name="line">The Line of the file where this method is called.</param>
        /// <include file='Logger/XMLs/Logger.xml' path='Docs/method[@name="Info"]/*' />
        [DebuggerStepThrough]
        public static void Info(string text, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?")
        {
            Vi.Logger._Logger.Info(text, line, member, file);
        }


        /// <summary>
        /// Use this overload to 'Format' a message like the method 'System.String.Format'. The sintax is: Info().Format(string format, params object[] args).
        /// </summary>
        /// <param name="line">The Line of the file where this method is called.</param>
        /// <param name="member">Used for debug pourposes: the name of the member from which the log comes.</param>
        /// <param name="file">The name of the file from where this method is called.</param>
        /// <returns>An instance of 'FormatClass' with the method 'Format' used to compose the text to log like the 'String.Format'</returns>
        [DebuggerStepThrough]
        public static FormatClass Info([CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?")
        {
            return new FormatClass(Logger.Info, line, member, file);
        }
        #endregion

        #region Warn
        /// <summary>
        /// Warning is often used for handled 'exceptions' or other important log events. For example, if your application requires a configuration setting but has a default in case the setting is missing, then the Warning level should be used to log the missing configuration setting.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="file">The name of the file from where this method is called.</param>
        /// <param name="member">The name of the member where this method is called.</param>
        /// <param name="line">The Line of the file where this method is called.</param>
        [DebuggerStepThrough]
        public static void Warn(string text, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?")
        {
            Vi.Logger._Logger.Warn(text, line, member, file);
        }


        /// <summary>
        /// Warning is often used for handled 'exceptions' or other important log events. For example, if your application requires a configuration setting but has a default in case the setting is missing, then the Warning level should be used to log the missing configuration setting.
        /// </summary>
        /// <param name="file">The name of the file from where this method is called.</param>
        /// <param name="member">The name of the member where this method is called.</param>
        /// <param name="line">The Line of the file where this method is called.</param>
        /// <returns>An instance of 'FormatClass' with the method 'Format' used to compose the text to log like the 'String.Format'</returns>
        [DebuggerStepThrough]
        public static FormatClass Warn([CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?")
        {
            return new FormatClass(Vi.Logger.Warn, line, member, file);
        }
        #endregion

        #region Error
        /// <summary>
        /// Error is used to log all unhandled exceptions. This is typically logged inside a catch block at the boundary of your application.
        /// </summary>
        /// <param name="se">The exception to log.</param>
        /// <param name="line">The line from where this method was called.</param>
        /// <param name="member">The member from where this method was called.</param>
        /// <param name="file">The file from where this method was called.</param>
        [DebuggerStepThrough]
        public static void Error(Exception se, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?")
        {
            Vi.Logger._Logger.Error(se, line, member, file);
        }


        #endregion

        #region Fatal
        /// <summary>
        /// Fatal is reserved for special exceptions/conditions where it is imperative that you can quickly pick out these events. Fatal should to be used early in an application's development. It's usually only with experience it is possible identify situations worthy of the FATAL moniker experience do specific events become worth of promotion to Fatal. After all, an error's an error.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="file">The name of the file from where this method is called.</param>
        /// <param name="member">The name of the member where this method is called.</param>
        /// <param name="line">The Line of the file where this method is called.</param>
        [DebuggerStepThrough]
        public static void Fatal(string text, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?")
        {
            Vi.Logger._Logger.Fatal(text, line, member, file);
        }


        /// <summary>
        /// Fatal is reserved for special exceptions/conditions where it is imperative that you can quickly pick out these events. Fatal should to be used early in an application's development. It's usually only with experience it is possible identify situations worthy of the FATAL moniker experience do specific events become worth of promotion to Fatal. After all, an error's an error.
        /// </summary>
        /// <param name="file">The name of the file from where this method is called.</param>
        /// <param name="member">The name of the member where this method is called.</param>
        /// <param name="line">The Line of the file where this method is called.</param>
        /// <returns>An instance of 'FormatClass' with the method 'Format' used to compose the text to log like the 'String.Format'</returns>
        [DebuggerStepThrough]
        public static FormatClass Fatal([CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?")
        {
            return new FormatClass(Logger.Fatal, line, member, file);
        } 
        #endregion

        #endregion

        #endregion
    }
}


///////////////////// <summary>
///////////////////// Ctor. Calls the overlad method and assign 'token'
///////////////////// </summary>
///////////////////// <param name="callBack">The callback function used to log one of {Debug; Info; Warn; Fatal}</param>
///////////////////// <param name="token">A number related with an incoming 'Signal'.  Used to group together all the logs for the same 'Signal'.</param>
///////////////////// <param name="file">The name of the file from where this method is called.</param>
///////////////////// <param name="member">The name of the member where this method is called.</param>
///////////////////// <param name="line">The Line of the file where this method is called.</param>
//////////////////public FormatClass(FormatDelegate callBack, int line, string member, string file)
//////////////////    : this(callBack, line, member, file)
//////////////////{
//////////////////    ///////this.Token = token;
//////////////////}