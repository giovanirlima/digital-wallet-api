using Digital.Wallet.Tables.v1;

namespace Digital.Wallet.Interfaces.v1;

public interface IUserWriteRepository
{
    Task<UserTable> GetUserByIdAsync(int id, CancellationToken cancellationToken);
    Task<UserTable> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task AddUserAsync(UserTable user, CancellationToken cancellationToken);
    Task UpdateUserAsync(UserTable user, CancellationToken cancellationToken);
    Task DeleteUserAsync(UserTable user, CancellationToken cancellationToken);
}