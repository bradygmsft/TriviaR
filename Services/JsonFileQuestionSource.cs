using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
