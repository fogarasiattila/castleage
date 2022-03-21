using backend.Persistence;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace webbot.Persistence
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}
