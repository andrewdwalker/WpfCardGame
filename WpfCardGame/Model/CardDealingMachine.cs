using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCardGame.Model
{
    public class CardDealingMachine
    {
        private DeckModel _deck;

        /// <summary>
        /// Deal a random card from the Deck.
        /// Create new deck if necessary
        /// </summary>
        /// <returns>PlayingCardModel</returns>
        public CardModel DealCard()
        {
            if (_deck == null)
            {
                _deck = new DeckModel();

            }
            if (_deck.DeckCount == 0)
            {
                _deck.PopulateDeck();
            }
            return _deck.DrawRandomCard();

        }

        // TODO we should have a constraint stating that numberOfCards > 0
        /// <summary>
        /// Deal n number of cards from deck
        /// </summary>
        /// <param name="numberOfCards"></param>
        /// <returns></returns>
        public List<CardModel> DealCards(ushort numberOfCards)
        {
            
            List<CardModel> cards = new List<CardModel>();
            for (ushort i = 0; i < numberOfCards; i++)
            {
                CardModel card = DealCard();
                cards.Add(card);
            }
            return cards;
        }
    }
}
