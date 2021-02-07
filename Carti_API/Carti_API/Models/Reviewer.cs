using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CartiAPI.Models
{
    public class Reviewer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3,ErrorMessage = "Minim 3 maxim 100")]
        public string Nume { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Minim 3 maxim 100")]
        public string Prenume { get; set; }
        public virtual ICollection<Review>Reviews { get; set; }
    }
}
