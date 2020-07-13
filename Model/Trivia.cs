using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Quiz
{
    public class Trivia : INotifyPropertyChanged
    {

        private readonly List<Question> Questions = new List<Question>();
        private readonly Random random = new Random();

        public event PropertyChangedEventHandler PropertyChanged;

        private Question _currentQuestion;
        public Question CurrentQuestion
        {
            get
            {
                if (_currentQuestion == null)
                {
                    NextQuestion();
                }
                return _currentQuestion;
            }
            set
            {
                _currentQuestion = value;
                OnPropertyChanged("_currentQuestion");
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Trivia()
        {
            Manager.WriteLine("Questions loading...");
            string[] categories = { "Entertainment", "FoodAndDrink", "Geography", "History", "Language", "Music", "PeopleAndPlaces", "ScienceAndNature", "SportAndLeisure", "Jeopardy" };
            foreach (string category in categories)
            {
                using StreamReader r = new StreamReader(@$"{category}.json");

                List<Question> temp = new List<Question>();
                foreach (Question q in JsonConvert.DeserializeObject<List<Question>>(r.ReadToEnd()))
                {
                    if (q.QuestionString.Length > 0 && q.AnswerString.Length > 0)
                    {
                        q.QuestionString.TrimEnd('.', ',', '!', '"');
                        q.QuestionString.TrimStart('"');
                        if (q.QuestionString[0].Equals("'") && q.QuestionString[q.QuestionString.Length - 1].Equals("'"))
                        {
                            q.QuestionString = q.QuestionString.Trim(new char[] { (char)39 });
                        }
                        q.CategoryString.Replace('_', ' ');
                        if (!q.QuestionString.EndsWith('?'))
                        {
                            q.QuestionString += '?';
                        }
                        temp.Add(q);
                    }
                }
                Questions.AddRange(temp);
            }
            Manager.WriteLine("Finished loading questions");
        }

        public Question NextQuestion()
        {
            CurrentQuestion = Questions[random.Next(Questions.Count)];
            return CurrentQuestion;
        }

    }
}