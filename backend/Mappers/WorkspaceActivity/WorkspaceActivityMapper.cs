using backend.DTOs.WorkspaceActivity.Input;
using backend.DTOs.WorkspaceActivity.Output;

namespace backend.Mappers.WorkspaceActivity;

public static class WorkspaceActivityMapper
{

    public static Models.WorkspaceActivity ToWorkspaceActivityFromCreate(this AddWorkspaceActivityDto workspaceActivityDto, string userId)
    {
        return new Models.WorkspaceActivity
        {
            WorkspaceId = workspaceActivityDto.WorkspaceId,
            UserId = userId,
            ActionType = workspaceActivityDto.ActionType,
            EntityName = workspaceActivityDto.EntityName
        };
    }

    public static WorkspaceActivityDto ToWorkspaceActivityDto(this Models.WorkspaceActivity workspaceActivity)
    {
        return new WorkspaceActivityDto
        {
            WorkspaceActivityId = workspaceActivity.WorkspaceActivityId,
            WorkspaceId = workspaceActivity.WorkspaceId,
            UserId = workspaceActivity.UserId,
            ActionType = workspaceActivity.ActionType,
            EntityName = workspaceActivity.EntityName,
            ActionDate = workspaceActivity.ActionDate
        };
    }
}