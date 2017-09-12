using System.ComponentModel.DataAnnotations;

namespace StyleChooser.Models
{
    public class Style
    {
        public int StyleId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}