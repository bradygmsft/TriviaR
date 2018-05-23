using System.Collections.Generic;
using TriviaR.Models;

namespace TriviaR.Services
{
    public interface IQuestionDataSource
    {
        IEnumerable<Question> GetQuestions();
    }
}
