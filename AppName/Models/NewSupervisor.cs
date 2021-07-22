using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PerformPro.Models
{
    public class NewSupervisor
    {
        public NewSupervisor() { }

        public NewSupervisor(bool Edit)
        {
            this.Edit = Edit;
        }

        public NewSupervisor(string SupervisorID, bool Deleted, string FirstName, string LastName, string Email, bool Edit)
        {
            this.SupervisorID = SupervisorID;
            this.Deleted = Deleted;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Edit = Edit;
        }

        public NewSupervisor(int SupervisorKey, string SupervisorID, bool Deleted, string FirstName, string LastName, string Email, bool Edit)
        {
            this.SupervisorKey = SupervisorKey;
            this.SupervisorID = SupervisorID;
            this.Deleted = Deleted;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Email = Email;
            this.Edit = Edit;
        }

        //Primary key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SupervisorKey { get; set; }

        //Supervisor's user-entered ID
        [Required(ErrorMessage = "Supervisor ID is required.")]
        [MaxLength(20)]
        public string SupervisorID { get; set; }

        //0 = not deleted, 1 = deleted
        [System.ComponentModel.DefaultValue(false)]
        public bool Deleted { get; set; }

        //Supervisor's first name
        [Required(ErrorMessage = "First Name is required.")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        //Supervisor's last name
        [Required(ErrorMessage = "Last Name is required.")]
        [MaxLength(50)]
        public string LastName { get; set; }

        //Supervisor's email
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [MaxLength(320)]
        public string Email { get; set; }

        //Whether or not the supervisor is being edited
        public bool Edit { get; set; }
    }
}
