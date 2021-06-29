using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
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
            List<Comment> comments = _commentRepo.GetCommentsByPost(id);

            return View(comments);
        }

        // GET: CommentControllers/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CommentControllers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CommentControllers/Create
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

        // GET: CommentControllers/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommentControllers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: CommentControllers/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CommentControllers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
