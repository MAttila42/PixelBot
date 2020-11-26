using System;
using System.Linq;
using Discord;
using Discord.WebSocket;
using PixelBot.Json;

namespace PixelBot.Commands.Dev
{
    public class Test
    {
        public static string[] Aliases()
        {
            string[] aliases =
            {
                "test",
                "teszt"
            };
            return aliases;
        }
        public static bool HasPerm(SocketMessage message)
        {
            bool hasPerm = false;
            foreach (var i in (message.Author as SocketGuildUser).Roles)
                if (BaseConfig.GetConfig().Roles.Admin.Contains(i.Id))
                {
                    hasPerm = true;
                    break;
                }
            return hasPerm;
        }

        public static async void DoCommand(SocketMessage message)
        {
            await Program.Log("command", message);
            await message.Channel.SendMessageAsync((await message.Channel.GetMessagesAsync(10).FlattenAsync()).Where(x => x.Author.Id == message.Author.Id).ElementAt(1).Content);
        }
    }
}
