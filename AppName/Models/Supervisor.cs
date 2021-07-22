using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PerformPro.Models
{
    public class Supervisor
    {
        public Supervisor(string SupervisorID, bool Deleted, string FirstName, string LastName)
        {
            this.SupervisorID = SupervisorID;
            this.Deleted = Deleted;
            this.FirstName = FirstName;
            this.LastName = LastName;
        }

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
    }
}
