using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Quiz
{
    public class Manager
    {

        public Trivia Trivia { get; set; } = new Trivia();
        public ObservableCollection<Player> Players = new ObservableCollection<Player>();
        public Timer Timer { get; set; } = new Timer();
	
        private readonly HttpServer server = new HttpServer("127.0.0.1", 80); // Replace with IP of host
        private readonly int ShowQuestionTime = 15;
        private readonly int AnsweringTime = 5;

        private bool QuizRunning = false;
        private bool CanPressButton = false;

        public Manager()
        {
            server.IncomingMessage += ReceivedMessage;
            server.StartListening(); 
        }

        public async void NextRound()
        {
            QuizRunning = true;
            foreach (Player p in Players)
            {
                p.FastestPlayer = false;
            }
            Task t = Timer.Start(ShowQuestionTime);
            Trivia.NextQuestion();
            CanPressButton = true;

            try { await t; } catch (TaskCanceledException) { }

            if (t.IsCanceled)
            {
                Timer.Stop();
                t = Timer.Start(AnsweringTime);
                try { await t; } catch (TaskCanceledException) { }
            }

        }

        private void ReceivedMessage(object sender, EventArgs e)
        {
            Message m = (e as MessageReceivedEventArgs).Message;
            WriteLine(m.ToString());
            PrintState(this);
            if (m.Type == Message.MessageType.NME)
            {
                if (!Players.Any(p => p.Name == m.Name))
                {
                    Player newPlayer = new Player(m.Name);
                    Players.Add(newPlayer);
                    WriteLine($"Added new player: {newPlayer.Name}");
                }
            }
            if (m.Type == Message.MessageType.BTN && QuizRunning && CanPressButton)
            {
                CanPressButton = false;
                Player player = Players.Single(p => p.Name == m.Name);
                player.FastestPlayer = true;
                Timer.Stop();
            }
        }

        public static void PrintState(Manager manager)
        {
            WriteLine($"Current State\n\tQuiz running: {manager.QuizRunning}\n\tCan press button: {manager.CanPressButton}");
        }

        public static void WriteLine(string value)
        {
            Console.WriteLine($"[{DateTime.Now.ToLongTimeString()}] {value}");
        }

    }
}
