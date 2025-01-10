namespace Application.Services.Authorization;

public interface IAuthorizationService
{
    Task<bool> CanAccessComment(string userId, int comment);
    Task<bool> CanAccessTask(string userId, int taskId);
    Task<bool> CanAccessList(string userId, int listId);
    Task<bool> CanAccessBoard(string userId, int boardId);
    Task<bool> IsMember(string userId, int workspaceId);
    Task<bool> OwnsWorkspace(string userId, int workspaceId);
    Task<bool> IsDeleted(string userId);
    Task<bool> IsAdmin(string userId);
}