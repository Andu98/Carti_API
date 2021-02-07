using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartiAPI.Models;

namespace Carti_API.Services
{
    public interface IAutorRepository
    {
        ICollection<Autor> GetAutori();
        Autor GetAutor(int autorId);
        ICollection<Autor> GetAutoriCarte(int bookId);
        ICollection<Book> GetCartiAleAutorului(int autorId);
        bool AutorulExista(int autorId);
        bool CreateAutor(Autor autor);
        bool UpdateAutor(Autor autor);
        bool DeleteAutor(Autor autor);
        bool Salvare();

    }
}
