using Discord;
using Discord.Commands;

namespace DiscordBot.Modules;

public class Admin : ModuleBase<SocketCommandContext>
{
    [Command("timeout")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task Timeout(IGuildUser user, int minutes)
    {
        if (minutes <= 0)
        {
            await Context.Message.ReplyAsync("Number of minutes must be positive!");
            return;
        }
        await user.SetTimeOutAsync(TimeSpan.FromMinutes(minutes));
        await Context.Message.ReplyAsync($"Timed out {user.Mention} for {minutes} minutes.");
    }
    
    [Command("untimeout")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task UnTimeout(IGuildUser user)
    {
        await user.RemoveTimeOutAsync();
        await Context.Message.ReplyAsync($"Cancelled timeout on {user.Mention}");
    }
    
    [Command("kick")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public async Task Kick(IGuildUser user)
    {
        await user.KickAsync();
        await Context.Message.ReplyAsync($"Kicked {user.Mention}");
    }
    
    [Command("ban")]
    [RequireUserPermission(GuildPermission.BanMembers)]
    public async Task Ban(IGuildUser user)
    {
        await user.BanAsync();
        await Context.Message.ReplyAsync($"Banned {user.Mention}");
    }
    
    [Command("purge")]
    [RequireUserPermission(GuildPermission.ManageMessages)]
    public async Task Purge(int amount)
    {
        if (amount <= 0)
        {
            await Context.Message.ReplyAsync("Number of messages must be positive!");
            return;
        }
        var messages = await Context.Channel.GetMessagesAsync(Context.Message, Direction.Before, amount).FlattenAsync();
        var filteredMessages = messages.Where(x => (DateTimeOffset.UtcNow - x.Timestamp).TotalDays <= 14).ToArray();
        var count = filteredMessages.Length;
        if (count == 0)
        {
            await Context.Message.ReplyAsync("Nothing to delete");
            return;
        }
        foreach (var msg in filteredMessages) 
            await Context.Channel.DeleteMessageAsync(msg);
        await Context.Message.ReplyAsync($"Removed {count} {(count > 1 ? "messages" : "message")}");
    }
}