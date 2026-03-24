using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarGame.Core.GameLogic;

namespace WarGame.Core.GameEngine
{
    public class ManagePot
    {
        private readonly List<Card> _pot = new();

        public IReadOnlyList<Card> Pot => _pot;

        public void Add(Card card)
        {
            _pot.Add(card);
        }

        public void AddRange(IEnumerable<Card> cards)
        {
            _pot.AddRange(cards);
        }

        public void AwardTo(Player winner, PlayerHands playerHands)
        {
            if (_pot.Count == 0) return;

            playerHands.Hands[winner].AddToBottom(_pot);
            _pot.Clear();
        }

        public List<Card> GetPot()
        {
            return _pot;
        }

        public void Clear()
        {
            _pot.Clear();
        }
    }
}