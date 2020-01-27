using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCardGame.Model
{
    public class BlackJackDealerModel : BlackJackParticipantModel
    {
        public bool MustHit
        {
            get
            {
                return Score < 17;
            }
        }
    }
}
