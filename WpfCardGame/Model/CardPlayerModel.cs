using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCardGame.Model
{
    public abstract class CardPlayerModel
    {
        List<CardModel> _cards;
        public List<CardModel> Cards
        {
            get
            {
                if (_cards == null)
                {
                    _cards = new List<CardModel>();
                }
                return _cards;
            }
            set
            {
                _cards = value;
            }
        }
       
    }
}
