using Domain.Entities;

namespace Domain.Interfaces;

public interface ITokenRepository : IBaseRepository<Token>
{
    Task<List<Token>> GetTokenByUserIdAsync(string userId);
    Task<Token> GetTokenByValueAsync(string token);
}
