using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;
using TabloidMVC.Models.ViewModels;


namespace TabloidMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UserProfileController(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }
        public IActionResult Index()
        {
                List<UserProfile> userProfiles = _userProfileRepository.GetAllUsers();
                return View(userProfiles);
        }

        public IActionResult UnauthorizedIndex()
        {
            List<UserProfile> userProfiles = _userProfileRepository.GetAllUnathenticatedUsers();
            return View(userProfiles);
        }
        // GET: UserProfileController/Details/5
        public ActionResult Details(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetUserProfileById(id);

            return View(userProfile);
        }

        // GET: UserProfileController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserProfileController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserProfileController/Edit/5
        public ActionResult Edit(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetUserProfileById(id);
            ProtectAdminViewModel vm = new ProtectAdminViewModel()
            {
                userProfile = userProfile
            };
            int count = _userProfileRepository.CheckNumOfAdmins();
            if (count < 2 && userProfile.UserTypeId == 1)
            {
                vm.CanDemote = false;
            }
            return View(vm);
        }

        // POST: UserProfileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UserProfile userProfile)
        {
            try
            {
                _userProfileRepository.EditUserType(userProfile);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(userProfile);
            }
        }

        // GET: UserProfileController/Delete/5
        public ActionResult Delete(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetUserProfileById(id);
            ProtectAdminViewModel vm = new ProtectAdminViewModel()
            {
                userProfile = userProfile
            };
            int count = _userProfileRepository.CheckNumOfAdmins();
            if (count < 2 && userProfile.UserTypeId == 1)
            {
                vm.CanDeactivate = false;
            }
            return View(vm);
        }

        // POST: UserProfileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, UserProfile userProfile)
        {
            try
            {
                _userProfileRepository.DeactivateUser(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(userProfile);
            }
        }

        public ActionResult Reactivate(int id)
        {
            UserProfile userProfile = _userProfileRepository.GetUserProfileById(id);
            return View(userProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Reactivate(int id, UserProfile userProfile)
        {
            try
            {
                _userProfileRepository.ReactivateUser(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(userProfile);
            }
        }
        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
        
    }
}
