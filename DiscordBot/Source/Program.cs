using Discord;
using Discord.WebSocket;
using DiscordBot;

MainAsync().GetAwaiter().GetResult();

async Task MainAsync()
{
    var client = new DiscordSocketClient();
    var token = Environment.GetEnvironmentVariable("TOKEN");
    await client.LoginAsync(TokenType.Bot, token);
    await client.StartAsync();
    var commands = new CommandHandler(client);
    await Task.Delay(-1);
}