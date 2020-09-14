using EF.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Models.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(entity => new {entity.UserId, entity.RoleId});
            builder.HasOne(entity => entity.User)
                .WithMany(user => user.Roles)
                .HasForeignKey(entity => entity.RoleId);
            builder.HasOne(entity => entity.Role)
                .WithMany(role => role.Users)
                .HasForeignKey(entity => entity.UserId);
            builder.Property(entity => entity.UserId)
                .HasColumnName(nameof(UserRole.UserId));
            builder.Property(entity => entity.RoleId)
                .HasColumnName(nameof(UserRole.RoleId));
        }
    }
}