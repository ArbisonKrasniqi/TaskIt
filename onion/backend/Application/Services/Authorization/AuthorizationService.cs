using Domain.Interfaces;

namespace Application.Services.Authorization;

public class AuthorizationService : IAuthorizationService
{
    private readonly IUserRepository _userRepo;
    private readonly IWorkspaceRepository _workspaceRepo;
    private readonly IMembersRepository _membersRepo;
    private readonly IBoardRepository _boardRepo;
    private readonly IListRepository _listRepo;
    private readonly ITasksRepository _taskRepo;
    private readonly ICommentRepository _commentRepo;

    public AuthorizationService(IUserRepository userRepo, IWorkspaceRepository workspaceRepo,
        IMembersRepository membersRepo, IBoardRepository boardRepo, IListRepository listRepo, ITasksRepository taskRepo,
        ICommentRepository commentRepo)
    {
        _userRepo = userRepo;
        _workspaceRepo = workspaceRepo;
        _membersRepo = membersRepo;
        _boardRepo = boardRepo;
        _listRepo = listRepo;
        _taskRepo = taskRepo;
        _commentRepo = commentRepo;
    }

    public async Task<bool> CanAccessComment(string userId, int commentId)
    {
        var user = (await _userRepo.GetUsers(userId: userId)).FirstOrDefault();

        var comment = (await _commentRepo.GetComments(commentId: commentId)).FirstOrDefault();
        if (user == null || comment == null) return false;
        
        var task = (await _taskRepo.GetTasks(taskId: comment.TaskId)).FirstOrDefault();
        if (user == null || task == null) return false;

        var list = (await _listRepo.GetLists(listId: task.ListId)).FirstOrDefault();
        if (list == null) return false;

        var board = (await _boardRepo.GetBoards(boardId: list.BoardId)).FirstOrDefault();
        if (board == null) return false;

        var workspace = (await _workspaceRepo.GetWorkspaces(workspaceId: board.WorkspaceId)).FirstOrDefault();
        if (workspace == null) return false;

        if (workspace.OwnerId == userId) return true;

        var member = (await _membersRepo.GetMembers(userId:user.Id,workspaceId: workspace.WorkspaceId)).FirstOrDefault();
        if (member == null) return false;

        return true;

    }

    public async Task<bool> CanAccessTask(string userId, int taskId)
    {
        var user = (await _userRepo.GetUsers(userId: userId)).FirstOrDefault();
        
        var task = (await _taskRepo.GetTasks(taskId: taskId)).FirstOrDefault();
        if (user == null || task == null) return false;

        var list = (await _listRepo.GetLists(listId: task.ListId)).FirstOrDefault();
        if (list == null) return false;

        var board = (await _boardRepo.GetBoards(boardId: list.BoardId)).FirstOrDefault();
        if (board == null) return false;

        var workspace = (await _workspaceRepo.GetWorkspaces(workspaceId: board.WorkspaceId)).FirstOrDefault();
        if (workspace == null) return false;

        if (workspace.OwnerId == userId) return true;

        var member = (await _membersRepo.GetMembers(userId:user.Id,workspaceId: workspace.WorkspaceId)).FirstOrDefault();
        if (member == null) return false;

        return true;
    }

    public async Task<bool> CanAccessList(string userId, int listId)
    {
        var user = (await _userRepo.GetUsers(userId: userId)).FirstOrDefault();
        var list = (await _listRepo.GetLists(listId: listId)).FirstOrDefault();
        if (list == null) return false;
        
        var board = (await _boardRepo.GetBoards(boardId: list.BoardId)).FirstOrDefault();
        if (board == null) return false;
        
        var workspace = (await _workspaceRepo.GetWorkspaces(workspaceId: board.WorkspaceId)).FirstOrDefault();
        if (workspace == null) return false;

        if (workspace.OwnerId == userId) return true;

        var member = (await _membersRepo.GetMembers(userId:user.Id,workspaceId: workspace.WorkspaceId)).FirstOrDefault();
        if (member == null) return false;

        return true;
    }

    public async Task<bool> CanAccessBoard(string userId, int boardId)
    {
        var user = (await _userRepo.GetUsers(userId: userId)).FirstOrDefault();
        
        var board = (await _boardRepo.GetBoards(boardId: boardId)).FirstOrDefault();
        if (board == null) return false;
        
        var workspace = (await _workspaceRepo.GetWorkspaces(workspaceId: board.WorkspaceId)).FirstOrDefault();
        if (workspace == null) return false;

        if (workspace.OwnerId == userId) return true;

        var member = (await _membersRepo.GetMembers(userId:user.Id,workspaceId: workspace.WorkspaceId)).FirstOrDefault();
        if (member == null) return false;

        return true;
    }

    public async Task<bool> IsMember(string userId, int workspaceId)
    {
        var user = (await _userRepo.GetUsers(userId: userId)).FirstOrDefault();
        
        var workspace = (await _workspaceRepo.GetWorkspaces(workspaceId: workspaceId)).FirstOrDefault();
        if (workspace == null) return false;

        if (workspace.OwnerId == userId) return true;

        var member = (await _membersRepo.GetMembers(userId:user.Id,workspaceId: workspace.WorkspaceId)).FirstOrDefault();
        if (member == null) return false;

        return true;
    }

    public async Task<bool> OwnsWorkspace(string userId, int workspaceId)
    {
        var user = (await _userRepo.GetUsers(userId: userId)).FirstOrDefault();
        var workspace = (await _workspaceRepo.GetWorkspaces(workspaceId: workspaceId)).FirstOrDefault();

        if (workspace == null || user == null) return false;
        if (user.Id == workspace.OwnerId) return true;

        return false;
    }

    public async Task<bool> IsDeleted(string userId)
    {
        var user = (await _userRepo.GetUsers(userId: userId)).FirstOrDefault();
        if (user is null || user.IsDeleted) return true;
        return false;
    }

    public async Task<bool> IsAdmin(string userId)
    {
        var user = (await _userRepo.GetUsers(userId: userId)).FirstOrDefault();
        if (user is null) return false;
        else if (user.Role == "Admin") return true;
        return false;
    }
}