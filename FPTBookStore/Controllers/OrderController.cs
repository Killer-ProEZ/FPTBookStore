using FPTBookStore.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPTBookStore.Controllers
{
    public class OrderController : Controller
    {
        private MyApplicationDBContext db = new MyApplicationDBContext();
        // GET: Order
        public ActionResult Index()
        {
            var orders = db.Orders.ToList();
            return View(orders);
        }
    }
}