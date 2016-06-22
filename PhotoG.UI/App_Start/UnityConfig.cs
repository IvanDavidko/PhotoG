using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Practices.Unity;
using PhotoG.BL.Services;
using PhotoG.DAL.Repositories;
using PhotoG.Infrastructure.Identity;
using Unity.Mvc3;

namespace PhotoG.UI
{ 
    public static class UnityConfig
    {
        public static void Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            //container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
            //container.RegisterType(typeof(IUserStore<ApplicationUser>), typeof(UserStore<ApplicationUser>));

            //  container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<IRoleStore<IdentityRole, string>, RoleStore<IdentityRole>>(new HierarchicalLifetimeManager());
            container.RegisterType<DbContext, PhotoG.Infrastructure.Identity.IdentityDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<IAuthenticationManager>(new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication));
            
            container.RegisterType<IAlbumService, AlbumService>();
            container.RegisterType<IAlbumRepository, AlbumRepository>();
            container.RegisterType<IPhotoService, PhotoService>();
            container.RegisterType<IPhotoRepository, PhotoRepository>();

            return container;
        }
    }
}