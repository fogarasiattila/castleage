using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webbot.Models
{
    public class ColosseumBattleModel
    {
        public string BattleId { get; set; }
        public string Bqh { get; set; }
        public List<OpponentModel> Opponents { get; set; }
    }
}
