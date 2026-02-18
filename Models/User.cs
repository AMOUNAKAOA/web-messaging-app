using System.ComponentModel.DataAnnotations;

namespace MessageApp.Backend.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        public DateTime JoinedAt { get; set; }

        public User()
        {
            JoinedAt = DateTime.UtcNow;
        }
    }
}