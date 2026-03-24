using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using WarGame.Core.GameLogic;
using WarGame.Core.Interfaces;


namespace WarGame.Core.GameEngine
{
    public class WarGameEngine : ICardGame
    {
        public string Name => "War";

        public Deck Deck { get; set; }

        public Player[] Players { get; }

        private readonly PlayerHands _playerHands = new();
        private readonly ManagePot _potManager = new();
        private readonly DealCards _cardDealer = new();

        private const int RoundLimit = 10000;

        public List<RoundResult> RoundHistory { get; } = new();

        public WarGameEngine(IEnumerable<Player> players)
        {
            Players = players.ToArray();

            foreach(var player in Players)
            {
                _playerHands.Hands[player] = new Hand();
            }
        }
        
        public void PlayHand()
        {
            int round = 0;

            while(_playerHands.ActivePlayers.Count() > 1 && round < RoundLimit)
            {
                round++;

                var result = PlayRound(_playerHands.ActivePlayers.ToList());

                RoundHistory.Add(result);
            }
        }
        
        public void StartHand()
        {
            Deck = new Deck();
            _cardDealer.Deal(Deck, _playerHands);
        }

        public void StopHand()
        {
            // not needed.
        }
        
        private RoundResult PlayRound(List<Player> players)
        {
            var result = new RoundResult();
            var played = new Dictionary<Player, Card>();
            var pot = new List<Card>();

            players = players.Where(p => _playerHands.Hands[p].Count > 0).ToList();

            foreach(var player in players)
            {
                var card = _playerHands.Hands[player].DrawTop();
                played[player] = card;
                pot.Add(card);
            }

            result.PotSnapshot = new List<Card>(pot);

            result.PlayedCards = new Dictionary<Player, Card>(played);

            var maxRank = played.Values.Max(c => c.Rank);
            var winners = played.Where(p => p.Value.Rank == maxRank).Select(p => p.Key).ToList();

            if (winners.Count == 1)
            {
                // for single winner
                result.Winner = winners[0];

                result.PotSnapshot = new List<Card>(pot);

                foreach (var card in pot)
                {
                    _playerHands.Hands[winners[0]].AddToBottom(card);
                }
            }
            else
            {
                // when tie is detected
                result.TiedPlayers = winners;
                ResolveTie(winners, result, pot);
            }

            // update the card counts after the round
            result.CardCounts = _playerHands.Hands.ToDictionary(h => h.Key, h => h.Value.Count);

            return result;
        }

        private void ResolveTie(List<Player> tiedPlayers, RoundResult result, List<Card> pot)
        {
            var active = new List<Player>(tiedPlayers);
            var tiebreakPlayed = new Dictionary<Player, Card>();

            while (true)
            {
                tiebreakPlayed.Clear();

                foreach (var player in active.ToList())
                {
                    if (_playerHands.Hands[player].Count == 0)
                    {
                        active.Remove(player);
                        continue;
                    }

                    var card = _playerHands.Hands[player].DrawTop();
                    tiebreakPlayed[player] = card;
                    pot.Add(card);
                }

                foreach (var kvp in tiebreakPlayed)
                {
                    result.PlayedCards[kvp.Key] = kvp.Value;
                }

                result.TieBreakerCards = new Dictionary<Player, Card>(tiebreakPlayed);
                result.PotSnapshot = new List<Card>(pot);

                if (active.Count == 1)
                {

                    result.Winner = active[0];

                    foreach (var card in pot)
                    {
                        _playerHands.Hands[active[0]].AddToBottom(card);
                    }

                    return;
                }

                var maxRank = tiebreakPlayed.Values.Max(c => c.Rank);

                active = tiebreakPlayed.Where(p => p.Value.Rank == maxRank).Select(p => p.Key).ToList();

                if (active.Count == 0)
                {
                    return;
                }
            }
        }

        public string GetWinner()
        {
            var active = _playerHands.ActivePlayers.ToList();

            if (active.Count == 1)
            {
                return active[0].Name;
            }

            var max = _playerHands.Hands.Max(h => h.Value.Count);

            var leaders = _playerHands.Hands.Where(h => h.Value.Count == max).Select(h => h.Key).ToList();

            return leaders.Count == 1 ? leaders[0].Name : "Draw";
        }
    }
}
