using Digital.Wallet.Context;
using Microsoft.EntityFrameworkCore;

namespace Digital.Wallet.Selectors;

public class ReadWriteContext : ContextDb
{
    public ReadWriteContext(DbContextOptions options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }
}