using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using RssReader.Classes;

namespace RssReader.Migrations
{
    [DbContext(typeof(MainViewModel))]
    [Migration("20161229091957_DbMigration")]
    partial class DbMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("RssReader.Classes.Channel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Link");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.ToTable("ChannelsDB");
                });

            modelBuilder.Entity("RssReader.Classes.RSSItem", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ChannelID");

                    b.Property<string>("Description");

                    b.Property<string>("Link");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.HasIndex("ChannelID");

                    b.ToTable("RSSItemsDB");
                });

            modelBuilder.Entity("RssReader.Classes.RSSItem", b =>
                {
                    b.HasOne("RssReader.Classes.Channel", "Channel")
                        .WithMany("RSSItems")
                        .HasForeignKey("ChannelID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
