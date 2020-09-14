using EF.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Models.Configurations
{
    public class PushSubscriptionConfiguration : IEntityTypeConfiguration<PushSubscription>
    {
        public void Configure(EntityTypeBuilder<PushSubscription> builder)
        {
            builder.HasKey(entity => entity.Endpoint);
            builder.HasOne(entity => entity.User)
                .WithMany(entity => entity.PushSubscriptions)
                .HasForeignKey(entity => entity.UserId);
            builder.Property(entity => entity.UserId)
                .IsRequired();
        }
    }
}