using System;
using System.Collections.Generic;
using System.Text;
using Biblioteczka.Model.DataModel;

namespace Biblioteczka.Contracts.DTO
{
    public record MovieDto 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public double Rating { get; set; }
        public string PosterUrl { get; set; }
        public AgeRating AgeRating { get; set; }
        public Person Director { get; set; }
        public int DirectorId { get; set; }
        public IList<Person>? Actors { get; set; }
        public IList<Review>? Reviews { get; set; }
    }
}
