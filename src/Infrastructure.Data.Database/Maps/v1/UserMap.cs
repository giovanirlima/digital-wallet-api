using Domain.Entities.v1;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Database.Maps.v1;

public class UserMap : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Birthday).IsRequired();
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.CreatedAt).HasDefaultValue(DateTime.UtcNow)
            .HasColumnType("timestamp with time zone")
            .HasConversion(
                v => v.Value.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        builder.Property(x => x.UpdatedAt).HasDefaultValue(DateTime.UtcNow)
            .HasColumnType("timestamp with time zone")
            .HasConversion(
                v => v.Value.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        builder.HasIndex(x => x.Email).IsUnique();

        builder.HasOne(c => c.Address)
            .WithOne()
            .HasForeignKey<User>(n => n.AddressId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}