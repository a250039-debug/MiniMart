using System.ComponentModel.DataAnnotations;

namespace MiniMart.Models
{
    public class Admin
    {
        public int AdminId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}