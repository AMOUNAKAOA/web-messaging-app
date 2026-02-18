using System.ComponentModel.DataAnnotations;

namespace MessageApp.Backend.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Username { get; set; }

        public DateTime Timestamp { get; set; }

        public Message()
        {
            Timestamp = DateTime.UtcNow;
        }
    }
}