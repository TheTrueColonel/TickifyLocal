using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Tickify.Deserialized;
using Tickify.Services;

// https://discordapp.com/oauth2/authorize?client_id=671898110276141076&scope=bot&permissions=268463120

namespace Tickify {
    public class Program {
        private CommandService _commandService;
        private DatabaseService _databaseService;
        private TicketService _ticketService;

        private IServiceProvider _services;
        
        public static DiscordSocketClient Client;
        public static Settings Settings;

        public const string Version = "1.0.0";
        
        // Basically makes Main Async
        public static void Main () {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        private async Task MainAsync () {
            DeserializeFiles();
            
            Client = new DiscordSocketClient(new DiscordSocketConfig {
                AlwaysDownloadUsers = true,
                MessageCacheSize = 50,
                LogLevel = LogSeverity.Verbose
            });
            
            _commandService = new CommandService(new CommandServiceConfig {
                CaseSensitiveCommands = false,
                DefaultRunMode = RunMode.Async
            });
            
            _databaseService = new DatabaseService();
            _ticketService = new TicketService(_databaseService);

            _services = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(_commandService)
                .AddSingleton(_databaseService)
                .AddSingleton(_ticketService)
                .BuildServiceProvider();

            _databaseService.CheckConnection();

            await InstallCommandsAsync();

            Client.Log += Logger;

            await Client.LoginAsync(TokenType.Bot, Settings.Token);
            await Client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task InstallCommandsAsync () {
            Client.MessageReceived += HandleCommandAsync;
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandAsync (SocketMessage messageParam) {
            if (!(messageParam is SocketUserMessage message)) {
                return;
            }

            var argPos = 0;

            if (!(message.HasCharPrefix(Settings.CommandPrefix, ref argPos) || message.HasMentionPrefix(Client.CurrentUser, ref argPos))) {
                return;
            }

            var context = new SocketCommandContext(Client, message);
            var result = await _commandService.ExecuteAsync(context, argPos, _services);
            if (!result.IsSuccess) {
                if (result.Error == CommandError.BadArgCount || result.Error == CommandError.UnmetPrecondition) {
                    await context.Channel.SendMessageAsync(result.ErrorReason);
                }
            }
        }
        
        private static Task Logger (LogMessage message) {
            var consoleColor = Console.ForegroundColor;
            Console.ForegroundColor = message.Severity switch {
                LogSeverity.Critical => ConsoleColor.Red,
                LogSeverity.Error => ConsoleColor.Red,
                LogSeverity.Warning => ConsoleColor.Yellow,
                LogSeverity.Info => ConsoleColor.White,
                LogSeverity.Verbose => ConsoleColor.DarkGray,
                LogSeverity.Debug => ConsoleColor.DarkGray,
                _ => throw new ArgumentOutOfRangeException()
            };

            Console.WriteLine($"{DateTime.Now} [{message.Severity} {message.Source}: {message.Message}]");
            Console.ForegroundColor = consoleColor;
            return Task.CompletedTask;
        }

        private static void DeserializeFiles () {
            using var file = File.OpenText(@"settings.json");
            Settings = JsonConvert.DeserializeObject<Settings>(file.ReadToEnd());
        }
    }
}