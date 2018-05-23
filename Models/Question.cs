namespace TriviaR.Models
{
    public class Question
    {
        public int id { get; set; }
        public string text { get; set; }
        public string[] answers { get; set; }
        public int correctAnswerIndex { get; set; }
    }
}
