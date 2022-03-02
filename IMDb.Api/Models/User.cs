using System.ComponentModel.DataAnnotations;

namespace IMDbApi.Models
{
    public class User : Base
    {
        [MaxLength(60)]
        public string Name { get; set; }

        [MaxLength(60)]
        public string Lastname { get; set; }

        [MaxLength(60)]
        public string Email { get; set; }

        [MaxLength(60)]
        public string Password { get; set; }

        public bool isDeleted { get; set; }

        public bool isAdmin { get; set; }
    }
}
