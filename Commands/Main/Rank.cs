using System.Collections.Generic;
using System.Linq;
using Discord;
using Discord.WebSocket;
using PixelBot.Json;

namespace PixelBot.Commands.Main
{
    class Rank
    {
        public static string[] Aliases =
        {
            "rank",
            "level",
            "lvl",
            "xp",
            "szint"
        };
        public async static void DoCommand(SocketMessage message)
        {
            await Program.Log("command", message);

            string[] m = message.Content.Split();
            ulong id = message.Author.Id;
            if (m.Length == 2)
                id = Program.GetUserId(message, m[1]);
            if (m.Length > 2)
            {
                await message.Channel.SendMessageAsync("❌ Too many parameters!");
                return;
            }
            if (id == 0)
                return;
            var members = Members.PullData();
            int xp;
            int memberIndex = Members.GetMemberIndex(message, members, id.ToString());
            if (memberIndex == -1)
            {
                memberIndex = members.Count();
                members.Add(new Members(id));
            }
            xp = members[memberIndex].XP;
            string progressBar = "";
            int partXp = xp;
            int rankup = 30;
            int totalXpNeeded = rankup;
            byte rank = 0;

            List<Members> orderedMembers = new List<Members>();
            foreach (var i in members.OrderByDescending(x => x.XP))
                orderedMembers.Add(i);
            int position = Members.GetMemberIndex(message, orderedMembers, id.ToString()) + 1;
            int percent = (int)((double)position / members.Count() * 100);

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
                        .WithName(Program._client.GetUser(id).Username)
                        .WithIconUrl("https://cdn.discordapp.com/attachments/781164873458778133/781906608376381470/XP.png");
                })
                .WithDescription($":trophy: Position: #**{position}** (Top **{percent}**%)\n:beginner: XP: **{xp}** /{totalXpNeeded}\n:medal: Rank: **{rank}**\n\nProgress:\n`{progressBar}`")
                .WithFooter(((SocketGuildChannel)message.Channel).Guild.Name)
                .WithThumbnailUrl(Program._client.GetUser(id).GetAvatarUrl())
                .WithColor(new Color(0xFFCC00)).Build();
            await message.Channel.SendMessageAsync(
                null,
                embed: embed)
                .ConfigureAwait(false);
        }
    }
}
