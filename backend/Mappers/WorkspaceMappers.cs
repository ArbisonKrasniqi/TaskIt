﻿using backend.DTOs.Workspace;
using backend.Models;

namespace backend.Mappers;

public static class WorkspaceMappers
{
    public static WorkspaceDto ToWorkspaceDto(this Workspace workspaceModel)
    {
        return new WorkspaceDto
        {   
            WorkspaceId = workspaceModel.WorkspaceId,
            Title = workspaceModel.Title,
            OwnerId = workspaceModel.OwnerId
           
        };
    }

    public static Workspace ToWorkspaceFromCreate(this CreateWorkspaceRequestDto workspaceDto)
    {
        return new Workspace
        {
            Title = workspaceDto.Title,
            Description = workspaceDto.Description,
            OwnerId = workspaceDto.OwnerId
        };
    }
    public static Workspace ToWorkspaceFromUpdate(this UpdateWorkspaceRequestDto workspaceDto)
    {
        return new Workspace
        {
            Title = workspaceDto.Title,
            Description = workspaceDto.Description,
            OwnerId = workspaceDto.OwnerId
        };
    }
}