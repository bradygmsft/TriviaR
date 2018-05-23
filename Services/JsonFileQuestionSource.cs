using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using TriviaR.Models;

namespace TriviaR.Services
{
    public class JsonFileQuestionSource : IQuestionDataSource
    {
        IHostingEnvironment _hostingEnvironment;

        public JsonFileQuestionSource(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public IEnumerable<Question> GetQuestions()
        {
            string path = Path.Combine(_hostingEnvironment.WebRootPath, "data", "questions.json");
            using (var rdr = File.OpenText(path))
            {
                return JsonSerializer
                    .Create()
                    .Deserialize<Question[]>(new JsonTextReader(rdr));
            }
        }
    }
}
