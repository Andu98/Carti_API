using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CartiAPI.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

       [Required]
       [StringLength(20,MinimumLength = 2,ErrorMessage = "Minim 2 caractere, Maxim 20")]
       public string ISBN { get; set; }

        [Required]
        [MaxLength(150)]
        public string Titlu { get; set; }
        public DateTime DataPublicarii { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<BookAutor> BookAutori { get; set; }
        public virtual ICollection<BookCategorie> BookCategorii { get; set; }

    }
}
