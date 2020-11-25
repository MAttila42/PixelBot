using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace PixelBot.Json
{
    public class Member
    {
        public ulong ID { get; set; }
        public int XP { get; set; }
        public byte Rank { get; set; }

        public static List<Member> PullData()
        {
            try { return JsonSerializer.Deserialize<List<Member>>(File.ReadAllText("Members.json")); }
            catch (Exception) { File.WriteAllText("Members.json", "[]"); }
            return JsonSerializer.Deserialize<List<Member>>(File.ReadAllText("Members.json"));
        }
        public static void PushData(List<Member> list)
        {
            File.WriteAllText("Members.json", JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true }));
        }

        public Member() { } // Ez a sztupid Json deserialize miatt kell. Használd a másik konstruktort!

        public Member(ulong id) // Alap, egyszerű konstruktor, egy ID-t megadsz, és létrehozza a saját objektumát a JSON-ben, üres adatokkal.
        {
            ID = id;
            XP = 0;
            Rank = 0;
        }
        public Member(ulong id, int xp) // Direkt az XP feljegyzésére létrehozott konstruktor. Kezdeti XP értéket kap.
        {
            ID = id;
            XP = xp;
            Rank = 0;
        }
    }
}
