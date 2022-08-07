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

    [Command("level")]
    public async Task GetLevel() => await GetLevel(Context.Message.Author);

    [Command("level")]
    public async Task GetLevel(IUser user)
    {
        var file = Utils.GetFile(_jsonFile);
        var members = JsonConvert.DeserializeObject<Dictionary<ulong, Dictionary<ulong, int>>>(await File.ReadAllTextAsync(file));
        await Context.Message.ReplyAsync($"{user.Mention} has **{members[user.Id][Context.Guild.Id]}** xp");
    }

    public async Task UserJoined(SocketGuildUser user)
    {
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
        var channel = message.Channel as SocketGuildChannel;
        var file = Utils.GetFile(_jsonFile);
        var members = JsonConvert.DeserializeObject<Dictionary<ulong, Dictionary<ulong, int>>>(await File.ReadAllTextAsync(file));
        var user = message.Author;
        if (members.ContainsKey(user.Id))
        {
            var member = members[user.Id];
            if (!member.ContainsKey(user.Id))
                member.Add(channel.Guild.Id, 1);
            else
                member[channel.Guild.Id] += 1;
        }
        else
        {
            members.Add(user.Id, new() {{channel.Guild.Id, 1}});
        }
        Console.WriteLine($"{message.Author.Username} has {members[user.Id][channel.Guild.Id]} coins");
        await File.WriteAllTextAsync(file, JsonConvert.SerializeObject(members));
    }
}