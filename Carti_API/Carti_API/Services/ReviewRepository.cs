using CartiAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carti_API.Services
{
    public class ReviewRepository:IReviewRepository
    {
        private BookDbContext _reviewContext;

        public ReviewRepository(BookDbContext reviewContext)
        {
            _reviewContext = reviewContext;
        }

        public bool CreateReview(Review review)
        {
            _reviewContext.Add(review);
           return Salvare();
        }

        public bool DeleteReview(Review review)
        {
            _reviewContext.Remove(review);
            return Salvare();
        }

        public bool DeleteReviews(List<Review> reviews)
        {
          _reviewContext.RemoveRange(reviews);
          return Salvare();
        }

        public Book GetBookOfAReview(int reviewId)
        {
            var bookId = _reviewContext.Reviews.Where(r => r.Id == reviewId).Select(b => b.Book.Id).FirstOrDefault();
            return _reviewContext.Books.Where(b => b.Id == bookId).FirstOrDefault();
        }

        public Review GetReview(int reviewId)
        {
            return _reviewContext.Reviews.Where(r => r.Id == reviewId).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _reviewContext.Reviews.OrderBy(r => r.Id).ToList();
        }

        public ICollection<Review> GetReviewsOfABook(int bookId)
        {
            return _reviewContext.Reviews.Where(b=>b.Book.Id==bookId).ToList();
        }

        public bool ReviewExista(int reviewId)
        {
            return _reviewContext.Reviews.Any(r => r.Id == reviewId);
        }

        public bool Salvare()
        {
            var salvat = _reviewContext.SaveChanges();
            return salvat >= 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
            _reviewContext.Update(review);
            return Salvare();
        }
    }
}
