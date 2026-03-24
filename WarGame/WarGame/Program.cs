using System;
using System.Linq;
using System.Collections.Generic;
using WarGame.Core.GameEngine;
using WarGame.Core.GameLogic;
using WarGame.Core.Interfaces;


namespace WarGame.ConsoleApp
{
    public class Program
    {
        static void Main(string[] args)
        {

            // get the number of players
            int playerCount = GetPlayerCount(args);

            // create the players
            var players = Enumerable.Range(1, playerCount).Select(i => new Player($"Player {i}")).ToArray();

            // create and start game
            ICardGame game = new WarGameEngine(players);

            var warGame = (WarGameEngine)game;
            game.StartHand();
            game.PlayHand();
            game.StopHand();

            // print formatted round summaries
            PrintRounds(warGame);

            Console.WriteLine("=== GAME OVER ===");
            Console.WriteLine($"Winner: {warGame.GetWinner()}!");

            static int GetPlayerCount(string[] args)
            {
                if (args.Length > 0 && int.TryParse(args[0], out int count))
                {
                    if (count >= 2 && count <= 4)
                    {
                        return count;
                    }
                }

                int result;
                do
                {
                    Console.Write("Enter the number of players (2-4): ");
                }
                while (!int.TryParse(Console.ReadLine(), out result) || result < 2 || result > 4);

                return result;
            }

            static void PrintRounds(WarGameEngine game)
            {
                int roundNumber = 1;

                foreach (var round in game.RoundHistory)
                {
                    Console.WriteLine($"--- Round {roundNumber} ----");

                    // print each players card for the round.
                    foreach (var kv in round.PlayedCards)
                    {
                        Console.WriteLine($"{kv.Key.Name} played {kv.Value}");
                    }

                    // display tie message
                    if (round.IsTie)
                    {
                        Console.WriteLine("Tie between: " + string.Join(", ", round.TiedPlayers.Select(p => p.Name)));
                    }

                    // display pot contents
                    if (round.PotSnapshot != null && round.PotSnapshot.Any())
                    {
                        Console.WriteLine("Pot Includes: " + string.Join(", ", round.PotSnapshot));
                    }

                    // display tiebreaker cards
                    if (round.TieBreakerCards != null && round.TieBreakerCards.Any())
                    {
                        Console.WriteLine("Tiebreaker: " + string.Join(" | ", round.TieBreakerCards.Select(kv => $"{kv.Key.Name}: {kv.Value}")));
                    }

                    // display the winner and card count
                    if (round.Winner != null)
                    {
                        Console.WriteLine($"Winner: {round.Winner.Name} " + $"(Cards: {string.Join(", ", round.CardCounts.Select(kv => $"{kv.Key.Name} = {kv.Value}"))})");
                    }

                    Console.WriteLine();
                    roundNumber++;
                }
            }
        }
    }
}
