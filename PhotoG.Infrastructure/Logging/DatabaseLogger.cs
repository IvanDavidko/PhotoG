using System.Data.Entity;

namespace PhotoG.Infrastructure.Logging
{
    public class DatabaseLogger
    {
        public static void InitFor<TContext>(TContext context)
            where TContext : DbContext
        {
            var logger = new NLogLoggerProxy<TContext>();
            context.Database.Log = msg => logger.Info(msg);
        }

    }
}
