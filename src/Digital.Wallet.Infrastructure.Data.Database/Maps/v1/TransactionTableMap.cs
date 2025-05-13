using Digital.Wallet.Tables.v1;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Digital.Wallet.Maps.v1;

public class TransactionTableMap : IEntityTypeConfiguration<TransactionTable>
{
    public void Configure(EntityTypeBuilder<TransactionTable> builder)
    {
        builder.ToTable("Transaction");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.FromUserId).IsRequired();
        builder.Property(x => x.FromWalletId).IsRequired();
        builder.Property(x => x.ToUserId).IsRequired();
        builder.Property(x => x.ToWalletId).IsRequired();
        builder.Property(x => x.TransactionType).IsRequired().HasConversion<string>();
        builder.Property(x => x.Amount).IsRequired().HasColumnType("decimal(18,2)");
        builder.Property(x => x.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.HasIndex(x => x.FromUserId);
        builder.HasIndex(x => x.ToUserId);
        builder.HasIndex(x => x.CreatedAt);

        builder.HasOne(t => t.FromUser)
            .WithMany(u => u.SentTransaction)
            .HasForeignKey(t => t.FromUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.ToUser)
            .WithMany(u => u.ReceivedTransaction)
            .HasForeignKey(t => t.ToUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}