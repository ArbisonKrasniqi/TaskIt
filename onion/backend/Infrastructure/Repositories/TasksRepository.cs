using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TasksRepository : ITasksRepository
    {
        private readonly AppDbContext _context;

        public TasksRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tasks>> GetTasks(int? taskId = null, int? index = null, DateTime? dateCreated = null, DateTime? dueDate = null,
            int? listId = null, int? boardId = null, int? workspaceId = null)
        {
            var query = _context.Tasks.AsQueryable();

            query = query.Include(t => t.List) // Include the List associated with the Task
                         .ThenInclude(l => l.Tasks) // Include the Tasks related to the List
                         .Include(t => t.List.Board) // Include the Board related to the List
                         .ThenInclude(b => b.Workspace) // Include the Workspace related to the Board
                         .Include(t => t.Comments); 
            
            if (taskId.HasValue)
                query = query.Where(t => t.TaskId == taskId);
            if (index.HasValue)
                query = query.Where(t => t.Index == index);
            if (dateCreated.HasValue)
                query = query.Where(t => t.DateCreated.Date == dateCreated.Value.Date);
            if (dueDate.HasValue)
                query = query.Where(t => t.DueDate.Date == dueDate.Value.Date);
            if (listId.HasValue)
                query = query.Where(t => t.ListId == listId);
            if (boardId.HasValue)
                query = query.Where(t => t.List.BoardId == boardId);
            if (workspaceId.HasValue)
                query = query.Where(t => t.List.Board.WorkspaceId == workspaceId);
            
            return await query.ToListAsync();
        }

        public async Task<Tasks> CreateTask(Tasks task)
        {
            await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<Tasks> UpdateTask(Tasks task)
        {
            var existingTask = await _context.Tasks.FindAsync(task.TaskId);

            if (existingTask == null)
                throw new Exception("Task not found");

            _context.Entry(existingTask).CurrentValues.SetValues(task);
            await _context.SaveChangesAsync();

            return existingTask;
        }

        public async Task<Tasks> DeleteTask(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);

            if (task == null)
                throw new Exception("Task not found");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return task;
        }
    }
}
