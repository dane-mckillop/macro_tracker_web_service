using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace macro_tracker_web_service.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [JsonIgnore] 
        public string HashPassword { get; set; }

        public int Age { get; set; }

        public float Height { get; set; }

        public float Weight { get; set; }
    }
}