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

        /// <summary>
        /// Tagok adatainak lekérése a Json-ből.
        /// </summary>
        /// <returns></returns>
        public static List<Member> PullData()
        {
            try { return JsonSerializer.Deserialize<List<Member>>(File.ReadAllText("Members.json")); }
            catch (Exception) { File.WriteAllText("Members.json", "[]"); }
            return JsonSerializer.Deserialize<List<Member>>(File.ReadAllText("Members.json"));
        }
        /// <summary>
        /// Tagok adatainak feltöltése a Json-be.
        /// </summary>
        /// <param name="list"></param>
        public static void PushData(List<Member> list)
        {
            File.WriteAllText("Members.json", JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true }));
        }

        /// <summary>
        /// Ez a sztupid Json deserialize miatt kell. Használj másik konstruktort!
        /// </summary>
        public Member() { }

        /// <summary>
        /// Alap, egyszerű konstruktor, egy ID-t megadsz, és létrehozza a saját objektumát a JSON-ben, üres adatokkal.
        /// </summary>
        /// <param name="id"></param>
        public Member(ulong id)
        {
            ID = id;
            XP = 0;
            Rank = 0;
        }
        /// <summary>
        /// Direkt az XP feljegyzésére létrehozott konstruktor. Kezdeti XP értéket kap.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="xp"></param>
        public Member(ulong id, int xp)
        {
            ID = id;
            XP = xp;
            Rank = 0;
        }
    }
}
