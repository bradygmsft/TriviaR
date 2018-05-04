using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TriviaR.Services;

namespace TriviaR.Controllers
{
    [Produces("application/json")]
    [Route("api/Questions")]
    public class QuestionsController : Controller
    {
        public QuestionsController(IQuestionDataSource questionDataSource)
        {
            QuestionDataSource = questionDataSource;
        }

        public IQuestionDataSource QuestionDataSource { get; }

        public ActionResult Get()
        {
            return Json(QuestionDataSource.GetQuestions());
        }
    }
}