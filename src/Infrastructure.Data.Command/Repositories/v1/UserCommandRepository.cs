using Domain.Entities.v1;
using Domain.Extensions;
using Infrastructure.Data.Command.Interfaces.v1;
using Infrastructure.Data.Database.Selectors;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Command.Repositories.v1;

public class UserCommandRepository(ReadWriteContext context) : IUserCommandRepository
{
    private readonly ReadWriteContext _context = context;

    public async Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken) =>
        await _context.User.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken) =>
        await _context.User.WhereLikeAny(x => x.Email, [email]).FirstOrDefaultAsync(cancellationToken);

    public async Task AddUserAsync(User user, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        await _context.User.AddAsync(user, cancellationToken);

        await _context.Address.AddAsync(user.Address!, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }

    public async Task UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        _context.User.Update(user);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserAsync(User user, CancellationToken cancellationToken)
    {
        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        _context.User.Remove(user);

        _context.Address.Remove(user.Address!);

        await _context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }
}