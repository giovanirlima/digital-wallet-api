using Digital.Wallet.Tables.v1;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Digital.Wallet.Maps.v1;

public class AddressTableMap : IEntityTypeConfiguration<AddressTable>
{
    public void Configure(EntityTypeBuilder<AddressTable> builder)
    {
        builder.ToTable("Address");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Street).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Number).IsRequired();
        builder.Property(x => x.City).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Country).IsRequired().HasMaxLength(50);
    }
}