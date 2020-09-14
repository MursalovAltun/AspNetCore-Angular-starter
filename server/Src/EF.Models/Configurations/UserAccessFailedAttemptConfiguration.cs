using System;
using EF.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Models.Configurations
{
    class UserAccessFailedAttemptConfiguration : IEntityTypeConfiguration<UserAccessFailedAttempt>
    {
        public void Configure(EntityTypeBuilder<UserAccessFailedAttempt> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.Property(entity => entity.Id)
                .HasDefaultValueSql("newsequentialid()");
            builder.Property(entity => entity.Date)
                .HasConversion(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));;
            builder.HasOne(entity => entity.User)
                .WithMany(entity => entity.UserAccessFailedAttempts)
                .HasForeignKey(entity => entity.UserId);
        }
    }
}