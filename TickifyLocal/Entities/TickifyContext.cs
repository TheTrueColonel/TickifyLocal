using Microsoft.EntityFrameworkCore;

namespace Tickify.Entities {
    public class TickifyContext : DbContext {
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseMySql(Program.Settings.ConnectionString);
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