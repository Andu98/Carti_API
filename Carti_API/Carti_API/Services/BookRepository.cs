using CartiAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carti_API.Services
{
    public class BookRepository:IBookRepository
    {
        private BookDbContext _bookContext;

        public BookRepository(BookDbContext bookContext)
        {
            _bookContext = bookContext;
        }

        public bool BookExista(int bookId)
        {
            return _bookContext.Books.Any(b => b.Id == bookId);
        }

        public bool BookExista(string bookIsbn)
        {
            return _bookContext.Books.Any(b => b.ISBN == bookIsbn);
        }

        public bool CreateBook(List<int> autoriId, List<int> categoriiId, Book book)
        {
            var autori = _bookContext.Autori.Where(a => autoriId.Contains(a.Id)).ToList();
            var categorii = _bookContext.Categorii.Where(c => categoriiId.Contains(c.Id)).ToList();

         foreach(var autor in autori)
         {
             var bookAutor = new BookAutor()
             {
                 Autor = autor,
                 Book = book
             };
             _bookContext.Add(bookAutor);

         }
         foreach (var categorie in categorii)
         {
             var bookCategorie = new BookCategorie()
             {
                 Categorie = categorie,
                 Book = book
             };
             _bookContext.Add(bookCategorie);
            }
            _bookContext.Add(book);

         return Salvat();
        }

        public bool DeleteBook(Book book)
        {
            _bookContext.Remove(book);
            return Salvat();
        }

        public bool EsteDuplicatIsbn(int bookId, string bookIsbn)
        {
            var book = _bookContext.Books.FirstOrDefault(b => b.ISBN.Trim().ToUpper() == bookIsbn.Trim().ToUpper() 
                                                              && b.Id != bookId);
            return book == null ? false : true;
        }

        public Book GetBook(int bookId)
        {
            return _bookContext.Books.Where(b => b.Id == bookId).FirstOrDefault();
        }

        public Book GetBook(string bookIsbn)
        {
            return _bookContext.Books.Where(b => b.ISBN == bookIsbn).FirstOrDefault();
        }

        public decimal GetBookRating(int bookId)
        {
            var review = _bookContext.Reviews.Where(r => r.Book.Id == bookId);
            if (review.Count() <= 0)
                return 0;
            return ((decimal) review.Sum(r => r.Rating) / review.Count());
        }

        public ICollection<Book> GetBooks()
        {
            return _bookContext.Books.OrderBy(b => b.Id).ToList();
        }

        public bool Salvat()
        {
            var salvat = _bookContext.SaveChanges();
            return salvat >= 0 ? true : false;
        }

        public bool UpdateBook(List<int> autoriId, List<int> categoriiId, Book book)
        {
            var autori = _bookContext.Autori.Where(a => autoriId.Contains(a.Id)).ToList();
            var categorii = _bookContext.Categorii.Where(c => categoriiId.Contains(c.Id)).ToList();

            //inainte de a adauga o inregistrare noua, stergem legatura care era intre carte si autor
            var bookAutoriDeSters = _bookContext.BookAutori.Where(b => b.BookId == book.Id);
            var categoriiAutoriDeSters = _bookContext.BookCategorii.Where(b => b.BookId == book.Id);


            _bookContext.RemoveRange(bookAutoriDeSters);
            _bookContext.RemoveRange(categoriiAutoriDeSters);

            foreach (var autor in autori)
            {
                var bookAutor = new BookAutor()
                {
                    Autor = autor,
                    Book = book
                };
                _bookContext.Add(bookAutor);

            }

            foreach (var categorie in categorii)
            {
                var bookCategorie = new BookCategorie()
                {
                    Categorie = categorie,
                    Book = book
                };
                _bookContext.Add(bookCategorie);
            }
            _bookContext.Update(book);

            return Salvat();

        }
    }
}
