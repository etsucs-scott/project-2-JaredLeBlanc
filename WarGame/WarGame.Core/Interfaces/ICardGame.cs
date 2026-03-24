using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarGame.Core.GameLogic;

namespace WarGame.Core.Interfaces
{
    // interface that defines what a card game should have
    public interface ICardGame
    {
        // Get name of card game
        string Name { get; }

        // Get deck used by card game
        Deck Deck { get; }

        // Gets players in the card game
        Player[] Players { get; }

        void StartHand();

        void PlayHand();

        void StopHand();
    }
}
