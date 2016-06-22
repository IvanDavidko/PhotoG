using Microsoft.AspNet.Identity.EntityFramework;

namespace PhotoG.Infrastructure.Identity
{
    public class IdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public IdentityDbContext()
            : base("PhotoG", throwIfV1Schema: false)
        {
        }

        public static IdentityDbContext Create()
        {
            return new IdentityDbContext();
        }
    }
}
