using backend.Data;
using backend.DTOs.User.Output;
using backend.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace backend.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDBContext _context;
    private readonly IMapper _mapper;
    public UserRepository(ApplicationDBContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<bool> UserExists(string id)
    {
        return await _context.Users.AnyAsync(s => s.Id.Equals(id));
    }

    public async Task<bool> UserOwnsWorkspace(string userId, int? workspaceId)
    {
        //Check if user owns workspace
        var workspaceModel =
            await _context.Workspace.FirstOrDefaultAsync(x => x.WorkspaceId == workspaceId && x.OwnerId == userId);
        return workspaceModel != null;
    }

    public async Task<bool> UserIsMember(string userId, int workspaceId)
    {
        var workspaceModel =
            await _context.Members.FirstOrDefaultAsync(x => x.WorkspaceId == workspaceId && x.UserId == userId);
        return workspaceModel != null;
    }

    public async Task<List<SearchUserDto>> SearchUsersAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return new List<SearchUserDto>();
        }

        var users = await _context.Users
            .Where(u => u.UserName.Contains(query) || 
                        u.Email.Contains(query) ||
                        u.FirstName.Contains(query) ||
                        u.LastName.Contains(query)).ToListAsync();
        var userDtos = _mapper.Map<List<SearchUserDto>>(users);
        return userDtos;
    }
}