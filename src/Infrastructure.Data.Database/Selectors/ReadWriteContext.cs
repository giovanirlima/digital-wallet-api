using Infrastructure.Data.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Database.Selectors;

public class ReadWriteContext : ContextDb
{
    public ReadWriteContext(DbContextOptions options) : base(options)
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }
}