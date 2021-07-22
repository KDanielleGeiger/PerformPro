using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PerformPro.Models;
using PerformPro.ViewModels;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace PerformPro.Controllers
{
    public class FormsController : Controller
    {
        private readonly ConnectionStringClass _cc;

        private UserManager<AppUser> UserManager { get; }

        public FormsController(ConnectionStringClass cc, UserManager<AppUser> UserManager)
        {
            _cc = cc;
            this.UserManager = UserManager;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ToLogin", "Account");
            }

            if (User.IsInRole("Administrator"))
            {
                var viewModel = from f in _cc.Form
                                where f.Complete == true
                                where f.Deleted == false
                                from e in _cc.Employee
                                where f.Employee == e.EmployeeKey
                                from s in _cc.Supervisor
                                where f.CreatedBy == s.SupervisorKey
                                orderby e.FirstName
                                select new FormsListViewModel { Form = f, Employee = e, Supervisor = s };
                return View("AdminView", viewModel);
            }
            else
            {
                var UserID = await UserManager.GetUserAsync(HttpContext.User);
                var UserKey = UserID.SupervisorKey;

                var viewModel = from f in _cc.Form
                                where f.CreatedBy == UserKey
                                where f.Deleted == false
                                from e in _cc.Employee
                                where f.Employee == e.EmployeeKey
                                from s in _cc.Supervisor
                                where f.CreatedBy == s.SupervisorKey
                                orderby f.Complete, e.FirstName
                                select new FormsListViewModel { Form = f, Employee = e, Supervisor = s };
                return View("SupervisorView", viewModel);
            }
        }

        public async Task<IActionResult> New()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ToLogin", "Account");
            }

            if (User.IsInRole("Supervisor"))
            {
                var UserID = await UserManager.GetUserAsync(HttpContext.User);
                var UserKey = UserID.SupervisorKey;

                var SupID = (from s in _cc.Supervisor
                            where s.SupervisorKey == UserKey
                            select s.SupervisorID).Single();

                return View("New", new Form(SupID) { });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult Edit(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ToLogin", "Account");
            }

            if (User.IsInRole("Supervisor"))
            {
                return View("New", _cc.Form.Find(id));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult View(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ToLogin", "Account");
            }
            return View("View", _cc.Form.Find(id));
        }

        [HttpPost]
        public IActionResult Save(Form edits, int ID)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ToLogin", "Account");
            }

            if (User.IsInRole("Supervisor"))
            {
                Form form = _cc.Form.Find(ID) ?? edits;
                PropertyInfo[] props = typeof(Form).GetProperties();
                var subgroup = props.Where(p => !p.Name.Contains("FormID") && !p.Name.Equals("CreatedBy") && p.CanWrite);
                foreach (PropertyInfo property in subgroup)
                {
                    property.SetValue(form, property.GetValue(edits) != null ? property.GetValue(edits) : "");
                }

                if (_cc.Supervisor.Any(s => s.SupervisorID == edits.CreatedByID)) { form.CreatedBy = _cc.Supervisor.Where(s => s.SupervisorID == edits.CreatedByID).First().SupervisorKey; }
                else { ModelState.AddModelError(string.Empty, $"No supervisor found for {edits.CreatedByID} Supervisor ID."); }
                if (_cc.Employee.Any(e => e.EmployeeID == edits.EmployeeID)) { form.Employee = _cc.Employee.Where(e => e.EmployeeID == edits.EmployeeID).First().EmployeeKey; }
                else { ModelState.AddModelError(string.Empty, $"No employee found for {edits.EmployeeID} Employee ID."); }
                if (_cc.Form.Any(u => u.Employee == edits.Employee && u.FormID != form.FormID)) { ModelState.AddModelError(string.Empty, $"Another form already exists for {edits.Employee} Employee ID."); }

                form.Complete = evaluateComplete(form);
                if (form.Complete == true)
                {
                    string[] groups = { "Communication", "Appreciation", "Development", "Teamwork" };
                    foreach (string group in groups)
                    {
                        var property = props.Where(p => p.Name.Contains(group) && p.PropertyType == typeof(decimal)).FirstOrDefault();
                        property.SetValue(form, getAvgProperties(group, form));
                    }
                }

                if (ModelState.IsValid == true)
                {
                    if (_cc.Form.Find(ID) == null) { _cc.Form.Add(form); }
                    _cc.SaveChanges();
                    return RedirectToAction("Index", "Forms");
                }
                return View("New", form);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        bool evaluateComplete(Form form)
        {
            PropertyInfo[] props = typeof(Form).GetProperties();
            var integers = props.Where(p => p.PropertyType == typeof(int) && !p.Name.Contains("ID") && !p.Name.Contains("Key"));
            foreach (PropertyInfo property in integers)
            {
                if ((int) property.GetValue(form) == 0) { return false;  }
            }
            var strings = props.Where(p => p.PropertyType == typeof(string) && !p.Name.Contains("ID") && !p.Name.Contains("Key"));
            foreach (PropertyInfo property in strings)
            {
                if (property.GetValue(form).ToString().Length == 0 ) { return false; }
            }
            return true;
        }

        decimal getAvgProperties(string root, Form form)
        {
            PropertyInfo[] props = typeof(Form).GetProperties();
            var subgroup = props.Where(p => p.Name.Contains(root));
            Decimal avg = 0;
            foreach (PropertyInfo property in subgroup)
            {
                if (property.PropertyType == typeof(int))
                {
                    avg += (int)property.GetValue(form);
                }
            }
            return avg / (subgroup.Count()-1);
        }
        
        public IActionResult Delete(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ToLogin", "Account");
            }

            if (User.IsInRole("Administrator"))
            {
                var form = _cc.Form.Find(id);
                if (form != null)
                {
                    form.Deleted = !form.Deleted;
                    _cc.SaveChanges();
                }
                return RedirectToAction("Index", "Forms");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}