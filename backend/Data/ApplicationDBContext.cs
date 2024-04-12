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
    public DbSet<Board> Boards { get; set;}
    //Shtohen public DbSet<Entiteti> Entiteti { get; set; } per secilin entitet perveq User
    
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
            },
            new IdentityRole
            {
                Name = "User",
                NormalizedName = "USER"
            },
        };
        builder.Entity<IdentityRole>().HasData(roles); }
    
}