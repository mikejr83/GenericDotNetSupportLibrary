using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericDotNetSupportLibrary.Extensions
{
    /// <summary>
    /// Extensions to the Exception class. <seealso cref="Exception"/>
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Converts the expression into a readable string for output to a log or message.
        /// </summary>
        /// <param name="exception">The exception</param>
        /// <returns>A string containing the exception and any inner exception messages.</returns>
        public static string ToOutputString(this Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(exception.Message);

            Exception inner = exception.InnerException;
            int i = 1;
            while (inner != null)
            {
                for (int j = 0; j < i; j++)
                    sb.Append("\t");

                sb.AppendLine("^----v");
                sb.AppendLine(inner.Message);
                inner = inner.InnerException;
                i++;
            }

            sb.AppendLine("----");
            sb.AppendLine(exception.StackTrace);

            return sb.ToString();
        }
    }
}
