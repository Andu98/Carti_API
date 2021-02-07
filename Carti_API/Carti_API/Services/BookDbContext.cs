using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartiAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Carti_API.Services
{
    public class BookDbContext:DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options)
        :base(options)
        {
            //orice schimbare in cod va afecta si baza de date
            Database.Migrate();

        }
        //DbSet creaza tabele conform proprietatiilor din clasa. Vor fi setate virtual, pentru ca Entity Framework sa le poata suprascrie.
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Autor> Autori { get; set; }
        public virtual DbSet<BookAutor> BookAutori { get; set; }
        public virtual DbSet<BookCategorie> BookCategorii { get; set; }
        public virtual DbSet<Categorie> Categorii { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Reviewer> Reviewers { get; set; }
        public virtual DbSet<Tara> Tari{ get; set; }

        //pentru cele 2 relatii Many-To-Many (BookAutor si BookCategorii)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //pentru relatia ManyToMany Book-Categorie
            modelBuilder.Entity<BookCategorie>()
                //setare chei primare
                .HasKey(bc => new {bc.BookId, bc.CategorieId});

            modelBuilder.Entity<BookCategorie>()
                //setare relatii: 1) Cartile au mai multe categorii
                .HasOne(b => b.Book)              //Avem o carte
                .WithMany(bc => bc.BookCategorii)       // in mai multe categorii
                .HasForeignKey(b => b.BookId);  //iar cheia primara este CategorieId


            modelBuilder.Entity<BookCategorie>()
                //2)Categoriile au mai multe carti:
                .HasOne(c => c.Categorie)            
                .WithMany(bc => bc.BookCategorii)       
                .HasForeignKey(c => c.CategorieId);

            //pentru relatia ManyToMany Book-Autor
            modelBuilder.Entity<BookAutor>()
                .HasKey(ba => new {ba.BookId, ba.AutorId});

            modelBuilder.Entity<BookAutor>()
                .HasOne(b => b.Book)
                .WithMany(ba => ba.BookAutori)
                .HasForeignKey(b => b.BookId);

            modelBuilder.Entity<BookAutor>()
                .HasOne(a => a.Autor)
                .WithMany(ba => ba.BookAutori)
                .HasForeignKey(a => a.AutorId);

        }

    }
}
