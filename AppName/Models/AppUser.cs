using Microsoft.AspNetCore.Identity;

namespace PerformPro.Models
{
    public class AppUser : IdentityUser<int>
    {
        public int SupervisorKey { get; set; }

        public bool PasswordChanged { get; set; }

        public bool Deleted { get; set; }
    }
}
