using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartiAPI.Models
{
    public class BookAutor
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public int AutorId { get; set; }
        public Autor Autor { get; set; }
    }
}
