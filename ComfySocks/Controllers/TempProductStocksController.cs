using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ComfySocks.Models;
using ComfySocks.Models.ProductStock;
using Microsoft.AspNet.Identity;

namespace ComfySocks.Controllers
{
    public class TempProductStocksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TempProductStocks
        public ActionResult Index()
        {
            var tempProductStocks = db.TempProductStocks.Include(t => t.ApplicationUser).Include(t => t.ProductCode).Include(t => t.Unit);
            return View(tempProductStocks.ToList());
        }

        // GET: TempProductStocks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation is detected";
                return RedirectToAction("Index");
            }
            TempProductStock tempProductStock = db.TempProductStocks.Find(id);
            if (tempProductStock == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Not Found 404 is detected";
                return RedirectToAction("Index");
                
            }
            return View(tempProductStock);
        }

        // GET: TempProductStocks/Create
        public ActionResult Create()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            {
                var productcode = db.ProductCodes.ToList();
                var unit = db.Units.ToList();

                ViewBag.productcode = "";
                ViewBag.unit = "";
                if (productcode.Count() == 0)
                {
                    ViewBag.productcode = "Register Product Code Information";
                    ViewBag.RequiredItems = true;
                }
                if (unit.Count() == 0)
                {
                    ViewBag.unit = "Register Unit Information frist";
                    ViewBag.RequiredItems = true;
                }
            }
            ViewBag.ProductCodeID = new SelectList(db.ProductCodes, "ID", "Code");
            ViewBag.UnitID = new SelectList(db.Units, "ID", "Name");
            return View();
        }

        // POST: TempProductStocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProductName,ProductCodeID,UnitID,ApplicationUserID")] TempProductStock tempProductStock)
        {
            tempProductStock.ApplicationUserID = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.TempProductStocks.Add(tempProductStock);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            ViewBag.ProductCodeID = new SelectList(db.ProductCodes, "ID", "Code", tempProductStock.ProductCodeID);
            ViewBag.UnitID = new SelectList(db.Units, "ID", "Name", tempProductStock.UnitID);
            return View(tempProductStock);
        }

        // GET: TempProductStocks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation is detected";
                return RedirectToAction("Index");
            }
            TempProductStock tempProductStock = db.TempProductStocks.Find(id);
            if (tempProductStock == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Not Found 404 error Invalid  Url";
                return RedirectToAction("Index");
            }
            ViewBag.ProductCodeID = new SelectList(db.ProductCodes, "ID", "Code", tempProductStock.ProductCodeID);
            ViewBag.UnitID = new SelectList(db.Units, "ID", "Name", tempProductStock.UnitID);
            return View(tempProductStock);
        }

        // POST: TempProductStocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProductName,ProductCodeID,UnitID,ApplicationUserID")] TempProductStock tempProductStock)
        {
            tempProductStock.ApplicationUserID = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Entry(tempProductStock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCodeID = new SelectList(db.ProductCodes, "ID", "Code", tempProductStock.ProductCodeID);
            ViewBag.UnitID = new SelectList(db.Units, "ID", "Name", tempProductStock.UnitID);
            return View(tempProductStock);
        }

        // GET: TempProductStocks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation is detected";
                return RedirectToAction("Index");
            }
            TempProductStock tempProductStock = db.TempProductStocks.Find(id);
            if (tempProductStock == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Not Found";
                return RedirectToAction("Index");
            }
            return View(tempProductStock);
        }

        // POST: TempProductStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TempProductStock tempProductStock = db.TempProductStocks.Find(id);
            db.TempProductStocks.Remove(tempProductStock);
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
