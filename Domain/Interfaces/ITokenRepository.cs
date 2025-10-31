using Domain.Entities;

namespace Domain.Interfaces;

public interface ITokenRepository : IBaseRepository<Token>
{
    Task<Token?> GetTokenByValueAsync(string token);
    Task<List<Token>> GetTokenByUserIdAsync(string userId);
}
