using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimplePhotoGallery.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SimplePhotoGallery.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel m)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            UserAdmin userAdmin = new UserAdmin();
            IdentityUser user = new IdentityUser()
            {
                UserName = m.UserName
            };
            IdentityResult result = userAdmin.AddUser(user, m.Password);
            if (result.Succeeded)
            {
                bool booSignIn = userAdmin.SignInUser(user.UserName, m.Password);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", result.Errors.First());
                return View();
            }
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel m, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            UserAdmin userAdmin = new UserAdmin();
            bool booSignIn = userAdmin.SignInUser(m.UserName, m.Password);
            if (booSignIn)
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Login failed!");
                return View();
            }
        }

        public ActionResult LogOut()
        {
            UserAdmin userAdmin = new UserAdmin();
            userAdmin.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}