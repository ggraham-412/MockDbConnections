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
        public static ILog LoggerImpl { get; set; } = null;

        public static void Log(string message)
        {
            if (LoggerImpl == null)
                return;

            LoggerImpl.Log(message);
        }
    }
}
