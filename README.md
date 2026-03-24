War Game

	A console-based card game in C# simulating the classic game of War, supporting 2–4 players with automatic round handling, tiebreakers, and a pot system.

Build & Run

	git clone <https://github.com/etsucs-scott/project-2-JaredLeBlanc.git>
	cd WarGame
	dotnet build
	dotnet run --project WarGame.ConsoleApp

OverView

	This project implements a modified version of the card game War, where multiple players compete to collect all cards. The game uses object-oriented design, including classes for Player, Card, Deck, Hand, and WarGameEngine.
  
Key Features

	Supports 2–4 players
	Round-by-round card play with tie resolution and tiebreakers
	Pot system: all played cards are collected into a pot; winner of round collects the entire pot
	Automatic round limit (10,000 rounds) to prevent infinite games
	Console output with round results, ties, tiebreakers, and card counts
  
Gameplay Rules

	Each player starts with an equal share of a standard 52-card deck. Extra cards (if uneven division) go to the first players in order.
	Ranks order: 2 < 3 < 4 < … < 10 < J < Q < K < A. Suits are ignored.
	Each round, players reveal the top card from their hand.
	The highest card wins the round and collects all cards in the pot.
	In case of a tie for the highest card:
	Only tied players play a tiebreaker round.
	All cards from the round remain in the pot.
	Winner of the tiebreaker collects the entire pot.
	Players with no cards are eliminated.
	The game continues until a single player has all cards or the round limit is reached.
	If the round limit (10,000) is reached, the player with the most cards wins. If tied, the game ends in a draw.

Player Selection

	The Program will prompt you to enter the number of players (2-4).
	Default Player names: Player1, Player2, etc.

Example Game

	Enter the number of players (2-4): 3
	--- Round 1 ---
	Player 1 played [K♠]
	Player 2 played [5♦]
	Player 3 played [K♥]
	Tie between Player 1 and Player 3!
	Pot includes: [K♠], [5♦], [K♥]
	Tiebreaker: Player 1: [9♣] | Player 3: [2♦]
	Winner: Player 1 (Cards: P1=26, P2=12, P3=14)
  
Classes and Structures
	
	The main classes include:
	ICardGame: Interface for card games
	Card, Deck, Hand, Player: Core card and player models
	WarGameEngine: Implements game logic, round handling, tiebreakers, and pot management
	RoundResult: Stores results of a round including played cards, tied players, pot, and winner
	PlayerHands, ManagePot, DealCards: Helper classes for managing hands, pot, and dealing cards
	Program: Console application entry point
  
