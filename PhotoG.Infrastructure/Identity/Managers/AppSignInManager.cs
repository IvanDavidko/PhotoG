using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace PhotoG.Infrastructure.Identity.Managers
{
    // Configure the application sign-in manager which is used in this application.
    public class AppSignInManager : SignInManager<ApplicationUser, string>
    {
        public AppSignInManager(AppUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((AppUserManager)UserManager);
        }

        public static AppSignInManager Create(IdentityFactoryOptions<AppSignInManager> options, IOwinContext context)
        {
            return new AppSignInManager(context.GetUserManager<AppUserManager>(), context.Authentication);
        }
    }
}
