using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using webbot.Persistence;

namespace webbot.Models
{
    public class PlayerDto
    {
        public int Id { get; set; }
        public string Displayname { get; set; }
        public string Username { get; set; }
        public string PlayerCode { get; set; }
        public string ArmyCode { get; set; }
        public bool HasCookie { get; set; }
        public List<int> MemberOf { get; set; }
        public bool Touched { get; set; }
    }
}
