using Newtonsoft.Json;

namespace Quiz
{
    public class Question
    {

        [JsonProperty("questionID")]
        public string QuestionString { get; set; }

        [JsonProperty("answer")]
        public string AnswerString { get; set; }

        [JsonProperty("category_id")]
        public string CategoryString { get; set; }

        private Question()
        {

        }

        override
        public string ToString()
        {
            return $"Question\n\t{CategoryString}\n\t{QuestionString}\n\t{AnswerString}";
        }

    }
}