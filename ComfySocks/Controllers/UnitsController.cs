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
        [Authorize(Roles="Super Admin, Admin Store Manager")]
        public ActionResult UnitList()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            var units = db.Units;
            if (units.ToList().Count > 0)
            {
                ViewBag.HaveList = true;
            }
            else {
                ViewBag.HaveList = false;
            }
            return View(units.ToList());
        }

        // GET: Units/Details/5
        [Authorize(Roles = "Super Admin, Admin, Store Manger")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalide Navigation is detected";
                return RedirectToAction("UnitList");
            }
            Unit unit = db.Units.Find(id);
            if (unit == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unit Value is Not Found!!!";
                return RedirectToAction("UnitList");
            }
            return View(unit);
        }

        // GET: Units/Create
        [Authorize(Roles = "Super Admin, Admin, Store Manger")]
        public ActionResult Create()
        {
            //error Message display
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            return View();
        }

        // POST: Units/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin, Admin, Store Manger")]
        public ActionResult Create([Bind(Include = "ID,Name,ProductUnit,ApplicationUserID")] Unit unit)
        {
            //error Message display
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            unit.ApplicationUserID = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                if((from u in db.Units where u.Name == unit.Name orderby u.ID descending select u).Count() == 0) {
                    db.Units.Add(unit);
                    db.SaveChanges();
                    TempData[User.Identity.GetUserId() + "succsessMessage"] = "New unit information created";
                    return RedirectToAction("UnitList");

                }
                ViewBag.errorMessage = "Duplicated Unit Name";
                
            }

            return View(unit);
        }

        // GET: Units/Edit/5
        [Authorize(Roles = "Super Admin, Admin, Store Manger")]
        public ActionResult Edit(int? id)
        {
            //error Message display
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Edit Invalid ID";
                return RedirectToAction("UnitList");
            }
            Unit unit = db.Units.Find(id);
            if (unit == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unit Is Not Found";
                return RedirectToAction("UnitList");
            }
            Item unitItem = (from ut in db.Items where ut.UnitID == id select ut).First();
            if (unitItem != null)
            {
                ViewBag.errorMessage = "You can not Update Unit related Item is found";
            }
            //return RedirectToAction("UnitList");
            return View(unit);
        }

        // POST: Units/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin, Admin, Store Manger")]
        public ActionResult Edit([Bind(Include = "ID,Name,ProductUnit,ApplicationUserID")] Unit unit)
        {
            //error Message display
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
          
            unit.ApplicationUserID = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                if ((from u in db.Units where unit.Name == u.Name orderby u.ID descending select u).Count() == 0)
                {
                    db.Entry(unit).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData[User.Identity.GetUserId() + "infoMessage"] = "Unit is Succsfully Updated";
                    return RedirectToAction("UnitList");
                }
                ViewBag.errorMessage = "Duplicate Unit Update!!";
            }
            return View(unit);
        }

        // GET: Units/Delete/5
        [Authorize(Roles = "Super Admin, Admin, Store Manger")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId()+"errorMessage"] = "Bad Request, Invalid Navigation Is Detected";
                return RedirectToAction("UnitList");
            }
            Unit unit = db.Units.Find(id);
            if (unit == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unit Value is Not Found";
                return RedirectToAction("UnitList");
            }
            
            if ((from u in db.Items where u.UnitID == id select u).Count() > 0)
            {
                TempData[User.Identity.GetUserId()+"errorMessage"] = "You can't remove Unit INFO related Item is found!!!";
                return RedirectToAction("UnitList");
            }
            return View(unit);
        }

        // POST: Units/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin, Admin, Store Manger")]
        public ActionResult DeleteConfirmed(int id)
        {
            Unit unit = db.Units.Find(id);
            db.Units.Remove(unit);
            db.SaveChanges();
            TempData[User.Identity.GetUserId() + "infoMessage"] = "Unit Info is successfully Deleted";
            return RedirectToAction("UnitList");
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
