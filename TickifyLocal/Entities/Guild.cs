using System.Collections.Generic;

namespace Tickify.Entities {
    public class Guild {
        public ulong GuildId { get; set; }
        public string NewTicketMessage { get; set; }
        public string TicketPrefix { get; set; }
        public string TicketCategory { get; set; }
        public string SupportTeam { get; set; }
        public bool RandomTicketNames { get; set; }
        public char CommandPrefix { get; set; }
        
        public List<Ticket> Tickets { get; set; }
    }
}