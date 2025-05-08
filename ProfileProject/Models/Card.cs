using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProfileProject.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string? Bio { get; set; }
        public IdentityUser FK_User { get; set; }
        public string PFPath { get; set; }
    }
}
