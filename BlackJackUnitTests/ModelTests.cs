using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfCardGame.Model;

namespace BlackJackUnitTests
{
    [TestClass]
    public class ModelTests
    {
        /// <summary>
        /// Verify that a card can be created and that it returns the correct suit and rank
        /// </summary>
        [TestMethod]
        public void CardModelTest()
        {
            CardModel card = new CardModel(CardSuit.Club, CardRank.Ace);
            Assert.AreEqual(CardSuit.Club, card.Suit);
            Assert.AreEqual(CardRank.Ace, card.Rank);
        }

        [TestMethod]
        public void DeckModelIsEmptyOnCreation()
        {
            DeckModel deck = new DeckModel();
            Assert.AreEqual(0, deck.DeckCount);
        }

        [TestMethod]
        public void DeckModel_CountIsCorrect()
        {
            DeckModel deck = new DeckModel();
            deck.PopulateDeck();
            Assert.AreEqual(52, deck.DeckCount);
            deck.DrawRandomCard();
            Assert.AreEqual(51, deck.DeckCount);

            // remove all cards
            for (int i = 0; i < 51; i++)
            {
                deck.DrawRandomCard();
            }

            Assert.AreEqual(0, deck.DeckCount);
            CardModel card = deck.DrawRandomCard();
            Assert.AreEqual(null, card); // there were no more cards to pick, so card should be null
        }

        [TestMethod]
        public void CardDealingMachine_SanityTests()
        {
            CardDealingMachine chute = new CardDealingMachine();
            CardModel card1 = chute.DealCard();
            Assert.AreNotEqual(null, card1);

            CardModel card2 = chute.DealCard();
            Assert.IsTrue(card1.Rank != card2.Rank || card1.Suit != card2.Suit); // we should not get the same card

            // in fact, we should not get the same card in a 52 card deck!
            for (int i = 0; i < 50; i++)
            {
                CardModel card = chute.DealCard();
                Assert.IsTrue(card1.Rank != card.Rank || card1.Suit != card.Suit); // we should not get the same card

            }

            // Currently, we should be able to ask for another card, and the CardDealingMachine should create another deck if necessary
            for (int i=0; i<105;i++)
            {
                CardModel card = chute.DealCard();
                Assert.AreNotEqual(null, card);
            }

            // We shoud be able to ask for multiple cards at once
            List<CardModel> cards = chute.DealCards(500);
            Assert.AreEqual(500, cards.Count);
        }
    }
}
