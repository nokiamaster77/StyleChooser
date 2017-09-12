namespace StyleChooser.Models
{
    public class UserStyle
    {
        public int UserStyleId { get; set; }
        public int UserId { get; set; }
        public int StyleId { get; set; }
        public int Count { get; set; }
        public User User { get; set; }
        public Style Style { get; set; }
    }
}