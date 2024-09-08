using backend.DTOs.TaskMember.Input;
using backend.DTOs.TaskMember.Output;
using backend.Models;

namespace backend.Interfaces;

public interface ITaskMemberRepository
{
    Task<List<TaskMember>> GetAllTaskMembersAsync();
    Task<TaskMember> GetTaskMemberByIdAsync(int taskMemberId);
    Task<List<TaskMemberDto>> GetAllTaskMembersByTaskIdAsync(int taskId);
    Task<TaskMember> AddTaskMemberAsync(AddTaskMemberDto addTaskMemberDto);
    Task<TaskMember?> UpdateTaskMemberAsync(UpdateTaskMemberDto updateTaskMemberDto);
    Task<User> RemoveTaskMemberAsync(int taskId, string userId);
    Task<TaskMember?> DeleteTaskMemberByIdAsync(int taskId);
    Task<bool> IsATaskMember(string userId, int taskId);
    Task<List<TaskMember>> DeleteTaskMembersByTaskIdAsync(int taskId);
}