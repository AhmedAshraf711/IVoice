namespace IVoice.ViewModel
{
    public class ContactUsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [RegularExpression("^[a-z0-9_\\+-]+(\\.[a-z0-9_\\+-]+)*@[a-z0-9-]+(\\.[a-z0-9]+)*\\.([a-z]{2,4})$", ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [RegularExpression(@"^\+?\d{1,3}?\-?\d{6,14}$", ErrorMessage = "Invalid Phone Number")]
        public string PhoneNumber { get; set; }
        public string Feedback { get; set; }
    }
}
