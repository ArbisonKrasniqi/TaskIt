using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly ApplicationDBContext _context;

    public TokenRepository(ApplicationDBContext context)
    {
        _context = context;
    }


    public async Task<RefreshToken> GetUserRefreshToken(User user)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == user.Id);
    }

    public async Task<RefreshToken> GetRefreshTokenValue(string refreshTokenValue)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == refreshTokenValue);
    }

    public async Task<RefreshToken> AddRefreshToken(RefreshToken refreshToken)
    {
        //Gjej refreshTokenin ekzistues per userin qe kerkon refresh 
        var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == refreshToken.UserId);
        
        //Nese nuk ka token te ruajtur
        if (existingToken == null)
        {
            //Shto tokenin e ri
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        //Nese gjetet tokeni, ateher zevendeso me tokenin e ri
        existingToken.Token = refreshToken.Token;
        existingToken.Expires = refreshToken.Expires;
        await _context.SaveChangesAsync();
        
        return refreshToken;
    }

    public async Task<RefreshToken> RemoveRefreshToken(RefreshToken refreshToken)
    {
        var existingToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.UserId == refreshToken.UserId);
        if (existingToken == null)
        {
            return refreshToken;
        }

        _context.RefreshTokens.Remove(refreshToken);
        await _context.SaveChangesAsync();
        return refreshToken;

    }
}