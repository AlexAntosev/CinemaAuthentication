using AuthenticationManager.Persisted.Configurations;
using AuthenticationManager.Persisted.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationManager.Persisted.Context
{
    public class CinemaAuthContext : IdentityUserContext<AuthUser>
    {
        public DbSet<AuthUser> AuthUsers { get; set; }
        public DbSet<IdentityRole> Roles { get; set; }
        public DbSet<IdentityUserRole<string>> UserRoles { get; set; }

        public CinemaAuthContext(DbContextOptions options) : base(options)
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new IdentityUserClaimsConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityUserLoginConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityUserRolesConfiguration());
        }

    }
}
