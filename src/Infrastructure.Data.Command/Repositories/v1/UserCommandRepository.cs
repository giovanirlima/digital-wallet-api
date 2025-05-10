using Domain.Extensions;
using Infrastructure.Data.Command.Interfaces.v1;
using Infrastructure.Data.Database.Selectors;
using Infrastructure.Data.Database.Tables.v1;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Command.Repositories.v1;

public class UserCommandRepository(ReadWriteContext context) : IUserCommandRepository
{
    private readonly ReadWriteContext _context = context;

    public async Task<UserTable?> GetUserByIdAsync(int id, CancellationToken cancellationToken) =>
        await _context.User.Include(x => x.AddressTable).Include(x => x.WalletTable).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<UserTable?> GetUserByEmailAsync(string email, CancellationToken cancellationToken) =>
        await _context.User.WhereLikeAny(x => x.Email.ToLower(), [email.ToLower()]).FirstOrDefaultAsync(cancellationToken);

    public async Task AddUserAsync(UserTable user, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        await _context.User.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        user.WalletTable = new()
        {
            UserId = user.Id
        };

        await _context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }

    public async Task UpdateUserAsync(UserTable user, CancellationToken cancellationToken)
    {
        _context.User.Update(user);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserAsync(UserTable user, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        _context.User.Remove(user);
        _context.Wallet.Remove(user.WalletTable!);
        _context.Address.Remove(user.AddressTable!);

        await _context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }
}