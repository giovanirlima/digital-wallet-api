using Digital.Wallet.Tables.v1;

namespace Digital.Wallet.Interfaces.v1;

public interface IUserReadRepository
{
    Task<IEnumerable<UserTable>> GetUserByFiltersAsync(IEnumerable<int>? id, IEnumerable<string>? name, IEnumerable<string>? email, CancellationToken cancellationToken);
}