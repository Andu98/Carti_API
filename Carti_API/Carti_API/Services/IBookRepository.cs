using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using CartiAPI.Models;

namespace Carti_API.Services
{
    public interface IBookRepository
    {
        ICollection<Book> GetBooks();
        Book GetBook(int bookId);
        Book GetBook(string bookIsbn);
        decimal GetBookRating(int bookId);
        bool BookExista(int bookId);
        bool BookExista(string bookIsbn);
        bool EsteDuplicatIsbn(int bookId, string bookIsbn);
        bool CreateBook(List<int> autoriId, List<int> categoriiId, Book book);
        bool UpdateBook(List<int> autoriId, List<int> categoriiId, Book book);
        bool DeleteBook(Book book);
        bool Salvat();


    }
}
