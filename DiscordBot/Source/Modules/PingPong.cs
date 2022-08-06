using Discord.Commands;

namespace DiscordBot.Modules;

public class PingPong : ModuleBase<SocketCommandContext>
{
    [Command("ping")]
    public async Task Ping() => await Context.Channel.SendMessageAsync("Pong");
}