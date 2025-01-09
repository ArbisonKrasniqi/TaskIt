using Domain.Interfaces;

namespace Application.Handlers;

public class TaskDeleteHandler : ITaskDeleteHandler
{
    private readonly ITasksRepository _tasksRepository;

    public TaskDeleteHandler(ITasksRepository tasksRepository/*, ICommentDeleteHandler commentDeleteHandler*/)
    {
        _tasksRepository = tasksRepository;
        //_commentDeleteHandler = commentDeleteHandler;
    }
    
    public async Task HandleDeleteRequest(int taskId)
    {
        var taskComments = (await _tasksRepository.GetTasks(taskId: taskId)).FirstOrDefault().Comments;
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
    }
}