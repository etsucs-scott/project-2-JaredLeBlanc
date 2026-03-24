using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarGame.Core.GameLogic
{
    // class for each player in the game.
    public class Player
    {
        public string Name { get; }

        // gets players hand
        public Hand Hand { get; }

        // creates player with a new hand
        public Player (string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Player Name cannot be empty");
            }

            Name = name;
            Hand = new Hand();
        }

        public override string ToString()
        {
            return $"{Name}: | {Hand}";
        }
    }
}
