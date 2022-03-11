using System.ComponentModel.DataAnnotations;
using webbot.Enums;

namespace backend.Persistence
{
    public class Settings
    {
        [Key]
        [Required]
        public string Name { get; set; }
        [Required]
        public State State { get; set; }
    }
}
