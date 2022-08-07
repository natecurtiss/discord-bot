using System.Runtime.CompilerServices;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace DiscordBot.Modules;

public class Welcome : ModuleBase<SocketCommandContext>
{
    [Command("welcome")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task SetWelcomeChannel(long channelID, [CallerFilePath] string path = default)
    {
        var guildID = (long) Context.Guild.Id;
        var file = Utils.GetFile("Data.json");
        var dict = JsonConvert.DeserializeObject<Dictionary<long, long>>(await File.ReadAllTextAsync(file));
        if (dict.ContainsKey(guildID))
            dict[guildID] = channelID;
        else
            dict.Add(guildID, channelID);
        await File.WriteAllTextAsync(file, JsonConvert.SerializeObject(dict));
        await Context.Message.ReplyAsync($"Changed welcome channel to <#{channelID}>");
    }
}