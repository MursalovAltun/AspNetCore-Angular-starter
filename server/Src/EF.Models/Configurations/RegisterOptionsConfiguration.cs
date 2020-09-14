using EF.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Models.Configurations
{
    public class RegisterOptionsConfiguration : IEntityTypeConfiguration<RegisterOptions>
    {
        public void Configure(EntityTypeBuilder<RegisterOptions> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.Property(entity => entity.Id).HasDefaultValueSql("newsequentialid()");

            builder.HasOne(entity => entity.User)
                .WithOne(entity => entity.RegisterOptions)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey<RegisterOptions>(entity => entity.UserId);
        }
    }
}