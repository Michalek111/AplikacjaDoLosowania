﻿// <auto-generated />
using System;
using AplikacjaDoLosowania.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AplikacjaDoLosowania.Migrations
{
    [DbContext(typeof(ApplicationDBContext))]
    [Migration("20250320115349_AddPlayersIdToUsers")]
    partial class AddPlayersIdToUsers
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AplikacjaDoLosowania.Models.Match", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Map")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("MatchDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Team1Players")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Team1Score")
                        .HasColumnType("int");

                    b.Property<string>("Team2Players")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Team2Score")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("AplikacjaDoLosowania.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("GamesPlayed")
                        .HasColumnType("int");

                    b.Property<int>("GamesWon")
                        .HasColumnType("int");

                    b.Property<string>("Nick")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("AplikacjaDoLosowania.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("passwordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AplikacjaDoLosowania.Models.User", b =>
                {
                    b.HasOne("AplikacjaDoLosowania.Models.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });
#pragma warning restore 612, 618
        }
    }
}
