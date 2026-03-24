using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarGame.Core.GameLogic
{
    // A hand held by each player
    public class Hand
    {
        private readonly Queue<Card> _cards = new();

        public int Count => _cards.Count;

        public void AddToBottom(IEnumerable<Card> cards)
        {
            foreach (var card in cards)
                _cards.Enqueue(card);
        }

        public void AddToBottom(Card card)
            => _cards.Enqueue(card);

        public Card DrawTop()
            => _cards.Dequeue();

        public override string ToString()
        {
            return Count == 0 ? "[empty]" : string.Join(", ", _cards);
        }
    }
}
