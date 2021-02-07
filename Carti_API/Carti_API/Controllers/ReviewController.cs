using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carti_API.Dtos;
using Carti_API.Services;
using CartiAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Remotion.Linq.Clauses.ExpressionVisitors;

namespace Carti_API.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController:Controller
    {
        private IReviewerRepository _reviewerRepository;
        private IReviewRepository _reviewRepository;
        private IBookRepository _bookRepository;

        public ReviewController( IReviewerRepository reviewerRepository, IReviewRepository reviewRepository, IBookRepository bookRepository)
        {
            _reviewerRepository = reviewerRepository;
            _reviewRepository = reviewRepository;
            _bookRepository = bookRepository;

        }

        //api/reviews
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        public IActionResult GetReviews()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var reviewsDto = new List<ReviewDto>();
            var reviews = _reviewRepository.GetReviews().ToList();
            foreach (var rev in reviews)
            {
                reviewsDto.Add(new ReviewDto
                {
                    HeadLine = rev.Headline,
                    Id = rev.Id,
                    Rating = rev.Rating,
                    ReviewText = rev.ReviewText
                });
            }
            return Ok(reviewsDto);
            }

        //api/reviews/{reviewId}
        [HttpGet("{reviewId}",Name="GetReview")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        public IActionResult GetReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExista(reviewId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var review = _reviewRepository.GetReview(reviewId);
            var reviewDto = new ReviewDto()
            {
                HeadLine = review.Headline,
                Id=review.Id,
                Rating = review.Rating,
                ReviewText = review.ReviewText
            };
            return Ok(reviewDto);
        }
        //api/reviews/Books/{bookId}
        [HttpGet("books/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewDto>))]
        public IActionResult GetReviewsOfABook(int bookId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviews = _reviewRepository.GetReviewsOfABook(bookId);
            var reviewsDto = new List<ReviewDto>();
            foreach (var review in reviews)
            {
                reviewsDto.Add(new ReviewDto
                {
                    HeadLine = review.Headline,
                    Id = review.Id,
                    Rating = review.Rating,
                    ReviewText = review.ReviewText
                });
            }
            return Ok(reviewsDto);
        }
        //api/reviews/{reviewId}/book
        [HttpGet("{reviewId}/book")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ReviewDto))]
        public IActionResult GetBookOfAReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExista(reviewId))
            {
                return NotFound();
            }

            var book = _reviewRepository.GetBookOfAReview(reviewId);
            var bookDto = new BookDto()
            {
                Id = book.Id,
                DataPublicarii = book.DataPublicarii,
                ISBN = book.ISBN,
                Titlu = book.Titlu

            };
            return Ok(bookDto);
        }

        //POST
        //api/reviews
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Review))] //created
        [ProducesResponseType(400)] //badrequest
        [ProducesResponseType(422)] //dublura
        [ProducesResponseType(500)] //serverError
        public IActionResult CreateReview([FromBody] Review reviewDeCreat)
        {
            if (reviewDeCreat == null)
                return BadRequest(ModelState);

            if(!_reviewerRepository.ReviewerulExista(reviewDeCreat.Reviewer.Id))
                ModelState.AddModelError("", "Reviewerul nu exista");
            if(!_bookRepository.BookExista(reviewDeCreat.Book.Id))
                ModelState.AddModelError("", "Cartea nu exista!");
        

            reviewDeCreat.Book = _bookRepository.GetBook(reviewDeCreat.Book.Id);
            reviewDeCreat.Reviewer = _reviewerRepository.GetReviewer(reviewDeCreat.Reviewer.Id);
            if (!ModelState.IsValid)
                return StatusCode(404, ModelState);

            if (!_reviewRepository.CreateReview(reviewDeCreat))
            {
                ModelState.AddModelError("",$"Nu s-a putut salva review-ul");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetReview", new{reviewId = reviewDeCreat.Id},reviewDeCreat);
        }

        //Update
        //api/reviews/{reviewId}
        [HttpPut("{reviewId}")]
        [ProducesResponseType(204)] //created
        [ProducesResponseType(400)] //badrequest
        [ProducesResponseType(404)] //dublura
        [ProducesResponseType(500)] //serverError
        public IActionResult UpdateReview(int reviewId, [FromBody] Review reviewDeUpdatat)
        {
            if (reviewDeUpdatat == null)
                return BadRequest(ModelState);
            if(!_reviewRepository.ReviewExista(reviewId))
                ModelState.AddModelError("", "Review-ul nu exista");
            if(!_reviewerRepository.ReviewerulExista(reviewDeUpdatat.Reviewer.Id))
                ModelState.AddModelError("","Reviewer-ul nu exista");
            if(!_bookRepository.BookExista(reviewDeUpdatat.Book.Id))
                ModelState.AddModelError("","Cartea nu exista");

            if (!ModelState.IsValid)
                return StatusCode(404, ModelState);

            reviewDeUpdatat.Book = _bookRepository.GetBook(reviewDeUpdatat.Book.Id);
            reviewDeUpdatat.Reviewer = _reviewerRepository.GetReviewer(reviewDeUpdatat.Reviewer.Id);
            
            if (!_reviewRepository.UpdateReview(reviewDeUpdatat))
            {
                ModelState.AddModelError("","Nu s-a putut updata");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //Delete
        //api/review/{reviewId}
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(204)] //created
        [ProducesResponseType(400)] //badrequest
        [ProducesResponseType(404)] //dublura
        [ProducesResponseType(500)] //serverError
        public IActionResult DeleteReview(int reviewId)
        {
            if (!_reviewRepository.ReviewExista(reviewId))
                return NotFound();
            var reviewDeSters = _reviewRepository.GetReview(reviewId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_reviewRepository.DeleteReview(reviewDeSters))
            {
                ModelState.AddModelError("","Nu s-a putut sterge");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}
