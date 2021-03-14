using gpxViewer.Configs;
using gpxViewer.DataAccess;
using gpxViewer.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;


namespace gpxViewer.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login login)
        {
            if (ModelState.IsValid)
            {
                using (GpxContext db = new GpxContext())
                {
                    var user = db.UserAccounts.SingleOrDefault(u => u.Username == login.Username);
                    if (user != null && BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
                    {
                        Session["UserId"] = user.UserId.ToString();
                        Session["Username"] = user.Username;
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError("", Resources.Resource.WrongData);
                }
            }
            return View();
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserAccount account)
        {
            if (ModelState.IsValid)
            {
                account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
                account.ConfirmPassword = account.Password;
                using (GpxContext db = new GpxContext())
                {
                    if (db.UserAccounts.Any(user => user.Username == account.Username))
                    {
                        ViewBag.Message = Resources.Resource.Username + " " + account.Username + " " + Resources.Resource.AlreadyExists;
                        return View();
                    }
                    db.UserAccounts.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.Username + " " + Resources.Resource.SuccessfullyRegistered;
            }

            return View();
        }
    }

}