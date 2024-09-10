using System.Collections.Immutable;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend.Data;

public class ApplicationDBContext : IdentityDbContext<User>
{
    public ApplicationDBContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {

    }

    //Shtohen public DbSet<Entiteti> Entiteti { get; set; } per secilin entitet perveq User
    public DbSet<Workspace> Workspace { get; set; }
    public DbSet<Board> Board { get; set; }
    public DbSet<List> List { get; set; }
    public DbSet<Tasks> Tasks { get; set; }
    public DbSet<Checklist> Checklist { get; set; }
    public DbSet<ChecklistItem> ChecklistItem { get; set; }
    public DbSet<Members> Members { get; set; }
    public DbSet<Background> Background { get; set; }
    public DbSet<StarredBoard> StarredBoard { get; set; }
    public DbSet<Invite> Invite { get; set; }
    public DbSet<TaskMember> TaskMember { get; set; }
    public DbSet<Label> Label { get; set; }
    public DbSet<Comment> Comment { get; set; }
    public DbSet<WorkspaceActivity> WorkspaceActivity { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<TaskLabel> TaskLabel { get; set; }
    public DbSet<TaskActivity> TaskActivity { get; set; }

    //User roles
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        List<IdentityRole> roles = new List<IdentityRole>()
            //Every user will have an Identity Role (Either ADMIN or normal USER)
            {
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                }
            };
        builder.Entity<IdentityRole>().HasData(roles);
        
        builder.Entity<Background>()
            .HasOne(b => b.User) 
            .WithMany(u => u.Backgrounds)
            .HasForeignKey(b => b.CreatorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Workspace>()
            .HasOne(b => b.User)
            .WithMany(u => u.Workspaces)
            .HasForeignKey(b => b.OwnerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Invite>()
            .HasOne(i => i.Inviter)
            .WithMany(u => u.SentInvites)
            .HasForeignKey(i => i.InviterId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<Invite>()
            .HasOne(i => i.Invitee)
            .WithMany(u => u.ReceivedInvites)
            .HasForeignKey(i => i.InviteeId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.Entity<StarredBoard>()
            .HasOne(sb => sb.Workspace)
            .WithMany()                  
            .HasForeignKey(sb => sb.WorkspaceId)
            .OnDelete(DeleteBehavior.NoAction);
        
        builder.Entity<TaskLabel>()
            .HasOne(tl => tl.Task)
            .WithMany(t => t.TaskLabels)
            .HasForeignKey(tl => tl.TaskId)
            .OnDelete(DeleteBehavior.NoAction);

        // Configure TaskLabel and Label relationship
        builder.Entity<TaskLabel>()
            .HasOne(tl => tl.Label)
            .WithMany()
            .HasForeignKey(tl => tl.LabelId)
            .OnDelete(DeleteBehavior.NoAction);
    }
    
    
}