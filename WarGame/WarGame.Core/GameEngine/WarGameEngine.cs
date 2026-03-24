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
        /*
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
        */
        public void StartHand()
        {
            Deck = new Deck();
            _cardDealer.Deal(Deck, _playerHands);
        }

        public void StopHand()
        {
            // not needed.
        }

        public RoundResult? PlayNextRound()
        {
            if (_playerHands.ActivePlayers.Count() <= 1 || RoundHistory.Count >= RoundLimit)
            {
                return null;
            }

            var result = PlayRound(_playerHands.ActivePlayers.ToList());
            RoundHistory.Add(result);
            return result;
        }

        private RoundResult PlayRound(List<Player> players)
        {
            var result = new RoundResult();
            var played = new Dictionary<Player, Card>();

            players = players.Where(p => _playerHands.Hands[p].Count > 0).ToList();

            foreach(var player in players)
            {
                var card = _playerHands.Hands[player].DrawTop();
                played[player] = card;
                _potManager.Add(card);
            }

            var PlayedCard = new Dictionary<Player, Card>(played);

            var maxRank = played.Values.Max(c => c.Rank);

            var winners = played.Where(p => p.Value.Rank == maxRank).Select(p => p.Key).ToList();

            if (winners.Count == 1)
            {
                // for single winner
                result.Winner = winners[0];
                _potManager.AwardTo(winners[0], _playerHands);
            }
            else
            {
                // when tie is detected
                result.TiedPlayers = winners;
                ResolveTie(winners, result);
            }

            // update the card counts after the round
            result.CardCounts = _playerHands.Hands.ToDictionary(h => h.Key, h => h.Value.Count);

            return result;
        }

        private void ResolveTie(List<Player> tiedPlayers, RoundResult result)
        {
            var active = tiedPlayers;

            while (true)
            {
                var played = new Dictionary<Player, Card>();

                foreach (var player in active.ToList())
                {
                    if (_playerHands.Hands[player].Count == 0)
                    {
                        active.Remove(player);
                        continue;
                    }

                    var card = _playerHands.Hands[player].DrawTop();
                    played[player] = card;
                    _potManager.Add(card);
                }

                if (!played.Any())
                {
                    return;
                }

                foreach (var kvp in played)
                {
                    result.PlayedCards[kvp.Key] = kvp.Value;
                }

                if (active.Count == 1)
                {
                    result.Winner = active[0];
                    _potManager.AwardTo(active[0], _playerHands);
                    return;
                }

                var maxRank = played.Values.Max(c => c.Rank);

                active = played.Where(p => p.Value.Rank == maxRank).Select(p => p.Key).ToList();
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
