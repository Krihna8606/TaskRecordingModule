using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaskRecordingModule.Models;

namespace TaskRecordingModule.Controllers
{
    public class LoginController : Controller
    {
        private TaskRecordingDBEntities db = new TaskRecordingDBEntities();

        public ActionResult Index(string registerstatus="")
        {
            if (registerstatus == "success")
            {
                ViewBag.registerstatus = "success";
            }
            else if (registerstatus == "email")
            {
                ViewBag.registerstatus = "email";
            }
            else
            {
                ViewBag.registerstatus = "";
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Users users = db.Users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password);
                if (users == null)
                {
                    return HttpNotFound();

                }
                else
                {
                    Session["LOGGED_USERID"] = users.Id;
                    Session["LOGGED_USERNAME"] = users.Name;
                    Session["LOGGED_USERTYPE"] = users.UserType;

                    if (users.UserType.ToUpper() == "ADMIN")
                    {
                        return RedirectToAction("Index", "Task");
                    }
                    else if(users.UserType.ToUpper() == "USER")
                    {
                        return RedirectToAction("TaskList", "User");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Login");
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Logout()
        {
            Session["LOGGED_USERID"] = null;
            Session["LOGGED_USERNAME"] = null;
            Session["LOGGED_USERTYPE"] = null;
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register( RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Users users = db.Users.FirstOrDefault(x => x.Email == model.Email);
                if (users != null)
                {
                    return RedirectToAction("Index", "Login", new { registerstatus = "email" });
                }
                else
                {
                    Users registerUsers= new Users();
                    registerUsers.Email = model.Email;
                    registerUsers.Password = model.Password;
                    registerUsers.Name = model.Name;
                    registerUsers.Address = model.Address;
                    registerUsers.ContactNumber = model.ContactNumber;
                    registerUsers.UserType = "user";
                    registerUsers.CreatedDate = DateTime.Now;

                    db.Users.Add(registerUsers);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Login",new{registerstatus="success"});
                    
                }
            }
            return View();
        }

    }
}