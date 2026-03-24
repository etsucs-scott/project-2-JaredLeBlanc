using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarGame.Core.GameLogic;

namespace WarGame.Core.GameEngine
{
    public class RoundResult
    {
        public Player? Winner { get; set; }

        public List<Player> TiedPlayers { get; set; } = new List<Player>();

        public Dictionary<Player, Card> PlayedCards { get; set; } = new();

        public Dictionary<Player, int> CardCounts { get; set; } = new();

        public List<Card> PotSnapshot { get; set; } = new List<Card>();

        public Dictionary<Player, Card>? TieBreakerCards { get; set; } = null;

        public bool IsTie => TiedPlayers.Count() > 1;
    }
}
