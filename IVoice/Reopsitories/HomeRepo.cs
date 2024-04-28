namespace IVoice.Reopsitories
{
    public class HomeRepo:IHome
    {
        private readonly ApplicationDbContext dbContext;
    

        public HomeRepo(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
 
        }

        public async Task newcontactus(ContactUsViewModel contactUsView)
        {
             

            Contactus contactus = new()
            {
                Name = contactUsView.Name,
                Email = contactUsView.Email,
                PhoneNumber = contactUsView.PhoneNumber,
                Feedback = contactUsView.Feedback,
            };

            dbContext.Add(contactus);
            dbContext.SaveChanges();
        }


    }
}
