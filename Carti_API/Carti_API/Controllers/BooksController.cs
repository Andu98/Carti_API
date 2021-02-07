using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carti_API.Dtos;
using Carti_API.Services;
using CartiAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Carti_API.Controllers
{[Route("api/carti")] 
    [ApiController]
    public class BooksController:Controller
    {
        private IBookRepository _bookRepository;
        private IAutorRepository _autorRepository;
        private ICategorieRepository _categorieRepository;
        private IReviewRepository _reviewRepository;

        public BooksController(IBookRepository bookRepository, IAutorRepository autorRepository, ICategorieRepository categorieRepository, IReviewRepository reviewRepository)
        {
            _bookRepository = bookRepository;
            _autorRepository = autorRepository;
            _categorieRepository = categorieRepository;
            _reviewRepository = reviewRepository;
        }
        //api/carti
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        public IActionResult GetBooks()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var books = _bookRepository.GetBooks().ToList();
            var booksDto = new List<BookDto>();
            foreach (var book in books)
            {
                booksDto.Add(new BookDto
                {
                    DataPublicarii = book.DataPublicarii,
                    Id = book.Id,
                    ISBN = book.ISBN,
                    Titlu = book.Titlu
                });
            }

            return Ok(booksDto);
        }
        //api/carti/{bookId}
        [HttpGet("{bookId}",Name = "GetBook")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        public IActionResult GetBook(int bookId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_bookRepository.BookExista(bookId))
            {
                return NotFound();
            }

            var book = _bookRepository.GetBook(bookId);
            var bookDto = new BookDto
            {
                DataPublicarii = book.DataPublicarii,
                Id = book.Id,
                ISBN = book.ISBN,
                Titlu = book.Titlu
            };
            return Ok(bookDto);
        }

        //api/carti/isbn/{bookIsbn}
        [HttpGet("isbn/{bookisbn}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        public IActionResult GetBook(string bookIsbn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_bookRepository.BookExista(bookIsbn))
            {
                return NotFound();
            }

            var book = _bookRepository.GetBook(bookIsbn);
            var bookDto = new BookDto
            {
                DataPublicarii = book.DataPublicarii,
                Id = book.Id,
                ISBN = book.ISBN,
                Titlu = book.Titlu
            };
            return Ok(bookDto);
        }

        //api/carti/{bookdId}/rating
        [HttpGet("{bookId}/rating")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(decimal))]
        public IActionResult GetBookRating(int bookId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_bookRepository.BookExista(bookId))
            {
                return NotFound();
            }

            var rating = _bookRepository.GetBookRating(bookId);
            return Ok(rating);

        }

        //api/carti?autId=1&autId=2&catId=1&catId=2
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Book))] //created
        [ProducesResponseType(400)] //badrequest
        [ProducesResponseType(402)] //duplicat
        [ProducesResponseType(404)] //notFound
        [ProducesResponseType(500)] //serverError
        //FromQuery semnifica faptul rezultatul e in functie de datele din URI
        //numele precizat aici e cel care va fi folosit in URI
        public IActionResult CreateBook([FromQuery] List<int> autId, [FromQuery] List<int> catId,
            [FromBody] Book bookDeCreat)
        {
            var statusCode = ValidareBook(autId, catId, bookDeCreat);
            if (!ModelState.IsValid)
                return StatusCode(statusCode.StatusCode);
            if (!_bookRepository.CreateBook(autId, catId, bookDeCreat))
            {
                ModelState.AddModelError("", $" Nu s-a putut crea cartea {bookDeCreat.Titlu}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetBook", new {bookId = bookDeCreat.Id}, bookDeCreat);
        }


        //Update
        //api/carti/carteID?autId=1&autId=2&catId=1&catId=2
        [HttpPut("{bookId}")]
        [ProducesResponseType(204)] //created
        [ProducesResponseType(400)] //badrequest
        [ProducesResponseType(402)] //duplicat
        [ProducesResponseType(404)] //notFound
        [ProducesResponseType(500)] //serverError
        //FromQuery semnifica faptul rezultatul e in functie de datele din URI
        //numele precizat aici e cel care va fi folosit in URI
        public IActionResult UpdateBook(int bookId,[FromQuery] List<int> autId, [FromQuery] List<int> catId,
            [FromBody] Book bookDeUpdatat)
        {
            var statusCode = ValidareBook(autId, catId, bookDeUpdatat);
            //verificare match intre id din link si din body
            if (bookId != bookDeUpdatat.Id)
                return BadRequest();

            if (!_bookRepository.BookExista(bookId))
                return NotFound();


            if (!ModelState.IsValid)
                return StatusCode(statusCode.StatusCode);
            if (!_bookRepository.UpdateBook(autId, catId, bookDeUpdatat))
            {
                ModelState.AddModelError("", $" Nu s-a putut updata cartea {bookDeUpdatat.Titlu}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //Delete
        //api/carti/{bookId}
        [HttpDelete("{bookId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteBook(int bookId)
        {
            if (!_bookRepository.BookExista(bookId))
                return NotFound();
            var reviewuriDeSters = _reviewRepository.GetReviewsOfABook(bookId);
            var carteDeSters = _bookRepository.GetBook(bookId);
            if (!ModelState.IsValid)
                return BadRequest();
            if (!_reviewRepository.DeleteReviews(reviewuriDeSters.ToList()))
            {
                ModelState.AddModelError("","Nu s-au putut sterge reviewurile");
                return StatusCode(500, ModelState);
            }

            if (!_bookRepository.DeleteBook(carteDeSters))
            {
                ModelState.AddModelError("",$"Nu s-a putut sterge cartea {carteDeSters.Titlu}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        private StatusCodeResult ValidareBook(List<int> autorId, List<int> categoriiId, Book book)
        {
            
            if (book == null || autorId.Count() <= 0 || categoriiId.Count() <= 0)
            {
                ModelState.AddModelError("", "Cartea,autorul sau categoria lipesc");
                return BadRequest();
            }

            if (_bookRepository.EsteDuplicatIsbn(book.Id, book.ISBN))
            {
                ModelState.AddModelError("","ISBN duplicat");
                return StatusCode(422);
            }

            foreach (var id in autorId)
            {
                if (!_autorRepository.AutorulExista(id))
                {
                    ModelState.AddModelError("", "Autorul nu a fost gasit");
                    return StatusCode(404);
                }
            }

            foreach (var id in categoriiId)
            {
                if (!_categorieRepository.CategoriaExista(id))
                {
                    ModelState.AddModelError("","Categoria nu exista.");
                    return StatusCode(404);
                }
            }

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("","Eroare");
                return BadRequest();
            }

            return NoContent();
        }
    }
}
