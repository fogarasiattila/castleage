﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using backend.Persistence;

#nullable disable

namespace webbot.Migrations
{
    [DbContext(typeof(BotContext))]
    partial class BotContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.3");

            modelBuilder.Entity("backend.Persistence.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ArmyCode")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Cookie")
                        .HasMaxLength(1024)
                        .HasColumnType("TEXT");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("EmailPassword")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("PlayerCode")
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("backend.Persistence.Settings", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.HasKey("Name");

                    b.ToTable("Settings");
                });
#pragma warning restore 612, 618
        }
    }
}
