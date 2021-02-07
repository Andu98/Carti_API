using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carti_API.Dtos
{
    public class BookDto
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Titlu { get; set; }
        public DateTime? DataPublicarii { get; set; }
    }
}
