namespace IVoice.Models
{
    public class Admindraft
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string? YourThought { get; set; }
    }
}
