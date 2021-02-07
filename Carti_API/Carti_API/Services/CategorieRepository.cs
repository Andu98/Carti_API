using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartiAPI.Models;

namespace Carti_API.Services
{
    public class CategorieRepository:ICategorieRepository
    {
        private BookDbContext _categorieContext;

        public CategorieRepository(BookDbContext categorieContext)
        {
            _categorieContext = categorieContext;
        }

        public bool CategoriaExista(int categorieId)
        {
            return _categorieContext.Categorii.Any(c => c.Id == categorieId);
        }

        public bool CreateCategorie(Categorie categorie)
        {
            _categorieContext.Add(categorie);
            return Salvare();
        }

        public bool DeleteCategorie(Categorie categorie)
        {
            _categorieContext.Remove(categorie);
            return Salvare();
        }

        public bool EsteDuplicatCategorieNume(int categorieId, string categorieNume)
        {
            var categorie = _categorieContext.Categorii.Where(c => c.Nume.Trim().ToUpper() == categorieNume.Trim().ToUpper()
                                                                            && c.Id == categorieId).FirstOrDefault();
            return categorie == null ? false : true;
        }

        public ICollection<Book> GetCartiDinCateogia(int categorieId)
        {
          return _categorieContext.BookCategorii.Where(c => c.CategorieId == categorieId).Select(b => b.Book).ToList();

        }

        public Categorie GetCategorie(int categorieId)
        {
            return _categorieContext.Categorii.Where(c => c.Id == categorieId).FirstOrDefault();

        }

        public ICollection<Categorie> GetCategorii()
        {
            return _categorieContext.Categorii.OrderBy(c => c.Nume).ToList();

        }

        public ICollection<Categorie> GetCategoriiCarte(int bookId)
        {
            //in tabelul BookCategorii gasim cartea care are Id-ul cerut. Apoi selectam toate categoriile ce ii apartin. Si transformam in lista.
            return _categorieContext.BookCategorii.Where(b => b.BookId == bookId).Select(c => c.Categorie).ToList();
        }

        public bool Salvare()
        {
            var salvat = _categorieContext.SaveChanges();
            return salvat >=0 ? true :false;
        }

        public bool UpdateCategorie(Categorie categorie)
        {
            _categorieContext.Update(categorie);
            return Salvare();
        }
    }
}
