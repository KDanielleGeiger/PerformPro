using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PerformPro.Models;
using PerformPro.ViewModels;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PerformPro.Controllers
{
    public class SupervisorsController : Controller
    {
        private readonly ConnectionStringClass _cc;

        private UserManager<AppUser> UserManager { get; }

        public SupervisorsController(ConnectionStringClass cc, UserManager<AppUser> UserManager)
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
                List<Supervisor> supervisors = _cc.Supervisor.FromSqlRaw("SELECT * FROM Supervisor").ToList();
                List<AppUser> users = await UserManager.Users.ToListAsync();
                List<SupervisorsViewModel> multipleModel = new List<SupervisorsViewModel>();

                foreach (Supervisor supervisor in supervisors)
                {
                    foreach (AppUser user in users)
                    {
                        if (supervisor.SupervisorKey == user.SupervisorKey)
                        {
                            SupervisorsViewModel singleModel = new SupervisorsViewModel(supervisor, user.UserName, user.PasswordChanged);
                            multipleModel.Add(singleModel);
                            break;
                        }
                    }
                }
                SupervisorsListViewModel viewModel = new SupervisorsListViewModel(multipleModel);
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult New()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ToLogin", "Account");
            }

            if (User.IsInRole("Administrator"))
            {
                return View("New", new NewSupervisor(false) { });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task <IActionResult> Edit(int id)
        {
            var email = "";
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ToLogin", "Account");
            }

            if (User.IsInRole("Administrator"))
            {
                Supervisor supervisor = _cc.Supervisor.Find(id);

                var users = await UserManager.GetUsersInRoleAsync("Supervisor");
                foreach (var user in users)
                {
                    if (user.SupervisorKey == id)
                    {
                        email = user.Email;
                    }
                }

                NewSupervisor newSupervisor = new NewSupervisor(supervisor.SupervisorKey, supervisor.SupervisorID, supervisor.Deleted, supervisor.FirstName, supervisor.LastName, email, true);
                return View("New", newSupervisor);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Save(NewSupervisor newSupervisor, int ID)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ToLogin", "Account");
            }

            if (User.IsInRole("Administrator"))
            {
                Supervisor supervisor = new Supervisor(newSupervisor.SupervisorID, newSupervisor.Deleted, newSupervisor.FirstName, newSupervisor.LastName);
                Supervisor sv = _cc.Supervisor.Find(ID) != null ? _cc.Supervisor.Find(ID) : supervisor;
                PropertyInfo[] props = typeof(Supervisor).GetProperties();
                var subgroup = props.Where(p => !p.Name.Contains("SupervisorKey") && p.CanWrite);
                foreach (PropertyInfo property in subgroup)
                {
                    property.SetValue(sv, property.GetValue(supervisor) != null ? property.GetValue(supervisor) : "");
                }

                if (_cc.Supervisor.Any(s => s.SupervisorID == sv.SupervisorID && s.SupervisorKey != sv.SupervisorKey)) { ModelState.AddModelError(string.Empty, $"Another supervisor already exists for {sv.SupervisorID} Supervisor ID."); }
                if (newSupervisor.Edit == false)
                {
                    AppUser user = await UserManager.FindByEmailAsync(newSupervisor.Email);
                    if (user != null)
                    {
                        ModelState.AddModelError(string.Empty, $"An account already exists with this email.");
                    }
                    else
                    {
                        user = new AppUser();
                        user.UserName = newSupervisor.Email;
                        user.Email = newSupervisor.Email;
                        user.PasswordChanged = false;
                        user.Deleted = false;
                    }

                    if (ModelState.IsValid == true)
                    {
                        if (_cc.Supervisor.Find(ID) == null) { _cc.Supervisor.Add(sv); }
                        _cc.SaveChanges();

                        var SupKey = (from s in _cc.Supervisor
                                      where s.SupervisorID == newSupervisor.SupervisorID
                                      select s.SupervisorKey).Single();
                        user.SupervisorKey = SupKey;
                        IdentityResult result = await UserManager.CreateAsync(user, "Test123!");

                        if (result.Succeeded)
                        {
                            var roleResult = await UserManager.AddToRoleAsync(user, "Supervisor");
                        }

                        return RedirectToAction("Index", "Supervisors");
                    }
                }
                else
                {
                    sv.FirstName = supervisor.FirstName;
                    sv.LastName = supervisor.LastName;
                    sv.SupervisorID = supervisor.SupervisorID;
                    _cc.SaveChanges();

                    return RedirectToAction("Index", "Supervisors");
                }
                
                return View("New", newSupervisor);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            AppUser targetUser = null;
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ToLogin", "Account");
            }

            if (User.IsInRole("Administrator"))
            {
                var supervisor = _cc.Supervisor.Find(id);
                if (supervisor != null)
                {
                    supervisor.Deleted = !supervisor.Deleted;
                    _cc.SaveChanges();
                }

                var users = await UserManager.GetUsersInRoleAsync("Supervisor");
                foreach (var user in users)
                {
                    if (user.SupervisorKey == id)
                    {
                        targetUser = user;
                    }
                }
                targetUser.Deleted = !targetUser.Deleted;
                await UserManager.UpdateAsync(targetUser);
                return RedirectToAction("Index", "Supervisors");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}