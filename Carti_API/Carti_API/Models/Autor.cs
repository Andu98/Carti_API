using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CartiAPI.Models
{
    public class Autor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100,ErrorMessage = "Maxim 100 caractere")]
        public string Nume { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Maxim 100 caractere")]
        public string Prenume { get; set; }
        public virtual Tara Tara { get; set; }
        public virtual ICollection<BookAutor> BookAutori { get; set; }
    }
}
