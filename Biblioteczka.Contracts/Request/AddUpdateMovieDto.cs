using System;
using System.Collections.Generic;
using System.Text;
using Biblioteczka.Model.DataModel;

namespace Biblioteczka.Contracts.Request
{
    public record AddUpdateMovieDto
    {
        public int? Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateOnly ReleaseDate { get; set; }
        public double Rating { get; set; }
        public string PosterUrl { get; set; } = null!;
        public AgeRating AgeRating { get; set; }
    }
}
