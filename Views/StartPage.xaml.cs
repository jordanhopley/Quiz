using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Quiz
{
    public partial class StartPage : Page
    {

        private readonly App app = (App)Application.Current;
        private readonly Manager manager;

        public StartPage()
        {
            InitializeComponent();
            manager = app.Manager;

            manager.Players.CollectionChanged += Players_CollectionChanged;
        }

        private void Players_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Player player = (Player)e.NewItems[0];
            string name = player.Name;
            Dispatcher.Invoke(() =>
            {
                if (PlayerOne.Text.Length == 0)
                {
                    PlayerOne.Text = name;
                }
                else if (PlayerTwo.Text.Length == 0)
                {
                    PlayerTwo.Text = name;
                }
                else if (PlayerThree.Text.Length == 0)
                {
                    PlayerThree.Text = name;
                }
                else if (PlayerFour.Text.Length == 0)
                {
                    PlayerFour.Text = name;
                }
            });
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new QuizPage());
        }

    }
}
