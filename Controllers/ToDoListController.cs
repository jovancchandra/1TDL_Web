using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _1TDL_Web.Models;
using Microsoft.AspNet.Identity;

namespace _1TDL_Web.Controllers
{
    public class ToDoListController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();         //The Database

        // GET: ToDoList
        public ActionResult Index()
        {
            //string currentUserId = User.Identity.GetUserId();                                     //User-generated lines
            //ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);    //Query in database for the User currently logged on

            //return View(db.ToDoList.ToList().Where(x => x.User == currentUser));                  //Only returns view of ToDo items associated with current User logged on

            //var toDo = from x in db.ToDoList select x;
            //toDo = toDo.OrderBy(x => x.Due);

            //return View(toDo.ToList());
            return View();
        }

        private IEnumerable<ToDo> GetMyToDoList()
        {
            string currentUserId = User.Identity.GetUserId();                                                          //User-generated lines
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);                         //Query in database for the User currently logged on

            IEnumerable<ToDo> myToDoList = db.ToDoList.ToList().Where(x => x.User == currentUser);

            int completeCount = 0;
            foreach (ToDo toDo in myToDoList)
            {
                if (toDo.Completed)
                    completeCount++;
            }

            ViewBag.Percent = Math.Round(100f * ( (float) completeCount / (float) myToDoList.Count() ));

            return myToDoList;
        }

        public ActionResult BuildToDoTable()
        {
            return PartialView("_ToDoTable", GetMyToDoList());                  //Returns a partial view of ToDo items associated with current User logged on
        }

        // GET: ToDoList/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDoList.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            return View(toDo);
        }

        // GET: ToDoList/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToDoList/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Due,Completed")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();                                     //User-generated lines
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);    //Query in database for the User currently creating the ToDo item
                toDo.User = currentUser;

                db.ToDoList.Add(toDo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(toDo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AJAXCreate([Bind(Include = "Id,Description,Due")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                string currentUserId = User.Identity.GetUserId();                                     //User-generated lines
                ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);    //Query in database for the User currently creating the ToDo item
                toDo.User = currentUser;
                toDo.Completed = false;
                toDo.Due = DateTime.Now;

                db.ToDoList.Add(toDo);
                db.SaveChanges();
                //return RedirectToAction("Index");
            }

            //return View(toDo);
            return PartialView("_ToDoTable", GetMyToDoList());
        }

        // GET: ToDoList/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDoList.Find(id);

            if (toDo == null)
            {
                return HttpNotFound();
            }

            string currentUserId = User.Identity.GetUserId();                                                          //User-generated lines
            ApplicationUser currentUser = db.Users.FirstOrDefault(x => x.Id == currentUserId);                         //Query in database for the User currently logged on

            if (toDo.User != currentUser)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(toDo);
        }

        // POST: ToDoList/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Due,Completed")] ToDo toDo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(toDo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(toDo);
        }

        [HttpPost]
        public ActionResult AJAXEdit(int? id, bool value)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDoList.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            } 
            else
            {
                toDo.Completed = value;
                db.Entry(toDo).State = EntityState.Modified;
                db.SaveChanges();

                return PartialView("_ToDoTable", GetMyToDoList());
            }
        }

        // GET: ToDoList/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ToDo toDo = db.ToDoList.Find(id);
            if (toDo == null)
            {
                return HttpNotFound();
            }
            return View(toDo);
        }

        // POST: ToDoList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ToDo toDo = db.ToDoList.Find(id);
            db.ToDoList.Remove(toDo);
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
