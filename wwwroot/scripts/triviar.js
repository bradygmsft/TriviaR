const connection = new signalR.HubConnectionBuilder()
    .withUrl('/gamehub', { 
        logger: signalR.LogLevel.Verbose 
    })
    .build();

connection.on("playerCountUpdated", (currentUserCount) => { 
    const currentPlayers = document.getElementById("currentPlayers");
    currentPlayers.textContent = currentUserCount;
});

function playerLogin()
{
    connection.send("PlayerLogin").catch(err => console.error)
    console.log('playerLogin');
}