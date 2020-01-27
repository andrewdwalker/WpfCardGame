using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WpfCardGame.Model;

namespace WpfCardGame.ViewModel
{
    public class CardGameViewModel : ViewModelBase
    {

        public CardGameViewModel()
        {
            //NumberOfPlayers = NumberOfPlayersEnum.Three;

            NumberOfPlayersCollection = new ObservableCollection<NumberOfPlayersClass>();
            foreach (NumberOfPlayersEnum value in Enum.GetValues(typeof(NumberOfPlayersEnum)))
            {
                NumberOfPlayersClass myClass = new NumberOfPlayersClass();
                myClass.Player = value;
                myClass.IsChecked = value == NumberOfPlayersEnum.Two ? true : false; // default to using 2 players
                myClass.Title = Enum.GetName(typeof(NumberOfPlayersEnum), value);
                NumberOfPlayersCollection.Add(myClass);
            }

        }

        #region Fields

        private BlackJackGameModel _game;
       private ICommand _drawCardCommand;
        private ICommand _startCommand;
        private ICommand _standCommand;
        private ICommand _numberOfPlayersCommand;
       //private ObservableCollection<NumberOfPlayersClass> _numberOfPlayersCollection;
        #endregion Fields

        #region Public Properties/Commands


        public ObservableCollection<NumberOfPlayersClass> NumberOfPlayersCollection { get; private set; }

        
        public ICommand NumberOfPlayersCommand
        {
            get
            {
                if (_numberOfPlayersCommand == null)
                {
                    _numberOfPlayersCommand = new RelayCommand(new Action<object>(ResolveCheckBoxes));

                }
                return _numberOfPlayersCommand;
            }
        }
        public ICommand StandCommand
        {
            get
            {
                if (_standCommand == null)
                {
                    _standCommand = new RelayCommand(new Action<object>(PlayerStands));
                       
                }
                return _standCommand;
            }
        }
         
        public ICommand StartCommand
        {
            get
            {
                if (_startCommand == null)
                {
                    _startCommand = new RelayCommand(
                   param => StartGame()
                      
                    );
                }
                return _startCommand;
            }

            
        }

        public ICommand DrawCardCommand
        {
            get
            {
                if (_drawCardCommand == null)
                {
                    _drawCardCommand =new RelayCommand(new Action<object>(GetPlayerCard));       
                }
                return _drawCardCommand;
            }
        }

        public bool GameOver { 
            
            get
            {
                if (_game == null)
                    return true;
                else
                    return _game.GameStatus == GameStatusEnum.GameOver;
            }
        }
        public bool GameInProgress
        {
            get
            {
                return !GameOver;

            }
        }

        public bool CanShowDealerScore
        {
            get
            {
                if (_game != null)
                    return (_game.GameStatus == GameStatusEnum.DealersTurn || _game.GameStatus == GameStatusEnum.GameOver);
                else
                    return false;
            }
        }

        public ObservableCollection<PlayerInfo> PlayersInfo
        {
            get
            {
                ObservableCollection<PlayerInfo> results = new ObservableCollection<PlayerInfo>();
                if (_game != null)
                {
                    foreach (BlackJackPlayerModel player in _game.Players)
                    {
                        PlayerInfo info = new PlayerInfo(player);
                        foreach (var card in player.Cards)
                        {
                            info.CardImages.Add(LoadImage(@"Images\\" + GetCardFileName(card)));
                            
                        }
                        
                        results.Add(info);
                    }
                    return results;
                }
                else
                {
                    return null;
                }
            }
        }


        public PlayerInfo DealerInfo
        {
            get
            {
                if (_game != null)
                {
                    PlayerInfo info = new PlayerInfo(_game.Dealer);
                
                    // for dealer, second card should be down .... unless it's his turn.
                    for (int i = 0; i < _game.Dealer.Cards.Count; i++ )
                    {
                        if (i == 1 && _game.GameStatus != GameStatusEnum.DealersTurn && _game.GameStatus != GameStatusEnum.GameOver)
                        {
                            info.CardImages.Add(LoadImage(@"Images\\" + @"card-back.png"));
                        }
                        else
                        {
                            info.CardImages.Add(LoadImage(@"Images\\" + GetCardFileName(_game.Dealer.Cards[i])));
                        }
                    }
                    
                    
                    return info;
                }
                else
                {
                    return null;
                }
            }
        }
       
        
        #endregion Public Properties/Command
        #region Helper Methods

        private void PlayerStands(object playerNumber)
        {
            _game.PlayerStands(Convert.ToUInt16(playerNumber));
            OnPropertyChanged("PlayersInfo");

            // if there is no more player action (players are all bust or standed), dealer goes...
            if (_game.GameStatus == GameStatusEnum.DealersTurn)
            {
                do // using a do/while forces the dealer to acknowledge he's done even if he needs no cards
                {
                    _game.DealDealerCard();
                } while (_game.Dealer.MustHit);
                OnPropertyChanged("DealerInfo");
                // Game is over at this point, force an update of GameOver property, which will force messages to appear on GUI
                OnPropertyChanged("GameOver");
                OnPropertyChanged("CanShowDealerScore");
                // 
            }

        }
        private void GetPlayerCard(object playerNumber)
        {
            if (playerNumber == null)
            {
                throw new Exception("playerNumber must be valid ushort in GetPlayerCard");
            }
            _game.DealPlayerCard(Convert.ToUInt16(playerNumber));
            OnPropertyChanged("PlayersInfo");

            // if there is no more player action (players are all bust or standed), dealer goes...
            if (_game.GameStatus == GameStatusEnum.DealersTurn)
            {
                do // using a do/while forces the dealer to acknowledge he's done even if he needs no cards
                {
                    _game.DealDealerCard();
                } while (_game.Dealer.MustHit);
                OnPropertyChanged("DealerInfo");
                // Game is over at this point, force an update of GameOver property, which will force messages to appear on GUI
                OnPropertyChanged("GameOver");
                OnPropertyChanged("CanShowDealerScore");
            }
        }
        
        
        private void StartGame()
        {

            NumberOfPlayersClass arg = NumberOfPlayersCollection.First<NumberOfPlayersClass>(t => t.IsChecked == true);
            if (arg == null)
            {
                // BIG problem.  Log and find out why
                throw new Exception("Could not resolve number of players in StartGame");
            }
            _game = new BlackJackGameModel(Convert.ToUInt16(arg.Player));
            _game.StartGame();
            OnPropertyChanged("DealerInfo");
            OnPropertyChanged("PlayersInfo");
            OnPropertyChanged("CanShowDealerScore");
        }


        private string GetCardFileName(CardModel card)
        {
            // actually get correct card
            string rank = "";
            string suit = "";

            switch (card.Rank)
            {
                case CardRank.Jack:
                    rank = "11";
                    break;
                case CardRank.Queen:
                    rank = "12";
                    break;
                case CardRank.King:
                    rank = "13";
                    break;
                default:
                    rank = ((int) card.Rank).ToString();//card.Rank.ToString();//Enum.GetName(typeof(CardRank), card.Rank);
                    break;

            }
            suit = Enum.GetName(typeof(CardSuit), card.Suit);
            suit = suit.ToLower();

            //diamond-1.png
            return suit + @"-" + rank + @".png";

        }

        private ImageSource LoadImage(string fileName)
        {
            var image = new BitmapImage();

            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
            }

            return image;
        }

        private void ResolveCheckBoxes(object checkBoxNumber)
        {

            NumberOfPlayersEnum myEnum = (NumberOfPlayersEnum)checkBoxNumber;
            NumberOfPlayersClass theClass = NumberOfPlayersCollection.First<NumberOfPlayersClass>(t => t.Player == myEnum);

            // ok, they want to check this one, let them and uncheck all else
            foreach (NumberOfPlayersClass iter in NumberOfPlayersCollection)
            {
                iter.IsChecked = false;
            }
            theClass.IsChecked = true;


            
        }
        #endregion Helper Methods

        #region internal classes

        public enum NumberOfPlayersEnum
        {
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4,
        }
        public class NumberOfPlayersClass : ViewModelBase
        {
            public NumberOfPlayersClass()
            {
                IsChecked = false;
            }
            public NumberOfPlayersEnum Player { get; set; }
            private bool _isChecked = false;

            public bool IsChecked
            {
                get
                {
                    return _isChecked;
                }
                set
                {
                    _isChecked = value;
                    OnPropertyChanged("IsChecked");
                }

            }
            public string Title { get; set; }

        }

        public class PlayerInfo
        {
            public PlayerInfo(BlackJackParticipantModel player)
            {
                CardImages = new ObservableCollection<ImageSource>();
                Player = player;
                
            }
            public ObservableCollection<ImageSource> CardImages { get;  set; }
            public BlackJackParticipantModel Player { get; private set; }

        }
        #endregion internal classes


    }
}
