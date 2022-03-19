﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Tuldok.Bowling.Repo.Data;

#nullable disable

namespace Tuldok.Bowling.Repo.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20220318090309_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.3");

            modelBuilder.Entity("Tuldok.Bowling.Data.Entities.Frame", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GameId")
                        .HasColumnType("TEXT");

                    b.Property<int>("SequenceNumber")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Frames", (string)null);
                });

            modelBuilder.Entity("Tuldok.Bowling.Data.Entities.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Games", (string)null);
                });

            modelBuilder.Entity("Tuldok.Bowling.Data.Entities.Shot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("FallenPins")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("FrameId")
                        .HasColumnType("TEXT");

                    b.Property<int>("SequenceNumber")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FrameId");

                    b.ToTable("Shots");
                });

            modelBuilder.Entity("Tuldok.Bowling.Data.Entities.Frame", b =>
                {
                    b.HasOne("Tuldok.Bowling.Data.Entities.Game", "Game")
                        .WithMany("Frames")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Tuldok.Bowling.Data.Entities.Shot", b =>
                {
                    b.HasOne("Tuldok.Bowling.Data.Entities.Frame", "Frame")
                        .WithMany("Shots")
                        .HasForeignKey("FrameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Frame");
                });

            modelBuilder.Entity("Tuldok.Bowling.Data.Entities.Frame", b =>
                {
                    b.Navigation("Shots");
                });

            modelBuilder.Entity("Tuldok.Bowling.Data.Entities.Game", b =>
                {
                    b.Navigation("Frames");
                });
#pragma warning restore 612, 618
        }
    }
}
