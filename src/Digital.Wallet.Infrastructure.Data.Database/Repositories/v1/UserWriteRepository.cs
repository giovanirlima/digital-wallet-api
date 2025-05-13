using Digital.Wallet.Extensions;
using Digital.Wallet.Interfaces.v1;
using Digital.Wallet.Selectors;
using Digital.Wallet.Tables.v1;
using Microsoft.EntityFrameworkCore;

namespace Digital.Wallet.Repositories.v1;

public class UserWriteRepository(ReadWriteContext readWriteContext) : IUserWriteRepository
{
    private readonly ReadWriteContext _readWriteContext = readWriteContext;

    public async Task<UserTable?> GetUserByIdAsync(int id, CancellationToken cancellationToken) =>
        await _readWriteContext.User.Include(x => x.AddressTable).Include(x => x.WalletTable).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<UserTable?> GetUserByEmailAsync(string email, CancellationToken cancellationToken) =>
        await _readWriteContext.User.WhereLikeAny(x => x.Email.ToLower(), [email.ToLower()]).FirstOrDefaultAsync(cancellationToken);

    public async Task AddUserAsync(UserTable user, CancellationToken cancellationToken)
    {
        using var transaction = await _readWriteContext.Database.BeginTransactionAsync(cancellationToken);

        await _readWriteContext.User.AddAsync(user, cancellationToken);
        await _readWriteContext.SaveChangesAsync(cancellationToken);

        user.WalletTable = new()
        {
            UserId = user.Id
        };

        await _readWriteContext.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }

    public async Task UpdateUserAsync(UserTable user, CancellationToken cancellationToken)
    {
        _readWriteContext.User.Update(user);

        await _readWriteContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserAsync(UserTable user, CancellationToken cancellationToken)
    {
        using var transaction = await _readWriteContext.Database.BeginTransactionAsync(cancellationToken);

        _readWriteContext.User.Remove(user);
        _readWriteContext.Wallet.Remove(user.WalletTable!);
        _readWriteContext.Address.Remove(user.AddressTable!);

        await _readWriteContext.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }
}