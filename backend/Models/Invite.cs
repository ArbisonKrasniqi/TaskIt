namespace backend.Models;

public class Invite
{
  public int InviteId { get; set; }
  public int WorkspaceId { get; set; }
  public string InviterId { get; set; } = string.Empty;
  public string InviteeId { get; set; } = string.Empty;
  public string InviteStatus { get; set; } = string.Empty; //Pending, Accepted, Declined maybe 
  public DateTime DateSent { get; set; } 
  
  //dateaccepted, datedeclined, dateExpired
}