﻿namespace backend.DTOs.Invite.Input;

public class UpdateInviteAdminDto
{
    public int InviteId { get; set; }
    public int WorkspaceId { get; set; }
    public string InviterId { get; set; } = string.Empty;
    public string InviteeId { get; set; } = string.Empty;
    public string InviteStatus { get; set; } = string.Empty; //Pending, Accepted, Declined maybe 


}