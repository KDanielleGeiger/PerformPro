using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PerformPro.Models
{
    public class Form
    {
        public Form(string CreatedByID)
        {
            this.CreatedByID = CreatedByID;
        }

        public Form() { }

        //Primary key
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FormID { get; set; }

        //Foreign key reference to Employee
        [ForeignKey("Employee")]
        public int Employee { get; set; }

        //Employee ID
        [Required(ErrorMessage = "Employee ID is required.")]
        [MaxLength(20)]
        public string EmployeeID { get; set; }

        //Foreign key reference to Supervisor
        [ForeignKey("Supervisor")]
        public int CreatedBy { get; set; }

        //Supervisor ID
        [Required(ErrorMessage = "Created By ID is required.")]
        [MaxLength(20)]
        public string CreatedByID { get; set; }

        //Employee's job title
        [Required(ErrorMessage = "Job Title is required.")]
        [MaxLength(50)]
        public string Title { get; set; }

        //Assessment period
        [Required(ErrorMessage = "Assessment Period is required.")]
        [MaxLength(50)]
        public string Period { get; set; }

        //0 = incomplete, 1 = complete
        [System.ComponentModel.DefaultValue(false)]
        public bool Complete { get; set; }

        //0 = not deleted, 1 = deleted
        [System.ComponentModel.DefaultValue(true)]
        public bool Deleted { get; set; }

        //Rating of 1-5
        public int Communication1 { get; set; }

        //Rating of 1-5
        public int Communication2 { get; set; }

        //Rating of 1-5
        public int Communication3 { get; set; }

        //Average of above ratings
        public decimal CommunicationAvg { get; set; }

        //Rating of 1-5
        public int Appreciation1 { get; set; }

        //Rating of 1-5
        public int Appreciation2 { get; set; }

        //Rating of 1-5
        public int Appreciation3 { get; set; }

        //Average of above ratings
        public decimal AppreciationAvg { get; set; }

        //Rating of 1-5
        public int Development1 { get; set; }

        //Rating of 1-5
        public int Development2 { get; set; }

        //Rating of 1-5
        public int Development3 { get; set; }

        //Average of above ratings
        public decimal DevelopmentAvg { get; set; }

        //Rating of 1-5
        public int Teamwork1 { get; set; }

        //Rating of 1-5
        public int Teamwork2 { get; set; }

        //Rating of 1-5
        public int Teamwork3 { get; set; }

        //Average of above ratings
        public decimal TeamworkAvg { get; set; }

        //Quality metric
        [MaxLength(200)]
        public string Quality { get; set; }

        //Quality actions & results
        [MaxLength(200)]
        public string QualityAr { get; set; }

        //Service metric
        [MaxLength(200)]
        public string Service { get; set; }

        //Service actions & results
        [MaxLength(200)]
        public string ServiceAr { get; set; }

        //Finance metric
        [MaxLength(200)]
        public string Finance { get; set; }

        //Finance actions & results
        [MaxLength(200)]
        public string FinanceAr { get; set; }

        //People metric
        [MaxLength(200)]
        public string People { get; set; }

        //People actions & results
        [MaxLength(200)]
        public string PeopleAr { get; set; }

        //Growth metric
        [MaxLength(200)]
        public string Growth { get; set; }

        //Growth actions & results
        [MaxLength(200)]
        public string GrowthAr { get; set; }

        //Summary of performance
        [MaxLength(8000)]
        public string Summary { get; set; }

        //Developmental opportunities and personal goals
        [MaxLength(8000)]
        public string Opportunities { get; set; }
    }
}
