namespace PerformPro.Models
{
    public class AccountModel
    {
        public AccountModel(string Email)
        {
            this.Email = Email;
        }

        //User's email address
        public string Email { get; set; }
    }
}
