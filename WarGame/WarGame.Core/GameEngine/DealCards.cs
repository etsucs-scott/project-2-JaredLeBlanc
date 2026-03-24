using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarGame.Core.GameLogic;

namespace WarGame.Core.GameEngine
{
    public class DealCards
    {
        public void Deal(Deck deck, PlayerHands playerHands)
        {
            var players = playerHands.Hands.Keys.ToList();

            int index = 0;

            while (deck.HasCards)
            {
                var card = deck.Draw();
                var player = players[index % players.Count];

                playerHands.Hands[player].AddToBottom(card);

                index++;
            }
        }

    }
}
