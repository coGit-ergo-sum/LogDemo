using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Vi
{
    /// <summary>
    /// The base interface that every class must inherit from, to be used with the Vi.Log4Vi.Logger. 
    /// </summary>
    /// <include file='Logger/XMLs/ILog.xml' path='Docs/interface[@name="ILog"]/*' />
    public interface ILog
    {
        /// <summary>
        /// This is the most verbose logging level (maximum volume setting). Debug should be out-of-bounds for a production system and used only for development and testing.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="line">The line from where this method was called.</param>
        /// <param name="member">The member from where this method was called.</param>
        /// <param name="file">The file from where this method was called.</param>
        /// <include file='Logger/XMLs/ILog.xml' path='Docs/method[@name="Debug"]/*' />
        #region Debug
        void Debug(string text, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?");
        #endregion

        /// <summary>
        /// Fatal is reserved for special exceptions/conditions where it is imperative that you can quickly pick out these events. 
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="line">The line from where this method was called.</param>
        /// <param name="member">The member from where this method was called.</param>
        /// <param name="file">The file from where this method was called.</param>
        /// <include file='Logger/XMLs/ILog.xml' path='Docs/method[@name="Fatal"]/*' />
        #region Fatal
        void Fatal(string text, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?");
        #endregion

        /// <summary>
        /// The 'Info' level is typically used to output information that is useful to the running and management of your system (production). 
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="line">The line from where this method was called.</param>
        /// <param name="member">The member from where this method was called.</param>
        /// <param name="file">The file from where this method was called.</param>
        /// <include file='Logger/XMLs/ILog.xml' path='Docs/method[@name="Info"]/*' />
        #region Info
        void Info(string text, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?");
        #endregion

        /// <summary>
        /// Warning is often used for handled 'exceptions' or other important log events.
        /// </summary>
        /// <param name="text">The text to log.</param>
        /// <param name="line">The line from where this method was called.</param>
        /// <param name="member">The member from where this method was called.</param>
        /// <param name="file">The file from where this method was called.</param>
        /// <include file='Logger/XMLs/ILog.xml' path='Docs/method[@name="Warn"]/*' />
        #region Warn
        void Warn(string text, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?");
        #endregion

        /// <summary>
        /// Error is used to log all unhandled exceptions. 
        /// </summary>
        /// <param name="se">The exception to log.</param>
        /// <param name="line">The line from where this method was called.</param>
        /// <param name="member">The member from where this method was called.</param>
        /// <param name="file">The file from where this method was called.</param>
        /// <include file='Logger/XMLs/ILog.xml' path='Docs/method[@name="Error"]/*' />
        #region Error
        void Error(Exception se, [CallerLineNumber] int line = 0, [CallerMemberName] System.String member = "?", [CallerFilePath] System.String file = "?"); 
        #endregion
    }
}
