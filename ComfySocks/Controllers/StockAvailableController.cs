using ComfySocks.Models.Repository;
using ComfySocks.Repository;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ComfySocks.Controllers
{
    [Authorize(Roles ="Super Admin, Admin, Sales, Store Manager, Finance, Production")]
    public class StockAvailableController : Controller
    {
        ComfySocks.Models.ApplicationDbContext db = new Models.ApplicationDbContext();
        // GET: StockAvailable
        public ActionResult StockAvailable()
        {
            var Items = (from I in db.Items where I.StoreType == Models.Items.StoreType.OfficeMaterial || I.StoreType == Models.Items.StoreType.RowMaterial || I.StoreType == Models.Items.StoreType.ProductItem select I);
            return View(Items.ToList());
        }
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult EditRowMaterialQuantity(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var stockqty = (from sq in db.RowMaterialRepositeries where sq.ID == id select sq).FirstOrDefault();
            if (stockqty == null)
            {
                return HttpNotFound();
            }
            return View(stockqty);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult EditRowMaterialQuantity(RowMaterialRepositery stockqty)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stockqty).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.successMessage = "Stock Quantity is Updated";
                return RedirectToAction("StockAvailable");
            }
            return View(stockqty);
        }

        //ProductAvaliable edit
        public ActionResult EditProductQuantity(int? id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productqty = (from p in db.ProductMaterialRepositories where p.ID == id select p).FirstOrDefault();
            if (productqty == null) {
                return HttpNotFound();
            }
            return View(productqty);
        }

        //PostMethod
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult EditProductQuantity(ProductMaterialRepository productStock)
        {
            if (ModelState.IsValid) {
                db.Entry(productStock).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.successMessage = "Product Quantity is successfuly Updated";
                return RedirectToAction("StockAvailable");
            }
            return View(productStock);
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