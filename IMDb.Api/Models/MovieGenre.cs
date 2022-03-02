using System.ComponentModel.DataAnnotations;

namespace IMDbApi.Models
{
    public class MovieGenre : Base
    {
        public int MovieGenreId { get; set; }
        [MaxLength(60)]
        public string Name { get; set; }


    }
}
