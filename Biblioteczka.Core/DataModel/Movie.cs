using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteczka.Model.DataModel
{
    public class Movie : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public double Rating { get; set; }
        public string PosterUrl { get; set; }
        public AgeRating AgeRating { get; set; }
        public Person Director { get; set; }
        public int DirectorId { get; set; }
        public IList<Person> Actors { get; set; }
        public IList<Review> Reviews { get; set; }

    }
}
