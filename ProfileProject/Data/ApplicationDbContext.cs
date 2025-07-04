﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProfileProject.Models;

namespace ProfileProject.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public DbSet<PostModel> Posts { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        //Database.Migrate();
        //Posts.Load();
    }
}