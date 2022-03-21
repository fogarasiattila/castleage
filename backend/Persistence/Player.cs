using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using webbot.Persistence;

namespace backend.Persistence
{
    public class Player
    {
        public int Id { get; set; }
        public string Displayname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailPassword { get; set; }
        public string PlayerCode { get; set; }
        public string ArmyCode { get; set; }
        public string Cookie { get; set; }
        [NotMapped]
        public bool InBattle { get; set; }
        public ICollection<Group> Groups { get; set; }
    }
}