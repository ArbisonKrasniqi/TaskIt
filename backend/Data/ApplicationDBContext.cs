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
    //public DbSet<Board> Boards { get; set;}

    
    //public DbSet<List> Lists { get; set; }
    //public DbSet<Tasks> Tasks { get; set; }

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