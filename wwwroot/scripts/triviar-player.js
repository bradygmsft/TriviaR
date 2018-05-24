connection.on("receiveQuestion", (question) => {
    console.log(question);
    var source = $('#questionTemplate').html();
    var template = Handlebars.compile(source);
    var result = template(question);
    $('#questionContainer').html(result);
    $('#questionDialog')
        .modal('show');
});

connection.on("gameStarted", (questionId) => {
    document.getElementById("intro").innerText = "Here we go! In a moment they'll pick a question and you'll see it pop up here.";
});

connection.on("gameStopped", (questionId) => {
    document.getElementById("intro").innerText = "Let's give everyone a moment to hop into the game, and then give Scott and Scott a moment to pick a question.";
});

connection.on("incorrectAnswer", () => {
    $('#incorrectDialog')
        .modal('show');
});

connection.on("correctAnswer", () => {
    $('#correctDialog')
        .modal('show');
});

connection
    .start()
    .then(() => {
        $("#logo").removeClass("disconnected");
        connection.invoke("PlayerLogin");
    })
    .catch(console.error);

function logAnswer() {
    var questionId = 0;
    var answer = "";
    //questionContainer.questionHeader[data-id]

    questionId = $('#questionContainer').find('#questionHeader').data('id');
    answer = $('input[name=answer]:checked').val();

    console.log(questionId + ':' + answer);

    connection
        .send('LogAnswer', questionId, answer)
        .catch(err => console.error);
}