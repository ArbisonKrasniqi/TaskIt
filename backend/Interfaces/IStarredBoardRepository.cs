﻿using backend.Models;

namespace backend.Interfaces;

public interface IStarredBoardRepository
{
    Task<StarredBoard?> StarBoardAsync(string userId, int boardId,  int workspaceId);
    Task<StarredBoard?> UnStarBoardAsync(string userId, int boardId, int workspaceId);
    Task<List<StarredBoard>> GetStarredBoardsAsync(string userId);
    Task<StarredBoard?> GetStarredBoardByIdAsync(int id);
    Task<bool> IsBoardStarredAsync(string userId, int boardId);
    Task<List<StarredBoard>> GetStarredBoardsByWorkspaceAsync(string userId, int workspaceId);
}