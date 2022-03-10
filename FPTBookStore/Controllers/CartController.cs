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
            var quanlity = 0;
            Session["Quality"] = 0;
            foreach (var item in list)
            {
                sum += item.Quality * item.Book.Price;
                quanlity += item.Quality;
                Session["Money"] = sum;
            }
            Session["Quality"] = quanlity;
            return View(list);
        }
        public ActionResult AddCart(int? productid, int? quality)
        {
            var book = db.Books.Where(x => x.BookID == productid).FirstOrDefault(); ;
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
                Session["Add"] = "Add";
                var quanlity = 0;
                Session["Quality"] = 0;
                foreach (var item in list)
                {
                    quanlity += item.Quality;
                }
                Session["Quality"] = quanlity;
                return RedirectToAction("Index","Home");
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
                var quanlity = 0;
                Session["Quality"] = 0;
                foreach (var item1 in list)
                {
                    quanlity += item1.Quality;
                }
                Session["Quality"] = quanlity;
            }
            Session["Add"] = "Add";
            return RedirectToAction("Index","Home");
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
        public ActionResult Order()
        {
            if (Session["Error"] != null)
            {
                return RedirectToAction("Index", "Cart");
            }
            if (Session["UserName"] == null)
            {
                Session["infor"] = "You must login before ordering";
                return RedirectToAction("Login", "Home");
            }
            string user = Session["UserName"].ToString();
            var account = db.Accounts.Where(x => x.UserName == user).FirstOrDefault();
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }
        [HttpPost]
        public ActionResult Order(string total)
        {
            if (Session["Error"] != null)
            {
                return RedirectToAction("Index","Cart");
            }
            if (Session["UserName"] == null)
            {
                Session["infor"] = "You must login before ordering";
                return RedirectToAction("Login", "Home");
            }
            string user = Session["UserName"].ToString();
            var account = db.Accounts.Where(x => x.UserName == user).FirstOrDefault();
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }
        [HttpPost]
        public ActionResult Confirm(string fullname, int tel, string money, string address)
        {
            var user = Session["UserName"].ToString();
            var person = db.Accounts.Where(x => x.UserName == user).FirstOrDefault();
            if (String.IsNullOrWhiteSpace(address))
            {
                Session["Address"] = "Address can't be empty";
                return RedirectToAction("Order");
            }
            int total = Convert.ToInt32(Session["Money"]);
            Order order = new Order();
            order.OrderDate = DateTime.Now;
            order.TotalPrice = total;
            order.UserName = user;
            order.Address = address;
            db.Orders.Add(order);
            db.SaveChanges();
            List<Cart> list = (List<Cart>)Session[CartSession];
            var order1 = db.Orders.OrderByDescending(x => x.OrderID).FirstOrDefault();
            foreach (var item in list)
            {              
                Orderdetail orderdetail = new Orderdetail();
                orderdetail.BookID = Convert.ToInt32(item.Book.BookID);
                orderdetail.Quality = Convert.ToInt32(item.Quality);
                orderdetail.OrderID = Convert.ToInt32(order1.OrderID);
                orderdetail.Price = Convert.ToInt32(item.Quality * item.Book.Price);
                db.Orderdetails.Add(orderdetail);
                db.SaveChanges();
                var book = db.Books.Where(x => x.BookID == item.ProductID).FirstOrDefault();
                book.Stock = book.Stock - item.Quality;
                db.SaveChanges();
            }
            Session.Clear();
            Session["UserName"] = user;
            Session["Order"] = "Order";
            return RedirectToAction("Index","Home");
        }
    }
}