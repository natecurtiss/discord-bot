using System.Runtime.CompilerServices;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace DiscordBot.Modules.Greetings;

public class Welcome : ModuleBase<SocketCommandContext>
{
    readonly string _jsonFile = "Welcome.json";
    
    [Command("welcome")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public async Task SetWelcomeChannel(long channelID, [CallerFilePath] string path = default)
    {
        var guildID = (long) Context.Guild.Id;
        var file = Utils.GetFile(_jsonFile);
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
        var file = Utils.GetFile(_jsonFile);
        var dict = JsonConvert.DeserializeObject<Dictionary<long, long>>(await File.ReadAllTextAsync(file));
        var guildID = (long) user.Guild.Id;
        if (dict.ContainsKey(guildID))
        {
            var channel = user.Guild.GetChannel((ulong) dict[guildID]);
            if (channel is not ISocketMessageChannel msgChannel)
                return;
            // await msgChannel.SendMessageAsync($"{user.Mention} welcome to n8dev's cafe!");
            var embed = new EmbedBuilder()
            {
                Title = $"Welcome to n8dev's cafe {user.Username}!",
                Description = "We hope you enjoy your stay :)",
                Color = Resources.EmbedColor,
                ThumbnailUrl = user.GetAvatarUrl()
            };
            await msgChannel.SendMessageAsync("", false, embed.Build());
        }
    }
    
    public async Task UserLeft(SocketGuild guild, SocketUser user)
    {
        var file = Utils.GetFile(_jsonFile);
        var dict = JsonConvert.DeserializeObject<Dictionary<long, long>>(await File.ReadAllTextAsync(file));
        var guildID = (long) guild.Id;
        if (dict.ContainsKey(guildID))
        {
            var channel = guild.GetChannel((ulong) dict[guildID]);
            if (channel is not ISocketMessageChannel msgChannel)
                return;
            var embed = new EmbedBuilder
            {
                Title = $"I can't believe {user.Username} left.",
                Description = "We didn't need them anyway.",
                Color = Resources.EmbedColor,
                ThumbnailUrl = user.GetAvatarUrl()
            };
            await msgChannel.SendMessageAsync("", false, embed.Build());
        }
    }
}