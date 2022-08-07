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

    public async Task UserJoined(SocketGuildUser user)
    {
        var file = Utils.GetFile("Data.json");
        var dict = JsonConvert.DeserializeObject<Dictionary<long, long>>(await File.ReadAllTextAsync(file));
        var guildID = (long) user.Guild.Id;
        if (dict.ContainsKey(guildID))
        {
            var channel = user.Guild.GetChannel((ulong) dict[guildID]);
            if (channel is not ISocketMessageChannel msgChannel)
                return;
            await msgChannel.SendMessageAsync($"{user.Mention} welcome to n8dev's cafe!");
        }
    }
}