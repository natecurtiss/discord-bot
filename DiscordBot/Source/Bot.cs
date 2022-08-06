using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBot;

class Bot
{
    readonly DiscordSocketClient _client = new();

    public async Task MainAsync()
    {
        _client.Log += Log;

        var token = Environment.GetEnvironmentVariable("TOKEN");
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        await Task.Delay(-1);
    }

    Task Log(LogMessage message)
    {
        Console.WriteLine(message.ToString());
        return Task.CompletedTask;
    }
}