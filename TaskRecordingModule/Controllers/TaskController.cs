using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using TaskRecordingModule.Models;

namespace TaskRecordingModule.Controllers
{
    //[Authorize]

    public class TaskController : Controller
    {
        private TaskRecordingDBEntities db = new TaskRecordingDBEntities();

        // GET: Task
        public ActionResult Index()
        {
            if (Session["LOGGED_USERID"] == null || Session["LOGGED_USERNAME"] == null || Session["LOGGED_USERTYPE"] ==null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LOGGED_USERTYPE"].ToString().ToUpper() != "ADMIN")
            {
                return RedirectToAction("Index", "Login");
            }
            return View(db.Task.ToList());
        }

        // GET: Task/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Task.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: Task/Create

        public ActionResult Create()
        {
            if (Session["LOGGED_USERID"] == null || Session["LOGGED_USERNAME"] == null || Session["LOGGED_USERTYPE"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LOGGED_USERTYPE"].ToString().ToUpper() != "ADMIN")
            {
                return RedirectToAction("Index", "Login");
            }

            var list = db.Users.ToList().Where(x => x.UserType.ToUpper() == "USER");
            List<SelectListItem> selectUserList = new List<SelectListItem>();
            selectUserList.Add(new SelectListItem { Text = "Select User", Value = "", Selected = true });
            foreach (var user in list)
            {
                selectUserList.Add(new SelectListItem { Text = user.Name, Value = user.Id.ToString() });
            }
            List<SelectListItem> status = new List<SelectListItem>();
            status.Add(new SelectListItem { Text = "Select Task Status", Value = "", Selected = true });
            status.Add(new SelectListItem { Text = "InComplete", Value = "incomplete", Selected = true });
            status.Add(new SelectListItem { Text = "Completed", Value = "complete", Selected = true });

            ViewBag.UserList = selectUserList;
            ViewBag.Status = status;

            return View();
        }

        // POST: Task/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaskModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate =DateTime.Now;
                Task task =new Task();
                task.Name = model.Name;
                task.Description = model.Description;
                task.UserId = model.UserId;
                task.Status = model.Status;
                task.CreatedDate = model.CreatedDate;
                task.SubmittedDate = model.SubmittedDate;
                db.Task.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Task/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Task.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            var list = db.Users.ToList().Where(x => x.UserType.ToUpper() == "USER");
            List<SelectListItem> selectUserList = new List<SelectListItem>();
            selectUserList.Add(new SelectListItem { Text = "Select User", Value = "" });
            bool selectedbool = new bool();
            foreach (var user in list)
            {
                if (user.Id == id)
                {
                    selectUserList.Add(new SelectListItem { Text = user.Name, Value = user.Id.ToString(), Selected = true });
                }
                selectUserList.Add(new SelectListItem { Text = user.Name, Value = user.Id.ToString()  });
            }
            List<SelectListItem> status = new List<SelectListItem>();
            selectedbool = string.IsNullOrEmpty(task.Status)?false : task.Status.ToLower() == "complete"?true:false;

            status.Add(new SelectListItem { Text = "Select Task Status", Value = "" });
            status.Add(new SelectListItem { Text = "Completed", Value = "complete", Selected = selectedbool });

            ViewBag.UserList = selectUserList;
            ViewBag.Status = status;

            TaskModel model =new TaskModel();
            model.Name = task.Name;
            model.Description = task.Description;
            model.UserId = task.UserId;
            model.Status = task.Status;
            return View(model);
        }

        // POST: Task/Edit/5
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,UserId,Status,CreatedDate,SubmittedDate")] TaskModel model)
        {
            if (ModelState.IsValid)
            {
                model.CreatedDate = DateTime.Now;
                Task task = new Task();
                task.Name = model.Name;
                task.Description = model.Description;
                task.UserId = model.UserId;
                task.Status = model.Status;
                task.CreatedDate = model.CreatedDate;
                task.SubmittedDate = model.SubmittedDate;
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Task/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Task task = db.Task.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = db.Task.Find(id);
            db.Task.Remove(task);
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
    }
}
