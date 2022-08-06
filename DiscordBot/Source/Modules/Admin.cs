using Discord;
using Discord.Commands;

namespace DiscordBot.Modules;

public class Admin : ModuleBase<SocketCommandContext>
{
    [Command("timeout")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task Timeout(IGuildUser user, int minutes)
    {
        await user.SetTimeOutAsync(TimeSpan.FromMinutes(minutes));
        await Context.Message.ReplyAsync($"Timed out {user.Mention} for {minutes} minutes.");
    }
    
    [Command("untimeout")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task UnTimeout(IGuildUser user)
    {
        await user.RemoveTimeOutAsync();
        await Context.Message.ReplyAsync($"Cancelled timeout on {user.Mention}");
    }
    
    [Command("kick")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task Kick(IGuildUser user)
    {
        await user.KickAsync();
        await Context.Message.ReplyAsync($"Kicked {user.Mention}");
    }
}