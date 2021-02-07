using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Carti_API.Dtos;
using Carti_API.Services;
using CartiAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carti_API.Controllers
{
    [Route("api/revieweri")]
    [ApiController]
    public class RevieweriController:Controller
    {
        private IReviewerRepository _reviewerRepository;
        private IReviewRepository _reviewRepository;

        public RevieweriController(IReviewerRepository reviewerRepository, IReviewRepository reviewRepository)
        {
            _reviewerRepository = reviewerRepository;
            _reviewRepository = reviewRepository;
        }

        //api/revieweri
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        public IActionResult GetRevieweri()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var revieweri = _reviewerRepository.GetRevieweri().ToList();
            var revieweriDto = new List<ReviewerDto>();
            foreach (var reviewer in revieweri)
            {
                revieweriDto.Add(new ReviewerDto
                {
                    Id=reviewer.Id,
                    Nume = reviewer.Nume,
                    Prenume = reviewer.Prenume
                });
            }

            return Ok(revieweriDto);
        }
        //api/revieweri/{reviewerId}
        [HttpGet("{reviewerId}",Name = "GetReviewer")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(ReviewerDto))]
        public IActionResult GetReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerulExista(reviewerId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewer = _reviewerRepository.GetReviewer(reviewerId);
            var reviewerDto = new ReviewerDto
            {
                Id = reviewer.Id,
                Nume=reviewer.Nume,
                Prenume = reviewer.Prenume
            };
            return Ok(reviewerDto);
        }

        //GetReviewuriByReviewer
        //api/revieweri/reviewer/{reviewerId}
        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        public IActionResult GetReviewuriByReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerulExista(reviewerId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reviewuri = _reviewerRepository.GetReviewuriByReviewer(reviewerId);
            var reviewuriDto = new List<ReviewDto>();
            foreach (var review in reviewuri)
            {
                reviewuriDto.Add(new ReviewDto
                {
                    Id = review.Id,
                    HeadLine = review.Headline,
                    Rating = review.Rating,
                    ReviewText = review.ReviewText
                });
            }

            return Ok(reviewuriDto);


        }
        //api/reviewer/{reviewId}/reviewer
        [HttpGet("{reviewId}/reviewer")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewerDto>))]
        public IActionResult GetReviewerOfAReview(int reviewId)
        {
            if (!_reviewerRepository.ReviewerulExista(reviewId))
            {
                return NotFound();
            }

            var reviewer = _reviewerRepository.GetReviewerOfAReview(reviewId);
            var reviewerDto = new ReviewerDto
            {
                Id = reviewer.Id,
                Nume =reviewer.Nume,
                Prenume = reviewer.Prenume
            };
            return Ok(reviewerDto);
        }

        //POST
        //api/revieweri
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Reviewer))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateReviewer([FromBody] Reviewer reviewerDeCreat)
        {
            if (reviewerDeCreat == null)
                return BadRequest(ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_reviewerRepository.CreateReviewer(reviewerDeCreat))
            {
                ModelState.AddModelError("",$"Nu s-a putut crea reviewerul {reviewerDeCreat.Nume} {reviewerDeCreat.Prenume}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetReviewer", new {reviewerId = reviewerDeCreat.Id}, reviewerDeCreat);
        }

        //Update
        //api/reviewer/{reviewerId}
        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] Reviewer reviewerDeUpdatat)
        {
            if (reviewerDeUpdatat == null)
                BadRequest(ModelState);
            if (reviewerId != reviewerDeUpdatat.Id)
                BadRequest(ModelState);
            if (!_reviewerRepository.ReviewerulExista(reviewerId))
                return NotFound(ModelState);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_reviewerRepository.UpdateReviewer(reviewerDeUpdatat))
            {
                ModelState.AddModelError("", $"Nu s-a putut updata {reviewerDeUpdatat.Nume} {reviewerDeUpdatat.Prenume}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //Delete. Cand va sterge un reviewer ii va sterge si toate reviewerile
        //api/revieweri/{reviewerId}
        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            if (!_reviewerRepository.ReviewerulExista(reviewerId))
                return NotFound();
            var reviewerDeSters = _reviewerRepository.GetReviewer(reviewerId);
            var reviewuriDeSters = _reviewerRepository.GetReviewuriByReviewer(reviewerId);

            if (!ModelState.IsValid)
                return BadRequest();
            if (!_reviewerRepository.DeleteReviewer(reviewerDeSters))
            {
                ModelState.AddModelError("",$"Nu s-a putut sterge  reviewerul {reviewerDeSters.Nume} {reviewerDeSters.Prenume}");
                return StatusCode(500, ModelState);
            }

            if (!_reviewRepository.DeleteReviews(reviewuriDeSters.ToList()))
            {
                ModelState.AddModelError("", $"Nu s-au putut sterge reviewurile autorului  {reviewerDeSters.Nume} {reviewerDeSters.Prenume}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
