using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarGame.Core.GameLogic;

namespace WarGame.Core.GameEngine
{
    public class PlayerHands
    {
        public Dictionary<Player, Hand> Hands { get; } = new(); 

        public IEnumerable<Player> ActivePlayers =>
            Hands.Where(h => h.Value.Count > 0).Select(h => h.Key);

        public int TotalCards(Player player)
            => Hands[player].Count;

    }
}
