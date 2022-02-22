using FPTBookStore.DB;
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
            var authors = db.Authors.ToList();
            return View(authors);
        }
    }
}