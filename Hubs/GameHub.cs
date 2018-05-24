using System;
using System.Collections.Generic;
using System.Linq;
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

        private static int currentPlayerCount;

        private static int incorrectAnswers;

        private static int correctAnswers;

        private static HashSet<string> admins = new HashSet<string>();

        private static bool gameStarted;

        public GameHub(IQuestionDataSource dataSource)
        {
            questionDataSource = dataSource;
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            if (!admins.Remove(Context.ConnectionId))
            {
                Interlocked.Decrement(ref currentPlayerCount);
                // make sure player count won't be negative in case someone didn't call player or admin login
                if (currentPlayerCount < 0) currentPlayerCount = 0;
                return Clients.All.SendAsync("playerCountUpdated", currentPlayerCount);
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task PlayerLogin()
        {
            Interlocked.Increment(ref currentPlayerCount);
            await Clients.All.SendAsync("playerCountUpdated", currentPlayerCount);
            if (gameStarted) await Clients.Caller.SendAsync("gameStarted");
        }

        [Authorize(Policy = "Admin_Only")]
        public async Task AdminLogin()
        {
            admins.Add(Context.ConnectionId);
            await Groups.AddToGroupAsync(Context.ConnectionId, AdminGroupName);
            if (gameStarted) await Clients.Caller.SendAsync("gameStarted");
            await Clients.Caller.SendAsync("playerCountUpdated", currentPlayerCount);
            await Clients.Caller.SendAsync("correctAnswerUpdated", correctAnswers);
            await Clients.Caller.SendAsync("incorrectAnswerUpdated", incorrectAnswers);
        }

        [Authorize(Policy = "Admin_Only")]
        public async Task PushQuestion(int questionId)
        {
            var question = questionDataSource.GetQuestions().First(x => x.id == questionId);

            await Clients.All.SendAsync("receiveQuestion", new
            {
                question.text,
                question.answers,
                question.id
            });
        }

        [Authorize(Policy = "Admin_Only")]
        public async Task StartGame()
        {
            correctAnswers = 0;
            incorrectAnswers = 0;
            gameStarted = true;
            await Clients.Group(AdminGroupName).SendAsync("correctAnswerUpdated", correctAnswers);
            await Clients.Group(AdminGroupName).SendAsync("incorrectAnswerUpdated", incorrectAnswers);
            await Clients.All.SendAsync("gameStarted");
        }

        [Authorize(Policy = "Admin_Only")]
        public async Task StopGame()
        {
            gameStarted = false;
            await Clients.All.SendAsync("gameStopped");
        }

        public async Task LogAnswer(int questionId, string answer)
        {
            var question = questionDataSource.GetQuestions().First(x => x.id == questionId);

            var correctAnswer = question.answers[question.correctAnswerIndex];

            if (correctAnswer != answer)
            {
                Interlocked.Increment(ref incorrectAnswers);
                await Clients.Caller.SendAsync("incorrectAnswer");
                await Clients.Group(AdminGroupName).SendAsync("incorrectAnswerUpdated", incorrectAnswers);
            }
            else
            {
                Interlocked.Increment(ref correctAnswers);
                await Clients.Caller.SendAsync("correctAnswer");
                await Clients.Group(AdminGroupName).SendAsync("correctAnswerUpdated", correctAnswers);
            }
        }
    }
}
