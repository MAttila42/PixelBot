using System;
using System.Collections.Generic;
using System.Diagnostics;
using PixelBot.Json;

namespace PixelBot.Commands.Dev
{
    class Restart
    {
        public static List<ulong> AllowedRoles =
            new List<ulong>(BaseConfig.GetConfig().Roles.Admin);

        public static string[] Aliases =
        {
            "restart"
        };

        public async static void DoCommand()
        {
            var message = Recieved.Message;
            string output;
            try { output = "pwd".Bash(); }
            catch (Exception) { output = "❌ Can't find bash."; }
            await message.Channel.SendMessageAsync(output);
        }
    }

    public static class ShellHelper
    {
        public static string Bash(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }
}
