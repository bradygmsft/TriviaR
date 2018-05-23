using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TriviaR.Services;

namespace TriviaR.Hubs
{
    public class GameHub : Hub
    {
        private const string AdminGroupName = "Admins";

        private readonly IQuestionDataSource questionDataSource;

        static int currentPlayerCount;

        static int incorrectAnswers;

        static int correctAnswers;

        public GameHub(IQuestionDataSource dataSource)
        {
            questionDataSource = dataSource;
        }

        public override Task OnConnectedAsync()
        {
            if (Context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Administrator"))
            {
                return Groups.AddToGroupAsync(Context.ConnectionId, AdminGroupName);
            }
            else
            {
                Interlocked.Increment(ref currentPlayerCount);
                return Clients.All.SendAsync("playerCountUpdated", currentPlayerCount);
            }
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (!Context.User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Administrator"))
            {
                Interlocked.Decrement(ref currentPlayerCount);
                return Clients.All.SendAsync("playerCountUpdated", currentPlayerCount);
            }

            return base.OnDisconnectedAsync(exception);
        }

        [Authorize]
        public async void PushQuestion(int questionId)
        {
            var question = questionDataSource.GetQuestions().First(x => x.id == questionId);

            await Clients.All.SendAsync("receiveQuestion", new
            {
                question.text,
                question.answers,
                question.id
            });
        }

        [Authorize]
        public async void StartGame()
        {
            correctAnswers = 0;
            incorrectAnswers = 0;
            await Clients.All.SendAsync("gameStarted");
        }

        [Authorize]
        public async void StopGame()
        {
            await Clients.All.SendAsync("gameStopped");
        }

        public async void LogAnswer(int questionId, string answer)
        {
            var question = questionDataSource.GetQuestions().First(x => x.id == questionId);

            var correctAnswer = question.answers[question.correctAnswerIndex];

            if (correctAnswer != answer)
            {
                Interlocked.Increment(ref incorrectAnswers);
                await Clients.Caller.SendAsync("incorrectAnswer");
                await Clients.Group(AdminGroupName).SendAsync("incorrectAnswer", incorrectAnswers);
            }
            else
            {
                Interlocked.Increment(ref correctAnswers);
                await Clients.Caller.SendAsync("correctAnswer");
                await Clients.Group(AdminGroupName).SendAsync("correctAnswer", correctAnswers);
            }
        }
    }
}
