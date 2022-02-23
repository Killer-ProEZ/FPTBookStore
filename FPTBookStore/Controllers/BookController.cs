using FPTBookStore.DB;
using FPTBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPTBookStore.Controllers
{
    public class BookController : Controller
    {
        private MyApplicationDBContext db = new MyApplicationDBContext();
        // GET: Book
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            var books = db.Books.ToList();
            return View(books);
        }
        [HttpPost]
        public ActionResult Index(string searchstring)
        {
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            List<Book> data = new List<Book>();
            data = db.Books.Where(x => x.BookName.Contains(searchstring)).ToList();
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }
        public ActionResult Create()
        {
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName");
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName", book.AuthorID);
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", book.CategoryID);
                if (file != null)
                {
                    string pic = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(Server.MapPath("~/Content/images/"), pic);
                    file.SaveAs(path);
                    book.Img = path.ToString();
                }
                if (book == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    Console.WriteLine(book);
                    db.Books.Add(book);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View("Create");

        }
        public ActionResult Edit(int? id)
        {
            var book = db.Books.Where(x => x.BookID == id).FirstOrDefault();
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName", book.AuthorID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", book.CategoryID);
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            if (book == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(book);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName", book.AuthorID);
                ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", book.CategoryID);
                if (Session["Admin"] == null)
                {
                    Session["UserName"] = null;
                    return RedirectToAction("Login", "Home");
                }
                var rebook = db.Books.Where(x => x.BookID == book.BookID).FirstOrDefault();
                if (rebook == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    rebook.BookName = book.BookName;
                    rebook.Price = book.Price;
                    rebook.Img = book.Img;
                    rebook.Stock = book.Stock;
                    rebook.Description = book.Description;
                    rebook.AuthorID = book.AuthorID;
                    rebook.CategoryID = book.CategoryID;
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
            var book = db.Books.Where(x => x.BookID == id).FirstOrDefault();
            if (book == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.Books.Remove(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}