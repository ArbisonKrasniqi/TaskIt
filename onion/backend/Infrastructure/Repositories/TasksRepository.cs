using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories
{
    public class TasksRepository : ITasksRepository
    {
        private readonly AppDbContext _context;

        public TasksRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Tasks>> GetTasks(int? taskId = null, int? index = null, DateTime? dateCreated = null, DateTime? dueDate = null,
            int? listId = null, int? boardId = null, int? workspaceId = null)
        {
            throw new NotImplementedException();
            //Masi kerkon boardId dhe workspaceId duhet me i prit modelet tjera qe me mujt me i kthy tasks sipas board dhe workspace
            //Logjika do tjet e njejt me UserRepository
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
