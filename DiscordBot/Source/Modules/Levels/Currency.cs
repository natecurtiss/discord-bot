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
    
    [Command("clearlevels")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task ClearLevels()
    {
        var file = Utils.GetFile(_jsonFile);
        await File.WriteAllTextAsync(file, "{}");
        await Context.Message.ReplyAsync("Cleared level system");
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
            var next = members.MaxBy(m => m.Value[Context.Guild.Id]);
            members.Remove(next.Key);
            list.Append($"**{i + 1}.** {Context.Guild.GetUser(next.Key).Username} - {next.Value[Context.Guild.Id]}xp\n");
        }
        var embed = new EmbedBuilder
        {
            Title = $"Top 10 users in {Context.Guild.Name}",
            Color = Resources.EmbedColor,
            Description = list.ToString(),
        };
        Console.WriteLine(Context.Guild.IconUrl);
        await Context.Message.ReplyAsync("", false, embed.Build());
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

    public async Task MessageSent(SocketMessage message)
    {
        if (message.Author.IsBot)
            return;
        var channel = message.Channel as SocketGuildChannel;
        var file = Utils.GetFile(_jsonFile);
        var members = JsonConvert.DeserializeObject<Dictionary<ulong, Dictionary<ulong, int>>>(await File.ReadAllTextAsync(file));
        var user = message.Author;
        if (members.ContainsKey(user.Id))
        {
            var member = members[user.Id];
            if (!member.ContainsKey(channel.Guild.Id))
                member.Add(channel.Guild.Id, 1);
            else
                member[channel.Guild.Id] += 1;
        }
        else
        {
            members.Add(user.Id, new() {{channel.Guild.Id, 1}});
        }
        await File.WriteAllTextAsync(file, JsonConvert.SerializeObject(members));
    }
}