using Digital.Wallet.Tables.v1;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Digital.Wallet.Maps.v1;

public class WalletTableMap : IEntityTypeConfiguration<WalletTable>
{
    public void Configure(EntityTypeBuilder<WalletTable> builder)
    {
        builder.ToTable("Wallet");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Amount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        builder.Property(x => x.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasOne(c => c.User)
            .WithOne()
            .HasForeignKey<WalletTable>(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}