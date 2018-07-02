function pushQuestion(questionId) {
    console.log('pushQuestion(' + questionId + ')');
    var query = "[data-questionCard='" + questionId + "']";
    console.log(query);
    var node = document.querySelectorAll(query);
    console.log(node);
    document.getElementById('questions').removeChild(node[0]);
    endGameIfNoMoreQuestionsInQueue();

    connection
        .invoke('PushQuestion', questionId)
        .catch(err => console.error);
}

function endGameIfNoMoreQuestionsInQueue() {
    if (document.getElementsByClassName('question').length == 0) {
        document.getElementById('gameStarted').checked = false;
        document.getElementById('gameStarted').setAttribute('disabled', 'disabled');
        document.getElementById('showAnswers').setAttribute('disabled', 'disabled');
        toggleGameStarted();
    }
}

function toggleGameStarted() {
    if (document.getElementById('gameStarted').checked) {
        console.log('game on');
        connection.invoke('StartGame');
    }
    else {
        console.log('game off');
        connection.invoke('StopGame');
    }
}

function toggleAnswers() {
    if (document.getElementById('showAnswers').checked) {
        if (confirm('Are you SURE you want to turn them on? If you\'re on screen people will see the answers.') == true) {
            console.log('turn them on');
            for (i = 0; i < document.querySelectorAll('[data-isCorrect]').length; i++)
                document.querySelectorAll('[data-isCorrect]')[i].checked = true;
        }
        else
            document.getElementById('showAnswers').checked = false;
    }
    else {
        console.log('turn them off');
        for (i = 0; i < document.querySelectorAll('[data-isCorrect]').length; i++)
            document.querySelectorAll('[data-isCorrect]')[i].checked = false;
    }
}

function logAnswer() {
    //questionContainer.questionHeader (get attribute data-id)
    var questionId = $("#questionContainer").children("#questionHeader").data("id");
    console.log("questionId:" + questionId);
}

connection.on("incorrectAnswerUpdated", (answerCount) => {
    document.getElementById("wrongAnswers").innerText = answerCount;
    console.log("incorrect: " + answerCount);
});

connection.on("correctAnswerUpdated", (answerCount) => {
    document.getElementById("rightAnswers").innerText = answerCount;
    console.log("correct: " + answerCount);
});

connection.on("gameStarted", () => {
    document.getElementById('gameStarted').checked = true;
    pushButtons = document.getElementsByClassName('question-push-button');

    for (x = 0; x < pushButtons.length; x++) {
        pushButtons[x].removeAttribute('disabled');
    }
});

connection.on("gameStopped", () => {
    document.getElementById('gameStarted').checked = false;
    pushButtons = document.getElementsByClassName('question-push-button');

    for (x = 0; x < pushButtons.length; x++) {
        pushButtons[x].setAttribute('disabled', "disabled");
    }
});

connection
    .start()
    .then(() => {
        $("#logo").removeClass("disconnected");
        connection.invoke("AdminLogin");
    })
    .catch(console.error);
