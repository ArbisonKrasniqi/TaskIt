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
    public DbSet<Members> Members { get; set; }
    public DbSet<Background> Background { get; set; }
    public DbSet<StarredBoard> StarredBoard { get; set; }
    public DbSet<Invite> Invite { get; set; }
    public DbSet<TaskMember> TaskMember { get; set; }


    public DbSet<RefreshToken> RefreshTokens { get; set; }
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
    }
    
    
}