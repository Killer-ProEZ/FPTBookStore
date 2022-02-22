using FPTBookStore.DB;
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
            var category = db.Categories.ToList();
            return View(category);
        }
    }
}