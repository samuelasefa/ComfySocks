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
    public class ItemSettingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ItemSetting
        public ActionResult Index()
        { //Succsess and error message goes upon here
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            var items = db.Items.Include(i => i.ItemType).Include(i => i.StoreType).Include(i => i.Unit);

            if (items.ToList().Count > 0)
            {
                ViewBag.HaveList = true;
            }
            else
            {
                ViewBag.HaveList = false;
            }
            return View(items.ToList());
        }
            
        // GET: ItemSetting/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // GET: ItemSetting/Create
        public ActionResult Create()
        {
            ViewBag.ItemTypeID = new SelectList(db.ItemTypes, "ID", "Name");
            ViewBag.StoreTypeID = new SelectList(db.StoreTypes, "ID", "Name");
            ViewBag.UnitID = new SelectList(db.Units, "ID", "Name");
            return View();
        }

        // POST: ItemSetting/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,StoreTypeID,ItemTypeID,UnitID,Code,ApplicationUserID")] Item item)
        {
            item.ApplicationUserID = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                db.SaveChanges();
                TempData[User.Identity.GetUserId() + "succsessMessage"] = "New  Item is created";
                return RedirectToAction("Index");
            }

            ViewBag.ItemTypeID = new SelectList(db.ItemTypes, "ID", "Name", item.ItemTypeID);
            ViewBag.StoreTypeID = new SelectList(db.StoreTypes, "ID", "Name", item.StoreTypeID);
            ViewBag.UnitID = new SelectList(db.Units, "ID", "Name", item.UnitID);
            return View(item);
        }

        // GET: ItemSetting/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.ItemTypeID = new SelectList(db.ItemTypes, "ID", "Name", item.ItemTypeID);
            ViewBag.StoreTypeID = new SelectList(db.StoreTypes, "ID", "Name", item.StoreTypeID);
            ViewBag.UnitID = new SelectList(db.Units, "ID", "Name", item.UnitID);
            return View(item);
        }

        // POST: ItemSetting/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,StoreTypeID,ItemTypeID,UnitID,Code,ApplicationUserID")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemTypeID = new SelectList(db.ItemTypes, "ID", "Name", item.ItemTypeID);
            ViewBag.StoreTypeID = new SelectList(db.StoreTypes, "ID", "Name", item.StoreTypeID);
            ViewBag.UnitID = new SelectList(db.Units, "ID", "Name", item.UnitID);
            return View(item);
        }

        // GET: ItemSetting/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: ItemSetting/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
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
