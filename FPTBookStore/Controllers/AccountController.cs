﻿using FPTBookStore.DB;
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
    public class AccountController : Controller
    {
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
        private MyApplicationDBContext db = new MyApplicationDBContext();
        // GET: Account
        public ActionResult Index()
        {
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            var accounts = db.Accounts.ToList();
            return View(accounts);
        }
        public ActionResult Create()
        {
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            List<int> number = new List<int>();
            number.Add(0);
            number.Add(1);
            SelectList stateList = new SelectList(number);
            ViewBag.stateList = stateList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Account account)
        {
            if (ModelState.IsValid)
            {
                if (account == null)
                {
                    return HttpNotFound();
                }
                account.Password = GetMD5(account.Password);
                account.RePassword = GetMD5(account.RePassword);
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Create");
        }
        public ActionResult Edit(string username)
        {
            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            var account = db.Accounts.Where(x => x.UserName == username).FirstOrDefault();
            List<int> number = new List<int>();
            if (account.State == 0)
            {
                number.Add(0);
                number.Add(1);
            }
            else
            {
                number.Add(1);
                number.Add(0);
            }
            SelectList stateList = new SelectList(number);
            ViewBag.stateList = stateList;
            return View(account);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Account account)
        {
            if (ModelState.IsValid)
            {
                if (account == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    var reaccount = db.Accounts.Where(x => x.UserName == account.UserName).FirstOrDefault();
                    reaccount.Fullname = account.Fullname;
                    reaccount.Email = account.Email;
                    reaccount.Address = account.Address;
                    reaccount.Password = GetMD5(account.Password);
                    reaccount.RePassword = GetMD5(account.RePassword);
                    reaccount.Tel = account.Tel;
                    reaccount.State = account.State;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View("Edit");
        }

        public ActionResult Delete(string username)
        {

            if (Session["Admin"] == null)
            {
                Session["UserName"] = null;
                return RedirectToAction("Login", "Home");
            }
            var account = db.Accounts.Where(x => x.UserName == username).FirstOrDefault();
            if (account == null)
            {
                return HttpNotFound();
            }
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}