using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carti_API.Dtos;
using Carti_API.Services;
using CartiAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Carti_API.Controllers
{
    [Route("api/tari")]
    [ApiController]
    public class TariController : Controller
    {
        private ITaraRepository _taraRepository;

        public TariController(ITaraRepository taraRepository)
        {
            _taraRepository = taraRepository;
        }

        //api/tari
        [HttpGet] //creem o actiune de tipul Get
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TaraDto>))] //OK
        public IActionResult GetTari()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //creem o lista a tuturor tarilor (cu toate proprietatile) 
            var tari = _taraRepository.GetTari().ToList();

            var tariDto = new List<TaraDto>();
            foreach (var tara in tari)
            {
                tariDto.Add(new TaraDto
                {
                    //cele 2 variabile create in TaraDto (pentru imputinarea proprietatilor afisate)
                    Id = tara.Id,
                    Nume = tara.Nume
                });
            }

            return Ok(tariDto);
        }

        // api/tari/{taraID}
        [HttpGet("{taraId}", Name = "GetTara")]
        [ProducesResponseType(400)] //bad request
        [ProducesResponseType(404)] //not Found
        [ProducesResponseType(200, Type = typeof(TaraDto))] //ok
        public IActionResult GetTara(int taraId)
        {
            if (!_taraRepository.TaraExista(taraId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //vom avea un singur obiect stocat in tara, cel care are id-ul resepectiv.
            var tara = _taraRepository.GetTara(taraId);

            var taraDto = new TaraDto()
            {
                Id = tara.Id,
                Nume = tara.Nume

            };

            return Ok(taraDto);
        }

        // api/tari/autotori/{autorId}
        [HttpGet("autori/{autorId}")]
        [ProducesResponseType(400)] //bad Request
        [ProducesResponseType(404)] //not found
        [ProducesResponseType(200, Type = typeof(TaraDto))] //ok
        public IActionResult GetTaraAutorului(int autorId)
        {
            var tara = _taraRepository.GetTaraAutorului(autorId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var taraDto = new TaraDto()
            {
                Id = tara.Id,
                Nume = tara.Nume

            };
            return Ok(taraDto);

        }

        //api/tari/taraId/autori
        [HttpGet("{taraId}/autori")]
        [ProducesResponseType(404)] //not found
        [ProducesResponseType(400)] //bad Request
        [ProducesResponseType(200, Type = typeof(IEnumerable<AutorDto>))] //ok
        public IActionResult GetAutoriDinTara(int taraId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_taraRepository.TaraExista(taraId))
            {
                return NotFound();
            }

            var autoriDto = new List<AutorDto>();
            var autori = _taraRepository.GetAutoriDinTara(taraId);
            foreach (var autor in autori)
            {
                autoriDto.Add(new AutorDto
                {
                    Id = autor.Id,
                    Nume = autor.Nume,
                    Prenume = autor.Prenume
                });
            }

            return Ok(autoriDto);
        }

        //api/tari POST
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Tara))] //created
        [ProducesResponseType(400)] //badrequest
        [ProducesResponseType(422)] //dublura
        [ProducesResponseType(500)] //serverError
        public IActionResult CreateTara([FromBody] Tara taraDeCreat)
        {
            if (taraDeCreat == null)
                return BadRequest(ModelState);

            var tara = _taraRepository.GetTari()
                .Where(c => c.Nume.Trim().ToUpper() == taraDeCreat.Nume.Trim().ToUpper())
                .FirstOrDefault();

            if (tara != null)
            {
                ModelState.AddModelError("", $"Tara {taraDeCreat.Nume} exista deja");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_taraRepository.CreateTara(taraDeCreat))
            {
                ModelState.AddModelError("", $"Nu s-a putut salva {taraDeCreat.Nume}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTara", new {taraId = taraDeCreat.Id}, taraDeCreat);
        }

        //Update
        //api/tari/{taraId}
        [HttpPut("{taraId}")]

        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateTara(int taraId, [FromBody] Tara updatedTaraInfo)
        {
            if (updatedTaraInfo == null)
                return BadRequest(ModelState);


            if (taraId != updatedTaraInfo.Id) //asigurare ca nu se confunda id-ul pe drum
                return BadRequest(ModelState);

            if (!_taraRepository.TaraExista(taraId))
                return NotFound();

            //duplicat
            if (_taraRepository.EsteDuplicatTaraNume(taraId, updatedTaraInfo.Nume))
            {
                ModelState.AddModelError("", $"Tara {updatedTaraInfo.Nume} exista deja");
                return StatusCode(422, ModelState); //eroare 422 care inseamna dublu
            }

            //in final validam modelstate sa vedem ca totul a fost ok
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_taraRepository.UpdateTara(updatedTaraInfo))
            {
                ModelState.AddModelError("", $" Nu s-a putut update {updatedTaraInfo.Nume}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
        //Delete
        //api/tari/{taraId}
        [HttpDelete("{taraId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult DeleteTara(int taraId)
        {
            if (!_taraRepository.TaraExista(taraId))
                return NotFound();

            var taraDeSters = _taraRepository.GetTara(taraId);
            if (_taraRepository.GetAutoriDinTara(taraId).Count > 0)
            {
                ModelState.AddModelError("", $"Tara {taraDeSters.Nume}" + "nu poate fi stearsa pentru ca apartine unui autor.");
                return StatusCode(406, ModelState);//409 e conflict
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_taraRepository.DeleteTara(taraDeSters))
            {
                ModelState.AddModelError("", $" Nu s-a putut sterge {taraDeSters.Nume}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
