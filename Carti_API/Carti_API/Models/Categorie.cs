using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CartiAPI.Models
{
    public class Categorie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
       [Required]
       [StringLength(40,MinimumLength = 3,ErrorMessage = "Minim 3, Maxim 40")]
        public string Nume { get; set; }
        public virtual ICollection<BookCategorie> BookCategorii { get; set; }
    }
}
