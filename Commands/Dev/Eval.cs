using System;
using System.Timers;
using Discord;

namespace PixelBot.Commands.Dev
{
    class Eval
    {
        public static string[] Aliases = { "eval" };

        static IUserMessage response;
        static string result;
        static int counter;
        static Timer timer;

        public async static void DoCommand()
        {
            await Program.Log("command");

            var message = Recieved.Message;
            response = await message.Channel.SendMessageAsync("Evaluating...", allowedMentions: AllowedMentions.None);
            string code;
            try { code = message.Content.Substring(6, message.Content.Length - 6); }
            catch (Exception)
            {
                await response.ModifyAsync(m => m.Content = "❌ Add code to evaluate!");
                return;
            }
            result = null;
            counter = 0;
            try
            {
                timer = new Timer(1000);
                timer.Elapsed += CheckIfTimedOut;
                timer.AutoReset = true;
                timer.Enabled = true;
                result = Z.Expressions.Eval.Execute(code).ToString();
            }
            catch (Exception e) { result = e.Message; }
            if (result.Length <= 2000)
                await response.ModifyAsync(m => m.Content = $"```{result}```");
            else
                await response.ModifyAsync(m => m.Content = "❌ 2000+ characters!");
            timer.Enabled = false;
            timer.Stop();
            timer.Dispose();
        }

        static void CheckIfTimedOut(object source, ElapsedEventArgs e)
        {
            if (counter++ > 30 && result == null)
            {
                response.ModifyAsync(m => m.Content = "❌ Timed out!");
                timer.Enabled = false;
                timer.Stop();
                timer.Dispose();
            }
        }
    }
}
