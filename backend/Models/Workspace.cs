﻿namespace backend.Models;

public class Workspace
{
    //Attributes
    public int WorkspaceId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int UserId { get; set; }
}