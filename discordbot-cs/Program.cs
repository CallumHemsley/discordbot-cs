﻿using Discord;
using Discord.Legacy;
using System;
using System.Linq;
using System.Threading;

namespace discordbot_cs
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new DiscordClient();

            #pragma warning disable CS1998

            // Async method lacks 'await' operators and will run synchronously
            client.MessageReceived += async (s, e) => //e = event basically..
            {
                // bot ignores itself.
                if (e.Message.IsAuthor) return;

                // logs to console.
                Console.WriteLine(e.Message);

                // message command switch.
                switch (e.Message.Text)
                {
                    case "hello":
                        await e.Channel.SendMessage("waddup");
                        break;

                    case "time":
                        get_time(e);
                        break;

                    case "exit":
                        exit_bot(e);
                        break;

                    case "clean":
                        clean(e);
                        break;

                    default:
                        break;
                }
            };
            #pragma warning restore CS1998
            
            //Convert our sync method to an async one and block the Main function until the bot disconnects
            client.ExecuteAndWait(async () =>
            {
                while (true)
                {
                    try
                    {
                        await client.Connect("MTcwOTIxNjQwMDE4OTY4NTc2.CgC43A.YehTx9EojzDxU4NrLoIr4hQu3XQ");
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        Thread.Sleep(1000);
                    }
                }
            });

        }

        private static async void exit_bot(MessageEventArgs e)
        {
            if (e.User.ToString() != "Nopply#9852") return;
            await e.Channel.SendMessage("Exiting... :skull_crossbones: ");
            Thread.Sleep(1000);
            Environment.Exit(0);
        }

        private static async void get_time(MessageEventArgs e)
        {
            DateTime now = DateTime.Now;
            await e.Channel.SendMessage(now.ToString() + " :alarm_clock:");
        }

        private static async void clean(MessageEventArgs e)
        {

        }
    }
}