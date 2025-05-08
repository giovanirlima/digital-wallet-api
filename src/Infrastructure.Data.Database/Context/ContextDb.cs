using Domain.Entities.v1;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Database.Context;

public class ContextDb(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> User { get; set; }
    public DbSet<Address> Address { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("wallet");

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContextDb).Assembly);
    }
}