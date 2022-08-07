using Discord;
using Discord.WebSocket;
using DiscordBot;

MainAsync().GetAwaiter().GetResult();

async Task MainAsync()
{
    var intents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers | GatewayIntents.GuildBans;
    var config = new DiscordSocketConfig { GatewayIntents = intents, AlwaysDownloadUsers = true };
    var client = new DiscordSocketClient(config);
    var token = Environment.GetEnvironmentVariable("TOKEN");
    await client.LoginAsync(TokenType.Bot, token);
    await client.StartAsync();
    var commands = new CommandHandler(client);
    await Task.Delay(-1);
}