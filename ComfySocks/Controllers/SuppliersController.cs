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
    public class SuppliersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Suppliers
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult Index()
        {
            return View(db.Suppliers.ToList());
        }

        // GET: Suppliers/Details/5
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation is detected";
                return RedirectToAction("Index");
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Not Found!!";
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        // GET: Suppliers/Create
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult Create([Bind(Include = "ID,Name,No")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                if ((from s in db.Suppliers where s.ID == supplier.ID || s.No == supplier.No orderby s.ID select s).Count() == 0)
                {
                    db.Suppliers.Add(supplier);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.errorMessage = "Duplicate Supplier or Invoice Number is Found";
            }

            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation is detected";
                return RedirectToAction("Index");
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Not Found";
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult Edit([Bind(Include = "ID,Name,No")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                if ((from s in db.Suppliers where s.ID == supplier.ID || s.No == supplier.No orderby s.ID select s).Count() == 0) {
                    db.Entry(supplier).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.errorMessage = "Duplicate Update of Supplier and Invoice Number";
            }
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation is detected";
                return RedirectToAction("Index");
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Not Found";
                return RedirectToAction("Index");
            }
            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [Authorize(Roles = "Super Admin, Admin")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            db.Suppliers.Remove(supplier);
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
