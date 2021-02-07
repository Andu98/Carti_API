using CartiAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Carti_API.Services
{
    public class ReviewerRepository : IReviewerRepository
    {
        private BookDbContext _reviewerContext;

        public ReviewerRepository(BookDbContext reviewerContext)
        {
            _reviewerContext = reviewerContext;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _reviewerContext.Add(reviewer);
            return Salvare();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _reviewerContext.Remove(reviewer);
            return Salvare();
        }

        public Reviewer GetReviewer(int reviewerId)
        {
            return _reviewerContext.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
        }

        public ICollection<Reviewer> GetRevieweri()
        {
            return _reviewerContext.Reviewers.OrderBy(r => r.Nume).ToList();
        }

        public Reviewer GetReviewerOfAReview(int reviewId)
        {
           var reviewerId= _reviewerContext.Reviews.Where(r => r.Id == reviewId).Select(reviewer => reviewer.Reviewer.Id).FirstOrDefault();
           return _reviewerContext.Reviewers.Where(r => r.Id == reviewerId).FirstOrDefault();
        }

        public ICollection<Review> GetReviewuriByReviewer(int reviewerId)
        {
            return _reviewerContext.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToList();
        }

        public bool ReviewerulExista(int reviewerId)
        {
            return _reviewerContext.Reviewers.Any(r => r.Id == reviewerId);
        }

        public bool Salvare()
        {
            var salvat = _reviewerContext.SaveChanges();
            return salvat >=0 ? true : false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _reviewerContext.Update(reviewer);
            return Salvare();
        }
    }
}
