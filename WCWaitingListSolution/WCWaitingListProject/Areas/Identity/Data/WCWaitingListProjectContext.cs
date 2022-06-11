using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WCWaitingListProject.Models;

namespace WCWaitingListSolution.Data;

public class WCWaitingListProjectContext : IdentityDbContext<AppUser>
{
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<WCWaitingListProject.Models.Appointment> Appointment { get; set; }
    public WCWaitingListProjectContext(DbContextOptions<WCWaitingListProjectContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
