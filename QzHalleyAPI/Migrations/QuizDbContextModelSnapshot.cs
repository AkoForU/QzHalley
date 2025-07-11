﻿// <auto-generated />
using System;
using Main.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace QzHalleyAPI.Migrations
{
    [DbContext(typeof(QuizDbContext))]
    partial class QuizDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.5");

            modelBuilder.Entity("Main.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CorrectAnswer")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Option1")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Option2")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Option3")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Option4")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("QuestionText")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("img")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Questions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CorrectAnswer = "Paris",
                            Option1 = "Berlin",
                            Option2 = "Paris",
                            Option3 = "Madrid",
                            Option4 = "Rome",
                            QuestionText = "What is the capital of France?",
                            img = "qzphotos/question_20250609_1330_1.jpg"
                        },
                        new
                        {
                            Id = 2,
                            CorrectAnswer = "Madrid",
                            Option1 = "Lisbon",
                            Option2 = "Madrid",
                            Option3 = "Barcelona",
                            Option4 = "Seville",
                            QuestionText = "What is the capital of Spain?",
                            img = "qzphotos/question_20250609_1330_2.jpg"
                        });
                });

            modelBuilder.Entity("Main.Models.QuizResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("INTEGER");

                    b.Property<int>("QuestionId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SelectedOption")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("QuestionId");

                    b.HasIndex("UserId");

                    b.ToTable("QuizResults");
                });

            modelBuilder.Entity("Main.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("CompletedQuiz")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Created")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Score")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "TestUser",
                            Password = "password123",
                            Score = "0"
                        });
                });

            modelBuilder.Entity("Main.Models.QuizResult", b =>
                {
                    b.HasOne("Main.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Main.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
