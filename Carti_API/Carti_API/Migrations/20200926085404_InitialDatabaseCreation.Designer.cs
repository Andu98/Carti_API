﻿// <auto-generated />
using System;
using Carti_API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Carti_API.Migrations
{
    [DbContext(typeof(BookDbContext))]
    [Migration("20200926085404_InitialDatabaseCreation")]
    partial class InitialDatabaseCreation
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CartiAPI.Models.Autor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nume")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Prenume")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int?>("TaraId");

                    b.HasKey("Id");

                    b.HasIndex("TaraId");

                    b.ToTable("Autori");
                });

            modelBuilder.Entity("CartiAPI.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DataPublicarii");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<string>("Titlu")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.HasKey("Id");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("CartiAPI.Models.BookAutor", b =>
                {
                    b.Property<int>("BookId");

                    b.Property<int>("AutorId");

                    b.HasKey("BookId", "AutorId");

                    b.HasIndex("AutorId");

                    b.ToTable("BookAutori");
                });

            modelBuilder.Entity("CartiAPI.Models.BookCategorie", b =>
                {
                    b.Property<int>("BookId");

                    b.Property<int>("CategorieId");

                    b.HasKey("BookId", "CategorieId");

                    b.HasIndex("CategorieId");

                    b.ToTable("BookCategorii");
                });

            modelBuilder.Entity("CartiAPI.Models.Categorie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nume")
                        .IsRequired()
                        .HasMaxLength(40);

                    b.HasKey("Id");

                    b.ToTable("Categorii");
                });

            modelBuilder.Entity("CartiAPI.Models.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BookId");

                    b.Property<string>("Headline")
                        .IsRequired()
                        .HasMaxLength(200);

                    b.Property<int>("Rating");

                    b.Property<string>("ReviewText")
                        .IsRequired()
                        .HasMaxLength(300);

                    b.Property<int?>("ReviewerId");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("ReviewerId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("CartiAPI.Models.Reviewer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nume")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Prenume")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Reviewers");
                });

            modelBuilder.Entity("CartiAPI.Models.Tara", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nume")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Tari");
                });

            modelBuilder.Entity("CartiAPI.Models.Autor", b =>
                {
                    b.HasOne("CartiAPI.Models.Tara", "Tara")
                        .WithMany("Autori")
                        .HasForeignKey("TaraId");
                });

            modelBuilder.Entity("CartiAPI.Models.BookAutor", b =>
                {
                    b.HasOne("CartiAPI.Models.Autor", "Autor")
                        .WithMany("BookAutori")
                        .HasForeignKey("AutorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CartiAPI.Models.Book", "Book")
                        .WithMany("BookAutori")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CartiAPI.Models.BookCategorie", b =>
                {
                    b.HasOne("CartiAPI.Models.Book", "Book")
                        .WithMany("BookCategorii")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CartiAPI.Models.Categorie", "Categorie")
                        .WithMany("BookCategorii")
                        .HasForeignKey("CategorieId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CartiAPI.Models.Review", b =>
                {
                    b.HasOne("CartiAPI.Models.Book", "Book")
                        .WithMany("Reviews")
                        .HasForeignKey("BookId");

                    b.HasOne("CartiAPI.Models.Reviewer", "Reviewer")
                        .WithMany("Reviews")
                        .HasForeignKey("ReviewerId");
                });
#pragma warning restore 612, 618
        }
    }
}
