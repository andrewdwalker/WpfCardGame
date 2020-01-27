using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfCardGame.ViewModel;

namespace BlackJackUnitTests
{
    [TestClass]
    public class ModelViewTests
    {
        [TestMethod]
        public void CreateViewModelWithNumberOfPlayersTest()
        {
            CardGameViewModel target = new CardGameViewModel();
            target.NumberOfPlayersCommand.Execute(1);
            target.StartCommand.Execute(null);
            Assert.AreEqual(1, target.PlayersInfo.Count);

            target = new CardGameViewModel();
            target.NumberOfPlayersCommand.Execute(3);
            target.StartCommand.Execute(null);
            Assert.AreEqual(3, target.PlayersInfo.Count);

        }

        [TestMethod]
        public void SimpleSanityTest()
        {
            CardGameViewModel target = new CardGameViewModel();
            target.NumberOfPlayersCommand.Execute(1);
            target.StartCommand.Execute(null);
            Assert.AreEqual(target.PlayersInfo.Count, 1, "Wrong number of players");
            Assert.AreEqual(target.GameOver, false, "Game should not be over at this point");
            // Ensure player and dealer have two cards each
            Assert.AreEqual(target.PlayersInfo[0].Player.Cards.Count, 2, "Wrong card count for player");
            Assert.AreEqual(target.DealerInfo.Player.Cards.Count,2, "Wrong card count for dealer");

           System.Diagnostics.Debug.WriteLine("Dealer Score: " + target.DealerInfo.Player.Score);
           System.Diagnostics.Debug.WriteLine("Player Score: " + target.PlayersInfo[0].Player.Score);

           while (target.PlayersInfo[0].Player.Score < 17)
            {
                target.DrawCardCommand.Execute(1);
                System.Diagnostics.Debug.WriteLine("Player Score: " + target.PlayersInfo[0].Player.Score);
            }

           Assert.IsTrue((!(target.PlayersInfo[0].Player.Score > 21) && !target.PlayersInfo[0].Player.IsBust) || (target.PlayersInfo[0].Player.Score > 21 && target.PlayersInfo[0].Player.IsBust), "Score and PlayerBust do not match");

           if (target.PlayersInfo[0].Player.Score > 21)
            {
                Assert.AreEqual( true, target.GameOver,"Game should  be over at this point as player has > 21");
            }
            else
            {
                Assert.IsTrue((target.DealerInfo.Player.Score < 17 && (target.DealerInfo.Player as WpfCardGame.Model.BlackJackDealerModel).MustHit == true) || (target.DealerInfo.Player.Score >= 17 && (target.DealerInfo.Player as WpfCardGame.Model.BlackJackDealerModel).MustHit == false), "DealerScore and DealerMustHit do not match");
                
                target.StandCommand.Execute(1);
                Assert.AreEqual(target.GameOver, true, "Game should  be over at this point as dealer has taken cards");
                System.Diagnostics.Debug.WriteLine("Dealer data. Score : " + target.DealerInfo.Player.Score
                    + " Dealer Bust: " + target.DealerInfo.Player.IsBust
                    + " Dealer must hit" + (target.DealerInfo.Player as WpfCardGame.Model.BlackJackDealerModel).MustHit);
            }
        }
    }
}
