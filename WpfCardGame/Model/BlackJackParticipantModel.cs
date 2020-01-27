using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCardGame.Model
{
    abstract public class BlackJackParticipantModel : CardPlayerModel
    {
        public BlackJackParticipantModel(ushort playerNumber = 0)
        {
            IsPlayable = false;
            PlayerNumber = playerNumber;
        }

        public ushort PlayerNumber { get; protected set; }
        
        public bool IsBust
        {
            get
            {
                return CalculateScore(Cards) > 21;
            }
        }
        public ushort Score
        {
            get
            {
                return CalculateScore(Cards);
            }
        }
        private bool _isPlayable = false;
        public bool IsPlayable
        {
            get
            {
                if (IsBust)
                {
                    _isPlayable = false;
                }
                return _isPlayable;
            }
            set
            {
                _isPlayable = value;
            }
        }

        public enum FinalPlayerResultEnum
        {
            NoResult = 0,
            Winner = 1,
            Loser = 2,
            Pusher = 3,
            Busted = 4,

        }

        public FinalPlayerResultEnum FinalPlayerResult { get; internal set; }

        #region Helper Methods
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
        #endregion Helper Methods
    }
}
