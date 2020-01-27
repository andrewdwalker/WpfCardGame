using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCardGame.Model
{
    public class BlackJackGameModel
    {
        #region fields
        // TODO decide if we really want SortedList... Doesn't seem much point now since each player is going to have an id anyhow.
        private SortedList<ushort, BlackJackPlayerModel> _players = new SortedList<ushort, BlackJackPlayerModel>();
       // private List<BlackJackPlayerModel> _players = new List<BlackJackPlayerModel>();
        private BlackJackDealerModel _dealer = new BlackJackDealerModel();
        private GameStatusEnum _gameStatus = new GameStatusEnum();
       
        private CardDealingMachine _cardChute = new CardDealingMachine();
        #endregion fields

        #region properties
        public GameStatusEnum GameStatus
        {
            get { return _gameStatus; }

        }
        public BlackJackDealerModel Dealer
        {
            get
            {
                // TODO should I return copy of _dealer? data integrity vs. performance....
                return _dealer;
            }
        }

        public List<BlackJackPlayerModel> Players
        {
            get
            {
                // should I return copy of _players?
                return _players.Values.ToList<BlackJackPlayerModel>();
                //return new List<BlackJackPlayerModel>(_players.Values);// _players.ToList<_players.Select(>();
            }
        }
        #endregion properties

        #region public methods
        public BlackJackGameModel(ushort numberOfPlayers)
        {
            // for now, we will demand that players have ids starting at 1...
            // 0 is reserved for dealer
            for (ushort i = 1; i <= numberOfPlayers; i++)
            {
                _players.Add(i, new BlackJackPlayerModel(i));
                
            }
        }

        public bool StartGame()
        {
            bool returnValue = false;
            if (_gameStatus == GameStatusEnum.NotStarted)
            {
                List<CardModel> cards = _cardChute.DealCards(2);
                _dealer.Cards.AddRange(cards);
                foreach (BlackJackPlayerModel player in _players.Values)
                {
                    cards = _cardChute.DealCards(2);
                    player.Cards.AddRange(cards);
                }
                // Note that we have started game
                _gameStatus = GameStatusEnum.InitialCardsDealt;

                // and make first player playable
                _players.FirstOrDefault().Value.IsPlayable = true;
                returnValue = true;
            }
            return returnValue;
        }

        public bool DealDealerCard()
        {

            bool return_value = false;
            if (_gameStatus != GameStatusEnum.DealersTurn)
            {
                // TODO log exception
                throw new Exception("Unexpected GameStatus in DealDealerCard");
            }

            if (_dealer.MustHit == true && _dealer.IsBust == false)
            {
                return_value = true;
                CardModel card = _cardChute.DealCard();
                _dealer.Cards.Add(card);
            }
            // if dealer is done, assign player results and note that the game is over
            if (_dealer.IsBust || !_dealer.MustHit)
            {
                _gameStatus = GameStatusEnum.GameOver;
                AssignPlayerResults();
            }
            return return_value;
        }
        public bool DealPlayerCard(ushort playerNumber)
        {

            if (_players.ContainsKey(playerNumber) == false)
                return false;
            if (_players[playerNumber].IsPlayable == false)
                return false;
            if (_players[playerNumber].IsBust)
                return false;

            _gameStatus = GameStatusEnum.PlayersTurn;
           CardModel card = _cardChute.DealCard();
            _players[playerNumber].Cards.Add(card);

            // if playerNumber is now unplayable, we need to pass on playability to next player
            // player would be unplayable because he/she just bust!
            if (_players[playerNumber].IsPlayable == false)
            {
                int nextKeyIndex = _players.IndexOfKey(playerNumber) + 1;
                if (nextKeyIndex < _players.Keys.Count)
                {
                    ushort nextKey = _players.Keys[nextKeyIndex];
                    _players[nextKey].IsPlayable = true;
                }
                else
                {
                    _gameStatus = GameStatusEnum.DealersTurn;
                    _dealer.IsPlayable = true;
                }
            }
            return true;

        }

        public bool PlayerStands(ushort playerNumber)
        {
            if (_players.ContainsKey(playerNumber) == false)
                return false;
            if (_players[playerNumber].IsPlayable == false)
                return false;
            if (_players[playerNumber].IsBust)
                return false;

            _gameStatus = GameStatusEnum.PlayersTurn;
            // make this player unplayable and unlock the next player(if there is one)
            _players[playerNumber].IsPlayable = false;
            int nextKeyIndex = _players.IndexOfKey(playerNumber) + 1;
            if (nextKeyIndex < _players.Keys.Count)
            {
                ushort nextKey = _players.Keys[nextKeyIndex];
                _players[nextKey].IsPlayable = true;
            }
            else
            {
                _gameStatus = GameStatusEnum.DealersTurn;
                _dealer.IsPlayable = true;
                
            }
            return true;
        }

        #endregion public methods

        #region helper methods

        /// <summary>
        /// Assign the results of the game
        /// TODO #1 Do the results really belong with the Players?
        /// TODO #2 Try to clean up this method.  It's overly messy (too many if/else).
        /// </summary>
        private void AssignPlayerResults()
        {
            foreach (BlackJackPlayerModel player in _players.Values)
            {
                if (player.IsBust)
                {
                    player.FinalPlayerResult = BlackJackPlayerModel.FinalPlayerResultEnum.Busted;
                }
                else if (_dealer.IsBust)
                {
                    player.FinalPlayerResult = BlackJackPlayerModel.FinalPlayerResultEnum.Winner;
                }
                else if (_dealer.Score > player.Score)
                {
                    player.FinalPlayerResult = BlackJackPlayerModel.FinalPlayerResultEnum.Loser;
                }
                else if (_dealer.Score < player.Score)
                {
                    player.FinalPlayerResult = BlackJackPlayerModel.FinalPlayerResultEnum.Winner;
                }
                else if (_dealer.Score == player.Score)
                {
                    player.FinalPlayerResult = BlackJackPlayerModel.FinalPlayerResultEnum.Pusher;
                }
                else
                {
                    // we have problems!  log a message and/or throw.  There is logic we missed
                    throw new Exception("Exception in AssignPlayerResults()");
                }

            }
        }
        #endregion 
    }

    public enum GameStatusEnum
    {
        NotStarted  = 0,
        InitialCardsDealt = 1,
        PlayersTurn = 2,
        DealersTurn = 3,
        GameOver =4
    }
    
}
