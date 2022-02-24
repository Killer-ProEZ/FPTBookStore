using FPTBookStore.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FPTBookStore.Models;
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
            if (cart != null)
            {
                list = (List<Cart>)cart;
            }
            return View(list);
        }
        public ActionResult AddCart(int? productid, int? quality)
        {
            var cart = Session[CartSession];
            Console.WriteLine(cart);
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
                Console.WriteLine(list);
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
        public Action
    }
}