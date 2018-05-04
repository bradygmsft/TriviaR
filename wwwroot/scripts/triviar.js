const connection = new signalR.HubConnection(
    "/gamehub", 
    { 
        logger: signalR.LogLevel.Verbose 
    });

connection.on("playerCountUpdated", (currentUserCount) => { 
    const currentPlayers = document.getElementById("currentPlayers");
    currentPlayers.textContent = currentUserCount;
});

connection.on("receiveQuestion", (question) => { 
    const currentPlayers = document.getElementById("currentPlayers");
    currentPlayers.textContent = currentUserCount;
});

function playerLogin()
{
    connection.send("PlayerLogin").catch(err => console.error)
    console.log('playerLogin');
}


/*
document.getElementById("sendButton").addEventListener("click", event => {
    const user = document.getElementById("userInput").value;
    const message = document.getElementById("messageInput").value;    
    connection.invoke("SendMessage", user, message).catch(err => console.error);
    event.preventDefault();
});
*/