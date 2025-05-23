using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NextPost.Core.Models;
using NextPost.Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextPost.Infrastructure
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            // Rename Identity tables
            builder.Entity<AppUser>().ToTable("Users", "security");
            builder.Entity<AppRole>().ToTable("Roles", "security");
            builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles", "security");
            builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims", "security");
            builder.Entity<IdentityUserLogin<int>>().ToTable("UserLogins", "security");
            builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims", "security");
            builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens", "security");


        }
    }
}
