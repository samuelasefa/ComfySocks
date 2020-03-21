using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ComfySocks.Models;
using ComfySocks.Models.ProductInfo;
using ComfySocks.Models.Items;
using Microsoft.AspNet.Identity;
using ComfySocks.Models.InventoryModel;
using ComfySocks.Models.Repository;
using ComfySocks.Models.ProductTransferInfo;

namespace ComfySocks.Controllers
{
    [Authorize(Roles = "Super Admin, Production, Store Manager, Finance")]
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Stocks
        public ActionResult ProductList()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            var products = (from p in db.TransferInformation where p.Status == "Transferd" || p.Status == "Recivied" || p.Status == "Rejected" select p).Include(e => e.ApplicationUser);
            ViewBag.center = true;

            return View(products.ToList());
        }

        public ActionResult ProductDetail(int? id)
        {
            //errormessage display
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }

            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation is detected!";
                return RedirectToAction("ProductTransferList");
            }
            TransferInformation TI = db.TransferInformation.Find(id);


            if (TI == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid navigation detected!";
                return RedirectToAction("ProductTransferList");
            }
            try
            {
                int LastId = (from s in db.TransferInformation orderby s.ID orderby s.ID ascending select s.ID).First();
                TI.FPTNo = "FGRNo-" + (LastId + 1).ToString("D6");
            }
            catch
            {
                TI.FPTNo = "FGRNo"+ 1.ToString("D6");
            }
            return View(TI);
        }

        // Production order Approval
        [Authorize(Roles = "Super Admin, Store Manager")]
        public ActionResult ProductApproved(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected";
                return RedirectToAction("ProductList");
            }

            TransferInformation transferInformation = db.TransferInformation.Find(id);
            if (transferInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load Temporary product Information";
                return RedirectToAction("ProductList");
            }

            bool pass = true;
            if (pass)
            {
                foreach (Transfer transfer in transferInformation.Transfers)
                {
                    transferInformation.Status = "Recivied";
                    db.Entry(transferInformation).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.succsessMessage = "Succesfully Recivied";
                }

            }
            return View("ProductDetail", transferInformation);
        }

        //Production order Rejection
        [Authorize(Roles = "Super Admin, Store Manager")]
        public ActionResult ProductRejected(int? id)
        {
            if (id == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected";
                return RedirectToAction("ProductList");
            }

            TransferInformation transferInformation = db.TransferInformation.Find(id);

            if (transferInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to load Temporary product Information";
                return RedirectToAction("ProductList");
            }

            bool pass = true;
            if (pass)
            {
                foreach (Transfer transfer in transferInformation.Transfers)
                {
                    transferInformation.Status = "Droped";
                    db.Entry(transferInformation).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.succsessMessage = "Rejected";
                }

            }
            return View("ProductDetail", transferInformation);
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
