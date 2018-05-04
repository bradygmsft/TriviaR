using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TriviaR.Models;
using TriviaR.Services;

namespace TriviaR.Pages
{
    public class AdminModel : PageModel
    {
        public IQuestionDataSource QuestionDataSource { get; set; }

        public IEnumerable<Question> Questions { get; set; }

        public AdminModel(IQuestionDataSource questionDataSource)
        {
            QuestionDataSource = questionDataSource;
        }

        public void OnGet()
        {
            Questions = QuestionDataSource.GetQuestions();
        }
    }
}