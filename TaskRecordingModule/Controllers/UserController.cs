using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using TaskRecordingModule.Models;

namespace TaskRecordingModule.Controllers
{
    public class UserController : Controller
    {
        private TaskRecordingDBEntities db = new TaskRecordingDBEntities();
        public ActionResult Profile(string id)
        {
            if (Session["LOGGED_USERID"] == null || Session["LOGGED_USERNAME"] == null || Session["LOGGED_USERTYPE"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(Convert.ToInt32(id));
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        public ActionResult TaskList()
        {
            if (Session["LOGGED_USERID"] == null || Session["LOGGED_USERNAME"] == null || Session["LOGGED_USERTYPE"] == null)
            {
                return RedirectToAction("Index", "Login");
            }


            var taskList = db.Task.ToList().Where(x => x.UserId.IsEmpty());
            List<TaskModel> models= new List<TaskModel>();
            foreach (var list in taskList)
            {
                TaskModel task =new TaskModel();
                task.Id = list.Id;
                task.Name = list.Name;
                task.Description = list.Description;
                task.UserId = list.UserId;
                task.Status = list.Status;
                task.CreatedDate = list.CreatedDate;

                models.Add(task);
            }
            return View(models);
        }

        public ActionResult AssignMe(int id)
        {
            Task tasks = db.Task.Find(id);
            if (tasks == null)
            {
                return HttpNotFound();
            }
            //Task assignMe=new Task();
            tasks.Id = id;

            tasks.UserId = Session["LOGGED_USERID"].ToString();

            db.Entry(tasks).State = EntityState.Modified;
            db.SaveChanges();



            //return RedirectToAction("Index", "Login");
	    //return redirect
            return RedirectToAction("TaskList");
        }

        public ActionResult MyTask()
        {
            var taskList = db.Task.ToList().Where(x => x.UserId== Session["LOGGED_USERID"].ToString());
            List<TaskModel> models = new List<TaskModel>();
            foreach (var list in taskList)
            {
                TaskModel task = new TaskModel();
                task.Id = list.Id;
                task.Name = list.Name;
                task.Description = list.Description;
                task.UserId = list.UserId;
                task.Status = list.Status;
                task.CreatedDate = list.CreatedDate;
                task.SubmittedDate = list.SubmittedDate;

                models.Add(task);
            }


            return RedirectToAction("Index", "Login");
            //return View(models);


        }
        public ActionResult Complete(int id)
        {

            Task tasks = db.Task.Find(id);
            if (tasks == null)
            {
                return HttpNotFound();
            }
            //Task assignMe = new Task();
            tasks.Status = "complete";
            tasks.SubmittedDate = DateTime.Now;
            db.Entry(tasks).State = EntityState.Modified;
            db.SaveChanges();


            return RedirectToAction("MyTask");
        }

        // GET: User
        public ActionResult Index()
        {
            int i = checkAdmin();
            if (i == 1)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(db.Users.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            int i = checkAdmin();
            if (i == 1)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            int i = checkAdmin();
            if (i == 1)
            {
                return RedirectToAction("Index", "Login");
            }
            List<SelectListItem> UserType = new List<SelectListItem>();
            UserType.Add(new SelectListItem { Text = "Select User Type", Value = "", Selected = true });
            UserType.Add(new SelectListItem { Text = "Admin", Value = "admin" });
            UserType.Add(new SelectListItem { Text = "User", Value = "user" });

            ViewBag.UserType = UserType;

            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterModel model)
        {
            int i = checkAdmin();
            if (i == 1)
            {
                return RedirectToAction("Index", "Login");
            }
            List<SelectListItem> UserType = new List<SelectListItem>();
            UserType.Add(new SelectListItem { Text = "Select User Type", Value = "", Selected = true });
            UserType.Add(new SelectListItem { Text = "Admin", Value = "admin" });
            UserType.Add(new SelectListItem { Text = "User", Value = "user" });

            ViewBag.UserType = UserType;

            if (ModelState.IsValid)
            {
                Users users = db.Users.FirstOrDefault(x => x.Email == model.Email);
                if (users != null)
                {
                    ModelState.AddModelError("Email", "Email Already Exist!");
                    return View(model);
                }
                else
                {
                    Users registerUsers = new Users();
                    registerUsers.Email = model.Email;
                    registerUsers.Password = model.Password;
                    registerUsers.Name = model.Name;
                    registerUsers.Address = model.Address;
                    registerUsers.ContactNumber = model.ContactNumber;
                    registerUsers.UserType = model.UserType;
                    registerUsers.CreatedDate = DateTime.Now;

                    db.Users.Add(registerUsers);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            return View(model);

        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            int i = checkAdmin();
            if (i == 1)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }

            return View(users);
        }

        // POST: User/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Email,Name,Password,UserType,ContactNumber,Address,CreatedDate")] Users users)
        {
            int i = checkUser();
            if (i == 1)
            {
                return RedirectToAction("Index", "Login");
            }
            if (i == 1)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            int i = checkAdmin();
            if (i == 1)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            int i = checkAdmin();
            if (i == 1)
            {
                return RedirectToAction("Index", "Login");
            }
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public int checkAdmin()
        {
            if (Session["LOGGED_USERID"] == null || Session["LOGGED_USERNAME"] == null || Session["LOGGED_USERTYPE"] == null)
            {
                return 1;
            }

            if (Session["LOGGED_USERTYPE"].ToString().ToUpper() != "ADMIN")
            {
                return 1;
            }

            return 0;
        }
        public int checkUser()
        {
            if (Session["LOGGED_USERID"] == null || Session["LOGGED_USERNAME"] == null || Session["LOGGED_USERTYPE"] == null)
            {
                return 1;
            }

            if (Session["LOGGED_USERTYPE"].ToString().ToUpper() != "USER")
            {
                return 1;
            }

            return 0;
        }
    }
}
