
using Discord;
using Discord.Commands;
using Discord.Commands.Permissions.Levels;
using Discord.Modules;
using Discord.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using DiscordBot;

namespace discordbot_cs
{
    class Program
    {
        static void Main(string[] args) => new Program().Start(args);

        private const string AppName = "Nopplybot";
        private DiscordClient client;

        private void Start(string[] args)
        {
            client = new DiscordClient(x =>
            {
                x.AppName = AppName;
                x.MessageCacheSize = 10;
                x.EnablePreUpdateEvents = true;
            })
            .UsingCommands(x =>
            {
                x.AllowMentionPrefix = true;
                x.PrefixChar = '-';
                x.HelpMode = HelpMode.Public;
                x.ErrorHandler = OnCommandError;
            })
            .UsingModules();

            #pragma warning disable CS1998

            client.Services.Get<CommandService>().CreateCommand("info") //create command greet
                .Description("General information about the bot.") //add description, it will be shown when ~help is used
                .Do(async e =>
                {
                    await e.Channel.SendMessage($"Information about the bot will go here..");
                    //sends a message to channel with the given text
                });

            client.Services.Get<CommandService>().CreateCommand("exit")
                .Description("Bot will exit everything, can only be used by Devs.")
                .Do(async e =>
                {
                    exit_bot(e);
                });
            // Async method lacks 'await' operators and will run synchronously
            client.MessageReceived += async (s, e) => //e = event basically..
            { 
                // bot ignores itself.
                if (e.Message.IsAuthor) return;
                if (e.Channel.ToString() == "cd_newsfeed")
                {
                    /* Regex explanation:
                    \b       -matches a word boundary (spaces, periods..etc)
                    (?:      -define the beginning of a group, the ?: specifies not to capture the data within this group.
                    https?://  - Match http or https (the '?' after the "s" makes it optional)
                    |        -OR
                    www\.    -literal string, match www. (the \. means a literal ".")
                    )        -end group
                    \S+      -match a series of non-whitespace characters.
                    \b       -match the closing word boundary. */
                    Regex linkParser = new Regex(@"\b(?:https?://|www\.)\S+\b", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                    int counter = 0;
                    foreach (Match m in linkParser.Matches(e.Message.ToString()))
                    {
                        counter++;
                    }
                    if (counter < 1)
                    {
                        await e.Channel.SendMessage("Too many/ No links in message.");
                        return;
                    }
                    //We need to count the remainder characters after the link or just be lazy and say >75 instead of spliting, up to you.

                }
                // logs to console.
                Console.WriteLine(e.Message);
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
                        client.SetGame("Type #help for help.");
                        Console.WriteLine("Connected :)");
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

        private static async void exit_bot(CommandEventArgs e)
        {
            if (e.User.ToString() != "Nopply#9852") return;
            await e.Channel.SendMessage("Exiting... :skull_crossbones: ");
            Thread.Sleep(1000);
            Environment.Exit(0);
        }

        private void OnCommandError(object sender, CommandErrorEventArgs e)
        {
            string msg = e.Exception?.Message;
            if (msg == null) //No exception - show a generic message
            {
                switch (e.ErrorType)
                {
                    case CommandErrorType.Exception:
                        msg = "Unknown error.";
                        break;
                    case CommandErrorType.BadPermissions:
                        msg = "You do not have permission to run this command.";
                        break;
                    case CommandErrorType.BadArgCount:
                        msg = "You provided the incorrect number of arguments for this command.";
                        break;
                    case CommandErrorType.InvalidInput:
                        msg = "Unable to parse your command, please check your input.";
                        break;

                }
            }
            if (msg != null)
            {
                client.ReplyError(e, msg);
            }
        }
    }
 }