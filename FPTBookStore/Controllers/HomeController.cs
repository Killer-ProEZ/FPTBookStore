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
            Session["Admin"] = null;
            var data = db.Books.ToList();
            return View(data);
        }
        [HttpPost]
        public ActionResult Index(string searchstring)
        {
            if (searchstring == null)
            {
                ViewBag.Error = "Searching can't be empty ";
                return View("Index");
            }
            Session["Admin"] = null;
            List<Book> data = new List<Book>();
            data = db.Books.Where(x => x.BookName.Contains(searchstring)||x.Category.CategoryName.ToLower()==searchstring.ToLower()).ToList();
            if (data==null)
            {
                return RedirectToAction("Index");
            }
            return View(data);
        }
        public ActionResult About()
        {
            Session["Admin"] = null;
            return View();
        }

        public ActionResult Help()
        {
            Session["Admin"] = null;
            return View();
        }
        public ActionResult Login()
        {
            Session["Admin"] = null;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                if (username.Equals("") || password.Equals(""))
                {
                    ViewBag.Error = "Username or password can't empty";
                    return View("Login");
                }
                var c_password = GetMD5(password);
                var data = db.Accounts.Where(s => s.UserName.Equals(username) && s.Password.Equals(c_password)).ToList();
                if (data.Count() > 0)
                {
                    if (data.FirstOrDefault().State == 0)
                    {
                        Session["UserName"] = data.FirstOrDefault().UserName.ToString();
                        if (TempData["No"] != null)
                        {
                            Session["No"] = 1;
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Session["Admin"] = data.FirstOrDefault().UserName.ToString();
                        return RedirectToAction("Index","Admin");
                    }
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
            Session.Clear();
            return RedirectToAction("Index");
        }
        public ActionResult Register()
        {
            Session["Admin"] = null;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Account account)
        {
            if (ModelState.IsValid)
            {
                var checkEmail = db.Accounts.FirstOrDefault(x => x.Email == account.Email);
                var checkUserName = db.Accounts.FirstOrDefault(x => x.UserName == account.UserName);
                if (checkEmail == null && checkUserName==null)
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
            Session["Admin"] = null;
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
      
        public ActionResult Edit()
        {
            Session["Admin"] = null;
            if (Session["No"]==null)
            {
                Session["infor"] = "You must re-enter before updating personal information";
                TempData["No"] = 1;
                return RedirectToAction("Login");
            }
            var user = Convert.ToString(Session["UserName"]);
            var account = db.Accounts.FirstOrDefault(x => x.UserName.Equals(user));
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Account account)
        {
            if (ModelState.IsValid)
            {
                var user = Convert.ToString(Session["UserName"]);
                var reaccount = db.Accounts.FirstOrDefault(x => x.UserName.Equals(user));
                if (account == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    if (account.Password != reaccount.Password)
                    {
                        reaccount.Password = GetMD5(account.Password);
                        reaccount.RePassword = GetMD5(account.RePassword);
                    }
                    reaccount.Address = account.Email;
                    reaccount.Fullname = account.Fullname;
                    reaccount.Tel = account.Tel;
                    reaccount.Email = account.Email;
                    reaccount.State = 0;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                    Session["Profile"] = "Profile";
                }
                return View(reaccount);
            }
            return View(account);
        }
    }
}

