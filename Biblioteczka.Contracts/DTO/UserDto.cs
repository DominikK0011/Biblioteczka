using System;
using System.Collections.Generic;
using System.Text;
using Biblioteczka.Model.DataModel;

namespace Biblioteczka.Contracts.DTO
{
    public record UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public UserRole Role { get; set; }
        public string AvatarUrl { get; set; }
        public Person? Person { get; set; }
        public int? PersonId { get; set; }
        public IList<Review>? UserReviews { get; set; }
    }
}
