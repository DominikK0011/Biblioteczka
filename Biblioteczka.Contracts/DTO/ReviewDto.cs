using System;
using System.Collections.Generic;
using System.Text;
using Biblioteczka.Model.DataModel;

namespace Biblioteczka.Contracts.DTO
{
    public record ReviewDto
    {
        public int Id { get; set; }
        public int? Score { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public MovieDto Movie { get; set; }
        public int MovieId { get; set; }
        public UserDto User { get; set; }
        public int UserId { get; set; }
        public ReviewDto? ParentReview { get; set; }
        public int? ParentReviewId { get; set; }
        public IList<ReviewDto>? Replies { get; set; }
    }
}
