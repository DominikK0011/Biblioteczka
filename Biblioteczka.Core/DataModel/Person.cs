using System;
using System.Collections.Generic;
using System.Text;

namespace Biblioteczka.Model.DataModel
{
    public class Person : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly BirthDay { get; set; }
        public string PhotoUrl { get; set; }
        public IList<Movie>? MovieDirector { get; set; }
        public IList<Movie>? MovieActor { get; set; }
    }
}
