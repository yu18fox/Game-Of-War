using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfWar
{
    public class Program
    {
        static void Main(string[] args)
        {
            var players = new Player[2]
            {
                new Player()
                {
                    PlayerName = "Player 1"
                },
                new Player()
                {
                    PlayerName = "Player 2"
                }
            };

            int playerShuffleInterval = 0;
            var dealer = new Dealer();
            dealer.GetNewDeck();
            dealer.DealCardsToPlayers(players);

            while (!players.Any(a => a.Hand.Count == 0))
            {
                playerShuffleInterval++;
                if (playerShuffleInterval > 20)
                {
                    foreach (var player in players)
                    {
                        player.Hand = dealer.Shuffle(player.Hand);
                    }
                    playerShuffleInterval = 0;
                }
                CalculateWinner(players, null);
                Console.WriteLine("Press a key to battle");
                Console.ReadLine();

            }
            Console.WriteLine(String.Format("The winner is {0}!", players.Single(a => a.Hand.Any()).PlayerName));
            Console.ReadLine();
        }

        public static void CalculateWinner(Player[] players, List<Card> warCards)
        {
            Player player1 = players[0];
            Player player2 = players[1];
            Console.WriteLine(String.Format("Player1 Card Count: {0}", player1.Hand.Count));
            Console.WriteLine(String.Format("Player2 Card Count: {0}", player2.Hand.Count));

            Card nextCardPlayer1 = player1.Hand.Dequeue();
            Card nextCardPlayer2 = player2.Hand.Dequeue();
            Console.WriteLine(String.Format("{0}: {1} {2} | {3}: {4} {5}", player1.PlayerName, nextCardPlayer1.Rank, nextCardPlayer1.Suite, player2.PlayerName, nextCardPlayer2.Rank, nextCardPlayer2.Suite));

            if (nextCardPlayer1.Rank > nextCardPlayer2.Rank)
            {
                //player1 wins this hand
                Console.WriteLine(String.Format("{0} wins this hand", player1.PlayerName));
                player1.Hand.Enqueue(nextCardPlayer1);
                player1.Hand.Enqueue(nextCardPlayer2);
                if (warCards != null)
                {
                    foreach (var card in warCards)
                    {
                        player1.Hand.Enqueue(card);
                    }
                }
            }
            else if (nextCardPlayer1.Rank < nextCardPlayer2.Rank)
            {
                //player2 winds this hand
                Console.WriteLine(String.Format("{0} wins this hand", player2.PlayerName));
                player2.Hand.Enqueue(nextCardPlayer1);
                player2.Hand.Enqueue(nextCardPlayer2);
                if (warCards != null)
                {
                    foreach (var card in warCards)
                    {
                        player2.Hand.Enqueue(card);
                    }
                }
            }
            else
            {
                //They are equal, I declare war
                Console.WriteLine("I DECLARE WAR!");
                if (warCards == null)
                {
                    warCards = new List<Card>() { };
                }
                warCards.Add(nextCardPlayer1);
                warCards.Add(nextCardPlayer2);
                GoToWar(warCards, players);
            }
        }
        public static void GoToWar(List<Card> warCards, Player[] players)
        {
            if (warCards == null)
            {
                warCards = new List<Card>();
            }
            foreach (var p in players)
            {
                for (int p1 = 3; p.Hand.Count > 1 && p1 > 0; p1--)
                {
                    warCards.Add(p.Hand.Dequeue());
                }
            }
            if (players.Any(a => a.Hand.Count == 0))
                {
                throw new Exception("Player had no cards left ");
            }
            CalculateWinner(players, warCards);
        }
    }
}

