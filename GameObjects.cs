using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfWar
{

    //Enums
    public enum Suite
    {
        DIAMONDS, CLUBS, HEARTS, SPADES
    }

    public enum Rank
    {
        TWO = 2, THREE = 3, FOUR = 4, FIVE = 5, SIX = 6, SEVEN = 7, EIGHT = 8, NINE = 9, TEN = 10, JACK = 11, QUEEN = 12, KING = 13, ACE = 14
    }

    public static class Extensions
    {
        public static T GetEnumValue<T>(string str) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }
            T val = ((T[])Enum.GetValues(typeof(T)))[0];
            if (!string.IsNullOrEmpty(str))
            {
                foreach (T enumValue in (T[])Enum.GetValues(typeof(T)))
                {
                    if (enumValue.ToString().ToUpper().Equals(str.ToUpper()))
                    {
                        val = enumValue;
                        break;
                    }
                }
            }

            return val;
        }

        public static T GetEnumValue<T>(int intValue) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }
            T val = ((T[])Enum.GetValues(typeof(T)))[0];

            foreach (T enumValue in (T[])Enum.GetValues(typeof(T)))
            {
                if (Convert.ToInt32(enumValue).Equals(intValue))
                {
                    val = enumValue;
                    break;
                }
            }
            return val;
        }

    }
    //Domain Objects
    public class Card
    {
        public Rank Rank { get; set; }
        public Suite Suite { get; set; }
    }
    public class Dealer
    {
        private Stack<Card> Deck;
        public Dealer()
        {
            GetNewDeck();
        }

        public void GetNewDeck()
        {
            Deck = new Stack<Card>();
            for (int suite = 1; suite < 5; suite++)
            {
                for (int rank = 2; rank < 15; rank++)
                {
                    Deck.Push(new Card()
                    {
                        Rank = Extensions.GetEnumValue<Rank>(rank),
                        Suite = Extensions.GetEnumValue<Suite>(suite)
                    });
                }
            }
        }

        public void DealCardsToPlayers(Player[] players)
        {
            Deck = Shuffle(Deck);
            while (Deck.Any())
            {
                foreach (var player in players)
                {
                    if (Deck.Count > 0)
                    {
                        player.Hand.Enqueue(Deck.Pop());
                    }
                }
            }
        }
        public Queue<Card> Shuffle(Queue<Card> deck)
        {
            Stack<Card> shuffledStack = Shuffle(new Stack<Card>(deck.ToList()));
            return new Queue<Card>(shuffledStack.ToList());            
        }
        public Stack<Card> Shuffle(Stack<Card> deck)
        {
            var random = new Random();
            int numberOfTimesToShuffle = random.Next(10, 20);

            for (; numberOfTimesToShuffle != 0; numberOfTimesToShuffle--)
            {
                //split the deck into 2 halfs
                Stack<Card> LeftDeck = new Stack<Card>();
                Stack<Card> RightDeck = new Stack<Card>();
                int halfDeckSize = (int)(deck.Count / 2);

                for (int c = 0; c <= halfDeckSize; c++)
                {
                    if (c < halfDeckSize)
                    {
                        LeftDeck.Push(deck.Pop());
                    }                   
                }
                for(int r = deck.Count; r!= 0; r--)
                {
                    RightDeck.Push(deck.Pop());
                }

                while (LeftDeck.Any() || RightDeck.Any())
                {
                    int randomNumberLeft = random.Next(1, 4);
                    int randomNumberRight = random.Next(1, 4);
                    for (; randomNumberLeft != 0; randomNumberLeft--)
                    {
                        if (LeftDeck.Any())
                        {
                            deck.Push(LeftDeck.Pop());
                        }
                    }
                    for(; randomNumberRight != 0; randomNumberRight--)
                    {
                        if(RightDeck.Any())
                        {
                            deck.Push(RightDeck.Pop());
                        }
                    }
                }
            }

            return deck;
        }

        public void PrintDeckToScreen()
        {
            Deck = Shuffle(Deck);
            foreach(var card in Deck)
            {
                Console.Write(String.Format("{0} {1} ", card.Rank, card.Suite));
            }
        }
    }
    public class Player
    {
        public Player()
        {
            Hand = new Queue<Card>();
        }
        public string PlayerName { get; set; }
        public Queue<Card> Hand { get; set; }

    }
}
