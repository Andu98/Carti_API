using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartiAPI.Models;
using Microsoft.AspNetCore.Routing.Constraints;

namespace Carti_API.Services
{
    public interface IReviewRepository
    {
        ICollection<Review> GetReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsOfABook(int bookId);
        Book GetBookOfAReview(int reviewId);
        bool ReviewExista(int reviewId);
        bool CreateReview(Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool DeleteReviews(List<Review> reviews);
        bool Salvare();
    }
}
