using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteczka.Model.DataModel
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public UserRole Role { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime RegisterAt { get; set; }
        public Person? Person { get; set; }
        public int? PersonId { get; set; }
        public IList<Review>? UserReviews { get; set; }
    }
}
