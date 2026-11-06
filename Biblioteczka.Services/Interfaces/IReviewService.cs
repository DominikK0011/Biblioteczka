using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Biblioteczka.API.Data;
using Biblioteczka.Contracts.DTO;
using Biblioteczka.Model.DataModel;
using Biblioteczka.Shared;
using Microsoft.Extensions.Logging;

namespace Biblioteczka.Services.Interfaces
{
    public interface IReviewService 
    {
        public Task<Result<Review>> AddReview(ReviewDto review);
        public Task<Result<ReviewDto>> GetReview(Expression<Func<Review, bool>> filterPredicate);
        public Task<Result<IEnumerable<ReviewDto>>> GetReviews(Expression<Func<Review, bool>>? filterPredicate = null);
        public Task<Result<Review>> EditReview(ReviewDto reviewDto);
        public Task<Result<Review>> DeleteReview(ReviewDto reviewDto);
    }
}
