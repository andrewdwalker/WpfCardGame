using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCardGame.Model
{
    public class DeckModel
    {
        private List<CardModel> _deck;
        Random _rand = new Random();
        public void PopulateDeck()
        {
            // first remove any old cards
            if (_deck != null)
            {
                _deck.Clear();
            }

            _deck = new List<CardModel>(52);
           
            foreach (CardRank rank in Enum.GetValues(typeof(CardRank)))
            {
                foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
                {
                    CardModel card = new CardModel(suit, rank);
                    _deck.Add(card);
                }
            }




        }

        public CardModel DrawRandomCard()
        {
            CardModel card = null;
            if (_deck != null && _deck.Count > 0)
            {
                
                int index = _rand.Next(_deck.Count);
                card = _deck[index];
                _deck.RemoveAt(index);
               
            }
            return  card;
        }

        // Properties
        public int DeckCount
        {
            get
            {
                return _deck == null ? 0 : _deck.Count;
            }
        }
    }
}
