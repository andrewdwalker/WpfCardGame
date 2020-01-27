using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCardGame.Model
{
    public class BlackJackPlayerModel : BlackJackParticipantModel
    {
       public BlackJackPlayerModel(ushort playerNumber) 
        {
           // TODO: Throwing in a constructor seems overly harsh
           // Each id should be unique? Enforce uniqueness at model layer.
            if (playerNumber < 1)
                throw new Exception("Players must have id > 0");
            PlayerNumber = playerNumber;

            FinalPlayerResult = FinalPlayerResultEnum.NoResult;
        }

       
    }
}
