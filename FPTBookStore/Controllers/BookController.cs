using FPTBookStore.DB;
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
            var books = db.Books.ToList();
            return View(books);
        }
    }
}