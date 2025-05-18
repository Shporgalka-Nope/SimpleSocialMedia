using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace ProfileProject.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public int FK_User { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Text { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
