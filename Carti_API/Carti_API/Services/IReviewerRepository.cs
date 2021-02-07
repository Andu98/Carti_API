using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartiAPI.Models;

namespace Carti_API.Services
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetRevieweri();
        Reviewer GetReviewer(int reviewerId);
        ICollection<Review> GetReviewuriByReviewer(int reviewerId);
        Reviewer GetReviewerOfAReview(int reviewId);
        bool ReviewerulExista(int reviewerId);
        bool CreateReviewer(Reviewer reviewer);
        bool UpdateReviewer(Reviewer reviewer);
        bool DeleteReviewer(Reviewer reviewer);
        bool Salvare();
    }
}
