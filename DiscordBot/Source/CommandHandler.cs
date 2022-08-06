using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBot;

class CommandHandler
{
    readonly DiscordSocketClient _client;
    readonly CommandService _service;
    
    public CommandHandler(DiscordSocketClient client)
    {
        _client = client;
        _service = new();
        _service.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        _client.MessageReceived += HandleCommands;
    }

    public async Task HandleCommands(SocketMessage msg)
    {
        if (msg is not SocketUserMessage message)
            return;
        var ctx = new SocketCommandContext(_client, message);
        var argPos = 0;
        if (message.HasCharPrefix('_', ref argPos))
        {
            var result = await _service.ExecuteAsync(ctx, argPos, null);
            if (!result.IsSuccess) 
                await ctx.Message.ReplyAsync(result.ErrorReason);
        }

    }
}