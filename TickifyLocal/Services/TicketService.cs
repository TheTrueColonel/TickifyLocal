using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Tickify.Entities;
using Tickify.Extensions;

namespace Tickify.Services {
    public class TicketService {
        private readonly DatabaseService _databaseService;

        public TicketService (DatabaseService databaseService) => _databaseService = databaseService;

        public async Task<ulong?> CheckForExistingCategoryIdAsync (ICommandContext context, string categoryName) {
            var categoryList = context.Guild.GetCategoriesAsync().Result;

            ulong? categoryId = null;

            foreach (var category in categoryList) {
                if (string.Equals(category.Name, categoryName, StringComparison.CurrentCultureIgnoreCase)) {
                    categoryId = category.Id;
                    break;
                }
            }

            if (categoryId == null) {
                var category = await context.Guild.CreateCategoryAsync(categoryName);
                categoryId = category.Id;
            }

            return categoryId;
        }

        public async Task<ITextChannel> CheckForExistingTextChannelAsync (ICommandContext context,
                                                                          Guild guild,
                                                                          ulong? categoryId,
                                                                          bool supportInvoked,
                                                                          SocketUser user) {
            user = supportInvoked ? user : context.User as SocketGuildUser;

            var channelName = _databaseService.GetGuildSettings((SocketGuild) context.Guild).RandomTicketNames
                ? $"{guild.TicketPrefix}-{user.Id.ToString().GetSha256().Substring(0, 8)}".ToDiscordChannel()
                : $"{guild.TicketPrefix}-{user.Username}".ToDiscordChannel();

            var activeTicket = _databaseService.GetActiveTicket((SocketGuild) context.Guild, user);

            if (activeTicket != null) {
                var channel = context.Guild.GetTextChannelAsync(activeTicket.ChannelId);

                if (channel != null) {
                    if (supportInvoked) {
                        await context.Channel.SendMessageAsync($"{user.Mention} already has an open ticket! Please use that channel!").DeleteAfterSeconds(15);
                    } else {
                        await context.Channel.SendMessageAsync($"{user.Mention}, you already have an open ticket! Please use that channel!").DeleteAfterSeconds(15);
                    }
                }

                return null;
            }

            return await context.Guild.CreateTextChannelAsync(channelName, x => 
                x.CategoryId = categoryId
            );
        }

        public async Task CreateTicketAsync (ICommandContext context,
                                             Guild guild,
                                             ITextChannel textChannel,
                                             IRole supportRole,
                                             string subject,
                                             bool supportInvoked,
                                             SocketUser user) {
            user = supportInvoked ? user : context.User as SocketGuildUser;
            var botUser = (IUser) Program.Client.CurrentUser;
            
            var everyonePerms = new OverwritePermissions(viewChannel: PermValue.Allow);
            var ticketPerms = new OverwritePermissions(viewChannel: PermValue.Allow);

            await textChannel.AddPermissionOverwriteAsync(context.Guild.EveryoneRole, everyonePerms);
            await textChannel.AddPermissionOverwriteAsync(botUser, ticketPerms);
            await textChannel.AddPermissionOverwriteAsync(supportRole, ticketPerms);
            await textChannel.AddPermissionOverwriteAsync(user, ticketPerms);

            await _databaseService.CreateNewTicketAsync((SocketGuild) context.Guild, textChannel.Id, user.Id, subject);

            var embed = new EmbedBuilder()
                .WithTitle("New Ticket")
                .WithColor(51, 140, 209)
                .WithDescription($"{user.Mention}{guild.NewTicketMessage}");

            if (!string.IsNullOrEmpty(subject)) {
                embed.AddField("Subject", $"The subject is: {subject}");
            }

            if (supportInvoked) {
                embed.AddField("Created By", context.User);
            }

            await textChannel.SendMessageAsync(embed: embed.Build());
        }
    }
}