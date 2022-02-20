using FPTBookStore.DB;
using FPTBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FPTBookStore.Controllers
{
    public class HomeController : Controller
    {
        private MyApplicationDBContext db = new MyApplicationDBContext();
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
        public ActionResult Index()
        {
            var data = db.Books.ToList();
            return View(data);
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Help()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var c_password = GetMD5(password);
                var data = db.Accounts.Where(s => s.UserName.Equals(username) && s.Password.Equals(c_password)).ToList();
                if (data.Count() > 0)
                {
                    TempData["UserName"] = data.FirstOrDefault().UserName;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Error = "Login failed";
                    return View("Login");
                }
            }
            ViewBag.Error = "Login failed";
            return View("Login");
        }
        public ActionResult Logout()
        {
            TempData["UserName"] = null;
            return RedirectToAction("Index");
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Account account)
        {
            if (ModelState.IsValid)
            {
                var check = db.Accounts.FirstOrDefault(x => x.Email == account.Email);
                if(check == null)
                {
                    account.Password = GetMD5(account.Password);
                    account.RePassword = GetMD5(account.RePassword);
                    account.State = 0;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Accounts.Add(account);
                    db.SaveChanges();
                    return View("Login");
                }
                else
                {
                    ViewBag.Error = "Account already exists";
                    return View ("Register");
                }
            }
            ViewBag.Error = "Account already exists";
            return View("Register");
        }
        public ActionResult Details(int? id)
        {
            var book = db.Books.FirstOrDefault(x => x.BookID == id);
            if (book == null)
            {
                return HttpNotFound();
            }
            else
            {
                var Author = db.Authors.FirstOrDefault(x => x.AuthorID == book.AuthorID);
                var Category = db.Categories.FirstOrDefault(x => x.CategoryID == book.CategoryID);
                ViewBag.Author = Author.AuthorName;
                ViewBag.Category = Category.CategoryName;
            }
            return View(book);
        }

        public ActionResult Cart()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View("Index");
        }
        [HttpPost]
        public ActionResult Search(string searchstring)
        {
            return View();
        }
    }
}