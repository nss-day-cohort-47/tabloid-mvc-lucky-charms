﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IPostRepository _postRepository;
        private readonly IPostTagRepository _postTagRepository;

        public TagController(ITagRepository tagRepository,
                             IPostRepository postRepository,
                             IPostTagRepository postTagRepository)
        {
            _tagRepository = tagRepository;
            _postRepository = postRepository;
            _postTagRepository = postTagRepository;
        }

        // GET: TagController
        public ActionResult Index()
        {
            List<Tag> tags = _tagRepository.GetAllTags();

            return View(tags);
        }

        // GET: TagController
        public ActionResult TagListByPost(int id)
        {
            List<Tag> tags = _tagRepository.GetAllTags();
            List<Tag> postTags = _tagRepository.GetTagsByPostId(id);

            TagManagerViewModel vm = new TagManagerViewModel()
            {
                AllTags = tags,
                AddedTags = postTags
            };

            return View(vm);
        }

        // GET: TagController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TagController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TagController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tag tag)
        {
            try
            {
                _tagRepository.Add(tag);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(tag);
            }
        }

        // GET: TagController/Edit/5
        public ActionResult Edit(int id)
        {
            Tag tag = _tagRepository.GetTagById(id);

            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        // POST: TagController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Tag tag)
        {
            try
            {
                _tagRepository.EditTag(tag);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(tag);
            }
        }

        // GET: TagController/Delete/5
        public ActionResult Delete(int id)
        {
            Tag tag = _tagRepository.GetTagById(id);
            return View(tag);
        }

        // POST: TagController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tag tag)
        {
            try
            {
                _tagRepository.DeleteTag(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View(tag);
            }
        }
    }
}
