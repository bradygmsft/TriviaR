const connection = new signalR.HubConnectionBuilder()
    .withUrl('/gamehub', { 
        logger: signalR.LogLevel.Verbose 
    })
    .build();

connection.onclose(() => $("#logo").addClass("disconnected"));

connection.on("playerCountUpdated", (currentUserCount) => { 
    const currentPlayers = document.getElementById("currentPlayers");
    currentPlayers.textContent = currentUserCount;
});
