using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartiAPI.Models;

namespace Carti_API.Services
{
    public interface ICategorieRepository
    {
        ICollection<Categorie> GetCategorii();
        Categorie GetCategorie(int categorieId);
        ICollection<Categorie> GetCategoriiCarte(int bookId);
        ICollection<Book> GetCartiDinCateogia(int categorieId);
        bool CategoriaExista(int categorieId);
        bool EsteDuplicatCategorieNume(int categorieId,string categorieNume);
        bool CreateCategorie(Categorie categorie);
        bool UpdateCategorie(Categorie categorie);
        bool DeleteCategorie(Categorie categorie);
        bool Salvare();
    }
}
