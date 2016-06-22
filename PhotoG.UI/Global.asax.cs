using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NLog;

namespace PhotoG.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            LogManager.Flush();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //UnityConfig.Initialise();
            InjectorConfig.Initialize();

            AutoMapperConfig.Initialize();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Fatal("UNHANDLED exception in AppDomain: {0}", e.ExceptionObject);
            LogManager.Flush();
        }
    }
}
