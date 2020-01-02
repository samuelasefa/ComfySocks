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
    public class UnitsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Units
        public ActionResult Index()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            var units = db.Units;
            return View(units.ToList());
        }

        // GET: Units/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit unit = db.Units.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        // GET: Units/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Units/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,ApplicationUserID")] Unit unit)
        {
            unit.ApplicationUserID = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Units.Add(unit);
                db.SaveChanges();
                TempData[User.Identity.GetUserId() + "succsessMessage"] = "New unit information created";
                return RedirectToAction("Index");
            }

            return View(unit);
        }

        // GET: Units/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit unit = db.Units.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        // POST: Units/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,ApplicationUserID")] Unit unit)
        {
            unit.ApplicationUserID = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Entry(unit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unit);
        }

        // GET: Units/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Unit unit = db.Units.Find(id);
            if (unit == null)
            {
                return HttpNotFound();
            }
            return View(unit);
        }

        // POST: Units/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Unit unit = db.Units.Find(id);
            db.Units.Remove(unit);
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
