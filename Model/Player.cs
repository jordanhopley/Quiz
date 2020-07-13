using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Quiz
{
    public class Player : INotifyPropertyChanged
    {

        public static int PlayerCount = 0;

        public int ID { get; }
        public string Name { get; set; }

        private int _score;
        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
                OnPropertyChanged("_score");
            }
        }

        private bool _fastestPlayer;
        public bool FastestPlayer
        {
            get
            {
                return _fastestPlayer;
            }
            set
            {
                _fastestPlayer = value;
                if (value)
                {
                    OnPropertyChanged("_fastestPlayer");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Player(string name)
        {
            PlayerCount++;

            Name = name;
            Score = 0;
            ID = PlayerCount;

            FastestPlayer = false;
        }

        override
        public string ToString()
        {
            return $"Player\n\tID: {ID}\n\tName: {Name}\n\tScore: {Score}";
        }

    }
}