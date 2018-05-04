connection.on("receiveQuestion", (question) => { 
    console.log(question);
});

connection.on("gameStarted", (questionId) => { 
    document.getElementById("intro").innerText = "Here we go! In a moment they'll pick a question and you'll see it pop up here.";
});

connection.on("gameStopped", (questionId) => { 
    document.getElementById("intro").innerText = "Let's give everyone a moment to hop into the game, and then give Scott and Scott a moment to pick a question.";
});

connection
    .start()
    .then(() => playerLogin())
    .catch(err => console.error);

