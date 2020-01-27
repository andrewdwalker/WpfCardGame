using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCardGame.Model
{
    public enum CardSuit
    {
        Heart = 0,
        Diamond = 1,
        Spade = 2,
        Club = 3,
    } ;

    public enum CardRank
    {
        Ace = 1,
       Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,

    };
    public class CardModel
    {
        public CardModel( CardSuit suit, CardRank rank)
        {
            Suit = suit;
            Rank = rank;
        }
        // State Properties
        public CardSuit Suit { get; set; }
        public CardRank Rank { get; set; }

    }
}
