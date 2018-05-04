using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TriviaR.Models;

namespace TriviaR.Services
{
    public interface IQuestionDataSource
    {
        IEnumerable<Question> GetQuestions();
    }
}
