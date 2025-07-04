﻿using Microsoft.AspNetCore.Identity;

namespace ProfileProject.Models
{
    public class PostModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string? Text { get; set; }
        public IdentityUser Author { get; set; }
        public DateTime CreationDateTime { get; set; } = DateTime.UtcNow;
    }
}
