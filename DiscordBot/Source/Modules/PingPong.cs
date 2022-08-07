using Discord;
using Discord.Commands;

namespace DiscordBot.Modules;

public class PingPong : ModuleBase<SocketCommandContext>
{
    [Command("ping")]
    [RequireBotPermission(GuildPermission.Administrator)]
    public async Task Ping() => await Context.Channel.SendMessageAsync("Pong");
}