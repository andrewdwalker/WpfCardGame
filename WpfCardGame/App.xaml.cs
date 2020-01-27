using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfCardGame.ViewModel;
using WpfCardGame.Views;

namespace WpfCardGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

           // MainWindow app = new MainWindow();
            
           // ImageWindo app = new ImageWindo();

            //WindowWithGrouping app = new WindowWithGrouping();
            ImageView app = new ImageView();
            CardGameViewModel context = new CardGameViewModel();
            app.DataContext = context;
            app.Show();
        }
    }
}
