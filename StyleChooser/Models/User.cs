using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StyleChooser.Models
{
    public class User
    {
        public int UserId { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public int? StyleId { get; set; }

        public List<UserStyle> UserStyles { get; set; }
    }
}