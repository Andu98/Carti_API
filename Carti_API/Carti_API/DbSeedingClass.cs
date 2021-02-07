using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carti_API.Services;
using CartiAPI.Models;

namespace Carti_API
{
    public static class DbSeedingClass
    {
        public static void SeedDataContext(this BookDbContext context)
        {
            //prin popularizarea tabelului BookAutor putem ajunge sa populam toate celelalte tabele
            //Book populeaza-> Book, BookCategorie (+Categorie) , Reviews (+Revieweri)
            //Autor populeaza -> Autor (+Tara)
            var booksAutori = new List<BookAutor>()
            {
                new BookAutor(){
                Book = new Book()
                {
                    ISBN = "124",
                    Titlu = "Harry Potter si Printul Semipur",
                    DataPublicarii = new DateTime(2006,10,21),
                    BookCategorii = new List<BookCategorie>()
                    {
                        new BookCategorie{Categorie = new Categorie(){Nume = "Actiune"}},
                        new BookCategorie{Categorie = new Categorie(){Nume = "Fantezie"}}
                    },
                    Reviews = new List<Review>()
                    {
                        new Review
                        {
                            Headline = "Foarte interesanta, plina de magie.", ReviewText = "Am fost placut surprins sa citesc aceasta carte cu Harry Potter",Rating = 5,
                            Reviewer = new Reviewer(){Nume = "Tudor",Prenume = "Alexandru"}},
                        new Review
                        {
                            Headline = "Cartea nu m-a surprins in mod deosebit.", ReviewText = "Ma asteptam la mai mult venind din partea lu R.K.Rowling",Rating = 2,
                            Reviewer = new Reviewer(){Nume = "Stanciu",Prenume="Mihai"}}

                    }
                },
                Autor=new Autor()
                {
                    Nume = "J.K.",
                    Prenume = "Rowling",
                    Tara = new Tara()
                    {
                        Nume = "Anglia"
                    }
                }
                },
                new BookAutor()
                {
                    Book = new Book()
                    {
                        ISBN = "342",
                        Titlu = "Sapiens",
                        DataPublicarii = new DateTime(2010,5,20),
                        BookCategorii = new List<BookCategorie>()
                        {
                            new BookCategorie{Categorie = new Categorie(){Nume = "Dezvoltare personala"}}
                        },
                        Reviews = new List<Review>()
                        {
                            new Review()
                            {
                                Headline = "Cartea m-a facut sa realizez.",ReviewText = "Cu ajutorul acestei carti am descoperit lucruri fabuloase despre oameni",Rating = 5,
                                Reviewer = new Reviewer(){Nume = "Rotaru ", Prenume = "Florin"}},
                            new Review()
                            {
                                Headline = "O carte de exceptie." ,ReviewText = "Cea mai buna carte pe care am citit-o vreodata",Rating = 5,
                                Reviewer = new Reviewer(){Nume = "Conea", Prenume = "Mihai"}}
                        }
                    },
                        Autor=new Autor(){
                            Nume= "Yuval",
                            Prenume = "Noah",
                            Tara = new Tara()
                            {
                                Nume = "Israel"
                            }
                        }

                },
                new BookAutor()
                {
                    Book = new Book()
                    {
                        ISBN = "532",
                        Titlu = "Ion",
                        DataPublicarii = new DateTime(1920,11,20),
                        BookCategorii = new List<BookCategorie>()
                        {
                            new BookCategorie{Categorie = new Categorie(){Nume = "Literatura"}}
                        },
                        Reviews = new List<Review>()
                        {
                            new Review()
                            {
                                Headline = "O carte marcanta.",ReviewText = "O carte deosebita redactata de Liviu Rebreanu",Rating = 5,
                                Reviewer = new Reviewer(){Nume = "Radu ", Prenume = "Marius"}},
                            new Review()
                            {
                                Headline = "Mai buna decat filmul." ,ReviewText = "Filmul de 3 ore nu cuprinde toate scenele importante din carte",Rating = 4,
                                Reviewer = new Reviewer(){Nume = "Ilie", Prenume = "Daniel"}},
                            new Review()
                            {
                                Headline = "Am citit-o pentru Bacalaureat.",ReviewText = "Principalul motiv pentru care am citit-o a fost examenul de Bacalaureat",Rating = 2,
                                Reviewer = new Reviewer(){Nume="Tanase",Prenume = "Sergiu"}}
                        }
                    },
                    Autor=new Autor(){
                        Nume= "Rebreanu",
                        Prenume = "Liviu",
                        Tara = new Tara()
                        {
                            Nume = "Romania"
                        }
                    }

                },
                new BookAutor()
                {
                    Book= new Book()
                    {
                        ISBN = "442",
                        Titlu = "Cei trei muschetari",
                        DataPublicarii = new DateTime(1844,3,12),
                        BookCategorii = new List<BookCategorie>()
                        {
                          new BookCategorie{Categorie= new Categorie(){Nume="Istoric"}}
                        },
                        Reviews = new List<Review>()
                        {
                            new Review()
                            {
                                Headline = "O carte istorica foarte motivationala.",ReviewText = "Am citit cartea foarte repede si am fost marcat de aventurile lui D'artagnan",Rating = 5,
                                Reviewer = new Reviewer(){Nume="Rosu",Prenume = "Marian"}}
                            }
                            },
                    Autor = new Autor(){
                        Nume = "Alexandre",
                        Prenume = "Dumas",
                        Tara = new Tara()
                        {
                            Nume = "Franta"
                        }

                    }
                }
            };
            context.BookAutori.AddRange(booksAutori);
            context.SaveChanges();
        }
    }
}
