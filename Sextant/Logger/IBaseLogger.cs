using System;

namespace Sextant
{
    public interface IBaseLogger
    {
		/// <summary>
        /// Logs the debug.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="message">Message.</param>
        void LogDebug(object sender, string message);

        /// <summary>
        /// Logs the info.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="message">Message.</param>
        void LogInfo(object sender, string message);

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="ex">Ex.</param>
        /// <param name="message">Message.</param>
        void LogError(object sender, Exception ex = null, string message = null);
    }
}