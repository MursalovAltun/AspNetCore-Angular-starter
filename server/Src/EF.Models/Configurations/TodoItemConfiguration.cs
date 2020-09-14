using EF.Models.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EF.Models.Configurations
{
    public class TodoItemConfiguration : IEntityTypeConfiguration<TodoItem>
    {
        public void Configure(EntityTypeBuilder<TodoItem> builder)
        {
            builder.HasKey(entity => entity.Id);
            builder.Property(entity => entity.Id).HasDefaultValueSql("newsequentialid()");
            builder.HasOne(entity => entity.User)
                .WithMany()
                .HasForeignKey(entity => entity.UserId)
                .IsRequired();
        }
    }
}