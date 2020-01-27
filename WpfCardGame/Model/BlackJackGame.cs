using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCardGame.Model
{
    public class BlackJackGame
    {
        #region Fields
        private CardDealingMachine _delear = new CardDealingMachine();
        private List<CardModel> _playerCards = new List<CardModel>();
        private List<CardModel> _dealerCards = new List<CardModel>();
        #endregion Fields
        public BlackJackGame()
        {
            PlayerHittable = DealerHittable = true;
        }

        #region Properties
        ushort PlayerScore
        {
            get
            {
                return CalculateScore(_playerCards);
               

            }
        }
        ushort DealerScore {
            get
            {
                return CalculateScore(_dealerCards);
            }
        }

        List<CardModel> PlayerCards {
            get
            {
                return new List<CardModel>(_playerCards);
            }

           
        }
        List<CardModel> DealerCards {
            get
            {
                return new List<CardModel>(_dealerCards);
            }
        }
        bool PlayerHittable { get; private set; }
        bool DealerHittable { get; private set; }
       
        //ushort NumberOfCardsDealtToPlayer
        #endregion Properties

        private void StartGame()
        {
            _playerCards.AddRange(_delear.DealCards(2));
            _dealerCards.AddRange(_delear.DealCards(2));
        }

        private ushort CalculateScore(List<CardModel> playingCards)
        {
            ushort aceCount = 0;
            ushort score = 0;
            foreach (CardModel card in playingCards)
            {
                switch (card.Rank)
                {
                    case CardRank.Ace:
                        aceCount++;
                        break;
                    case CardRank.Jack:
                    case CardRank.Queen:
                    case CardRank.King:
                        score += 10;
                        break;
                    default:
                        score += (ushort)card.Rank;
                        break;

                }
            }
            if (aceCount == 0)// no aces to consider
                return score;
            else
            {
                // it is impossible (until we do splits!) to have two aces at 11 points each,
                // so we check to see if we can have one ace at 11 and the rest at 1. If not, we count them all 
                // as ones
                ushort possibility = (ushort)(score + (ushort)(11 + 1 * (aceCount - 1)));
                if (possibility > 21)
                {
                    score += aceCount;
                }
                else
                {
                    score = possibility;
                }
                return score;
            }
        }
    }
}
