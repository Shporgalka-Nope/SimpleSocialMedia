using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProfileProject.Models;

namespace ProfileProject.Data;

public class ApplicationDbContext : IdentityDbContext
{
    DbSet<Card> Cards { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Cards.Load();
    }
}
