using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ComfySocks.Models;
using ComfySocks.Models.Items;
using Microsoft.AspNet.Identity;

namespace ComfySocks.Controllers
{
    public class StoreTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StoreTypes
        public ActionResult Index()
        {
            var storeTypes = db.StoreTypes;
            return View(storeTypes.ToList());
        }

        // GET: StoreTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            StoreType storeType = db.StoreTypes.Find(id);
            if (storeType == null)
            {
                return HttpNotFound();
            }
            return View(storeType);
        }

        // GET: StoreTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StoreTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,ApplicationUserID")] StoreType storeType)
        {
            storeType.ApplicationUserID = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.StoreTypes.Add(storeType);
                db.SaveChanges();
                TempData[User.Identity.GetUserId() + "succsessMessage"] = "New StoreType is Created";
                return RedirectToAction("Index");
            }
            return View(storeType);
        }

        // GET: StoreTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreType storeType = db.StoreTypes.Find(id);
            if (storeType == null)
            {
                return HttpNotFound();
            }
            return View(storeType);
        }

        // POST: StoreTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,ApplicationUserID")] StoreType storeType)
        {
            storeType.ApplicationUserID = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Entry(storeType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicationUserID = new SelectList(db.ApplicationUsers, "Id", "FullName", storeType.ApplicationUserID);
            return View(storeType);
        }

        // GET: StoreTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StoreType storeType = db.StoreTypes.Find(id);
            if (storeType == null)
            {
                return HttpNotFound();
            }
            return View(storeType);
        }

        // POST: StoreTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StoreType storeType = db.StoreTypes.Find(id);
            db.StoreTypes.Remove(storeType);
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
