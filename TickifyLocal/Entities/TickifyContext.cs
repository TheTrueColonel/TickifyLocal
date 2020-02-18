using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Tickify.Entities {
    public class TickifyContext : DbContext {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        private readonly string _connectionString;
        
        public TickifyContext() : base() {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false);

            var configuration = builder.Build();
            _connectionString = configuration.GetConnectionString("MySQLConnection");
        }

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseMySql(_connectionString);
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder) {
            modelBuilder.Entity<Guild>(entity => {
                entity.HasKey(e => e.GuildId);
                
                entity.HasIndex(e => e.GuildId)
                    .IsUnique();

                entity.Property(e => e.GuildId)
                    .HasMaxLength(25)
                    .ValueGeneratedNever();

                entity.Property(e => e.NewTicketMessage)
                    .IsRequired()
                    .HasMaxLength(1500)
                    .HasDefaultValue(", this is your chat to voice your complaint to the support team. When everything is finished between you and the support team, please do !close!");

                entity.Property(e => e.TicketPrefix)
                    .HasMaxLength(20)
                    .HasDefaultValue("ticket");

                entity.Property(e => e.TicketCategory)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue("tickets");

                entity.Property(e => e.SupportTeam)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue("Support Team");

                entity.Property(e => e.CommandPrefix)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasDefaultValue("!");
            });

            modelBuilder.Entity<Ticket>(entity => {
                entity.HasKey(e => e.TicketId);
                
                entity.HasIndex(e => e.TicketId)
                    .IsUnique();

                entity.Property(e => e.ChannelId)
                    .IsRequired()
                    .HasMaxLength(25)
                    .ValueGeneratedNever();

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(25)
                    .ValueGeneratedNever();

                entity.Property(e => e.Subject).HasMaxLength(500);
            });

            modelBuilder.Entity<Ticket>()
                .HasOne(x => x.Guild)
                .WithMany(y => y.Tickets)
                .HasForeignKey(x => x.GuildId);
        }
    }
}