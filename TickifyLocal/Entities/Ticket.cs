namespace Tickify.Entities {
    public class Ticket {
        public int TicketId { get; set; }
        public ulong ChannelId { get; set; }
        public ulong UserId { get; set; }
        public string Subject { get; set; }
        public bool Active { get; set; }
        
        public ulong GuildId { get; set; }
        public Guild Guild { get; set; }
    }
}