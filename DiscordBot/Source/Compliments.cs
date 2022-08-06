using Discord.WebSocket;

namespace DiscordBot;

static class Compliments
{
    static readonly List<string> _values = new()
    {
        "you look good",
        "you're the best",
        "ur the best",
        "u look good",
        "you're smart",
        "ur smart",
        "you're better than n8dev",
        "you are unbanned from ham sandwich club day",
        "you're the next n8dev",
        "ur better than n8dev",
        "ur the next n8dev",
        "you're the best person i know",
        "ur the best person i know",
        "good job",
        "you're awesome",
        "ur awesome",
        "you look good",
        "you are nice",
        "pp big",
        "ur pp big",
        "you are handsome",
        "you're better than squidbot 3.0",
        "u r the next n8dev",
        "nice drip g",
        "n69dev > sokobot",
        "you're better than n8dev",
        "you are better than n8dev",
        "you're handsome",
        "awesome mate",
        "tysm",
        "you are amazing in every way",
        "you are an epic gamer",
        "you're a good boi",
        "you are very cool and smart",
        "you are pushin p"
    };
    
    public static Task MessageReceived(SocketMessage message)
    {
        var command = message.Content.ToLower();
        if (command.Contains("<@1005466799573315585>"))
        {
            command = command.Split("<@1005466799573315585>")[1].Trim();
            if (_values.Contains(command))
                message.Channel.SendMessageAsync($"Thanks {message.Author.Mention}!");
        }
        return Task.CompletedTask;
    }
    
    
}