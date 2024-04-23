using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
//Using Identity allows for easier JWT implementation

namespace backend.Models;

public class User : IdentityUser
{
    //Attributes
    [MinLength(2, ErrorMessage = "First name should be at least 2 characters")]
    [MaxLength(20, ErrorMessage = "First name cannot be over 20 characters")]
    public string FirstName { get; set; } = string.Empty;
    
    [MinLength(2, ErrorMessage = "Last name should be at least 2 characters")]
    [MaxLength(20, ErrorMessage = "Last name cannot be over 20 characters")]
    public string LastName { get; set; } = string.Empty;
    public DateTime DateCreated { get; set; } = DateTime.Now;

    public List<Workspace> Workspaces { get; set; }
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