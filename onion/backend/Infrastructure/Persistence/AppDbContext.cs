using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    //Constructor
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Tasks> Tasks { get; set; }
    public DbSet<List> Lists { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<WorkspaceActivity> WorkspaceActivities { get; set; }
    public DbSet<Board> Boards { get; set; }
    public DbSet<StarredBoard> StarredBoards { get; set; }
    public DbSet<Workspace> Workspaces { get; set; }
    public DbSet<Invite> Invites { get; set; }
    public DbSet<Members> Members { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Workspace>()
            .HasOne(b => b.User)
            .WithMany(u => u.Workspaces)
            .HasForeignKey(b => b.OwnerId)
            .OnDelete(DeleteBehavior.NoAction);
        
        //Boards with workspaces
        builder.Entity<Board>()
            .HasOne(b => b.Workspace)
            .WithMany(w => w.Boards)
            .HasForeignKey(b => b.WorkspaceId)
            .OnDelete(DeleteBehavior.NoAction);
        
        //Workspace with members
        builder.Entity<Members>()
            .HasOne(m => m.Workspace)
            .WithMany(w => w.Members)
            .HasForeignKey(m => m.WorkspaceId)
            .OnDelete(DeleteBehavior.NoAction);
        
        //Members with user
        builder.Entity<Members>()
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        //Invites with workspace
        builder.Entity<Invite>()
            .HasOne(i => i.Workspace)
            .WithMany(w => w.Invites)
            .HasForeignKey(i => i.WorkspaceId)
            .OnDelete(DeleteBehavior.NoAction);
        
        //Invite with User (Inviter)
        builder.Entity<Invite>()
            .HasOne(i => i.Inviter)
            .WithMany(u => u.SentInvites)
            .HasForeignKey(i => i.InviterId)
            .OnDelete(DeleteBehavior.NoAction);
        
        //Invite with User (Invitee)
        builder.Entity<Invite>()
            .HasOne(i => i.Invitee)
            .WithMany(u => u.ReceivedInvites)
            .HasForeignKey(i => i.InviteeId)
            .OnDelete(DeleteBehavior.NoAction);

        //Board with list
        builder.Entity<List>()
            .HasOne(l => l.Board)
            .WithMany(b => b.Lists)
            .HasForeignKey(l => l.BoardId)
            .OnDelete(DeleteBehavior.NoAction);
        
        //List with task
        builder.Entity<Tasks>()
            .HasOne(t => t.List)
            .WithMany(l => l.Tasks)
            .HasForeignKey(t => t.ListId)
            .OnDelete(DeleteBehavior.NoAction);
        
        //Task with comment
        builder.Entity<Tasks>()
            .HasMany(t => t.Comments)
            .WithOne(c => c.Task)
            .HasForeignKey(c => c.TaskId)
            .OnDelete(DeleteBehavior.NoAction);
        
        //Comment with user
        builder.Entity<Comment>()
            .HasOne(c => c.User)
            .WithMany(u => u.Comments)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        //Workspace activity with workspace
        builder.Entity<WorkspaceActivity>()
            .HasOne(wa => wa.Workspace)
            .WithMany(w => w.Activity)
            .HasForeignKey(wa => wa.WorkspaceId)
            .OnDelete(DeleteBehavior.NoAction);
        
        //Workspace activity with user
        builder.Entity<WorkspaceActivity>()
            .HasOne(wa => wa.User)
            .WithMany(u => u.Activity)
            .HasForeignKey(wa => wa.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        //Starred board with board
        builder.Entity<StarredBoard>()
            .HasOne(sb => sb.Board)
            .WithMany(b => b.StarredBoards)
            .HasForeignKey(sb => sb.BoardId)
            .OnDelete(DeleteBehavior.NoAction);
        
        //Starred board with user
        builder.Entity<StarredBoard>()
            .HasOne(sb => sb.User)
            .WithMany(u => u.StarredBoards)
            .HasForeignKey(sb => sb.UserId)
            .OnDelete(DeleteBehavior.NoAction);
        
        //Starred board with workspace
        builder.Entity<StarredBoard>()
            .HasOne(sb => sb.Workspace)
            .WithMany(w => w.StarredBoards)
            .HasForeignKey(sb => sb.WorkspaceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}