﻿using BanSach.DataAccess.Data;
using BanSach.Model;
using Microsoft.AspNetCore.Mvc;

namespace BanSachWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == null)
            {
                ModelState.AddModelError("CustomError","The name must not be empty");
                //ModelState.AddModelError("name","The name must not be empty");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["Success"] = "Category create successfully";
                return RedirectToAction("index");
            }
            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? category = _db.Categories.FirstOrDefault(x=>x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == null || obj.Name == "")
            {
                ModelState.AddModelError("CustomError", "The name must not be empty");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["Success"] = "Category update successfully";
                return RedirectToAction("index");
            }
            return View(obj);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? category = _db.Categories.FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            Category? remove = _db.Categories.FirstOrDefault(y => y.Id == id);
            if (remove != null && ModelState.IsValid)
            {
                _db.Categories.Remove(remove);
                _db.SaveChanges();
                TempData["Success"] = "Category delete successfully";
                return RedirectToAction("index");
            }
            return View();
        }
    }
}
