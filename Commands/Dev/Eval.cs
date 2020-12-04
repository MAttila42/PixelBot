using System;
using System.Collections.Generic;
using PixelBot.Json;

namespace PixelBot.Commands.Dev
{
    class Eval
    {
        public static List<ulong> AllowedRoles =
            new List<ulong>(BaseConfig.GetConfig().Roles.Admin);

        public static string[] Aliases = { "eval" };

        public async static void DoCommand()
        {
            await Program.Log("command");

            var message = Recieved.Message;
            var response = await message.Channel.SendMessageAsync("Evaluating...");
            string code;
            try { code = message.Content.Substring(6, message.Content.Length - 6); }
            catch (Exception)
            {
                await message.Channel.SendMessageAsync("❌ Add code to evaluate!");
                return;
            }
            string result;
            try { result = Z.Expressions.Eval.Execute(code).ToString(); }
            catch (Exception e) { result = e.Message; }
            await response.ModifyAsync(m => m.Content = $"```{result}```");
        }
    }
}
