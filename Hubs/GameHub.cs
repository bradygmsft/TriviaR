using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using TriviaR.Services;
using System.Linq;

namespace TriviaR.Hubs
{
    public class GameHub : Hub
    {
        private readonly IQuestionDataSource _questionDataSource;

        public GameHub(IQuestionDataSource questionDataSource)
        {
            _questionDataSource = questionDataSource;
        }

        static int CurrentUserCount { get; set; }

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
            await Clients.All.SendAsync("gameStarted");
        }

        public async void StopGame()
        {
            await Clients.All.SendAsync("gameStopped");
        }
    }
}