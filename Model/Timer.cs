using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quiz
{
    public class Timer : INotifyPropertyChanged
    {

        public bool Running { get; set; } = false;

        private CancellationTokenSource Source;

        private int _time = 0;
        public int Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
                OnPropertyChanged("_time");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Timer()
        {

        }

        public async Task Start(int seconds) => await Task.Run(async () =>
        {
            if (Running)
            {
                Stop();
            }
            Running = true;
            Source = new CancellationTokenSource();
            Time = seconds;

            while (Time > 0)
            {
                await Task.Delay(1000, Source.Token);
                Time--;
            }
        });

        public void Stop()
        {
            Running = false;
            Source.Cancel();
        }

    }
}
