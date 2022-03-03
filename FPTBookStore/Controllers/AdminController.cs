using FPTBookStore.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPTBookStore.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private MyApplicationDBContext db = new MyApplicationDBContext();
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login","Home");
            }
            var orders = db.Orders.ToList();
            var numberOrder = db.Orders.ToList().Count();
            int revenue = 0;
            foreach (var order in orders)
            {
                revenue = +order.TotalPrice;
            }
            var user = db.Accounts.ToList().Count();
            var books = db.Books.ToList().Count();
            var bookdata = db.Books.ToList().OrderByDescending(x => x.Date_add);
            ViewBag.users = user;
            ViewBag.books = books;
            ViewBag.revenue = revenue;
            ViewBag.orders = numberOrder;
            return View(bookdata);
        }
     
        public ActionResult Report()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Report(DateTime startday, DateTime endday)
        {
            var order = db.Orders.Where(x => x.OrderDate > startday && x.OrderDate < endday);
            var sum = 0;
            foreach (var item in order)
            {
                sum = sum + item.TotalPrice;
            }
            ViewBag.Result = sum;
            return View();
        }
    }
}