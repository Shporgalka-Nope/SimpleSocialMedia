using Microsoft.AspNetCore.Identity;

namespace ProfileProject.Models
{
    public class ProfileViewModel
    {
        public string Username { get; set; }
        public string Bio { get; set; }
        public int Age { get; set; }
        public DateOnly CreationDate { get; set; }
        public string PFPath { get; set; }
    }
}
