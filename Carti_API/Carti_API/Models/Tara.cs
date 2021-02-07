using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CartiAPI.Models
{
    public class Tara
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50,MinimumLength = 3,ErrorMessage = "Minim 3 caractere, maxim 50")]
        public string Nume { get; set; }
        public virtual ICollection<Autor> Autori { get; set; }
    }
}
