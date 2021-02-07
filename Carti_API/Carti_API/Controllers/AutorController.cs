using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Carti_API.Dtos;
using Carti_API.Services;
using CartiAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carti_API.Controllers
{
    [Route("api/autori")]
    [ApiController]
    public class AutorController : Controller
    {
        private IAutorRepository _autorRepository;
        private IBookRepository _bookRepository;
        private ITaraRepository _taraRepository;

      
        public AutorController(IAutorRepository autorRepository, IBookRepository bookRepository, ITaraRepository taraRepository)
        {
            _autorRepository = autorRepository;
            _bookRepository = bookRepository;
            _taraRepository = taraRepository;
        }

        //api/autori
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AutorDto>))]
        public IActionResult GetAutori()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var autori = _autorRepository.GetAutori().ToList();
            var autoriDto = new List<AutorDto>();
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
        //api/autori/{autorId}
        [HttpGet("{autorId}",Name = "GetAutor")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AutorDto>))]
        public IActionResult GetAutor(int autorId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_autorRepository.AutorulExista(autorId))
            {
                return NotFound();
            }

            var autor = _autorRepository.GetAutor(autorId);
            var autorDto = new AutorDto()
            {
                Id = autor.Id,
                Nume = autor.Nume,
                Prenume = autor.Prenume

            };
            return Ok(autorDto);
        }


        //api/autori/books/{bookId}
        [HttpGet("books/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AutorDto>))]
        public IActionResult GetAutoriCarte(int bookId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           
            var autori = _autorRepository.GetAutoriCarte(bookId);
            var autoriDto = new List<AutorDto>();
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

        //api/autori/{autorId}/books
        [HttpGet("{autorId}/books")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AutorDto>))]
        public IActionResult GetCartiAleAutorului(int autorId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var carti = _autorRepository.GetCartiAleAutorului(autorId);
            var cartiDto = new List<BookDto>();
            foreach (var carte in carti)
            {
                cartiDto.Add(new BookDto
                {
                    DataPublicarii = carte.DataPublicarii,
                    Id = carte.Id,
                    ISBN = carte.ISBN,
                    Titlu = carte.Titlu
                });
            }

            return Ok(cartiDto);
        }

        //POST
        //api/autori
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Autor))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult CreateAutor([FromBody] Autor autorDeCreat)
        {
            if (autorDeCreat == null)
                BadRequest(ModelState);
            if (!_taraRepository.TaraExista(autorDeCreat.Tara.Id))
            {
                ModelState.AddModelError("","Id-ul tarii pe care ati introdus-o nu exista");
                return StatusCode(404, ModelState);
            }

            autorDeCreat.Tara = _taraRepository.GetTara(autorDeCreat.Tara.Id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_autorRepository.CreateAutor(autorDeCreat))
            {
                ModelState.AddModelError("",$"Nu s-a putut crea autorul {autorDeCreat.Nume} {autorDeCreat.Prenume}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetAutor", new {autorId = autorDeCreat.Id}, autorDeCreat);
        }

        //api/autori/{autorId}
        [HttpPut("{autorId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdatAutor(int autorId, [FromBody] Autor autorDeUpdatat)
        {
            if (autorDeUpdatat == null)
                return BadRequest(ModelState);
            if (autorId != autorDeUpdatat.Id)
                return BadRequest(ModelState);
            if(!_autorRepository.AutorulExista(autorId))
                ModelState.AddModelError("","Nu exista autor cu ID-ul acesta");
            if(!_taraRepository.TaraExista(autorDeUpdatat.Tara.Id))
                ModelState.AddModelError("","Tara cu acest ID nu exista");
            if (!ModelState.IsValid)
                return StatusCode(404, ModelState);

            autorDeUpdatat.Tara = _taraRepository.GetTara(autorDeUpdatat.Tara.Id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_autorRepository.UpdateAutor(autorDeUpdatat))
            {
                ModelState.AddModelError("",$"Autorul {autorDeUpdatat.Nume } nu s-a putut Updata");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        //api/autori/autorId
        [HttpDelete("{autorId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public IActionResult DeleteAutor(int autorId)
        {
            if (!_autorRepository.AutorulExista(autorId))
                return NotFound();
            var autorDeSters = _autorRepository.GetAutor(autorId);

            //daca autorul are carti scrise nu il stergem
            if (_autorRepository.GetCartiAleAutorului(autorId).Count() > 0)
            {
                ModelState.AddModelError("",$"Autorul {autorDeSters.Nume} nu poate fi sters");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_autorRepository.DeleteAutor(autorDeSters))
            {
                ModelState.AddModelError("",$"Nu s-a putut sterge autorul {autorDeSters.Nume}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
