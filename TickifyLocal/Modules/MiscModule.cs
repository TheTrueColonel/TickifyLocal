using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Tickify.Modules {
    public class MiscModule : ModuleBase {
        private readonly DiscordSocketClient _client;

        public MiscModule (DiscordSocketClient client) => _client = client;

        [Command("help", true)]
        [RequireUserPermission(GuildPermission.SendMessages)]
        private async Task HelpAsync () {
            await Context.Message.DeleteAsync();

            var builder = new EmbedBuilder()
                .WithTitle("Commands")
                .WithColor(51, 140, 209)
                .AddField("!help", "Shows this command.")
                .AddField("!info", "Shows info of this bot.")
                .AddField("!new", "Creates a new ticket.")
                .AddField("!new Subject", "Creates a new ticket with the given subject.")
                .AddField("!close", "**Can only be used in tickets.** Closes your currently open ticket.\nUsable by everyone who can access a ticket.")
                .Build();

            await ReplyAsync(embed: builder);
        }

        [Command("info", true)]
        [RequireUserPermission(GuildPermission.SendMessages)]
        private async Task InfoAsync () {
            await Context.Message.DeleteAsync();
            
            var builder = new EmbedBuilder()
                .WithDescription("Tickify is the open source ticketing service for Discord!")
                .WithColor(51, 140, 209)
                .AddField("Library", "Discord.NET", true)
                .AddField("Library Version", "2.2.0-dev-20191226.1", true)
                .AddField("Bot Version", Program.Version, true)
                .AddField("Latency", $"{_client.Latency}ms", true)
                .Build();

            await ReplyAsync(embed: builder);
        }
    }
}