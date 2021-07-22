using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PerformPro.Models
{
    public class Employee
    {
        //Primary key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeKey { get; set; }

        //Employee's user-entered ID
        [Required(ErrorMessage = "Employee ID is required.")]
        [MaxLength(20)]
        public string EmployeeID { get; set; }

        //0 = not deleted, 1 = deleted
        [System.ComponentModel.DefaultValue(false)]
        public bool Deleted { get; set; }

        //Employee's first name
        [Required(ErrorMessage = "First Name is required.")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        //Employee's last name
        [Required(ErrorMessage = "Last Name is required.")]
        [MaxLength(50)]
        public string LastName { get; set; }

        //Foreign key reference to Supervisor
        public int SupervisorKey { get; set; }

        //Foreign key reference to Supervisor
        [Required(ErrorMessage = "Supervisor ID is required.")]
        [MaxLength(20)]
        public string SupervisorID { get; set; }
    }
}
