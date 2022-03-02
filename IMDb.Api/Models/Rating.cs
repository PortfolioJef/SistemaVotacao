using System;
using System.ComponentModel.DataAnnotations;

namespace IMDbApi.Models
{
    public class Rating : Base
    {
        public int RatingId { get; set; }
        public int UserId { get; set; }

        public int MovieId { get; set; }

        [Range(0, 4, ErrorMessage = "Válido de 09 a 4.")]
        public int Rate { get; set; }
    }
}
