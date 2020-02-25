using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ComfySocks.Models;
using ComfySocks.Models.InventoryModel;
using Microsoft.AspNet.Identity;

namespace ComfySocks.Controllers
{
    public class StoresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Stores
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult Index()
        {
            var stores = db.Stores;
            return View(stores.ToList());
        }

        // GET: Stores/Details/5
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = db.Stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        // GET: Stores/Create
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]

        public ActionResult Create()
        {
            return View();
        }

        // POST: Stores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]

        public ActionResult Create([Bind(Include = "ID,Name,Location,ApplicationUserID")] Store store)
        {
            store.ApplicationUserID = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                if ((from s in db.Stores where s.Name == store.Name || s.Location == store.Location select s).Count() == 0)
                {
                    db.Stores.Add(store);
                    db.SaveChanges();
                    TempData[User.Identity.GetUserId() + "succsessMessage"] = "New Store Created!";
                    return RedirectToAction("Index");
                }
                ViewBag.errorMessage = "Duplicate Store Name or Location";
            }
            return View(store);
        }

        // GET: Stores/Edit/5
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = db.Stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        // POST: Stores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]

        public ActionResult Edit([Bind(Include = "ID,Name,Location,ApplicationUserID")] Store store)
        {
            store.ApplicationUserID = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                if ((from s in db.Stores where s.Name == store.Name || s.Location == store.Location select s).Count()==0){
                    db.Entry(store).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData[User.Identity.GetUserId() + "succsessMessage"] = "Store Is Edited Succesfully!!";
                    return RedirectToAction("Index");
                }
                ViewBag.errorMessage = "Duplicate update is found";
            }
            return View(store);
        }

        // GET: Stores/Delete/5
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = db.Stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }

        // POST: Stores/Delete/5
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Store store = db.Stores.Find(id);
            db.Stores.Remove(store);
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
