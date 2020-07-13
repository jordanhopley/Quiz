using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Quiz
{
    public partial class QuizPage : Page
    {

        private readonly App app = (App)Application.Current;
        private readonly Manager manager;
        
        public QuizPage()
        {
            InitializeComponent();
            manager = app.Manager;

            foreach (Player p in manager.Players)
            {
                NamesBox.Items.Add(p.Name);
                ScoresBox.Items.Add(p.Score);
                p.PropertyChanged += Player_PropertyChanged;
            }
            manager.Trivia.PropertyChanged += Trivia_PropertyChanged;
            manager.Timer.PropertyChanged += Timer_PropertyChanged;
        }

        private void Player_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Player player = (Player)sender;
            if (e.PropertyName.Equals("_score"))
            {
                Dispatcher.Invoke(() => {
                    ScoresBox.Items[player.ID - 1] = player.Score.ToString();
                });
            }
            else if (e.PropertyName.Equals("_fastestPlayer"))
            {
                Manager.WriteLine($"Fastest player: {player.Name}");
                Dispatcher.Invoke(() => {
                    NameBlock.Text = player.Name;
                    NameGrid.Visibility = Visibility.Visible;
                    new Thread(() => {
                        Thread.Sleep(2000);
                        Dispatcher.Invoke(() => {
                            NameGrid.Visibility = Visibility.Hidden;
                        });
                    }).Start();
                });
            }

        }

        private void Trivia_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Question question = (sender as Trivia).CurrentQuestion;
            string questionString = question.QuestionString;
            Dispatcher.Invoke(() => {
                CategoryBlock.Text = question.CategoryString;
                QuestionBlock.Text = questionString;
                AnswerBlock.Visibility = Visibility.Hidden;
                AnswerBlock.Text = question.AnswerString;
            });
        }

        private void Timer_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            int time = (sender as Timer).Time;
            Dispatcher.Invoke(() => {
                TimerBlock.Text = time.ToString();
                if (time == 0)
                {
                    NextButton.Visibility = Visibility.Visible;
                    CorrectButton.Visibility = Visibility.Visible;
                    WrongButton.Visibility = Visibility.Visible;
                    AnswerBlock.Visibility = Visibility.Visible;
                }
            });
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            manager.NextRound();
            Dispatcher.Invoke(() => {
                NextButton.Visibility = Visibility.Hidden;
                CorrectButton.Visibility = Visibility.Hidden;
                WrongButton.Visibility = Visibility.Hidden;
                AnswerBlock.Visibility = Visibility.Hidden;
            });
        }

        private void CorrectButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Player p in manager.Players)
            {
                if (p.FastestPlayer)
                {
                    p.Score += 2;
                }
            }
        }

        private void WrongButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Player p in manager.Players)
            {
                if (p.FastestPlayer)
                {
                    p.Score -= 1;
                }
            }
        }

    }
}
