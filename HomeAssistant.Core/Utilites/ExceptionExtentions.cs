using System;
using System.Text;

namespace HomeAssistant.Core.Utilites
{
    /// <summary>
    /// Extensions for the <see cref="Exception"/> class.
    /// </summary>
    public static class ExceptionExtentions
    {
        /// <summary>
        /// Returns the exception message inclusive all inner exceptions.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>The created error message.</returns>
        public static string FullMessage(this Exception exception)
        {
            StringBuilder messageBuilder = new StringBuilder(32 * 1024);

            while (exception != null)
            {
                messageBuilder.AppendLine(exception.Message);
                exception = exception.InnerException;
            }

            return messageBuilder.ToString();
        }
    }
}
