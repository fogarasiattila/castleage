using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webbot.Models
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string PlayerCode { get; set; }
        public string ArmyCode { get; set; }
        public bool HasCookie { get; set; }
    }
}
