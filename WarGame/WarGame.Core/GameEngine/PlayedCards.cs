using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarGame.Core.GameLogic;

namespace WarGame.Core.GameEngine
{
    public class PlayedCards
    {
        public Dictionary<string, Card> Cards { get; } = new();

        public void Add(string player, Card card)
            => Cards[player] = card;

        public void Clear() => Cards.Clear();
    }
}
