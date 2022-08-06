using Discord;
using Discord.WebSocket;

MainAsync().GetAwaiter().GetResult();

async Task MainAsync()
{
    var client = new DiscordSocketClient();
    client.Log += Log;

    var token = Environment.GetEnvironmentVariable("TOKEN");
    await client.LoginAsync(TokenType.Bot, token);
    await client.StartAsync();
    await Task.Delay(-1);
}

Task Log(LogMessage message)
{
    Console.WriteLine(message.ToString());
    return Task.CompletedTask;
}