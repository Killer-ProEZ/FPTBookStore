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
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            var orderdetails = db.Orderdetails.ToList();
            return View(orderdetails);
        }
        public ActionResult Delete(int? id)
        {
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            var orderdetails = db.Orderdetails.Where(x => x.OrderdetailsID == id).FirstOrDefault();
            if (orderdetails == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.Orderdetails.Remove(orderdetails);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}