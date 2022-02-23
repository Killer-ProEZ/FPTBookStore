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
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            var orders = db.Orders.ToList();
            return View(orders);
        }
        public ActionResult Delete(int? id)
        {
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            var order = db.Orders.Where(x => x.OrderID == id).FirstOrDefault();
            if (order == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.Orders.Remove(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}