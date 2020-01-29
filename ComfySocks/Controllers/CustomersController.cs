using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ComfySocks.Models;
using ComfySocks.Models.Order;
using Microsoft.AspNet.Identity;

namespace ComfySocks.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customers
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        // GET: Customers/Details/5
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                TempData[User.Identity.GetUserId()+"errorMessage"] = "Invalid navigation is Detected";
                return RedirectToAction("Index");
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Not Found";
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Create
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult Create([Bind(Include = "ID,TinNumber,FirstName,LastName,City,SubCity")] Customer customer)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (ModelState.IsValid)
            {
                if ((from c in db.Customers where c.ID == customer.ID && c.TinNumber == customer.TinNumber orderby c.ID descending select c).Count() == 0)
                {
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.errorMessage = "Duplicate Customer is Found";
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation Is Detected";
                return RedirectToAction("Index");
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Not Found";
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult Edit([Bind(Include = "ID,TinNumber,FirstName,LastName,City,SubCity")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                if ((from c in db.Customers where c.ID == customer.ID && c.TinNumber == customer.TinNumber orderby c.ID descending select c).Count() == 0)
                {
                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.errorMessage = "Duplicate update Found";
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        [Authorize(Roles = "Super Admin, Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation is detected";
                return RedirectToAction("Index");
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Not Found";
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Super Admin, Admin")]

        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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
