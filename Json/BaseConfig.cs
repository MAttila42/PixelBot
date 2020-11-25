using System.IO;
using System.Text.Json;

namespace PixelBot.Json
{
    public class Role
    {
        public ulong[] Admin { get; set; }
    }
    public class Channel
    {
        public ulong[] BotTerminal { get; set; }
    }
    public class BaseConfig
    {
        public string Token { get; set; }
        public char Prefix { get; set; }
        public Role Roles { get; set; }
        public Channel Channels { get; set; }

        public static BaseConfig GetConfig()
        {
            return JsonSerializer.Deserialize<BaseConfig>(File.ReadAllText("BaseConfig.json"));
        }
    }
}
