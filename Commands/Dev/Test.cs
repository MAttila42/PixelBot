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
            var members = Member.PullData();
            int xp = members[members.IndexOf(members.Find(x => x.ID == message.Author.Id))].XP;
            int rankup = 30;
            byte rank = 0;
            while (xp >= rankup)
            {
                rank++;
                xp -= rankup;
                rankup += rankup / 5;
            }

            var embed = new EmbedBuilder()
                .WithAuthor(author =>
                {
                    author
                        .WithName("Rank Up")
                        .WithIconUrl("https://cdn.discordapp.com/attachments/781164873458778133/781180684185108590/LevelUp.png");
                })
                .WithDescription($"Congratulations **{message.Author.Mention}**! You ranked up.\nNew level: `{rank}`")
                .WithFooter(((SocketGuildChannel)message.Channel).Guild.Name)
                .WithThumbnailUrl(message.Author.GetAvatarUrl())
                .WithColor(new Color(0xFFCC00)).Build();
            await message.Channel.SendMessageAsync(
                null,
                embed: embed)
                .ConfigureAwait(false);
            await Program.Log("command", message);
        }
    }
}
