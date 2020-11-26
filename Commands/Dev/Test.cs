using System;
using System.Linq;
using Discord;
using Discord.WebSocket;
using PixelBot.Json;

namespace PixelBot.Commands.Dev
{
    public class Test
    {
        public static string[] Aliases =
        {
            "test",
            "teszt"
        };
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

            string output = "";
            for (int i = 0; i < 2000; i++)
                output += "a";

            await message.Channel.SendMessageAsync(output);
        }
    }
}
