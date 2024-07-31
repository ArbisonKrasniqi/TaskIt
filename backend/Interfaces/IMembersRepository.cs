﻿using backend.DTOs.Members;
using backend.DTOs.User.Input;
using backend.DTOs.Workspace;

namespace backend.Interfaces;
using backend.Models;

public interface IMembersRepository
{
    Task AddMemberAsync (AddMemberDto addMemberDto);
    Task<List<User>> GetAllMembersAsync(int workspaceId);
    Task<User> RemoveMemberAsync(int workspaceId, string memberId);
}