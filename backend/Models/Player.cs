using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Persistence
{
    public class Player
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailPassword { get; set; }
        public string PlayerCode { get; set; }
        public string ArmyCode { get; set; }
        public string Cookie { get; set; }
        [NotMapped]
        public bool InBattle { get; set; }
    }
}