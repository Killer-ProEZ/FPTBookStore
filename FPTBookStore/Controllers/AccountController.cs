using FPTBookStore.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPTBookStore.Controllers
{
    public class AccountController : Controller
    {
        private MyApplicationDBContext db = new MyApplicationDBContext();
        // GET: Account
        public ActionResult Index()
        {
            var accounts = db.Accounts.ToList();
            return View(accounts);
        }
        public ActionResult Create()
        {
            List<int> number = new List<int>();
            number.Add(0);
            number.Add(1);
            SelectList stateList = new SelectList(number);

            // Set vào ViewBag
            ViewBag.stateList = stateList;
            return View();
        }
    }
}