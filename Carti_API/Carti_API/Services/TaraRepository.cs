using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartiAPI.Models;

namespace Carti_API.Services
{
    public class TaraRepository:ITaraRepository
    {
        private BookDbContext _taraContext;

        public TaraRepository(BookDbContext taraContext)
        {
            _taraContext = taraContext;
        }

        public bool CreateTara(Tara tara)
        {
            _taraContext.Add(tara);
            return Salvare();
        }

        public bool DeleteTara(Tara tara)
        {
            _taraContext.Remove(tara);
            return Salvare();
        }

        public bool EsteDuplicatTaraNume(int taraId, string taraNume)
        {
            var tara =_taraContext.Tari.Where(t => t.Nume.Trim().ToUpper() == taraNume.Trim().ToUpper() 
                                                   && t.Id == taraId)
                .FirstOrDefault();
            return tara == null ? false : true;
        }

        public ICollection<Autor> GetAutoriDinTara(int taraId)
        {
            return _taraContext.Autori.Where(t => t.Tara.Id == taraId).ToList();
        }

        public Tara GetTara(int taraId)
        {
            return _taraContext.Tari.Where(t => t.Id == taraId).FirstOrDefault();
        }

        public Tara GetTaraAutorului(int autorId)
        {
            return _taraContext.Autori.Where(a => a.Id == autorId).Select(t => t.Tara).FirstOrDefault();
        }

        public ICollection<Tara> GetTari()
        {
            //Tari este din BookDbContext
            return _taraContext.Tari.OrderBy(t=>t.Nume).ToList();
        }

        public bool Salvare()
        {
            //SaveChanges returneaza un int cu numarul schimbarilor
            var salvat = _taraContext.SaveChanges();
            return salvat >= 0 ? true : false;
        }

        public bool TaraExista(int taraId)
        {
            //daca id-ul exista,va returna true
            return _taraContext.Tari.Any(t => t.Id == taraId);
        }

        public bool UpdateTara(Tara tara)
        {
            _taraContext.Update(tara);
            return Salvare();
        }

    }
}
