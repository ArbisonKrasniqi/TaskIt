using Domain.Entities;
using Domain.Interfaces;

namespace Application.Handlers;

public class TaskDeleteHandler : ITaskDeleteHandler
{
    private readonly UserContext _userContext;
    private readonly ITasksRepository _tasksRepository;
    private readonly IWorkspaceActivityRepository _workspaceActivityRepository;
    
    public TaskDeleteHandler(UserContext userContext, ITasksRepository tasksRepository, IWorkspaceActivityRepository workspaceActivityRepository/*, ICommentDeleteHandler commentDeleteHandler*/)
    {
        _userContext = userContext;
        _tasksRepository = tasksRepository;
        _workspaceActivityRepository = workspaceActivityRepository;
        //_commentDeleteHandler = commentDeleteHandler;
    }
    
    public async Task HandleDeleteRequest(int taskId)
    {
        var task = (await _tasksRepository.GetTasks(taskId: taskId)).FirstOrDefault();
        var taskComments = task.Comments;
        if (taskComments.Any())
        {
            //Nese ka komente ne task, 
            //krijo nje for loop qe iteron ne te gjitha komentet
            //dhe i fshin ato komente permes CommentDeleteHandler
            /*
             * foreach (comment in taskComments) {
             *      await _commentDeleteHandler.HandleDeleteComment(comment.CommentId);
             * }
             */
        }
        await _tasksRepository.DeleteTask(taskId);
        
        // var newActivity = new Domain.Entities.WorkspaceActivity(task.List.Board.Workspace.WorkspaceId,
        //     _userContext.Id,
        //     "Updated",
        //     task.Title,
        //     DateTime.Now);
        // await _workspaceActivityRepository.CreateWorkspaceActivity(newActivity);
    }
}