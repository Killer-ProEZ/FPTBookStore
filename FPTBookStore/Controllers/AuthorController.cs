using FPTBookStore.DB;
using FPTBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPTBookStore.Controllers
{
    public class AuthorController : Controller
    {
        // GET: Author
        private MyApplicationDBContext db = new MyApplicationDBContext();
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            var authors = db.Authors.ToList();
            if (authors == null)
            {
                return HttpNotFound();
            }
            return View(authors);
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
        public ActionResult Create(Author author)
        {
            if (ModelState.IsValid)
            {
                var checkauthor = db.Authors.Where(x => x.AuthorName == author.AuthorName).FirstOrDefault();
                if (checkauthor != null)
                {
                    ViewBag.Error = "CategoryName is exits";
                    return View("Create");
                }
                if (author == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    db.Authors.Add(author);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(author);
            
        }
        public ActionResult Edit(int? id)
        {
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            var author = db.Authors.Where(x => x.AuthorID == id).FirstOrDefault();
            if (author == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(author);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Author author)
        {
            if (ModelState.IsValid)
            {
                var reauthor = db.Authors.Where(x => x.AuthorID == author.AuthorID).FirstOrDefault();
                if (reauthor == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    reauthor.AuthorName = author.AuthorName;
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
            var author = db.Authors.Where(x => x.AuthorID == id).FirstOrDefault();
            if (author == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.Authors.Remove(author);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}