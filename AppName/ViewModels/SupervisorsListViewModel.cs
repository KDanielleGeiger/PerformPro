using System.Collections.Generic;

namespace PerformPro.ViewModels
{
    public class SupervisorsListViewModel
    {
        public SupervisorsListViewModel(List<SupervisorsViewModel> Supervisors)
        {
            this.Supervisors = Supervisors;
        }

        public List<SupervisorsViewModel> Supervisors { get; set; }
    }
}
