using CartiAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carti_API.Services
{
    public class AutorRepository:IAutorRepository
    {
        private BookDbContext _autorContext;

        public AutorRepository(BookDbContext autorContext)
        {
            _autorContext = autorContext;
        }

        public bool AutorulExista(int autorId)
        {
            return _autorContext.Autori.Any(a => a.Id == autorId);

        }

        public bool CreateAutor(Autor autor)
        {
           _autorContext.Add(autor);
           return Salvare();
        }

        public bool DeleteAutor(Autor autor)
        {
            _autorContext.Remove(autor);
            return Salvare();
        }

        public Autor GetAutor(int autorId)
        {
            return _autorContext.Autori.Where(a => a.Id == autorId).FirstOrDefault();
        }

        public ICollection<Autor> GetAutori()
        {
            return _autorContext.Autori.OrderBy(a => a.Nume).ToList();
        }

        public ICollection<Autor> GetAutoriCarte(int bookId)
        {
            return _autorContext.BookAutori.Where(b => b.Book.Id == bookId).Select(a => a.Autor).ToList();
        }

        public ICollection<Book> GetCartiAleAutorului(int autorId)
        {
            return _autorContext.BookAutori.Where(a => a.Autor.Id == autorId).Select(b => b.Book).ToList();
        }

        public bool Salvare()
        {
            var salvat = _autorContext.SaveChanges();
            return salvat >= 0 ? true : false;
        }

        public bool UpdateAutor(Autor autor)
        {
            _autorContext.Update(autor);
            return Salvare();
        }
    }
}
