using System;
using System.Linq;
using Discord;
using Discord.WebSocket;
using PixelBot.Json;

namespace PixelBot.Events
{
    public class Xp
    {
        public async static void DoEvent(SocketMessage message)
        {
            bool isSpam = false;
            try
            {
                var lastMessage = (await message.Channel.GetMessagesAsync(10).FlattenAsync()).Where(x => x.Author.Id == message.Author.Id).ElementAt(1);
                isSpam = lastMessage.Content == message.Content || (message.CreatedAt.DateTime - lastMessage.CreatedAt.DateTime).Seconds < 5;
            }
            catch (Exception) { }

            if (message.Content.Length == 1 ||
                isSpam ||
                BaseConfig.GetConfig().Channels.BotChannel.Contains(message.Channel.Id))
                return;

            var members = Member.PullData();
            try { members[members.IndexOf(members.Find(x => x.ID == message.Author.Id))].XP++; }
            catch (Exception) { members.Add(new Member(message.Author.Id, 1)); }

            int xp = members[members.IndexOf(members.Find(x => x.ID == message.Author.Id))].XP;
            int rankup = 30;
            byte rank = 0;
            while (xp >= rankup)
            {
                rank++;
                xp -= rankup;
                rankup += rankup / 5;
            }

            if (rank > members[members.IndexOf(members.Find(x => x.ID == message.Author.Id))].Rank)
            {
                await Program.Log("rankup", message);

                var embed = new EmbedBuilder()
                    .WithAuthor(author =>
                    {
                        author
                            .WithName("Rank Up")
                            .WithIconUrl("https://cdn.discordapp.com/attachments/781164873458778133/781180684185108590/LevelUp.png");
                    })
                    .WithDescription($"Congratulations **{message.Author.Mention}**! You ranked up.\nNew rank: **{rank}**")
                    .WithFooter(((SocketGuildChannel)message.Channel).Guild.Name)
                    .WithThumbnailUrl(message.Author.GetAvatarUrl())
                    .WithColor(new Color(0xFFCC00)).Build();

                var channels = BaseConfig.GetConfig().Channels.LevelUp;
                if (channels[0] == 0)
                    return;
                foreach (var id in channels)
                    await ((IMessageChannel)Program._client.GetChannel(id)).SendMessageAsync(
                        null,
                        embed: embed)
                        .ConfigureAwait(false);
            }

            members[members.IndexOf(members.Find(x => x.ID == message.Author.Id))].Rank = rank;

            Member.PushData(members);
        }
    }
}
