using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MockDbConnections
{
    /// <summary>
    ///     Stub logger class
    /// </summary>
    public static class Logger
    {
        private static Action<string> LoggerImpl { get; set; } = null;

        public static void Log(string message)
        {
            LoggerImpl?.Invoke(message);
        }
    }
}
