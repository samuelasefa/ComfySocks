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
    public class ItemTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ItemTypes
        [Authorize(Roles="Super Admin, Admin")]
        public ActionResult Index()
        {
            //error Message display
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            var itemTypes = db.ItemTypes;
            if (itemTypes.ToList().Count > 0)
            {
                ViewBag.HaveItemType = true;
            }
            else {
                ViewBag.HaveItemType = false;
            }
            return View(itemTypes.ToList());
        }

        // GET: ItemTypes/Details/
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult Details(int? id)
        {
            //error Message display
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation is detected";
                return RedirectToAction("Index");
            }
            ItemType itemType = db.ItemTypes.Find(id);
            if (itemType == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Not Found";
                return RedirectToAction("Index");
            }
            return View(itemType);
        }

        // GET: ItemTypes/Create
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult Create()
        {
            //error Message display
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            return View();
        }

        // POST: ItemTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult Create([Bind(Include = "ID,Name,ApplicationUserID")] ItemType itemType)
        {

            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            
            itemType.ApplicationUserID = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                if ((from it in db.ItemTypes where it.Name == itemType.Name select it).Count() == 0) {
                    db.ItemTypes.Add(itemType);
                    db.SaveChanges();
                    TempData[User.Identity.GetUserId() + "succsessMessage"] = "New ItemType is Created";
                    return RedirectToAction("Index");
                }
                ViewBag.errorMessage = "Duplicate ItemType";
            }

            return View(itemType);
        }

        // GET: ItemTypes/Edit/5
        [Authorize(Roles="Super Admin, Admin")]
        
        public ActionResult Edit(int? id)
        {
            //error Message display
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation is detected";
                return RedirectToAction("Index");
            }
            ItemType itemType = db.ItemTypes.Find(id);
            if (itemType == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Not Found";
                return RedirectToAction("Index");
            }
            return View(itemType);
        }

        // POST: ItemTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult Edit([Bind(Include = "ID,Name,ApplicationUserID")] ItemType itemType)
        {
            //error Message display
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            itemType.ApplicationUserID = User.Identity.GetUserId();
            
            if (ModelState.IsValid)
            {
                if ((from it in db.ItemTypes where it.Name == itemType.Name select it).Count() == 0) {
                    db.Entry(itemType).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.errorMessage = "Duplicate itemType";
            }
            return View(itemType);
        }

        // GET: ItemTypes/Delete/5
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult Delete(int? id)
        {
            //error Message display
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation is detected!!";
                return RedirectToAction("Index");
            }
            ItemType itemType = db.ItemTypes.Find(id);
            if (itemType == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Not Found";
                return RedirectToAction("Index");
            }
            return View(itemType);
        }

        // POST: ItemTypes/Delete/5
        [Authorize(Roles = "Super Admin, Admin")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //error Message display
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            ItemType itemType = db.ItemTypes.Find(id);
            db.ItemTypes.Remove(itemType);
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
