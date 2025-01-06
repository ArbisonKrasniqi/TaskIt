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
    }
}