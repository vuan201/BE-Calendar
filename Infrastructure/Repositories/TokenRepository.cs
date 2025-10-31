 using System.Data.Entity;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;

namespace Infrastructure.Repositories;

public class TokenRepository : BaseRepository<Token, ApplicationDbContext>, ITokenRepository
{
    public TokenRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Token?> GetTokenByValueAsync(string token)
    {
        return await this.GetAsync(t => t.TokenValue == token);
    }

    public async Task<List<Token>> GetTokenByUserIdAsync(string userId)
    {
        var result = await this.GetListAsync(t => t.UserId == userId);

        return result.ToList();
    }
}
