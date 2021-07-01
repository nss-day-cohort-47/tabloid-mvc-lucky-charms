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
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ITagRepository _tagRepository;

        public PostController(IPostRepository postRepository,
                              ICategoryRepository categoryRepository,
                              IUserProfileRepository userProfileRepository,
                              ISubscriptionRepository subscriptionRepository,
                              ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _userProfileRepository = userProfileRepository;
            _subscriptionRepository = subscriptionRepository;
            _tagRepository = tagRepository;
        }

        public int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);

            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }

            bool ShouldShowSubscribe()
            {
                int currentUserId = GetCurrentUserId();
                int postAuthorId = post.UserProfileId;

                if (currentUserId == postAuthorId)
                {
                    return false;
                } // Checking if the subcription already exists. If it does, returns false
                else if (_subscriptionRepository.GetSubscriptionBySubPro(currentUserId, postAuthorId) != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            bool ShouldShowUnsubscribe()
            {
                int currentUserId = GetCurrentUserId();
                int postAuthorId = post.UserProfileId;

                if (_subscriptionRepository.GetSubscriptionBySubPro(currentUserId, postAuthorId) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            PostDetailsViewModel vm = new PostDetailsViewModel()
            {
                Post = post,
                ShowSubscribe = ShouldShowSubscribe(),
                ShowUnsubscribe = ShouldShowUnsubscribe()
            };

            return View(vm);
        }

        public IActionResult Subscribe(int id)
        {
            Post thisPost = _postRepository.GetPublishedPostById(id);
            UserProfile thisPostAuthor = _userProfileRepository.GetById(thisPost.UserProfileId);
            int currentUserId = GetCurrentUserId();

            Subscription subscription = new Subscription()
            {
                SubscriberUserProfileId = currentUserId,
                ProviderUserProfileId = thisPostAuthor.Id
            };

            _subscriptionRepository.AddSubscription(subscription);
            return RedirectToAction($"Details", new { id = id });
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
            PostAllCategoryViewModel vm = new PostAllCategoryViewModel();


            vm.Post = _postRepository.GetPublishedPostById(id);
            vm.AllCategories = _categoryRepository.GetAll();
            if (vm == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Post post)
        {
            try
            {
                //int catId = post.CategoryId;
                //post.Category = _categoryRepository.GetById(catId);

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
