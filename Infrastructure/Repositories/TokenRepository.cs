using System.Data.Entity;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.DataAccess;

namespace Infrastructure.Repositories;

public class TokenRepository : BaseRepository<Token, ApplicationDbContext>, ITokenRepository
{
    public TokenRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Token> GetTokenByValueAsync(string token)
    {
        return await _context.Tokens.Where(t => t.TokenValue == token).FirstAsync();
    }

    public async Task<List<Token>> GetTokenByUserIdAsync(string userId)
    {
        return await _context.Tokens.Where(t => t.UserId == userId).ToListAsync();
    }
}
