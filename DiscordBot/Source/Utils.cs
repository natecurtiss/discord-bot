using System.Runtime.CompilerServices;
using static System.IO.Directory;
using static System.IO.Path;
using static System.String;

namespace DiscordBot;

static class Utils
{
    static readonly Random _random = new();
    public static int Random(int min, int max) => _random.Next(min, max);
    public static string GetFile(string name, [CallerFilePath] string path = default) => Join(GetParent(path ?? Empty)?.FullName, $"/{name}");
}