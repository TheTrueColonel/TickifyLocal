using System.Linq;
using System.Threading.Tasks;
using Discord.WebSocket;
using Tickify.Entities;

namespace Tickify.Services {
    public class DatabaseService {
        public bool CheckConnection () {
            using var db = new TickifyContext();
            return db.Database.CanConnectAsync().Result;
        }
        
        public async Task AddOwnedServerAsync (SocketGuild guild) {
            await using var db = new TickifyContext();
            var newGuild = new Guild {
                GuildId = guild.Id,
                CommandPrefix = Program.Settings.CommandPrefix,
                RandomTicketNames = true
            };

            await db.Guilds.AddAsync(newGuild);
            await db.SaveChangesAsync();
        }

        public async Task RemoveOwnedServerAsync (SocketGuild guild) {
            await using var db = new TickifyContext();
            var dbguild = db.Guilds.FirstOrDefault(x => x.GuildId == guild.Id);

            db.Guilds.Remove(dbguild);
            await db.SaveChangesAsync();
        }

        public Guild GetGuildSettings (SocketGuild guild) {
            using var db = new TickifyContext();
            return db.Guilds.FirstOrDefault(x => x.GuildId == guild.Id);
        }

        public Ticket GetActiveTicket (SocketGuild guild, SocketUser user) {
            using var db = new TickifyContext();
            return db.Tickets.AsEnumerable().Where(x => x.GuildId == guild.Id &&
                                         x.UserId == user.Id).FirstOrDefault(y => y.Active);
        }

        public ulong GetTicketOwner (SocketGuild guild, SocketChannel channel) {
            using var db = new TickifyContext();
            return db.Tickets.AsEnumerable().Where(x => x.GuildId == guild.Id &&
                                                       x.ChannelId == channel.Id).Select(y => y.UserId).FirstOrDefault();
        }

        public async Task CreateNewTicketAsync (SocketGuild guild, ulong channelId, ulong userId, string subject) {
            await using var db = new TickifyContext();
            var newTicket = new Ticket {
                GuildId = guild.Id,
                ChannelId = channelId,
                UserId = userId,
                Subject = subject,
                Active = true
            };

            await db.AddAsync(newTicket);
            await db.SaveChangesAsync();
        }

        public async Task CloseTicketAsync (SocketGuild guild, SocketUser user) {
            await using var db = new TickifyContext();
            var ticket = db.Tickets.AsQueryable().Where(x => x.GuildId == guild.Id && 
                                                        x.UserId == user.Id).FirstOrDefault(y => y.Active);

            db.Tickets.Update(ticket).Entity.Active = false;
            await db.SaveChangesAsync();
        }

        public async Task CloseTicketAsync (SocketGuild guild, SocketChannel channel) {
            await using var db = new TickifyContext();
            var ticket = db.Tickets.AsQueryable().Where(x => x.GuildId == guild.Id && 
                                                             x.ChannelId == channel.Id).FirstOrDefault(y => y.Active);

            db.Tickets.Update(ticket).Entity.Active = false;
            await db.SaveChangesAsync();
        }
    }
}