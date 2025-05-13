using Digital.Wallet.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Digital.Wallet.Selectors;

public class ReadOnlyContext : ContextDb
{
    public ReadOnlyContext(DbContextOptions options) : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.LazyLoadingEnabled = false;
    }

    [Obsolete("This context is read-only", true)]
    public override int SaveChanges() =>
        throw new NotSupportedException();

    [Obsolete("This context is read-only", true)]
    public override int SaveChanges(bool acceptAllChangesOnSuccess) =>
        throw new NotSupportedException();

    [Obsolete("This context is read-only", true)]
    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken) =>
        throw new NotSupportedException();

    [Obsolete("This context is read-only", true)]
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken) =>
        throw new NotSupportedException();
}