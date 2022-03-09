using FPTBookStore.DB;
using FPTBookStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
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
            data = db.Books.Where(x => x.BookName.ToLower().Contains(searchstring.ToLower())).ToList();
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }
        public ActionResult Create()
        {
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName");
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book, HttpPostedFileBase file)
        {
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName", book.AuthorID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", book.CategoryID);
            if (file == null)
            {
                ViewBag.Error = "Image can't be empty";
                return View("Create");
            }
            if (ModelState.IsValid)
            {
                if (book.Stock <= 0)
                {
                    ViewBag.Stock = "Stock must be greater than 0";
                    return View("Create");
                }
                if (book.Price <= 0)
                {
                    ViewBag.Price = "Price must be greater than 0";
                    return View("Create");
                }
                var rebook = db.Books.Where(x => x.BookID == book.BookID).FirstOrDefault();
                if (rebook != null)
                {
                    ViewBag.Info = "BookName is exist";
                    return View("Create");
                }
                string pic = System.IO.Path.GetFileName(file.FileName);
                if (file != null)
                {
                    string path = Path.Combine(Server.MapPath("~/Content/images/"), Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    book.Img = pic.ToString();
                }

                if (book == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    book.Img = pic.ToString();
                    db.Books.Add(book);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName", book.AuthorID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", book.CategoryID);
            return View("Create");

        }
        public ActionResult Edit(int? id)
        {
            
            var book = db.Books.Where(x => x.BookID == id).FirstOrDefault();
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName");
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
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
        public ActionResult Edit(Book book, HttpPostedFileBase file)
        {
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            var rebook = db.Books.Where(x => x.BookID == book.BookID).FirstOrDefault();
            string pic = "";
            if (book.Stock <= 0)
            {
                Session["Stock"] = "Stock must be greater than 0";
                return RedirectToAction("Edit", new { id = book.BookID });
            }
            else
            {
                Session["Stock"] = null;
            }
            if (book.Price <= 0)
            {
                Session["Price"] = "Price must be greater than 0";
                return RedirectToAction("Edit");
            }
            else
            {
                Session["Price"] = null;
            }
            if (file != null)
            {
                string file_name = book.Img;
                string path1 = Server.MapPath("~/Content/images/");
                FileInfo file1 = new FileInfo(path1 + file_name);
                if (file1.Exists)
                {
                    file1.Delete();
                }
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/images/"), Path.GetFileName(file.FileName));
                file.SaveAs(path);
                rebook.Img = pic.ToString();
            }

            if (ModelState.IsValid)
            {
                if (rebook == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    rebook.BookName = book.BookName;
                    rebook.Price = book.Price;
                    rebook.Stock = book.Stock;
                    rebook.Description = book.Description;
                    rebook.AuthorID = book.AuthorID;
                    rebook.CategoryID = book.CategoryID;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "AuthorID", "AuthorName", book.AuthorID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", book.CategoryID);
            return View(book);
        }
        public ActionResult Delete(int? id)
        {

            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            var book = db.Books.Where(x => x.BookID == id).FirstOrDefault();
            string file_name = book.Img;
            string path = Server.MapPath("~/Content/images/");
            FileInfo file = new FileInfo(path + file_name);
            if (file.Exists)
            {
                file.Delete();
            }
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