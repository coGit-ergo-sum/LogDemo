using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Vi
{
    /// <summary>
    /// This class is a 'wrapper' around the provided 'Logger'. Its task is to 'intercepts' the messages addressed to the log file and send a copy (also) to the screen.
    /// </summary>
    /// <include file='Logger/XMLs/Wrapper.xml' path='Docs/type[@name="Wrapper"]/*' />
    public class Wrapper : Vi.ILog
    {
        /// <summary>
        /// The delegate used to define the event 'OnAppend'
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="line">The Line of the file where this method is called.</param>
        /// <param name="member">The name of the member where this method is called.</param>
        /// <param name="file">The name of the file from where this method is called.</param>
        /// <param name="level">Specifies which kind of log {Debug; Warn; ...}</param>
        public delegate void OnAppendHandler(string text, int line, string member, string file, Vi.Logger.Levels level);

        /// <summary>
        /// A 'store' for the Logger supplied with the CTor.
        /// </summary>
        private readonly Vi.ILog _Logger = null;

        /// <summary>
        /// This event fires everitime a message to the log file is intercepted. Triggers the addition of a new row in the listView.
        /// </summary>
        /// <include file='Logger/XMLs/Wrapper.xml' path='Docs/event[@name="OnAppend"]/*' />
        public event OnAppendHandler OnAppend;


        /// <summary>
        /// Does the proper checs and calls the 'OnAppend' method.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="line">The Line of the file where this method is called.</param>
        /// <param name="member">The name of the member where this method is called.</param>
        /// <param name="file">The name of the file from where this method is called.</param>
        /// <param name="level">Specifies which kind of log {Debug; Warn; ...}</param>
        private void Append(string text, int line, string member, string file, Vi.Logger.Levels level)
        {
            if (OnAppend != null)
            {
                OnAppend(text, line, member, file, level);
            }
        }


        /// <summary>
        /// The main CTor. Stores the supplied Logger.
        /// </summary>
        /// <param name="logger">The Logger used to log the messages.</param>
        public Wrapper(Vi.ILog logger)
        {
            this._Logger = logger ?? new Vi.Logger.FakeLog();
        }

        /// <summary>
        /// This is the most verbose logging level (maximum volume setting). Debug should be out-of-bounds for a production system and used only for development and testing.
        /// Logs a 'Debug in the log file if skepDebug (in config file) is false.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="line">The Line of the file where this method is called.</param>
        /// <param name="member">The name of the member where this method is called.</param>
        /// <param name="file">The name of the file from where this method is called.</param>
        public void Debug(string text, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?")
        {
            this.Append(text, line, member, file, Logger.Levels.DEBUG);
            this._Logger.Debug(text, line, member, file);
        }

        /// <summary>
        /// The 'Info' level is typically used to output information that is useful to the running and management of your system (production). 'Info' would also be the level used to log Entry and Exit points in key areas of your application. However, you may choose to add more entry and exit points at Debug level for more granularity during development and testing.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="line">The Line of the file where this method is called.</param>
        /// <param name="member">The name of the member where this method is called.</param>
        /// <param name="file">The name of the file from where this method is called.</param>
        public void Info(string text, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?")
        {
            this.Append(text, line, member, file, Logger.Levels.INFO);
            this._Logger.Info(text, line, member, file);
        }

        /// <summary>
        /// Warning is often used for handled 'exceptions' or other important log events. For example, if your application requires a configuration setting but has a default in case the setting is missing, then the Warning level should be used to log the missing configuration setting.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="line">The Line of the file where this method is called.</param>
        /// <param name="member">The name of the member where this method is called.</param>
        /// <param name="file">The name of the file from where this method is called.</param>
        public void Warn(string text, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?")
        {
            this.Append(text, line, member, file, Logger.Levels.WARN);
            this._Logger.Warn(text, line, member, file);
        }

        /// <summary>
        /// Error is used to log all unhandled exceptions. This is typically logged inside a catch block at the boundary of your application.
        /// </summary>
        /// <param name="se">The System.Exception to log.</param>
        /// <param name="line">The Line of the file where this method is called.</param>
        /// <param name="member">The name of the member where this method is called.</param>
        /// <param name="file">The name of the file from where this method is called.</param>
        public void Error(Exception se, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?")
        {
            this.Append(se.Message, line, member, file, Logger.Levels.ERROR);
            this._Logger.Error(se, line, member, file);
        }

        /// <summary>
        /// Fatal is reserved for special exceptions/conditions where it is imperative that you can quickly pick out these events. Fatal should to be used early in an application's development. It's usually only with experience it is possible identify situations worthy of the FATAL moniker experience do specific events become worth of promotion to Fatal. After all, an error's an error.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="line">The Line of the file where this method is called.</param>
        /// <param name="member">The name of the member where this method is called.</param>
        /// <param name="file">The name of the file from where this method is called.</param>
        public void Fatal(string text, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?")
        {
            this.Append(text, line, member, file, Logger.Levels.FATAL);
            this._Logger.Fatal(text, line, member, file);
        }
    } 

}
