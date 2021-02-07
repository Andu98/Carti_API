using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using CartiAPI.Models;

namespace Carti_API.Services
{
    public interface ITaraRepository
    {
        ICollection<Tara> GetTari();
        Tara GetTara(int taraId);
        Tara GetTaraAutorului(int autorId);
        ICollection<Autor> GetAutoriDinTara(int taraId);
        bool TaraExista(int taraId);
        bool EsteDuplicatTaraNume(int taraId, string taraNume);
        bool CreateTara(Tara tara);
        bool UpdateTara(Tara tara);
        bool DeleteTara(Tara tara);
        bool Salvare(); 

    }
}
