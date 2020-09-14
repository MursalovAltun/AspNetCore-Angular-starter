using EF.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Models.Configurations
{
    public class WebauthnCredentialConfiguration : IEntityTypeConfiguration<WebauthnCredential>
    {
        public void Configure(EntityTypeBuilder<WebauthnCredential> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.Property(entity => entity.Id).HasDefaultValueSql("newsequentialid()");

            builder.HasOne(entity => entity.User)
                .WithMany(entity => entity.WebauthnCredentials)
                .OnDelete(DeleteBehavior.Cascade)
                .HasForeignKey(entity => entity.UserId);
        }
    }
}