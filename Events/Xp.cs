using System;
using Discord.WebSocket;
using PixelBot.Json;

namespace PixelBot.Events
{
    public class Xp
    {
        public static void DoEvent(SocketMessage message)
        {
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
            members[members.IndexOf(members.Find(x => x.ID == message.Author.Id))].Rank = rank;

            Member.PushData(members);
        }
    }
}
