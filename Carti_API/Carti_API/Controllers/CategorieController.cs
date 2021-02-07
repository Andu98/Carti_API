using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carti_API.Dtos;
using Carti_API.Services;
using CartiAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;


namespace Carti_API.Controllers
{
    [Route("api/categorii")]
    [ApiController]
    public class CategorieController:Controller
    {
        private ICategorieRepository _categorieRepository;

        public CategorieController(ICategorieRepository categorieRepository)
        {
            _categorieRepository = categorieRepository;
        }

        //api/categorii
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategorieDto>))]
        public IActionResult GetCategorii()
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categorii = _categorieRepository.GetCategorii().ToList();
            var categoriiDto = new List<CategorieDto>();
            foreach (var categorie in categorii)
            {
                categoriiDto.Add(new CategorieDto
                {
                    //cele 2 variabile din CategorieDto
                    Id=categorie.Id,
                    Nume=categorie.Nume
                });
            }

            return Ok(categoriiDto);

        }

        //api/categorii/{categorieId}
        [HttpGet("{categorieId}",Name="GetCategorie")]
        [ProducesResponseType(400)]
        
        public IActionResult GetCategorie(int categorieId)
        {
            if (!_categorieRepository.CategoriaExista(categorieId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //vom avea un singur obiect stocat in categorie, cel care are id-ul resepectiv.
            var categorie = _categorieRepository.GetCategorie(categorieId);

            var categorieDto = new CategorieDto()
            {
                Id = categorie.Id,
                Nume = categorie.Nume

            };

            return Ok(categorieDto);
        }

        //api/categorii/books/{bookId}
        [HttpGet("books/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategorieDto>))]
        public IActionResult GetCategoriiCarte(int bookId)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categorii = _categorieRepository.GetCategoriiCarte(bookId);

            var categoriiDto = new List<CategorieDto>();
            foreach (var categorie in categorii)
            {
                categoriiDto.Add(new CategorieDto
                {
                    Id = categorie.Id,
                    Nume= categorie.Nume
                });
            }

            return Ok(categoriiDto);
        }
       

        //api/categorii/{CategorieId}/books
        [HttpGet("{categorieId}/books")]
        [ProducesResponseType(404)] //not found
        [ProducesResponseType(400)] //bad Request
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        public IActionResult GetCartiDinCategoria(int categorieId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!_categorieRepository.CategoriaExista(categorieId))
            {
                return NotFound();
            }
            var booksDto= new List<BookDto>();
            var books = _categorieRepository.GetCartiDinCateogia(categorieId);
            foreach (var book in books)
            {
                booksDto.Add(new BookDto
                {
                    Id=book.Id,
                    ISBN = book.ISBN,
                    Titlu = book.Titlu,
                    DataPublicarii = book.DataPublicarii
                });
            }

            return Ok(booksDto);
        }

        //POST
        //api/categorii
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Categorie))] //created
        [ProducesResponseType(400)] //badrequest
        [ProducesResponseType(422)] //dublura
        [ProducesResponseType(500)] //serverError
        public IActionResult CreateCategorie([FromBody] Categorie categorieDeCreat)
        {
            if (categorieDeCreat == null)
                return BadRequest(ModelState);

            var categorie = _categorieRepository.GetCategorii()
                .Where(c => c.Nume.Trim().ToUpper() == categorieDeCreat.Nume.Trim().ToUpper())
                .FirstOrDefault();

            if (categorie != null)
            {
                ModelState.AddModelError("", $"Tara {categorieDeCreat.Nume} exista deja");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categorieRepository.CreateCategorie(categorieDeCreat))
            {
                ModelState.AddModelError("", $"Nu s-a putut salva {categorieDeCreat.Nume}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategorie", new { categorieId = categorieDeCreat.Id }, categorieDeCreat);
        }
        
        //Update
        //api/categorii/{categorieId}
        [HttpPut("{categorieId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateCategorie(int categorieId, [FromBody] Categorie updatedCategorieInfo)
        {
            if (updatedCategorieInfo == null)
            {
                return BadRequest(ModelState);
            }

            if (categorieId != updatedCategorieInfo.Id)
            {
                return BadRequest(ModelState);
            }

            if (_categorieRepository.EsteDuplicatCategorieNume(categorieId,updatedCategorieInfo.Nume ))
            {
                ModelState.AddModelError("", $"Categoria {updatedCategorieInfo.Nume} exista deja");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_categorieRepository.UpdateCategorie(updatedCategorieInfo))
            {
                ModelState.AddModelError("", $"Categoria {updatedCategorieInfo.Nume} nu s-a putut updata");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //Delete
        //api/categorii/{categorieId}
        [HttpDelete("{categorieId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult DeleteCategorie(int categorieId)
        {
            if (!_categorieRepository.CategoriaExista(categorieId))
            {
                return NotFound();
            }

            var categorieDeSters = _categorieRepository.GetCategorie(categorieId);
            if (_categorieRepository.GetCartiDinCateogia(categorieId).Count > 0)
            {
                ModelState.AddModelError("",$"Categoria{categorieDeSters.Nume} nu poate fi stearsa, pentru ca din ea apartin carti ");
                return StatusCode(406, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
           
            if (!_categorieRepository.DeleteCategorie(categorieDeSters))
            {
                ModelState.AddModelError("",$" Nu s-a putut sterge {categorieDeSters.Nume}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
