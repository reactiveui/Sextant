using System;
using System.Diagnostics;

namespace Sextant
{
	public class BaseLogger : IBaseLogger
    {
        /// <summary>
        /// Logs the debug.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="message">Message.</param>
        public void LogDebug(object sender, string message)
        {
            Debug.WriteLine(string.Format("DEBUG {0}: {1}", sender?.GetType()?.Name, message));
        }

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="ex">Ex.</param>
        /// <param name="message">Message.</param>
        public void LogError(object sender, Exception ex = null, string message = null)
        {
            Debug.WriteLine(string.Format("ERROR {0}: {1}{2}{3}", sender?.GetType()?.Name, message, Environment.NewLine, ex?.ToString()));
        }

        /// <summary>
        /// Logs the info.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="message">Message.</param>
        public void LogInfo(object sender, string message)
        {
            Debug.WriteLine(string.Format("INFO {0}: {1}", sender?.GetType()?.Name, message));
        }
    }
}
