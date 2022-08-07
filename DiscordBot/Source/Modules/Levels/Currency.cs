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
}