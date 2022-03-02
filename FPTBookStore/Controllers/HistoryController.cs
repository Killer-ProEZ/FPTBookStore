using FPTBookStore.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPTBookStore.Controllers
{
    public class HistoryController : Controller
    {
        private MyApplicationDBContext db = new MyApplicationDBContext();
        // GET: History
        public ActionResult Index()
        {
            Session["Admin"] = null;
            var user = Session["UserName"].ToString();
            var order = db.Orders.Where(x => x.UserName == user).ToList();
            return View(order);
        }
        public ActionResult Details(int? id)
        {
            Session["Admin"] = null;
            var oderdetails = db.Orderdetails.Where(x => x.OrderID == id).ToList();
            return View(oderdetails);
        }
    }
}