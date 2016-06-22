using System;
using System.Configuration;
using System.Threading;
using System.Web;
using Elmah;
using NLog;
using NLog.Targets;

namespace PhotoG.Infrastructure.Logging
{
    [Target("ElmahSql")]
    public sealed class ElmahTargetSql : TargetWithLayout
    {
        private readonly Lazy<SqlErrorLog> _errorLogLazy;

        public string ConnectionString { get; set; }
        public string ApplicationName { get; set; }

        public ElmahTargetSql()
        {
            _errorLogLazy = new Lazy<SqlErrorLog>(() =>
            {
                if (string.IsNullOrWhiteSpace(ConnectionString))
                    ConnectionString = ConfigurationManager.ConnectionStrings["elmah:sql"].ConnectionString;

                return new SqlErrorLog(ConnectionString);
            });
        }

        private Error CreateError(Exception exception, HttpContext context)
        {
            if (exception == null)
                return new Error();

            try { return new Error(exception, context); }
            catch (HttpException)
            {
                return new Error(exception);
            }
        }

        protected override void Write(LogEventInfo logEvent)
        {
            if (string.IsNullOrWhiteSpace(ApplicationName))
                ApplicationName = ConfigurationManager.AppSettings["elmah:sql:applicationName"];

            var logMessage = Layout.Render(logEvent);

            var httpContext = HttpContext.Current;
            var errorLog = _errorLogLazy.Value;
            var error = CreateError(logEvent.Exception, httpContext);

            var type = error.Exception != null
                ? error.Exception.GetType().FullName
                : logEvent.Level.Name;
            error.Type = "NLog::" + type;
            error.ApplicationName = ApplicationName;
            error.Message = logEvent.FormattedMessage.Length > 500 ? logEvent.FormattedMessage.Substring(0, 500) : logEvent.FormattedMessage;
            error.Time = GetCurrentDateTime == null ? logEvent.TimeStamp : GetCurrentDateTime();
            error.HostName = Environment.MachineName;
            error.Detail = logEvent.Exception == null ? logMessage : logEvent.Exception.StackTrace;
            try
            {
                error.User = Thread.CurrentPrincipal.Identity.Name;
            }
            catch (ObjectDisposedException) { } // if thread has no identity set

            if (logEvent.Exception != null)
            {
                foreach (var key in logEvent.Exception.Data.Keys)
                {
                    var value = logEvent.Exception.Data[key];
                    error.ServerVariables.Add(key.ToString(), value != null ? value.ToString() : string.Empty);
                }
            }

            error.Source = logEvent.LoggerName;

            if (string.IsNullOrEmpty(errorLog.ApplicationName))
            {
                errorLog.ApplicationName = error.ApplicationName;
            }
            errorLog.Log(error);
        }

        public Func<DateTime> GetCurrentDateTime { get; set; }
    }
}
