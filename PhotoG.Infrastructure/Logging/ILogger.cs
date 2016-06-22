using System;

namespace PhotoG.Infrastructure.Logging
{
    public interface ILogger
    {
        void Info(string format, params object[] args);
        void Warning(string format, params object[] args);
        void Error(string format, params object[] args);

        void Error(Exception ex);
        void Fatal(Exception ex);

        void Error(Exception ex, string format, params object[] args);
        void Fatal(Exception ex, string format, params object[] args);
        
        bool IsInfoEnabled();
    }
}
