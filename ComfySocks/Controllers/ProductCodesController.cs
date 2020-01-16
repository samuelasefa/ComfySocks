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
    public class ProductCodesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ProductCodes
        public ActionResult Index()
        {
            return View(db.ProductCodes.ToList());
        }

        // GET: ProductCodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation is detected";
                return RedirectToAction("Index");
            }
            ProductCode productCode = db.ProductCodes.Find(id);
            if (productCode == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Not Found";
                return RedirectToAction("Index");
            }
            return View(productCode);
        }

        // GET: ProductCodes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductCodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCode productCode)
        {
            if (ModelState.IsValid)
            {
                db.ProductCodes.Add(productCode);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productCode);
        }

        // GET: ProductCodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation is detected";
                return RedirectToAction("Index");
            }
            ProductCode productCode = db.ProductCodes.Find(id);
            if (productCode == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Not Found";
                return RedirectToAction("Index");
            }
            return View(productCode);
        }

        // POST: ProductCodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCode productCode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productCode).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productCode);
        }

        // GET: ProductCodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation is detected";
                return RedirectToAction("Index");
            }
            ProductCode productCode = db.ProductCodes.Find(id);
            if (productCode == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Not Found";
                return RedirectToAction("Index");
            }
            return View(productCode);
        }

        // POST: ProductCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductCode productCode = db.ProductCodes.Find(id);
            db.ProductCodes.Remove(productCode);
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
