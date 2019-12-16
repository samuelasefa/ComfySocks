using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ComfySocks.DAL;
using ComfySocks.Models.Stock;

namespace ComfySocks.Controllers.Stock
{
    public class RowStocksController : Controller
    {
        private MyContext db = new MyContext();

        // GET: RowStocks
        public ActionResult Index()
        {
            var rowStock = db.RowStock.Include(r => r.Item);
            return View(rowStock.ToList());
        }

        // GET: RowStocks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RowStock rowStock = db.RowStock.Find(id);
            if (rowStock == null)
            {
                return HttpNotFound();
            }
            return View(rowStock);
        }

        // GET: RowStocks/Create
        public ActionResult Create()
        {
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Description");
            return View();
        }

        // POST: RowStocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ItemID,Quantity,UnitPrice,TotalPrice")] RowStock rowStock)
        {
            if (ModelState.IsValid)
            {
                db.RowStock.Add(rowStock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ItemID = new SelectList(db.Items, "ID", "Description", rowStock.ItemID);
            return View(rowStock);
        }

        // GET: RowStocks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RowStock rowStock = db.RowStock.Find(id);
            if (rowStock == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Description", rowStock.ItemID);
            return View(rowStock);
        }

        // POST: RowStocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ItemID,Quantity,UnitPrice,TotalPrice")] RowStock rowStock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rowStock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Description", rowStock.ItemID);
            return View(rowStock);
        }

        // GET: RowStocks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RowStock rowStock = db.RowStock.Find(id);
            if (rowStock == null)
            {
                return HttpNotFound();
            }
            return View(rowStock);
        }

        // POST: RowStocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RowStock rowStock = db.RowStock.Find(id);
            db.RowStock.Remove(rowStock);
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
