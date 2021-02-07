using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartiAPI.Models
{
    public class BookCategorie
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int CategorieId { get; set; }
        public Categorie Categorie { get; set; }

    }
}
