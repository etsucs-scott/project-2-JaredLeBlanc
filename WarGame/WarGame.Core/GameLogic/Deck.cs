using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarGame.Core.GameLogic
{
    public class Deck
    {
        private Stack<Card> _cards;
        private static readonly Random _random = new();

        public Deck()
        {
            var cards = new List<Card>();

            foreach(Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach(Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    cards.Add(new Card(suit, rank));
                }
            }

            Shuffle(cards);
            _cards = new Stack<Card>(cards);
        }

        private void Shuffle(List<Card> cards)
        {
            for(int i = cards.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                (cards[i], cards[j]) = (cards[j], cards[i]);
            }
        }

        public bool HasCards => _cards.Count > 0;
        
        public Card Draw() => _cards.Pop();
    }
}
