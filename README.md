# TriviaR - Play Trivia with Azure SignalR Service!

TriviaR is a real time online quiz contest built on top of ASP.NET Core and Azure SignalR Service. In this game, you can join the contest and answer the questions that administrator pushes to you.

## How to Play with TriviaR

First build and run TriviaR locally:

1. Create an Azure SignalR Service instance
2. Get connection string
3. Build and run TriviaR locally

   ```
   dotnet build
   dotnet user-secrets set Azure:SignalR:ConnectionString "<your connection string>"
   dotnet run
   ```

There're two UIs in TriviaR. Player UI is at http://localhost:5000/, open the page and you'll see how many players have already joined the game.

The admin UI is at http://localhost:5000/Admin, open the page, you'll see a button to start/stop the game and a list of questions that you can push to players. Click Start Game first, then choose the question you want to push to players. Players will see your questions immediately on their screen. When player answer the question, you'll the statistics updated (# of right and wrongs) in real time.

## Customize Your questions

The questions are located under [wwwroot/data/questions.json](wwwroot/data/questions.json). Update it with the questions you like.

## Deploy TriviaR to Azure

To deploy TriviaR to Azure Web App you need to first build the application into a container (a [Dockerfile](Dockerfile) is already available at the root of the repo):

```
docker build -t triviar .
```

To test the container locally:

```
docker run -p 5000:80 -e Azure__SignalR__ConnectionString="<connection_string>" triviar
```

Then push the container into a docker registry:

```
docker login <docker_registry>
docker tag triviar <docker_registry>/triviar
docker push <docker_registry>/triviar
```

The create a web app and update its container settings:

```
az group create --name <resource_group_name> --location CentralUS
az appservice plan create --name <plan_name> --resource-group <resource_group_name> --sku S1 --is-linux
az webapp create \
   --resource-group <resource_group_name> --plan <plan_name> --name <app_name> \
   --deployment-container-image-name nginx
az webapp config container set \
   --resource-group <resource_group_name> --name <app_name> \
   --docker-custom-image-name <docker_registry>/triviar \
   --docker-registry-server-url https://<docker_registry> \
   --docker-registry-server-user <docker_registry_name> \
   --docker-registry-server-password <docker_registry_password>
az webapp config appsettings set --resource-group <resource_group_name> --name <app_name> --setting PORT=80
az webapp config appsettings set --resource-group <resource_group_name> --name <app_name> \
   --setting Azure__SignalR__ConnectionString="<connection_string>"
```

Then open `https://<app_name>.azurewebsites.net`, now you share the url with your friends (or ask them to scan the QR code) to play TriviaR with them.
