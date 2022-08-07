using System.Text;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace DiscordBot.Modules.Levels;

public class Currency : ModuleBase<SocketCommandContext>
{
    readonly string _jsonFile = "Members.json";

    [Command("initlevels")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task InitLevels()
    {
        foreach (var user in Context.Guild.Users.ToArray()) 
            await UserJoined(user);
        await Context.Message.ReplyAsync($"Initialized {Context.Guild.Users.ToArray().Length} users with the level system");
    }
    
    [Command("resetlevels")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task ResetLevels()
    {
        foreach (var user in Context.Guild.Users.ToArray())
        {
            await UserLeft(Context.Guild, user);
            await UserJoined(user);
        }
        await Context.Message.ReplyAsync($"Reset {Context.Guild.Users.ToArray().Length} users with the level system");
    }
    
    [Command("hardresetalllevels")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task HardResetAllLevels()
    {
        var file = Utils.GetFile(_jsonFile);
        await File.WriteAllTextAsync(file, "{}");
        await Context.Message.ReplyAsync("Hard reset level system");
    }

    [Command("level")]
    public async Task GetLevel() => await GetLevel(Context.Message.Author);

    [Command("level")]
    public async Task GetLevel(IUser user)
    {
        var file = Utils.GetFile(_jsonFile);
        var members = JsonConvert.DeserializeObject<Dictionary<ulong, Dictionary<ulong, int>>>(await File.ReadAllTextAsync(file));
        await Context.Message.ReplyAsync($"{user.Mention} has **{members[user.Id][Context.Guild.Id]}** xp");
    }
    
    [Command("levels")]
    public async Task Levels()
    {
        var file = Utils.GetFile(_jsonFile);
        var members = JsonConvert.DeserializeObject<Dictionary<ulong, Dictionary<ulong, int>>>(await File.ReadAllTextAsync(file));
        var list = new StringBuilder();
        for (var i = 0; i < 10; i++)
        {
            var next = members.Where(m =>  m.Value.ContainsKey(Context.Guild.Id)).MaxBy(m => m.Value[Context.Guild.Id]);
            members.Remove(next.Key);
            list.Append($"**{i + 1}.** {Context.Guild.GetUser(next.Key).Username} - {next.Value[Context.Guild.Id]}xp\n");
        }
        var embed = new EmbedBuilder
        {
            Title = $"Top 10 users in {Context.Guild.Name}",
            Color = Resources.EmbedColor,
            Description = list.ToString(),
        };
        await Context.Message.ReplyAsync("", false, embed.Build());
    }

    [Command("give")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task Give(IUser user, int xp)
    {
        if (user.IsBot)
            return;
        var file = Utils.GetFile(_jsonFile);
        var members = JsonConvert.DeserializeObject<Dictionary<ulong, Dictionary<ulong, int>>>(await File.ReadAllTextAsync(file));
        if (members.ContainsKey(user.Id))
        {
            var member = members[user.Id];
            if (!member.ContainsKey(Context.Guild.Id))
                member.Add(Context.Guild.Id, xp);
            else
                member[Context.Guild.Id] += xp;
        }
        else
        {
            members.Add(user.Id, new() {{Context.Guild.Id, xp}});
        }
        await File.WriteAllTextAsync(file, JsonConvert.SerializeObject(members));
        await Context.Message.ReplyAsync($"Gave {user.Mention} **{xp}** xp");
    }
    
    public async Task UserJoined(SocketGuildUser user)
    {
        if (user.IsBot)
            return;
        var file = Utils.GetFile(_jsonFile);
        var members = JsonConvert.DeserializeObject<Dictionary<ulong, Dictionary<ulong, int>>>(await File.ReadAllTextAsync(file));
        if (members.ContainsKey(user.Id))
        {
            var member = members[user.Id];
            if (!member.ContainsKey(user.Guild.Id))
                member.Add(user.Guild.Id, 0);
        }
        else
        {
            members.Add(user.Id, new() {{user.Guild.Id, 0}});
        }
        await File.WriteAllTextAsync(file, JsonConvert.SerializeObject(members));
    }
    
    public async Task UserLeft(SocketGuild guild, SocketUser user)
    {
        if (user.IsBot)
            return;
        var file = Utils.GetFile(_jsonFile);
        var members = JsonConvert.DeserializeObject<Dictionary<ulong, Dictionary<ulong, int>>>(await File.ReadAllTextAsync(file));
        if (members.ContainsKey(user.Id))
        {
            var member = members[user.Id];
            if (member.ContainsKey(guild.Id))
                member.Remove(guild.Id);
        }
        await File.WriteAllTextAsync(file, JsonConvert.SerializeObject(members));
    }

    public async Task MessageSent(SocketMessage message)
    {
        if (message.Author.IsBot)
            return;
        if (message.Content.StartsWith(Resources.Prefix))
            return;
        var amt = Utils.Random(15, 25);
        var channel = message.Channel as SocketGuildChannel;
        var file = Utils.GetFile(_jsonFile);
        var members = JsonConvert.DeserializeObject<Dictionary<ulong, Dictionary<ulong, int>>>(await File.ReadAllTextAsync(file));
        var user = message.Author;
        if (members.ContainsKey(user.Id))
        {
            var member = members[user.Id];
            if (!member.ContainsKey(channel.Guild.Id))
                member.Add(channel.Guild.Id, amt);
            else
                member[channel.Guild.Id] += amt;
        }
        else
        {
            members.Add(user.Id, new() {{channel.Guild.Id, amt}});
        }
        await File.WriteAllTextAsync(file, JsonConvert.SerializeObject(members));
    }
}