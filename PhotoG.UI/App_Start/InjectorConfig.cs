
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using PhotoG.BL.Services;
using PhotoG.DAL.Repositories;
using PhotoG.Infrastructure.Identity;
using PhotoG.Infrastructure.Identity.Managers;
using PhotoG.Infrastructure.Logging;
using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Integration.Web.Mvc;

namespace PhotoG.UI
{
    public static class InjectorConfig
    {
        private static readonly Lazy<Container> Container = new Lazy<Container>(() =>
        {
            var container = new Container();
            InitializeInternal(container);

            return container;
        }, LazyThreadSafetyMode.ExecutionAndPublication);

        public static void Initialize()
        {
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(Container.Value));
        }

        private static void InitializeInternal(Container container)
        {
            container.RegisterPerWebRequest<HttpContextBase>(() => new HttpContextWrapper(HttpContext.Current));

            container.RegisterPerWebRequest<DbContext, PhotoG.Infrastructure.Identity.IdentityDbContext>();
            container.RegisterPerWebRequest<UserManager<ApplicationUser>>();
            container.RegisterPerWebRequest<IUserStore<ApplicationUser>>(() => new UserStore<ApplicationUser>(new PhotoG.Infrastructure.Identity.IdentityDbContext()));
            container.RegisterPerWebRequest<IRoleStore<IdentityRole, string>>(() => new RoleStore<IdentityRole, string, IdentityUserRole>(new PhotoG.Infrastructure.Identity.IdentityDbContext()));
            container.RegisterPerWebRequest<AppRoleManager>();
            container.RegisterPerWebRequest<AppUserManager>();
            container.RegisterPerWebRequest<AppSignInManager>();
            
            container.RegisterPerWebRequest<IAuthenticationManager>(() => container.IsVerifying()
                    ? new OwinContext(new Dictionary<string, object>()).Authentication
                    : HttpContext.Current.GetOwinContext().Authentication);

            container.Register<IAlbumService, AlbumService>();
            container.Register<IAlbumRepository, AlbumRepository>();
            container.Register<IPhotoService, PhotoService>();
            container.Register<IPhotoRepository, PhotoRepository>();

            ConfigureLogging(container);
        }

        private static void ConfigureLogging(Container container)
        {
            // this registration means that when ILogger is injected in target object
            // the name of the loggger will be full name of the class the logger is being injected to
            // -> in NLog configuration we are able to define logging rules using full type names 
            // of objects where loggers are injected
            container.RegisterConditional
                (typeof(ILogger),
                 context =>
                 {
                     if (context.Consumer == null)
                         return typeof(NLogLoggerProxy<>).MakeGenericType(typeof(InjectorConfig));

                     var genericArgs = context.Consumer.ImplementationType.GetGenericArguments();
                     return typeof(NLogLoggerProxy<>).MakeGenericType(genericArgs.Length >= 1
                         ? genericArgs[0]
                         : context.Consumer.ImplementationType);
                 },
                 Lifestyle.Singleton,
                 _ => true);
        }
    }
}