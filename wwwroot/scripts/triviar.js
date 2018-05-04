const connection = new signalR.HubConnection(
    "/gamehub", 
    { 
        logger: signalR.LogLevel.Verbose 
    });

connection.on("playerCountUpdated", (currentUserCount) => { 
    console.log(currentUserCount);
    const currentPlayers = document.getElementById("currentPlayers");
    currentPlayers.textContent = currentUserCount;
});

connection.on("receiveQuestion", (question) => { 
    console.log(currentUserCount);
    const currentPlayers = document.getElementById("currentPlayers");
    currentPlayers.textContent = currentUserCount;
});

/*
document.getElementById("sendButton").addEventListener("click", event => {
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;    
    connection.invoke("SendMessage", user, message).catch(err => console.error);
    event.preventDefault();
});
*/

connection.start().catch(err => console.error);