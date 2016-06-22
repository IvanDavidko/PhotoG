using System;
using NLog;

namespace PhotoG.Infrastructure.Logging
{
    public class NLogLoggerProxy<T> : ILogger
    {
        private static readonly NLog.ILogger _logger = LogManager.GetLogger(typeof (T).FullName);

        public void Info(string format, params object[] args)
        {
            _logger.Info(format, args);
        }

        public void Warning(string format, params object[] args)
        {
            _logger.Warn(format, args);
        }

        public void Error(string format, params object[] args)
        {
            _logger.Error(format, args);
        }

        public void Error(Exception ex)
        {
            var lei = LogEventInfo.Create(LogLevel.Error, 
                                          _logger.Name,
                                          ex,
                                          null,
                                          ex.ToString());
            _logger.Log(lei);
        }

        public void Fatal(Exception ex)
        {
            var lei = LogEventInfo.Create(LogLevel.Fatal, 
                                          _logger.Name,
                                          ex,
                                          null,
                                          ex.ToString());
            _logger.Log(lei);
        }

        public void Error(Exception ex, string format, params object[] args)
        {
            var lei = LogEventInfo.Create(LogLevel.Error, 
                                          _logger.Name,
                                          ex,
                                          null,
                                          format,
                                          args);
            _logger.Log(lei);
        }

        public void Fatal(Exception ex, string format, params object[] args)
        {
            var lei = LogEventInfo.Create(LogLevel.Fatal,
                                          _logger.Name,
                                          ex,
                                          null,
                                          format,
                                          args);
            _logger.Log(lei);
        }

        public bool IsInfoEnabled()
        {
            return _logger.IsInfoEnabled;
        }
    }
}
