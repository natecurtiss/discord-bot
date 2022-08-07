using Discord;
using Discord.Commands;

namespace DiscordBot.Modules;

public class Misc : ModuleBase<SocketCommandContext>
{
    [Command("ping")]
    public async Task Ping() => await Context.Channel.SendMessageAsync("Pong");

    [Command("avatar")]
    public async Task Avatar(IUser user)
    {
        /*var embed = new EmbedBuilder
        {
            ImageUrl = user.GetAvatarUrl(),
            Color = Resources.EmbedColor
        };
        await Context.Message.ReplyAsync("", false, embed.Build());*/
        await Context.Message.ReplyAsync(user.GetAvatarUrl());
    }
}