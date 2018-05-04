const connection = new signalR.HubConnection(
    "/gamehub", 
    { 
        logger: signalR.LogLevel.Verbose 
    });

connection.on("playerCountUpdated", (currentUserCount) => { 
    const currentPlayers = document.getElementById("currentPlayers");
    currentPlayers.textContent = currentUserCount;
});

function playerLogin()
{
    connection.send("PlayerLogin").catch(err => console.error)
    console.log('playerLogin');
}