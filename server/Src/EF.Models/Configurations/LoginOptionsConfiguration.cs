using EF.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Models.Configurations
{
    public class LoginOptionsConfiguration : IEntityTypeConfiguration<LoginOptions>
    {
        public void Configure(EntityTypeBuilder<LoginOptions> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.Property(entity => entity.Id).HasDefaultValueSql("newsequentialid()");

            builder.HasOne(entity => entity.User)
                .WithOne(entity => entity.LoginOptions)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey<LoginOptions>(entity => entity.UserId);
        }
    }
}