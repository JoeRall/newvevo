using Microsoft.AspNet.Identity.EntityFramework;
using NewVevo.Entity.Models;
using System.Data.Entity;

namespace NewVevo.Entity
{
    public class VevoContext : IdentityDbContext<ApplicationUser>
    {
        public VevoContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static VevoContext Create()
        {
            return new VevoContext();
        }

        public virtual DbSet<Video> Videos { get; set; }
        public virtual DbSet<WatchedVideo> WatchHistory { get; set; }
    }
}
