using FPTBookStore.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FPTBookStore.Models;
using System.Web.UI;

namespace FPTBookStore.Controllers
{
    public class CartController : Controller
    {
        private MyApplicationDBContext db = new MyApplicationDBContext();
        // GET: Cart
        private const string CartSession = "CartSesstion";
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<Cart>();
            var sum = 0;
            if (cart != null)
            {
                list = (List<Cart>)cart;
            }
            ViewBag.Sum = 0;
            foreach (var item in list)
            {
                sum += item.Quality * item.Book.Price;
                ViewBag.Sum = sum;
            }
            return View(list);
        }
        public ActionResult AddCart(int? productid, int? quality)
        {
            var book = db.Books.Where(x => x.BookID == productid).FirstOrDefault(); ;
            book.Stock -= 1;
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<Cart>)cart;
                if (list.Exists(x => x.ProductID == productid))
                {
                    foreach(var item in list)
                    {
                        if (item.ProductID == productid)
                        {
                            item.Quality =Convert.ToInt32(item.Quality+quality);
                        }
                    }
                }
                else
                {
                    var item = new Cart();
                    item.ProductID = (int)productid;
                    item.Quality = (int)quality;
                    item.Book=db.Books.Where(x=>x.BookID==productid).FirstOrDefault();
                    list.Add(item);
                }
                Session[CartSession] = list;
                return RedirectToAction("Index");
            }
            else
            {
                var item = new Cart();
                item.ProductID = (int)productid;
                item.Quality = (int)quality;
                item.Book = db.Books.Where(x => x.BookID == productid).FirstOrDefault();
                var list = new List<Cart>();
                list.Add(item);
                Session[CartSession] = list;
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Edit(int? productid, int quality)
        {
            List<Cart> list = (List<Cart>)Session[CartSession];
            foreach (var item in list)
            {
                if (item.ProductID == productid)
                {
                    if (quality <= 0)
                    {
                        Session["Error"] = Convert.ToInt32(item.ProductID);
                        Session["Text"] = "The quality must be greater than 0";
                        return RedirectToAction("Index");
                    }
                    if (item.Book.Stock < quality)
                    {
                        Session["Error"] = Convert.ToInt32(item.ProductID);
                        Session["Text"] = "The number of books is not enough to order";
                        return RedirectToAction("Index");
                    }
                    Session["Error"] = null;
                    item.Quality = quality;
                }
            }
            Session[CartSession] = list;
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int? id)
        {
            List<Cart> list = (List<Cart>)Session[CartSession];
            Cart book=list.Find(x => x.ProductID == id);
            list.Remove(book);
            Session[CartSession] = list;
            return RedirectToAction("Index");
        }
    }
}