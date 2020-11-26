using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using PixelBot.Json;

namespace PixelBot.Commands.Main
{
    class Rank
    {
        public static string[] Aliases()
        {
            string[] aliases =
            {
                "rank",
                "level",
                "lvl",
                "xp",
                "szint"
            };
            return aliases;
        }
        public async static void DoCommand(SocketMessage message)
        {
            var members = Member.PullData();
            int xp = members[members.IndexOf(members.Find(x => x.ID == message.Author.Id))].XP;
            string progressBar = "";
            int partXp = xp;
            int rankup = 30;
            int totalXpNeeded = rankup;
            byte rank = 0;

            List<Member> orderedMembers = new List<Member>();
            foreach (var i in members.OrderByDescending(x => x.XP))
                orderedMembers.Add(i);
            int position = orderedMembers.IndexOf(members.Find(x => x.ID == message.Author.Id)) + 1;

            while (partXp >= rankup)
            {
                rank++;
                partXp -= rankup;
                rankup += rankup / 5;
                totalXpNeeded += rankup;
            }

            byte progress = (byte)((double)partXp / rankup * 100);
            progress = (byte)(progress / 100.0 * 32);
            for (int i = 0; i < progress; i++)
                progressBar += "█";
            for (int i = 0; i < 32 - progress; i++)
                progressBar += " ";

            var embed = new EmbedBuilder()
                .WithAuthor(author =>
                {
                    author
                        .WithName(message.Author.Username)
                        .WithIconUrl("https://cdn.discordapp.com/attachments/781164873458778133/781180739089334302/XP.png");
                })
                .WithDescription($":trophy: Position: #**{position}**\n:beginner: XP: **{xp}** /{totalXpNeeded}\n:medal: Rank: **{rank}**\n\nProgress:\n`{progressBar}`")
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
