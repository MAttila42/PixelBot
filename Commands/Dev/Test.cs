using System;
using System.Linq;
using Discord.WebSocket;

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
            await message.Channel.SendMessageAsync("Válasz");
            await Program.Log("command", message);
        }
    }
}
