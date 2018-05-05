using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TriviaR.Services;
using System.Linq;

namespace TriviaR.Hubs
{
    public class GameHub : Hub
    {
        const string AdminGroupName = "Admins";

        private readonly IQuestionDataSource _questionDataSource;

        public GameHub(IQuestionDataSource questionDataSource)
        {
            _questionDataSource = questionDataSource;
        }

        static int CurrentUserCount { get; set; }

        static int IncorrectAnswers { get; set; }

        static int CorrectAnswers { get; set; }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            CurrentUserCount -= 1;
            if(CurrentUserCount < 0) CurrentUserCount = 0;
            Clients.All.SendAsync("playerCountUpdated", CurrentUserCount);
            return base.OnDisconnectedAsync(exception);
        }

        public async void PlayerLogin()
        {
            CurrentUserCount += 1;
            await Clients.All.SendAsync("playerCountUpdated", CurrentUserCount);
        }

        public async void PushQuestion(int questionId)
        {
            var question = _questionDataSource
                .GetQuestions().
                    First(x => x.id == questionId);

            await Clients.All.SendAsync("receiveQuestion", new {
                question = question.text,
                answers = question.answers,
                id = question.id
            });
        }

        public async void StartGame()
        {
            CorrectAnswers = 0;
            IncorrectAnswers = 0;
            await Clients.All.SendAsync("gameStarted");
        }

        public async void StopGame()
        {
            await Clients.All.SendAsync("gameStopped");
        }

        public async void AdminLogin()
        {
            await Groups.AddAsync(this.Context.ConnectionId, AdminGroupName);
        }

        public async void LogAnswer(int questionId, string answer)
        {
            var question = _questionDataSource
                .GetQuestions()
                    .First(x => x.id == questionId);

            var correctAnswer = question.answers[question.correctAnswerIndex];
            
            if(correctAnswer != answer)
            {
                IncorrectAnswers += 1;
                await Clients.Caller.SendAsync("incorrectAnswer");
                await Clients.Group(AdminGroupName).SendAsync("incorrectAnswer", IncorrectAnswers);
            }
            else
            {
                CorrectAnswers += 1;
                await Clients.Caller.SendAsync("correctAnswer");
                await Clients.Group(AdminGroupName).SendAsync("correctAnswer", CorrectAnswers);
            }
        }
    }
}