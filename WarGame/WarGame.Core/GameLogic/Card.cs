using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace WarGame.Core.GameLogic
{
    // Single card with its rank and suit
    public class Card
    {
        // get suit of card
        public Suit Suit { get; }
        // get rank of card
        public Rank Rank { get; }

        // creates a new card.
        public Card(Suit suit, Rank rank)
        {
            Suit = suit;
            Rank = rank;
        }

        private static string GetRankSymbol(Rank rank)
        {
            return rank switch
            {
                Rank.Ace => "A",
                Rank.King => "K",
                Rank.Queen => "Q",
                Rank.Jack => "J",
                _ => ((int)rank).ToString()
            };
        }

        private static char GetSuitSymbol(Suit suit)
        {
            return suit switch
            {
                Suit.Clubs => '♣',
                Suit.Diamonds => '♦',
                Suit.Hearts => '♥',
                Suit.Spades => '♠',
                _ => '?'
            };
        }

        public override string ToString()
        {
            return $"[{GetRankSymbol(Rank)}{GetSuitSymbol(Suit)}]";
        }
    }
}
