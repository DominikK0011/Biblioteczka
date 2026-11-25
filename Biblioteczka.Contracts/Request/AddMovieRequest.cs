using System;
using System.Collections.Generic;
using System.Text;
using Biblioteczka.Contracts.DTO;
using Biblioteczka.Model.DataModel;

namespace Biblioteczka.Contracts.Request
{
    public record AddMovieRequest
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateOnly ReleaseDate { get; set; }
        public string PosterUrl { get; set; }
        public AgeRating AgeRating { get; set; }
        public PersonDto Director { get; set; }
        public IList<PersonDto>? Actors { get; set; }
    }
}
