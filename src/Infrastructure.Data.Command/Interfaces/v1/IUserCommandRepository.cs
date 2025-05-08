using Domain.Entities.v1;

namespace Infrastructure.Data.Command.Interfaces.v1;

public interface IUserCommandRepository
{
    Task<User> GetUserByIdAsync(int id, CancellationToken cancellationToken);
    Task<User> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task AddUserAsync(User user, CancellationToken cancellationToken);
    Task UpdateUserAsync(User user, CancellationToken cancellationToken);
    Task DeleteUserAsync(User user, CancellationToken cancellationToken);
}