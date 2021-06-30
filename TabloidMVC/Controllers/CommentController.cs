using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepo;

        public CommentController(ICommentRepository commentRepository)
        {
            _commentRepo = commentRepository;
        }

        // GET: CommentControllers
        public ActionResult Index(int id)
        {
            var vm = new CommentCreateViewModel();
            vm.Comments = _commentRepo.GetCommentsByPost(id);
            vm.PostId = id;
            return View(vm);
        }

        // GET: CommentControllers/Details/5
        public ActionResult Details(int id)
        {
            Comment comment = _commentRepo.GetCommentById(id);

            return View(comment);
        }

        // GET: CommentControllers/Create
        public ActionResult Create(int id)
        {
            var vm = new CommentCreateViewModel();
            vm.Comment = new Comment();
            vm.PostId = id;
            vm.UserId = GetCurrentUserProfileId();
            return View(vm);
        }

        // POST: CommentControllers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommentCreateViewModel vm)
        {
            vm.Comment.PostId = vm.PostId;
            vm.Comment.UserProfileId = vm.UserId;
            try
            {
                _commentRepo.AddComment(vm.Comment);
                return RedirectToAction("Index", new { id = vm.PostId });
            }
            catch (Exception ex)
            {
                return View(vm.Comment);
            }
        }

        // GET: CommentControllers/Edit/5
        public ActionResult Edit(int id)
        {
            Comment comment = _commentRepo.GetCommentById(id);

            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // POST: CommentControllers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Comment comment)
        {
            try
            {
                _commentRepo.UpdateComment(comment);

                return RedirectToAction("Index", new { id = comment.PostId });
            }
            catch
            {
                return View(comment);
            }
        }

        // GET: CommentControllers/Delete/5
        public ActionResult Delete(int id)
        {
            Comment comment = _commentRepo.GetCommentById(id);

            return View(comment);
        }

        // POST: CommentControllers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Comment comment)
        {
            try
            {
                comment = _commentRepo.GetCommentById(id);
                int postId = comment.PostId;

                _commentRepo.DeleteComment(id);

                return RedirectToAction("Index", new { id = postId });
            }
            catch
            {
                return View(comment);
            }
        }

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
