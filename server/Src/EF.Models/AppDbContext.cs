using System;
using EF.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EF.Models
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, UserRole,
        IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<PushSubscription> PushSubscriptions { get; set; }
        public DbSet<UserAccessFailedAttempt> UserAccessFailedAttempts { get; set; }
        public DbSet<WebauthnCredential> WebauthnCredentials { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}