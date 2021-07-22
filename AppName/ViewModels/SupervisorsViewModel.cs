using PerformPro.Models;

namespace PerformPro.ViewModels
{
    public class SupervisorsViewModel
    {
        public SupervisorsViewModel(Supervisor Supervisor, string UserName, bool PasswordChanged)
        {
            this.Supervisor = Supervisor;
            this.UserName = UserName;
            this.PasswordChanged = PasswordChanged;
        }

        public Supervisor Supervisor { get; set; }

        public string UserName { get; set; }

        public bool PasswordChanged { get; set; }
    }
}
