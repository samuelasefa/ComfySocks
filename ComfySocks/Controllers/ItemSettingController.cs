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

            var items = db.Items.Include(i => i.ItemType).Include(i => i.Unit);
          

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
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Bad Request, Invalide Navigation is detected";
                return RedirectToAction("Index");
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Not Found!!!";
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: ItemSetting/Create
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]
        public ActionResult Create()
        {
            //error Message display
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }


            {
                //First information to Fill
                
                var itemTypes = db.ItemTypes.ToList();
                var unit = db.Units.ToList();
                
                ViewBag.itemTypes = "";
                ViewBag.unit = "";

                if (itemTypes.Count() == 0)
                {
                    ViewBag.itemTypes = "Register ItemType Information Frist";
                    ViewBag.RequiredItems = true;
                }
                if (unit.Count() == 0)
                {
                    ViewBag.unit = "Register Unit Information  Frist";
                    ViewBag.RequiredItems = true;
                }
            }

            ViewBag.ItemTypeID = new SelectList(db.ItemTypes, "ID", "Name");
            ViewBag.UnitID = new SelectList(db.Units, "ID", "Name");
            return View();
        }

        // POST: ItemSetting/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]
        public ActionResult Create(Item item)
        {
           item.ApplicationUserID = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                    if ((from c in db.Items where c.Code == item.Code select c).Count() == 0)
                    {

                        db.Items.Add(item);
                        db.SaveChanges();
                        TempData[User.Identity.GetUserId() + "succsessMessage"] = "New  Item is created";
                        return RedirectToAction("Create");
                    }
                ViewBag.errorMessage = "Duplicate item Code";
            }

            ViewBag.ItemTypeID = new SelectList(db.ItemTypes, "ID", "Name", item.ItemTypeID);
            ViewBag.UnitID = new SelectList(db.Units, "ID", "Name", item.UnitID);
            return View(item);
        }

        // GET: ItemSetting/Edit/5
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]
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
            ViewBag.UnitID = new SelectList(db.Units, "ID", "Name", item.UnitID);
            return View(item);
        }

        // POST: ItemSetting/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]
        public ActionResult Edit(Item item)
        {
            item.ApplicationUserID = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ItemTypeID = new SelectList(db.ItemTypes, "ID", "Name", item.ItemTypeID);
            ViewBag.UnitID = new SelectList(db.Units, "ID", "Name", item.UnitID);
            return View(item);
        }

        // GET: ItemSetting/Delete/5
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]
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
        [Authorize(Roles = "Super Admin, Admin, StoreManager")]

        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);

            if (item == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Item to remove";
            }
            if ((from i in db.Items where i.ItemTypeID == id select i).Count() > 0) {
                ViewBag.errorMessage = "You can't remove item information related item is found!!";
            }
            else
            {
                db.Items.Remove(item);
                db.SaveChanges();
                TempData[User.Identity.GetUserId() + "successMessage"] = "Item is Removed";
            }
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
