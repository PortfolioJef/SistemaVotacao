using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IMDbApi.Models
{
    public class Movie : Base
    {
        [MaxLength(60)]
        public string Name { get; set; }
        [MaxLength(60)]
        public string Director { get; set; }
        [MaxLength(1000)]
        public string Sinopse { get; set; }
        public int Year { get; set; }
        [MaxLength(60)]
        public string UrlTrailer { get; set; }

        public string urlImage { get; set; }
        public virtual List<MovieGenre> Genres { get; set; }
        public virtual List<Actor> Actors { get; set; }

        public bool isDeleted { get; set; }

    }
}
