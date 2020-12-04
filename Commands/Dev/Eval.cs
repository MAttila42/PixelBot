using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using PixelBot.Json;

namespace PixelBot.Commands.Dev
{
    class Eval
    {
        public static List<ulong> AllowedRoles =
            new List<ulong>(BaseConfig.GetConfig().Roles.Admin);

        public static string[] Aliases =
        {
            "eval"
        };

        public async static void DoCommand()
        {
            await Program.Log("command");

            var message = Recieved.Message;
            string code;
            try { code = message.Content.Substring(6, message.Content.Length - 6); }
            catch (Exception)
            {
                await message.Channel.SendMessageAsync("❌ Add code to evaluate!");
                return;
            }
            string result;
            try { result = CSharpScript.EvaluateAsync(code).Result.ToString(); }
            catch (Exception e) { result = e.Message; }
            await message.Channel.SendMessageAsync(result);
        }
    }
}
