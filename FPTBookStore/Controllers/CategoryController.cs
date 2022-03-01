using FPTBookStore.DB;
using FPTBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPTBookStore.Controllers
{
    public class CategoryController : Controller
    {
        private MyApplicationDBContext db = new MyApplicationDBContext();
        // GET: Category
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            var category = db.Categories.ToList();
            return View(category);
        }
        public ActionResult Create()
        {
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                var checkcategory = db.Categories.Where(x => x.CategoryName == category.CategoryName).FirstOrDefault();
                if (checkcategory != null)
                {
                    ViewBag.Error = "CategoryName is exits";
                    return View("Create");
                }
                if (category == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View("Create");
        }
        public ActionResult Edit(int? id)
        {
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            var category = db.Categories.Where(x => x.CategoryID == id).FirstOrDefault();
            if (category == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(category);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                var recategory = db.Categories.Where(x => x.CategoryID == category.CategoryID).FirstOrDefault();
                if (recategory == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    recategory.CategoryName = category.CategoryName;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View("Edit");
        }
        public ActionResult Delete(int? id)
        {
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            var category = db.Categories.Where(x => x.CategoryID == id).FirstOrDefault();
            if (category == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.Categories.Remove(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}