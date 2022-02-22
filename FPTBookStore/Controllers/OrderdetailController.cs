using FPTBookStore.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPTBookStore.Controllers
{
    public class OrderdetailController : Controller
    {
        private MyApplicationDBContext db = new MyApplicationDBContext();
        // GET: Orderdetail
        public ActionResult Index()
        {
            var orderdetails = db.Orderdetails.ToList();
            return View(orderdetails);
        }
    }
}