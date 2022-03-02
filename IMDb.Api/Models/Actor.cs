using System.ComponentModel.DataAnnotations;

namespace IMDbApi.Models
{
    public class Actor : Base
    {
        [MaxLength(60)]
        public string Name { get; set; }
    }
}
