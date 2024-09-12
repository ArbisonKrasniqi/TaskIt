using backend.Data;
using backend.DTOs.TaskMember.Input;
using backend.DTOs.TaskMember.Output;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories;

public class TaskMemberRepository : ITaskMemberRepository
{
    private readonly ApplicationDBContext _context;

    public TaskMemberRepository(ApplicationDBContext context)
    {
        _context = context;
    }
    
    //GET ALL
    public async Task<List<TaskMember>> GetAllTaskMembersAsync()
    {
        return await _context.TaskMember
            .Include(tm => tm.User)
            .Where(tm => !tm.User.isDeleted)
            .ToListAsync();
    }

    public async Task<TaskMember> GetTaskMemberByUserAndTask(string userId, int taskId)
    {
        return await _context.TaskMember.FirstOrDefaultAsync(tm => tm.UserId == userId && tm.TaskId == taskId);
    }
    
    //GET BY ID
    public async Task<TaskMember?> GetTaskMemberByIdAsync(int taskMemberId)
    {
        return await _context.TaskMember.FirstOrDefaultAsync(t => t.TaskMemberId == taskMemberId);
    }
    
    //GET ALL BY TaskID
    public async Task<List<TaskMember>> GetAllTaskMembersByTaskIdAsync(int taskId)
    {
        var task = await _context.Tasks
            .Include(t => t.TaskMembers)
            .ThenInclude(tm => tm.User)
            .FirstOrDefaultAsync(x => x.TaskId == taskId);

        // Handle the case where the task is not found
        if (task == null)
        {
            throw new KeyNotFoundException("Task Not found!");
        }

        // Map the task members to TaskMemberDto, including FirstName and LastName
        var taskMembersDto = task.TaskMembers
            .Where(tm => !tm.User.isDeleted)
            .Select(tm => new TaskMember
            {
                TaskMemberId = tm.TaskMemberId,
                UserId = tm.UserId,
                FirstName = tm.User.FirstName,
                LastName = tm.User.LastName, 
                DateJoined = tm.DateJoined,
                TaskId = tm.TaskId
            })
            .ToList();

        return taskMembersDto;
    }

    //ADD MEMBER TO TASK
    public async Task<TaskMember> AddTaskMemberAsync(AddTaskMemberDto addTaskMemberDto)
    {
        var task = await _context.Tasks.Include(t => t.TaskMembers)
            .FirstOrDefaultAsync(x => x.TaskId == addTaskMemberDto.TaskId);

        if (task == null)
        {
            throw new KeyNotFoundException("Task Not Found!");
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == addTaskMemberDto.UserId);

        if (user == null)
        {
            throw new KeyNotFoundException("User Not Found!");
        }

        if (task.TaskMembers.Any(tm => tm.UserId == addTaskMemberDto.UserId))
        {
            throw new InvalidOperationException("User is already a member of this task");
        }

        var taskMember = new TaskMember
        {
            UserId = addTaskMemberDto.UserId,
            User = user,
            TaskId = addTaskMemberDto.TaskId,
            DateJoined = DateTime.Now,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
        
        task.TaskMembers.Add(taskMember);
        await _context.SaveChangesAsync();

        return taskMember;
    }

    //UPDATE
    public async Task<TaskMember?> UpdateTaskMemberAsync(UpdateTaskMemberDto updateTaskMemberDto)
    {
        var existingTaskMember =
            await _context.TaskMember.FirstOrDefaultAsync(tm => tm.TaskMemberId == updateTaskMemberDto.TaskMemberId);
        if (existingTaskMember == null)
        {
            throw new Exception("Task Member not found!");
        }

        existingTaskMember.UserId = updateTaskMemberDto.UserId;
        existingTaskMember.TaskId = updateTaskMemberDto.TaskId;
        existingTaskMember.DateJoined = updateTaskMemberDto.DateJoined;

        await _context.SaveChangesAsync();
        return existingTaskMember;
    }

    //REMOVE TASKMEMBER
    public async Task<User> RemoveTaskMemberAsync(int taskId, string userId)
    {
        var task = await _context.Tasks.Include(t => t.TaskMembers).
            ThenInclude(tm => tm.User)
            .FirstOrDefaultAsync(x => x.TaskId == taskId);
        if (task == null)
        {
            throw new KeyNotFoundException("Task Not Found!");
        }

        var member = task.TaskMembers.FirstOrDefault(tm => tm.UserId == userId);
        if (member == null)
        {
            throw new KeyNotFoundException("User Not Found In This Task");
        }

        _context.TaskMember.Remove(member);
        await _context.SaveChangesAsync();

        return member.User;
    }

    //DELETE TASKMEMBER
    public async Task<TaskMember?> DeleteTaskMemberByIdAsync(int taskId)
    {
        var taskMemberModel = await _context.TaskMember.FirstOrDefaultAsync(tm => tm.TaskMemberId == taskId);
        if (taskMemberModel == null)
        {
            return null;
        }

        _context.TaskMember.Remove(taskMemberModel);
        await _context.SaveChangesAsync();

        return taskMemberModel;
    }

    //To check if user is already a member in the specific task
    public async Task<bool> IsATaskMember(string userId, int taskId)
    {
        return await _context.TaskMember.AnyAsync(tm => tm.UserId == userId && tm.TaskId == taskId);
    }
    //DELETE TASK MEMBERS BY TASK ID
    public async Task<List<TaskMember>> DeleteTaskMembersByTaskIdAsync(int taskId)
    {
        var taskMembers = await _context.TaskMember.Where(tm => tm.TaskId == taskId).ToListAsync();
        _context.TaskMember.RemoveRange(taskMembers);
        await _context.SaveChangesAsync();
        return taskMembers;
    }
}