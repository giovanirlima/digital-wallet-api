using Infrastructure.Data.Database.Tables.v1;

namespace Infrastructure.Data.Command.Interfaces.v1;

public interface IUserCommandRepository
{
    Task<UserTable> GetUserByIdAsync(int id, CancellationToken cancellationToken);
    Task<UserTable> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task AddUserAsync(UserTable user, CancellationToken cancellationToken);
    Task UpdateUserAsync(UserTable user, CancellationToken cancellationToken);
    Task DeleteUserAsync(UserTable user, CancellationToken cancellationToken);
}