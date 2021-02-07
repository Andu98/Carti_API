using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CartiAPI.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3,ErrorMessage = "Intre 3 si 200 caractere")]
        public string Headline { get; set; }

        [Required]
        [StringLength(300,MinimumLength = 3,ErrorMessage = "Intre 3 si 300")]
        public string ReviewText { get; set; }
       
        
        [Range(1,5,ErrorMessage = "Ratingul poate fi intre 1 si 5")]
        public int Rating { get; set; }
        public virtual Reviewer Reviewer { get; set; }
        public virtual Book Book { get; set; }
    }
}
