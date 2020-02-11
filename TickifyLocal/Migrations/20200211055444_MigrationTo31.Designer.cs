﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tickify.Entities;

namespace Tickify.Migrations
{
    [DbContext(typeof(TickifyContext))]
    [Migration("20200211055444_MigrationTo31")]
    partial class MigrationTo31
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Tickify.Entities.Guild", b =>
                {
                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned")
                        .HasMaxLength(25);

                    b.Property<string>("CommandPrefix")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(3) CHARACTER SET utf8mb4")
                        .HasMaxLength(3)
                        .HasDefaultValue("!");

                    b.Property<string>("NewTicketMessage")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(1500) CHARACTER SET utf8mb4")
                        .HasMaxLength(1500)
                        .HasDefaultValue(", this is your chat to voice your complaint to the support team. When everything is finished between you and the support team, please do !close!");

                    b.Property<bool>("RandomTicketNames")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SupportTeam")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20)
                        .HasDefaultValue("Support Team");

                    b.Property<string>("TicketCategory")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20)
                        .HasDefaultValue("tickets");

                    b.Property<string>("TicketPrefix")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(20) CHARACTER SET utf8mb4")
                        .HasMaxLength(20)
                        .HasDefaultValue("ticket");

                    b.HasKey("GuildId");

                    b.HasIndex("GuildId")
                        .IsUnique();

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("Tickify.Entities.Ticket", b =>
                {
                    b.Property<int>("TicketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ChannelId")
                        .IsRequired()
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25)
                        .IsUnicode(false);

                    b.Property<ulong>("GuildId")
                        .HasColumnType("bigint unsigned");

                    b.Property<string>("Subject")
                        .HasColumnType("varchar(500) CHARACTER SET utf8mb4")
                        .HasMaxLength(500);

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("varchar(25) CHARACTER SET utf8mb4")
                        .HasMaxLength(25)
                        .IsUnicode(false);

                    b.HasKey("TicketId");

                    b.HasIndex("GuildId");

                    b.HasIndex("TicketId")
                        .IsUnique();

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("Tickify.Entities.Ticket", b =>
                {
                    b.HasOne("Tickify.Entities.Guild", "Guild")
                        .WithMany("Tickets")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}