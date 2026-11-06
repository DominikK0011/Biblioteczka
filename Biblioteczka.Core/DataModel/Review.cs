using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteczka.Model.DataModel
{
    public class Review : BaseEntity
    {
        public int? Score { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public Movie Movie { get; set; }
        public int MovieId { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public Review? ParentReview { get; set; }
        public int? ParentReviewId { get; set; }
        public IList<Review>? Replies { get; set; }
    }
}
