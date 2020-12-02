using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            await message.Channel.SendMessageAsync("Restarting bot... (This may take a few moments)");
            string commands =
                "cd ..\n" + 
                "git pull\n" +
                "dotnet build -o build\n" +
                "cd build\n" +
                "dotnet PixelBot.dll";
            try
            {
                commands.Bash();
                await message.Channel.SendMessageAsync("Done.");
                Environment.Exit(0);
            }
            catch (Exception) { await message.Channel.SendMessageAsync("❌ Can't find bash!"); }
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
            return result;
        }
    }
}
