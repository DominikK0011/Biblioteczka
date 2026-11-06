using System;
using System.Collections.Generic;
using System.Text;
using Biblioteczka.Model.DataModel;

namespace Biblioteczka.Contracts.DTO
{
    public record PersonDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserRole Role { get; set; }
        public DateOnly BirthDay { get; set; }
        public string PhotoUrl { get; set; }
        public IList<Movie>? MovieDirector { get; set; }
        public IList<Movie>? MovieActor { get; set; }
    }
}
