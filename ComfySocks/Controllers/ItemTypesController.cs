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
    [Authorize]
    public class ItemTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ItemTypes
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
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]

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
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]
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
                    ViewBag.succsessMessage = "New ItemType is Created";
                    return RedirectToAction("Index");
                }
                ViewBag.errorMessage = "Duplicate ItemType";
            }

            return View(itemType);
        }

        // GET: ItemTypes/Edit/5
        [Authorize(Roles="Super Admin, Admin, Store Manager")]
        
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
            if (itemType != null)
            {
                ViewBag.infoMessage = "If You Change ItemType Name All related Item Is Changed";
            }
            return View(itemType);
        }

        // POST: ItemTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]

        public ActionResult Edit([Bind(Include = "ID,Name")] ItemType itemType)
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
                    ViewBag.succsessMessage = "ItemType Update is Done.";
                    return RedirectToAction("Index");
                }
                ViewBag.errorMessage = "Duplicate itemType";
            }
            return View(itemType);
        }

        // GET: ItemTypes/Delete/5
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]

        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation is detected!!";
                return RedirectToAction("Index");
            }
            if (saveChangesError.GetValueOrDefault()) {
                ViewBag.errorMessage = "Delete failed. Try agian and if the problem persists see your system adminstrator";
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
        [Authorize(Roles = "Super Admin, Admin, Store Manager")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //error Message display
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            try
            {
                ItemType itemType = db.ItemTypes.Find(id);
                db.ItemTypes.Remove(itemType);
                db.SaveChanges();
                ViewBag.succsessMessage = "Succesfully Deleted!";
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
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
