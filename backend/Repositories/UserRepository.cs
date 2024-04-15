using backend.Data;
using backend.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDBContext _context;
    public UserRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<bool> UserExists(string id)
    {
        return await _context.Users.AnyAsync(s => s.Id.Equals(id));
    }
}