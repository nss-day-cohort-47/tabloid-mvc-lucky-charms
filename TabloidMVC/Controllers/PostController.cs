using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUserProfileRepository _userProfileRepository;

        public PostController(IPostRepository postRepository,
                              ICategoryRepository categoryRepository,
                              ITagRepository tagRepository,
                              IUserProfileRepository userProfileRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _userProfileRepository = userProfileRepository;
        }

        public IActionResult Index()
        {
            int currentId = GetCurrentUserProfileId();
            UserProfile userProfile = _userProfileRepository.GetUserProfileById(currentId);
            var posts = _postRepository.GetAllPublishedPosts();
            foreach (Post post in posts)
            {
                if (post.UserProfileId == currentId || userProfile.UserTypeId == 1)
                {
                    post.CanInteract = true;
                }
            }
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            int currentId = GetCurrentUserProfileId();
            UserProfile userProfile = _userProfileRepository.GetUserProfileById(currentId);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            if (post.UserProfileId == currentId || userProfile.UserTypeId == 1)
            {
                post.CanInteract = true;
            }
            return View(post);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        public IActionResult GetMyPosts()
        {
            int CurrentUserId = GetCurrentUserProfileId();

            List<Post> posts = _postRepository.GetAllPostsByUser(CurrentUserId);

            return View(posts);
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        public ActionResult Delete(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Post post)
        {
            _postRepository.DeletePost(id);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Post post)
        {
            try
            {
                _postRepository.EditPost(post);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(post);
            }
        }
    }
}
