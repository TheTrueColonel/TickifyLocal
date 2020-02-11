using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Tickify.Services;

namespace Tickify.Modules {
    public class TicketModule : ModuleBase {
        private readonly DatabaseService _databaseService;
        private readonly TicketService _ticketService;

        public TicketModule (DatabaseService databaseService, TicketService ticketService) {
            _databaseService = databaseService;
            _ticketService = ticketService;
        }

        /// <summary>
        /// Creates a private channel only accessible by the mods, admins, and the user who used the command.
        /// </summary>
        [Command("new", RunMode = RunMode.Async), Summary("Opens a private channel to complain.")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(GuildPermission.ViewChannel)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        private async Task TicketAsync ([Remainder] string subject = null) {
            subject = subject?.Replace("\"", "");

            var guild = _databaseService.GetGuildSettings(Context.Guild as SocketGuild);

            var categoryId = await _ticketService.CheckForExistingCategoryIdAsync(Context, guild.TicketCategory);

            await Context.Message.DeleteAsync();

            var supportRole = Context.Guild.Roles.FirstOrDefault(x => x.Name.Equals(guild.SupportTeam, StringComparison.CurrentCultureIgnoreCase));

            if (supportRole == null) {
                await ReplyAsync($"You don't have a role named \"{guild.SupportTeam}\"! Please make a role with that name!");
                return;
            }

            var newChannel = await _ticketService.CheckForExistingTextChannelAsync(Context, guild, categoryId, false, Context.User as SocketGuildUser);

            if (newChannel == null) {
                return;
            }

            await _ticketService.CreateTicketAsync(Context, guild, newChannel, supportRole, subject, false,  Context.User as SocketGuildUser);
        }
        
        /// <summary>
        /// Creates a private channel only accessible by the mods, admins, and the user who used the command.
        /// </summary>
        [Command("new", RunMode = RunMode.Async), Summary("Opens a private channel to complain.")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(GuildPermission.ViewChannel)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        private async Task TicketAsync (SocketUser user, [Remainder] string subject = null) {
            var supportInvoked = user != null;

            subject = subject?.Replace("\"", "");
            
            var guildSettings = _databaseService.GetGuildSettings((SocketGuild) Context.Guild);

            var categoryId = await _ticketService.CheckForExistingCategoryIdAsync(Context, guildSettings.TicketCategory);

            await Context.Message.DeleteAsync();

            var supportRole = Context.Guild.Roles.FirstOrDefault(x => x.Name.Equals(guildSettings.SupportTeam, StringComparison.CurrentCultureIgnoreCase));

            if (supportRole == null) {
                await ReplyAsync($"You don't have a role named \"{guildSettings.SupportTeam}\"! Please make a role with that name!");
                return;
            }

            if (user != null && !((SocketGuildUser) Context.User).Roles.Contains(supportRole)) {
                await ReplyAsync($"You don't have the role \"{guildSettings.SupportTeam}\"! You cannot create a ticket for another user!");
                return;
            }

            user = user ?? Context.User as SocketGuildUser;
            
            var newChannel = await _ticketService.CheckForExistingTextChannelAsync(Context, guildSettings, categoryId, supportInvoked, user);

            if (newChannel == null) {
                return;
            }

            await _ticketService.CreateTicketAsync(Context, guildSettings, newChannel, supportRole, subject, supportInvoked, user);
        }
        
        /// <summary>
        /// Closes the ticket. No check on who used it, as unless a member of staff changes permissions, the only ones able to use this in a complaint
        /// channel is the user who created the channel and staff.
        /// </summary>
        [Command("close", true, RunMode = RunMode.Async), Summary("Closes the ticket")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        [RequireBotPermission(GuildPermission.ManageChannels)]
        [RequireBotPermission(GuildPermission.ViewChannel)]
        [RequireBotPermission(GuildPermission.EmbedLinks)]
        [RequireBotPermission(GuildPermission.SendMessages)]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        private async Task CloseAsync () {
            await Context.Message.DeleteAsync();

            var guild = _databaseService.GetGuildSettings((SocketGuild) Context.Guild);
            var ticketClient = _databaseService.GetTicketOwner((SocketGuild) Context.Guild, (SocketChannel) Context.Channel);

            var channel = (INestedChannel) Context.Channel;

            var category = Context.Guild.GetCategoriesAsync().Result.FirstOrDefault(c =>
                c.Name.Equals(guild.TicketCategory, StringComparison.OrdinalIgnoreCase));

            if (category != null && channel.CategoryId == category.Id) {
                await Context.Guild.GetChannelAsync(Context.Channel.Id).Result.DeleteAsync();

                if (Context.User.Id != ticketClient) {
                    await _databaseService.CloseTicketAsync((SocketGuild) Context.Guild, (SocketChannel) Context.Channel);
                } else {
                    await _databaseService.CloseTicketAsync((SocketGuild) Context.Guild, (SocketUser) Context.User);
                }
            }
        }
    }
}