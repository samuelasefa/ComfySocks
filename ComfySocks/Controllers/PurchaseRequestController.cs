using ComfySocks.Models;
using ComfySocks.Models.Items;
using ComfySocks.Models.PurchaseRequest;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ComfySocks.Models.PurchaseRequest.Purchase;

namespace ComfySocks.Controllers
{
    public class PurchaseRequestController : Controller
    {
        // GET: PurchaseRequest
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult PurchaseRequsetList()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            var PurchaseInfoList = (from p in db.PurchaseInformation orderby p.ID ascending select p);
            return View(PurchaseInfoList.ToList());
        }
        //GET:NewPurchaseRequest
        public ActionResult NewPurchaseRequestEntry()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            // requred Item Must be done
            {
                var item = db.Items.ToList();
                ViewBag.item = "";
                if (item.Count() == 0) {
                    ViewBag.item = "Register Item Information Frist";
                    ViewBag.RequiredItems = true;
                }
            }
            ViewBag.ItemID = (from I in db.Items where I.StoreType == Models.Items.StoreType.RowMaterial orderby I.ID descending select I).ToList();
            return View();
        }

        //POST:NewPurchaseRequest
        [HttpPost]
        public ActionResult NewPurchaseRequestEntry(Purchase purchase)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<PurchaseRequestVM> selectedPurchaserequest = new List<PurchaseRequestVM>();
            bool found = false;

            if (TempData[User.Identity.GetUserId() + "selectedPurchaserequest"] != null)
            {
                selectedPurchaserequest = (List<PurchaseRequestVM>)TempData[User.Identity.GetUserId() + "selectedPurchaserequest"];
            }
            purchase.PurchaseInformtionID = 1;
            if (ModelState.IsValid)
            {
                foreach (PurchaseRequestVM sr in selectedPurchaserequest)
                {
                    if (sr.Purchase.ItemID == purchase.ItemID)
                    {
                       sr.Purchase.Quantity += purchase.Quantity;
                        found = true;
                        ViewBag.infoMessage = "Item is Added !!!";
                        break;
                    }
                }
                if (found == false)
                {
                    PurchaseRequestVM purchaseRequestVM = new PurchaseRequestVM();
                    Item item = db.Items.Find(purchase.ItemID);
                    purchaseRequestVM.Description = item.Name;
                    purchaseRequestVM.Unit = item.Unit.Name;
                    purchaseRequestVM.Purchase = purchase;
                    selectedPurchaserequest.Add(purchaseRequestVM);
                }
            }
            else
            {
                ViewBag.errorMessage = "State is not valid";
            }
            TempData[User.Identity.GetUserId() + "selectedPurchaserequest"] = selectedPurchaserequest;
            ViewBag.selectedPurchaserequest = selectedPurchaserequest;
            if (selectedPurchaserequest.Count > 0)
            {
                ViewBag.haveItem = true;
            }
            ViewBag.ItemID = (from I in db.Items where I.StoreType == StoreType.RowMaterial orderby I.ID descending select I).ToList();
            return View("NewPurchaseRequestEntry");
        }

        //REMOVE: Purchase request list

        public ActionResult RemoveSelected(int id = 0)
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            List<PurchaseRequestVM> selectedPurchaserequest = new List<PurchaseRequestVM>();
            selectedPurchaserequest = (List<PurchaseRequestVM>)TempData[User.Identity.GetUserId() + "selectedPurchaserequest"];

            foreach (PurchaseRequestVM p in selectedPurchaserequest)
            {
                if (p.Purchase.ItemID == id)
                {
                    selectedPurchaserequest.Remove(p);
                    ViewBag.infoMessage = "Item is removed";
                    break;
                }
            }
            TempData[User.Identity.GetUserId() + "selectedPurchaserequest"] = selectedPurchaserequest;
            ViewBag.selectedPurchaserequest = selectedPurchaserequest;
            if (selectedPurchaserequest.Count > 0)
            {
                ViewBag.haveItem = true;
            }
            ViewBag.ItemID = (from I in db.Items where I.StoreType == StoreType.RowMaterial orderby I.ID descending select I).ToList();
            return View("NewPurchaseRequestEntry");
        }

        //Get Purchase request information

        public ActionResult NewPurchaseRequestInfo()
        {
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            if (TempData[User.Identity.GetUserId()+ "selectedPurchaserequest"] == null) {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Find selected Purchase request Information!";
                return RedirectToAction("PurchaseRequestList");
            }
            TempData[User.Identity.GetUserId() + "selectedPurchaserequest"] = TempData[User.Identity.GetUserId() + "selectedPurchaserequest"];

            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name");
            return View();
        }

        //Post PurchaseRequest
        [HttpPost]
        public ActionResult NewPurchaseRequestInfo(PurchaseInformation purchaseInformation, string Normal, string Urgent, string VeryUrgent)
        {
            if (Normal == "true")
            {
                purchaseInformation.isNormal = true;
            }
            else {
                purchaseInformation.isNormal = false;
            }
            if (Urgent == "true")
            {
                purchaseInformation.isUrgent = true;
            }
            else
            {
                purchaseInformation.isUrgent = false;
            }
            if (VeryUrgent == "true")
            {
                purchaseInformation.isVeryUrgent = true;
            }
            else {
                purchaseInformation.isVeryUrgent = false;
            }
            if (TempData[User.Identity.GetUserId() + "succsessMessage"] != null) { ViewBag.succsessMessage = TempData[User.Identity.GetUserId() + "succsessMessage"]; TempData[User.Identity.GetUserId() + "succsessMessage"] = null; }
            if (TempData[User.Identity.GetUserId() + "errorMessage"] != null) { ViewBag.errorMessage = TempData[User.Identity.GetUserId() + "errorMessage"]; TempData[User.Identity.GetUserId() + "errorMessage"] = null; }

            //
            if (TempData[User.Identity.GetUserId()+"selectedPurchaserequest"] == null){
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to Load selected Information";
                return RedirectToAction("PurchaseRequestList");
            }

            List<PurchaseRequestVM> selectedPurchaseRequest = new List<PurchaseRequestVM>();
            selectedPurchaseRequest = (List<PurchaseRequestVM>)TempData[User.Identity.GetUserId() + "ProductionOrders"];

            TempData[User.Identity.GetUserId() + "selectedPurchaseRequest"] = selectedPurchaseRequest;
            ViewBag.StoreID = new SelectList(db.Stores, "ID", "Name", purchaseInformation.StoreID);
            purchaseInformation.ApplicationUserID = User.Identity.GetUserId();
            purchaseInformation.Date = DateTime.Now;
            purchaseInformation.Status = "Submmited";
            if (ModelState.IsValid)
            {
                try
                {
                    db.PurchaseInformation.Add(purchaseInformation);
                    db.SaveChanges();
                    TempData[User.Identity.GetUserId() + "succsessMessage"] = "Purchase Request Info is  Succesfully add to database";

                    foreach (PurchaseRequestVM p in selectedPurchaseRequest)
                    {
                        p.Purchase.PurchaseInformtionID = purchaseInformation.ID;
                        db.Purchases.Add(p.Purchase);
                        db.SaveChanges();
                        TempData[User.Identity.GetUserId() + "succsessMessage"] = "Purchase request is saved";
                    }
                    PurchaseInformation po = db.PurchaseInformation.Find(purchaseInformation.ID);
                    po.PRNo = "PR-No: " + po.ID;
                    db.Entry(po).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData[User.Identity.GetUserId() + "succsessMessage"] = "Purchase Request Created!";
                    return RedirectToAction("PurchaseRequestDetails", new { id = purchaseInformation.ID });
                }
                catch
                {

                }

            }

            return View(purchaseInformation);
        }

        public ActionResult PurchaseRequestDetail(int? id)
        {
            if (id == 0)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Invalid Navigation is detected!! Please Try agian";
                return RedirectToAction("PurchaseRequestList");
            }
            PurchaseInformation purchaseInformation = db.PurchaseInformation.Find(id);
            if (purchaseInformation == null)
            {
                TempData[User.Identity.GetUserId() + "errorMessage"] = "Unable to find Purchase informtion";
                return RedirectToAction("PurchaseRequestList");
            }
            return View(purchaseInformation);
        }



    }
}