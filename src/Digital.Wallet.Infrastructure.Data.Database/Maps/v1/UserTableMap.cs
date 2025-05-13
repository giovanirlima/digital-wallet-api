using Digital.Wallet.Tables.v1;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Digital.Wallet.Maps.v1;

public class UserTableMap : IEntityTypeConfiguration<UserTable>
{
    public void Configure(EntityTypeBuilder<UserTable> builder)
    {
        builder.ToTable("User");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Password).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Birthday).IsRequired();
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(x => x.Email).IsUnique();

        builder.HasOne(x => x.AddressTable)
            .WithOne()
            .HasForeignKey<UserTable>(x => x.AddressId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.WalletTable)
            .WithOne()
            .HasForeignKey<UserTable>(u => u.WalletId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}