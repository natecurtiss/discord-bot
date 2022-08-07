using System.Runtime.CompilerServices;
using static System.IO.Directory;
using static System.IO.Path;
using static System.String;

namespace DiscordBot;

static class Utils
{
    public static string GetFile(string name, [CallerFilePath] string path = default) => Join(GetParent(path ?? Empty)?.Parent?.FullName, $"/{name}");
}