using Microsoft.AspNetCore.Identity;
//Using Identity allows for easier JWT implementation

namespace backend.Models;

public class User : IdentityUser
{
    //Attributes
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; } = DateTime.Now;

    //Relationships
    public List<Workspace> Workspaces { get; set; } //Each user will have a list of workspaces as an owner
}

//We dont have to use all of the new attributes below
//IdentityUser automatically adds a list of attributes:
/*
   Id
   UserName
   NormalizedUserName
   Email
   NormalizedEmail
   EmailConfirmed
   PasswordHash
   SecurityStamp
   ConcurrencyStamp
   PhoneNumber
   PhoneNumberConfirmed
   TwoFactorEnabled
   LockoutEnd
   LockoutEnabled
   AccessFailedCount
 */