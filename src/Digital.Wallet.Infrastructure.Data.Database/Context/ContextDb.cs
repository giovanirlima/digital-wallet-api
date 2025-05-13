using Digital.Wallet.Tables.v1;
using Microsoft.EntityFrameworkCore;

namespace Digital.Wallet.Context;

public class ContextDb(DbContextOptions options) : DbContext(options)
{
    public DbSet<UserTable> User { get; set; }
    public DbSet<WalletTable> Wallet { get; set; }
    public DbSet<TransactionTable> Transaction { get; set; }
    public DbSet<AddressTable> Address { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("bank");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContextDb).Assembly);
    }
}