using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<User>> GetUsers(string userId = null, string firstName = null, string lastName = null, string email = null,
        DateTime? dateCreated = null, bool? isDeleted = null, string role = null, string refreshToken = null)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(userId))
            query = query.Where(u => u.Id == userId);
        if (!string.IsNullOrEmpty(firstName))
            query = query.Where(u => EF.Functions.Like(u.FirstName, $"%{firstName}%"));
        if (!string.IsNullOrEmpty(lastName))
            query = query.Where(u => EF.Functions.Like(u.LastName, $"%{lastName}%"));
        if (!string.IsNullOrEmpty(email))
            query = query.Where(u => u.Email == email);
        if (dateCreated.HasValue)
            query = query.Where(u => u.DateCreated.Date == dateCreated.Value.Date);
        if (isDeleted.HasValue)
            query = query.Where(u => u.IsDeleted == isDeleted);
        if (!string.IsNullOrEmpty(role))
            query = query.Where(u => u.Role == role);
        if (!string.IsNullOrEmpty(refreshToken))
            query = query.Where(u => u.RefreshToken == refreshToken);
        
        return await query.ToListAsync();
    }

    public async Task<User> CreateUser(User user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

        if (existingUser != null)
            throw new Exception("A user with this email already exists.");
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUser(User user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);

        if (existingUser == null)
            throw new Exception("User not found");

        _context.Entry(existingUser).CurrentValues.SetValues(user);
        await _context.SaveChangesAsync();
        return existingUser;
    }

    public async Task<User> DeleteUser(string userId)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
            throw new Exception("User not found");

        user.IsDeleted = true;
        await _context.SaveChangesAsync();

        return user;
    }
}